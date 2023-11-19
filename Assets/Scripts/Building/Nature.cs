using UnityEngine;

public class Nature : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;

    #region SETUP

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);
    }

    void OnDestroy()
    {
        _approvalEvent.RaiseFloatEvent(_approval * -0.3f);
    }

    #endregion

    public override void Upgrade()
    {
        base.Upgrade();

        _approvalEvent.RaiseFloatEvent(_approval * (0.2f * (_level - 1)));
    }

    protected override BuildingInfo Information()
    {
        return new BuildingInfo(
            this.ToString(),
            _level.ToString(),
            (_approval * 100).ToString("0.0") + "%",
            UpgradeCost.ToString(),
            transform.GetSiblingIndex()
        );
    }
}
