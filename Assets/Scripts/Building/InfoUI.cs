using UnityEngine;

public class InfoUI : MonoBehaviour
{
    private Transform _camera;

    void Start()
    {
        _camera = Camera.main.transform;    
    }

    void Update()
    {
        transform.rotation = _camera.rotation;
    }
}
