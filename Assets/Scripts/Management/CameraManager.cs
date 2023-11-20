using UnityEngine;

public enum CameraState { Start, Idle, Depart, Moving }

public class CameraManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private Camera _camera;
    [SerializeField] private Quaternion _departRotation;
    [SerializeField] private GameObject _departEffect;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _fovSpeed;
    private CameraState _state = CameraState.Start;

    #region SETUP

    void OnEnable()
    {
        _departEvent.VoidEventRaised += Depart;
    }

    void OnDisable()
    {
        _departEvent.VoidEventRaised -= Depart;
    }

    void Start()
    {
        transform.localPosition = Vector3.forward * -100;
        transform.localRotation = Quaternion.identity;
    }

    #endregion

    void Update()
    {
        switch (_state)
        {
            case CameraState.Start:
                if (transform.localPosition.z < -10)
                    transform.localPosition += Vector3.forward * _moveSpeed;
                else
                    _state = CameraState.Idle;
                break;
            
            case CameraState.Idle:
                break;

            case CameraState.Depart:
                if (transform.localRotation != _departRotation)
                    transform.localRotation = Quaternion.RotateTowards(
                        transform.localRotation,
                        _departRotation,
                        _rotateSpeed
                    );
                else
                {
                    _state = CameraState.Moving;
                    Instantiate(
                        _departEffect,
                        transform.position + transform.forward * 3,
                        Quaternion.LookRotation(-transform.forward)
                    );
                }
                break;

            case CameraState.Moving:
                _camera.fieldOfView -= _fovSpeed * Time.deltaTime;
                _fovSpeed *= 1.05f;
                break;
        }
    }

    void Depart()
    {
        _state = CameraState.Depart;
    }
}
