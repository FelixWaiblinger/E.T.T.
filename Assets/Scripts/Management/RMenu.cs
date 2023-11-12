using UnityEngine;
using TMPro;

public class RMenu : MonoBehaviour
{
    [SerializeField] private Vector3 _activePosition;
    [SerializeField] private Vector3 _inactivePosition;
    [SerializeField] private RectTransform _menu;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _gain;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private float _transitionSpeed;
    private bool _show = false;

    void Start()
    {
        _menu.localPosition = _inactivePosition;
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

    public void Show(BuildingInfo info)
    {
        _show = true;
        _name.text = info.Name;
        _level.text = "Level " + info.Level;
        _gain.text = info.Gain;
        _cost.text = info.Cost;
    }

    public void Hide()
    {
        _show = false;
    }
}
