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
    [SerializeField] Texture2D _removeCursor;

    [Header("Input")]
    [SerializeField] private float _rotateSpeed;
    private bool _rotate = false;
    private GameInput gameInput;
    private Camera _camera;

    [Header("Build")]
    [SerializeField] private GameObject _denyEffect;
    [SerializeField] private Mockup[] _buildingMockups;
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private IntEventChannel _buildEvent;
    [SerializeField] private LayerMask _obstacleLayers;
    private Mockup _mockup = null;
    private int _mockupIndex = -1;
    private bool _isPlanet = false, _remove = false;

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
            _isPlanet = MouseToWorld(mouse, out var worldPosition);

            if (worldPosition != _mockup.transform.position)
            {
                _mockup.transform.position = worldPosition;
                _mockup.transform.up = worldPosition;
            }

            // slightly tint the mockup red or green
            _mockup.SetClear(_isPlanet);
        }
    }

    #region INPUT

    // select an object
	public void OnSelect(InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Started) return;

        // in build mode
        if (_mockup != null)
        {
            if (_isPlanet) _buildEvent.RaiseIntEvent(_mockupIndex);
            else Instantiate(
                _denyEffect,
                _mockup.transform.position + _mockup.transform.up,
                _mockup.transform.rotation
            );
        }
        // look for object at mouse position and select it if possible
        else
        {
            var mouse = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(new(mouse.x, mouse.y, 0));

            if (!Physics.Raycast(ray, out var hit)) return;

            if (!hit.collider.TryGetComponent<ISelectable>(out var obj))
            {
                if (_remove) Destroy(hit.collider.gameObject);
                else obj.Select();
            }
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

    // zoom in and out of the planet
	public void OnZoom(InputAction.CallbackContext context)
	{
        var camPosition = _camera.transform.localPosition;
        var zoom = context.ReadValue<Vector2>().y / 120;
        
        // clamp
        if (camPosition.z <= -25f && zoom < 0 ||
            camPosition.z >= -8f && zoom > 0)
                return;
        
        _camera.transform.localPosition += Vector3.forward * zoom;
	}

    // pause the game
	public void OnPause(InputAction.CallbackContext context)
	{
		Pause();
	}

    #endregion

    public void Remove()
    {
        _remove = !_remove;
        SetCursor(_remove ? _removeCursor : _normalCursor);
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

        if (!hit.collider.CompareTag("Planet")) return false;

        return Physics.OverlapBox(
            worldPosition,
            Vector3.one * 0.5f,
            Quaternion.FromToRotation(Vector3.up, worldPosition),
            _obstacleLayers.value,
            QueryTriggerInteraction.Collide
        ).Length == 0;
    }

    #endregion
}
