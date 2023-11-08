using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _speed;
    private Transform _target;
    private int _level;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asteroid")) return;

        var income = other.GetComponent<Asteroid>().Value;
        _moneyEvent.RaiseMoneyEvent(income);

        Instantiate(_explosionEffect, transform.position, transform.rotation);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    void Update()
    {
        var oldPosition = transform.position;
        transform.position = Vector3.MoveTowards(
            oldPosition,
            _target.position,
            _speed
        );

        transform.rotation = Quaternion.FromToRotation(transform.up, transform.position - oldPosition);
    }

    public void Init(Transform target, int level)
    {
        _target = target;
        _level = level;
    }
}
