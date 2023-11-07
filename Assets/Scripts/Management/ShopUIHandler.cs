using UnityEngine;

public enum ShopState { Inactive, Active, Aside }

public class ShopUIHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _shop;
    [SerializeField] private Vector3 _inactivePosition;
    [SerializeField] private Vector3 _activePosition;
    [SerializeField] private Vector3 _asidePosition;
    [SerializeField] private float _transitionSpeed;
    [SerializeField] private IntEventChannel _shopSelectEvent;
    private ShopState _state;
    private int _selectedIndex = -1;

    #region SETUP

    private void OnEnable()
    {
        _shopSelectEvent.IntEventRaised += SelectOption;
    }

    private void OnDisable()
    {
        _shopSelectEvent.IntEventRaised -= SelectOption;
    }

    void Start()
    {
        _shop.position = _inactivePosition;
        _state = ShopState.Inactive;
    }

    #endregion

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (_state)
        {
            case ShopState.Inactive:
                if (_shop.position != _inactivePosition)
                    _shop.position = Vector3.MoveTowards(
                        _shop.position,
                        _inactivePosition,
                        _transitionSpeed
                    );
                break;

            case ShopState.Active:
                if (_shop.position != _activePosition)
                    _shop.position = Vector3.MoveTowards(
                        _shop.position,
                        _activePosition,
                        _transitionSpeed
                    );
                break;

            case ShopState.Aside:
                if (_shop.position != _asidePosition)
                    _shop.position = Vector3.MoveTowards(
                        _shop.position,
                        _asidePosition,
                        _transitionSpeed
                    );
                break;
        }
    }

    void SelectOption(int option)
    {
        if (_selectedIndex == option)
        {
            _state = ShopState.Active;
            _selectedIndex = -1;
        }
        else
        {
            _state = ShopState.Aside;
            _selectedIndex = option;
        }
    }

    public void ToggleUIElement()
    {
        switch (_state)
        {
            case ShopState.Inactive:
                _state = ShopState.Active;
                break;

            case ShopState.Active:
                _state = ShopState.Inactive;
                break;
        }
    }
}
