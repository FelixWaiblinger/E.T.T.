using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private VoidEventChannel _titleEvent;
    [SerializeField] private VoidEventChannel _settingsEvent;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _music;

    #region SETUP

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        _titleEvent.VoidEventRaised += () => PlayMusic(0);
        _departEvent.VoidEventRaised += () => PlayMusic(Random.Range(1, _music.Length));
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

    void PlayMusic(int index)
    {
        _source.clip = _music[index];
        _source.Play();
    }
}
