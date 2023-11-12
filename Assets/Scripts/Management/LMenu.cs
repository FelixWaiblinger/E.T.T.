using UnityEngine;

public enum ShopState { Inactive, Active, Aside }

public class LMenu : MonoBehaviour
{
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private Vector3 _activePosition;
    [SerializeField] private Vector3 _inactivePosition;
    [SerializeField] private RectTransform _menu;
    [SerializeField] private float _transitionSpeed;
    private int _selectedIndex = -1;
    private bool _show = false;

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
        _menu.localPosition = _inactivePosition;
    }

    #endregion

    void Update()
    {
        if (_show && _menu.localPosition != _activePosition)
            _menu.localPosition = Vector3.MoveTowards(
                _menu.localPosition,
                _activePosition,
                _transitionSpeed
            );
        else if (!_show && _menu.localPosition != _inactivePosition)
            _menu.localPosition = Vector3.MoveTowards(
                _menu.localPosition,
                _inactivePosition,
                _transitionSpeed
            );
    }

    void SelectOption(int option)
    {
        _selectedIndex = _selectedIndex == option ? -1 : option;
    }

    public void Show()
    {
        _show = !_show;
    }

    public void Hide()
    {
        _show = false;
    }
}
