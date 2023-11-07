using UnityEngine;

public class Mine : Building
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
}
