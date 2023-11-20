using UnityEngine;

public class Military : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;
    [SerializeField] private float _shootTimer;
    [SerializeField] private Projectile _projectile;
    private float _timer = 0, _factor = 1;

    #region SETUP
    
    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);

        CreateInfo((_factor * 100).ToString("0.0") + "%");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asteroid") || _timer > 0) return;

        var p = Instantiate(_projectile, transform.position + transform.up, transform.rotation);
        p.Init(other.transform, _factor);
        _timer = _shootTimer;
    }

    #endregion

    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
    }

    public override void Upgrade()
    {
        _factor = UpgradeManager.MilitaryUpgrade(_factor);

        base.Upgrade();

        CreateInfo((_factor * 100).ToString("0.0") + "%");
        GameObject.FindGameObjectWithTag("Info").GetComponent<RMenu>().Show(Info);
    }
}
