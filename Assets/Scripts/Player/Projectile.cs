using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private AudioClip _explodeSound;
    [SerializeField] private AudioClip _startSound;
    [SerializeField] private float _speed;
    private Transform _target;
    private float _factor;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asteroid")) return;

        var income = other.GetComponent<Asteroid>().Value;
        _moneyEvent.RaiseMoneyEvent(income * _factor);

        Instantiate(_explosionEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(_explodeSound, transform.position, _gameData.Volume);
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

    public void Init(Transform target, float factor)
    {
        _target = target;
        _factor = factor;
        AudioSource.PlayClipAtPoint(_startSound, transform.position, _gameData.Volume);
    }
}
