using UnityEngine;

public class Reactor : MonoBehaviour
{
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private float _approval;
    [SerializeField] private float _boost;
    [SerializeField] private LayerMask _boostableLayer;

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
}
