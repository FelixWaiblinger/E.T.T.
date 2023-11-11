using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private VoidEventChannel _menuEvent;
    [SerializeField] private VoidEventChannel _settingsEvent;
    [SerializeField] private AudioSource _source;
    private bool _paused = false;

    #region SETUP

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        _menuEvent.VoidEventRaised += TogglePause;
        _settingsEvent.VoidEventRaised += UpdateSettings;
    }

    void OnDisable()
    {
        _menuEvent.VoidEventRaised -= TogglePause;
        _settingsEvent.VoidEventRaised -= UpdateSettings;
    }

    #endregion

    void TogglePause()
    {
        _paused = !_paused;
        _source.volume = _paused ? _gameData.Volume * 0.8f : _gameData.Volume;
        _source.pitch = _paused ? 0.8f : 1f;
    }

    void UpdateSettings()
    {
        _source.volume = _gameData.Volume;
    }
}
