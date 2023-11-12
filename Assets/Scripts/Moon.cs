using UnityEngine;

public class Moon : MonoBehaviour
{
    private Vector3 _rotation;
    
    void Start()
    {
        _rotation = Random.rotation.eulerAngles; 
    }

    void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime, Space.Self);
    }
}
