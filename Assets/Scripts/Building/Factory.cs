using UnityEngine;

public class Factory : Building, IBoostable
{
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private Money _income;
    [SerializeField] private float _incomeTimer;
    [SerializeField] private float _approval;
    private float _timer = 0;
    
    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);

        CreateInfo(_income.ToString());
    }

    void Update()
    {
        if (_timer < _incomeTimer) _timer += Time.deltaTime;
        else
        {
            _moneyEvent.RaiseMoneyEvent(_income);
            _timer = 0;
        }
    }

    public override void Upgrade()
    {
        _income = UpgradeManager.FactoryUpgrade(_income);

        base.Upgrade();

        CreateInfo(_income.ToString());
        GameObject.FindGameObjectWithTag("Info").GetComponent<RMenu>().Show(Info);
    }

    public void Boost(float factor)
    {
        _income = _income * factor;
    }
}
