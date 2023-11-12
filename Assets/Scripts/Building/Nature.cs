using UnityEngine;

public class Nature : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval * Mathf.Log10(_level + 10));
    }

    void OnDestroy()
    {
        _approvalEvent.RaiseFloatEvent(_approval * 0.3f);
    }

    protected override BuildingInfo Information()
    {
        return new BuildingInfo(
            this.name,
            _level.ToString(),
            (_approval * 100).ToString("0.0") + "%",
            _upgradeCost.ToString()
        );
    }
}
