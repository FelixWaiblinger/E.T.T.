using System.Collections.Generic;
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
    }

    void Update()
    {

        if (_timer < _incomeTimer) _timer += Time.deltaTime;
        else
        {
            _moneyEvent.RaiseMoneyEvent(_income * Mathf.Pow(2f, _level));
            _timer = 0;
        }
    }

    public void Boost(float factor)
    {
        _income = _income * factor;
    }
}
