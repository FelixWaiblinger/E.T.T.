using UnityEngine;

public class Nature : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;

    #region SETUP

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);

        CreateInfo((_approval * 100).ToString("0.0") + "%");
        GameObject.FindGameObjectWithTag("Info").GetComponent<RMenu>().Show(Info);
    }

    void OnDestroy()
    {
        _approvalEvent.RaiseFloatEvent(_approval * -1.2f);
    }

    #endregion

    public override void Upgrade()
    {
        base.Upgrade();

        CreateInfo((_approval * 100).ToString("0.0") + "%");

        _approvalEvent.RaiseFloatEvent(
            UpgradeManager.NatureUpgrade(_approval, _level)
        );
    }
}
