using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, GameInput.IControlsActions
{
    [Header("Cursor")]
    [SerializeField] Texture2D _normalCursor;
    [SerializeField] Texture2D _buildCursor;
    [SerializeField] Texture2D _rotateCursor;

    [Header("Input")]
    [SerializeField] private float _rotateSpeed;
    private bool _rotate = false;
    private GameInput gameInput;
    private Camera _camera;

    [Header("Build")]
    [SerializeField] private GameObject[] _buildingMockups;
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private IntEventChannel _buildEvent;
    [SerializeField] private LayerMask _obstacleLayers;
    private GameObject _mockup = null;
    private Color[] _mockupBaseColors;
    private Color _clearColor = new Color(0, 0.2f, 0, 0), _blockColor = new Color(0.2f, 0, 0, 0);
    private int _mockupIndex = -1;
    private bool _isClear = false;

    #region SETUP

    void Awake()
    {
        _camera = Camera.main;
        if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Controls.SetCallbacks(this);
		}

        StartCoroutine(Introduction());
    }

    void OnEnable()
    {
        _shopSelectEvent.IntEventRaised += CreateMockup;
    }

    void OnDisable()
    {
        _shopSelectEvent.IntEventRaised -= CreateMockup;
    }

    IEnumerator Introduction()
    {
        var camera = transform.GetChild(0);
        camera.position = Vector3.forward * -100;

        while (camera.position.z < -10)
        {
            camera.position += Vector3.forward;
            yield return null;
        }

        gameInput.Controls.Enable();
    }

    #endregion

    void Update()
    {
        if (_mockup != null)
        {
            // move the mockup along with the mouse
            var mouse = Mouse.current.position.ReadValue();
            _isClear = MouseToWorld(mouse, out var worldPosition);

            if (worldPosition != _mockup.transform.position)
            {
                _mockup.transform.position = worldPosition;
                _mockup.transform.up = worldPosition;
            }

            // slightly tint the mockup red or green
            var materials = _mockup.GetComponent<Mockup>().Materials;
            for (int i = 0; i < _mockupBaseColors.Length; i++)
                materials[i].color = _mockupBaseColors[i] + (_isClear ? _clearColor : _blockColor);
        }
    }

    #region INPUT

    // select an object
	public void OnSelect(InputAction.CallbackContext context)
	{
        // in build mode
        if (_mockup != null)
        {
            if (_isClear) _buildEvent.RaiseIntEvent(_mockupIndex);
            else Debug.Log("Can not build here"); // TODO create UI element
        }
        // look for object at mouse position and select it if possible
        else
        {
            var mouse = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(new(mouse.x, mouse.y, 0));

            if (!Physics.Raycast(ray, out var hit)) return;

            if (!hit.collider.TryGetComponent<ISelectable>(out var obj))
                obj.Select();
        }
	}

    // select an object
	public void OnRotate(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
        {
            _rotate = true;
            SetCursor(_rotateCursor);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _rotate = false;
            if (_mockup != null) SetCursor(_buildCursor);
            else SetCursor(_normalCursor);
        }
	}

    // rotate camera around planet
	public void OnDirection(InputAction.CallbackContext context)
	{
        if (!_rotate) return;

		var delta = context.ReadValue<Vector2>() * _rotateSpeed;
        transform.Rotate(new(-delta.y, delta.x, 0), Space.Self);
	}

    // pause the game
	public void OnPause(InputAction.CallbackContext context)
	{
		Pause();
	}

    #endregion

    // TODO
    public void Remove()
    {
        // destroy buildings
    }

    // TODO
    public void Pause()
    {
        CreateMockup(_mockupIndex); // cancel current build mode
        // open pause menu
    }

    void CreateMockup(int option)
    {
        // cancel
        if (_mockupIndex == option)
        {
            SetCursor(_normalCursor);
            Destroy(_mockup);
            _mockupIndex = -1;
            return;
        }

        SetCursor(_buildCursor);
        _mockup = Instantiate(_buildingMockups[option], transform);
        _mockupBaseColors = _mockup.GetComponent<Mockup>().Materials.Select(x => x.color).ToArray();
        _mockupIndex = option;
    }

    #region HELPER

    void SetCursor(Texture2D cursor)
    {
        Cursor.SetCursor(
                cursor,
                new(cursor.width / 2, cursor.height / 2),
                CursorMode.Auto
        );
    }

    bool MouseToWorld(Vector2 mouse, out Vector3 worldPosition)
    {
        worldPosition = Vector3.zero;
        var ray = _camera.ScreenPointToRay(new(mouse.x, mouse.y, 0));

        if (!Physics.Raycast(ray, out var hit)) return false;

        worldPosition = hit.point;
        var rotation = Quaternion.FromToRotation(Vector3.up, worldPosition);
        var isPlanet = hit.collider.CompareTag("Planet");
        var obstacles = Physics.OverlapBox(
            worldPosition, Vector3.one * 0.5f, rotation,
            _obstacleLayers.value, QueryTriggerInteraction.Collide
        );

        if (isPlanet && obstacles.Length == 0) return true;

        return false;
    }

    #endregion
}
