using UnityEngine;

public class Military : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;
    [SerializeField] private float _shootTimer;
    [SerializeField] private Projectile _projectile;
    private float _timer = 0;

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asteroid") || _timer > 0) return;

        var p = Instantiate(_projectile, transform.position + transform.up, transform.rotation);
        p.Init(other.transform, _level);
        _timer = _shootTimer;
    }

    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
    }
}
