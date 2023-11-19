using UnityEngine;

public class Asteroid : MonoBehaviour, ISelectable
{
    public Money Value { get; private set; }
    [SerializeField] private GameData _gameData;
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private GameObject _explodeEffect;
    [SerializeField] private AudioClip _explodeSound;
    private float _speed;
    private Vector3 _direction;
    private Vector3 _rotation;
    private Outline _outline;

    void OnMouseEnter()
    {
        _outline.enabled = true;
    }

    void OnMouseExit()
    {
        _outline.enabled = false;
    }
    
    void Update()
    {
        if (Vector3.Magnitude(transform.position) > 100) Destroy(gameObject);
        
        transform.position += _direction * _speed * Time.deltaTime;
        transform.Rotate(_rotation, Space.Self);
    }

    public void Init(Money value, Vector3 direction, float speed)
    {
        Value = value;
        _direction = direction;
        _speed = speed;
        _rotation = Random.onUnitSphere;
        _outline = GetComponent<Outline>();
    }

    public void Select()
    {
        _moneyEvent.RaiseMoneyEvent(Value);
        Instantiate(_explodeEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(
            _explodeSound,
            Vector3.zero + transform.position.normalized,
            _gameData.Volume
        );
        
        Destroy(gameObject);
    }

    public void Deselect() {}
}
