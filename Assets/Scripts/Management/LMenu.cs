using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LMenu : MonoBehaviour
{
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private Vector3 _activePosition;
    [SerializeField] private Vector3 _inactivePosition;
    [SerializeField] private RectTransform _menu;
    [SerializeField] private float _transitionSpeed;
    [SerializeField] private Button[] _shopOptions;
    [SerializeField] private BuildingSO[] _buildingOptions;
    private int _selectedIndex = -1;
    private bool _show = false;
    
    void Awake()
    {
        _menu.localPosition = _inactivePosition;

        for (int i = 0; i < _shopOptions.Length; i++)
        {
            var t = _shopOptions[i].transform;
            // icon
            t.GetChild(0).GetComponent<Image>().sprite = _buildingOptions[i].Icon;
            // name
            t.GetChild(1).GetComponent<TMP_Text>().text = _buildingOptions[i].Name;
            // cost
            t.GetChild(2).GetComponent<TMP_Text>().text = _buildingOptions[i].Cost.ToString();
        }
    }

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

    #region BUTTON

    public void SelectOption(int option)
    {
        _selectedIndex = _selectedIndex == option ? -1 : option;
        _shopSelectEvent.RaiseIntEvent(option);
    }

    public void Show()
    {
        _show = !_show;
    }

    public void Hide()
    {
        _show = false;
    }

    #endregion
}
