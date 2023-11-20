using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, GameInput.IControlsActions
{
    [Header("Events")]
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private IntEventChannel _buildEvent;
    [SerializeField] private BoolEventChannel _infoEvent;
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private VoidEventChannel _titleEvent;

    [Header("Cursor")]
    [SerializeField] Texture2D _normalCursor;
    [SerializeField] Texture2D _buildCursor;
    [SerializeField] Texture2D _rotateCursor;
    [SerializeField] Texture2D _removeCursor;

    [Header("Input")]
    [SerializeField] private GameObject _sMenu;
    [SerializeField] private float _rotateSpeed;
    private GameInput gameInput;
    private Camera _camera;
    private ISelectable _selectedObject;
    private bool _rotate = false;

    [Header("Building")]
    [SerializeField] private Image _removeButton;
    [SerializeField] private GameObject _denyEffect;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private Mockup[] _buildingMockups;
    private Mockup _mockup = null;
    private int _mockupIndex = -1;
    private bool _isPlanet = false, _remove = false, _info = false;

    [Header("Departure")]
    [SerializeField] private Image _departBar;
    [SerializeField] private RectTransform _transition;
    private float _departTimer = 0;
    private bool _depart = false;

    #region SETUP

    void Awake()
    {
        _camera = Camera.main;
        if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Controls.SetCallbacks(this);
		}

        gameInput.Controls.Enable();
    }

    void OnEnable()
    {
        _shopSelectEvent.IntEventRaised += CreateMockup;
        _titleEvent.VoidEventRaised += () => StartCoroutine(Depart(0));
    }

    void OnDisable()
    {
        _shopSelectEvent.IntEventRaised -= CreateMockup;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        gameInput.Controls.RemoveCallbacks(this);
    }

    IEnumerator Depart(float delay = 1.3f)
    {
        yield return new WaitForSeconds(delay);

        _transition.gameObject.SetActive(true);
        var speed = 2f;

        while (_transition.localScale.magnitude < 1000)
        {
            _transition.localScale += Vector3.one * speed * Time.deltaTime;
            speed *= 1.1f;
            yield return null;
        }
    }

    #endregion

    void Update()
    {
        UpdateMockup();

        UpdateDepart();
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

            if (hit.collider.TryGetComponent<ISelectable>(out var obj))
            {
                if (_remove) Destroy(hit.collider.gameObject);
                else
                {
                    StopRemove();

                    if (_selectedObject != null) _selectedObject.Deselect();
                    _selectedObject = obj;
                    obj.Select();
                }
            }
            else StopRemove();
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

    #region BUTTON

    public void Remove()
    {
        if (_remove) StopRemove();
        else
        {
            _remove = true;
            _removeButton.color = new Color(1, 0, 0, 1);
        }

        SetCursor(_remove ? _removeCursor : _normalCursor);
    }

    public void ToggleInfo()
    {
        StopRemove();
        _info = !_info;
        _infoEvent.RaiseBoolEvent(_info);
    }

    // TODO
    public void Pause()
    {
        StopRemove();

        // cancel current build mode
        if (_mockup != null) CreateMockup(_mockupIndex);

        // open pause menu
        _sMenu.SetActive(true);
    }

    public void StartDepart()
    {
        StopRemove();
        _depart = true;
    }

    public void StopDepart()
    {
        _depart = false;
        _departTimer = 0;
        _departBar.fillAmount = 0;
    }

    public void Deselect()
    {
        if (_selectedObject != null) _selectedObject.Deselect();
        _selectedObject = null;
    }

    public void DestroyOldMockup()
    {
        SetCursor(_normalCursor);
        _mockupIndex = -1;

        if (_mockup != null) Destroy(_mockup.gameObject);
    }

    #endregion

    #region EVENT

    void CreateMockup(int option)
    {
        DestroyOldMockup();

        SetCursor(_buildCursor);
        _mockup = Instantiate(_buildingMockups[option], transform);
        _mockupIndex = option;
    }

    #endregion

    #region HELPER

    void UpdateMockup()
    {
        if (_mockup == null) return;

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

    void UpdateDepart()
    {
        if (!_depart) return;

        if (_departTimer < 3f)
        {
            _departTimer += Time.deltaTime;
            _departBar.fillAmount = _departTimer / 3f;
            return;
        }

        _departEvent.RaiseVoidEvent();
        _depart = false;
        StartCoroutine(Depart());
    }

    void SetCursor(Texture2D cursor)
    {
        Cursor.SetCursor(
            cursor,
            new(74, 43),
            CursorMode.Auto
        );
    }

    void StopRemove()
    {
        _remove = false;
        _removeButton.color = new Color(1, 1, 1, 1);
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
