using UnityEngine;
using UnityEngine.UI;

public class SMenu : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private VoidEventChannel _titleEvent;
    [SerializeField] private VoidEventChannel _settingsEvent;
    [SerializeField] private Slider _volumeSlider;

    void Start()
    {
        _volumeSlider.value = _gameData.Volume;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        _settingsEvent.RaiseVoidEvent();
    }

    public void Exit()
    {
        _titleEvent.RaiseVoidEvent();
        _settingsEvent.RaiseVoidEvent();
    }

    public void UpdateVolume(float value)
    {
        _gameData.Volume = value;
    }
}
