using UnityEngine;

public class Reactor : Building
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;
    [SerializeField] private float _boost;
    [SerializeField] private LayerMask _boostableLayer;

    #region SETUP

    void Start()
    {
        _approvalEvent.RaiseFloatEvent(_approval);
        var objects = Physics.OverlapSphere(
            transform.position,
            5,
            _boostableLayer,
            QueryTriggerInteraction.Collide
        );

        foreach (var obj in objects)
            if (obj.TryGetComponent<IBoostable>(out var b))
                b.Boost(_boost);

        CreateInfo((_boost * 100).ToString("0.0") + "%");
    }

    void OnDestroy()
    {
        var objects = Physics.OverlapSphere(
            transform.position,
            5,
            _boostableLayer,
            QueryTriggerInteraction.Collide
        );

        foreach (var obj in objects)
            if (obj.TryGetComponent<IBoostable>(out var b))
                b.Boost(1 / _boost);
    }

    #endregion

    public override void Upgrade()
    {
        base.Upgrade();

        CreateInfo((_boost * 100).ToString("0.0") + "%");
        GameObject.FindGameObjectWithTag("Info").GetComponent<RMenu>().Show(Info);
    }
}
