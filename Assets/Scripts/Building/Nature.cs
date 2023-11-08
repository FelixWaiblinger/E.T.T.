using UnityEngine;

public class Nature : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;
    [SerializeField] private float _decayTimer;

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval * Mathf.Log10(_level + 10));
    }

    void Update()
    {
        if (_decayTimer > 0) _decayTimer -= Time.deltaTime;
        else
        {
            _approvalEvent.RaiseFloatEvent(_approval * -0.3f);
            Destroy(gameObject);
        }
    }
}
