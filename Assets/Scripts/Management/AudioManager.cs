using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private VoidEventChannel _settingsEvent;
    [SerializeField] private AudioSource _source;

    #region SETUP

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        _settingsEvent.VoidEventRaised += UpdateVolume;
    }

    void OnDisable()
    {
        _settingsEvent.VoidEventRaised -= UpdateVolume;
    }

    #endregion

    void UpdateVolume()
    {
        _source.volume = _gameData.Volume;
    }
}
