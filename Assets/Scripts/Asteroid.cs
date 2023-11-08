using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Money Value { get; private set; }
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
}
