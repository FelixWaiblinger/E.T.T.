using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private FloatEventChannel _timeEvent;
    private float _factor;
    private bool _forward = false;

    void OnEnable()
    {
        _timeEvent.FloatEventRaised += UpdateFactor;
    }
    
    void OnDisable()
    {
        _timeEvent.FloatEventRaised -= UpdateFactor;
    }

    void UpdateFactor(float time)
    {
        _factor = time;
        UpdateTimeScale();
    }

    public void UpdateTimeScale()
    {
        Time.timeScale *= _forward ? _factor : 1 / _factor;
    }
}
