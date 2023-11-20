using UnityEngine;
using UnityEngine.UI;

public class SMenu : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private VoidEventChannel _titleEvent;
    [SerializeField] private VoidEventChannel _settingsEvent;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Image _resetBar;
    private float _resetTimer = 0;
    private bool _reset = false;

    void Start()
    {
        _volumeSlider.value = _gameData.Volume;
    }

    void Update()
    {
        if (!_reset) return;

        _resetTimer += Time.deltaTime;
        _resetBar.fillAmount = _resetTimer / 3f;

        if (_resetTimer > 3f)
        {
            ResetGameData();
            StopReset();
        }
    }

    #region BUTTON

    public void Resume()
    {
        gameObject.SetActive(false);
        _settingsEvent.RaiseVoidEvent();
    }

    public void Exit()
    {
        _settingsEvent.RaiseVoidEvent();
        _titleEvent.RaiseVoidEvent();
    }

    public void UpdateVolume(float value)
    {
        _gameData.Volume = value;
    }

    public void StartReset()
    {
        _reset = true;
    }

    public void StopReset()
    {
        _reset = false;
        _resetTimer = 0;
    }

    #endregion

    void ResetGameData()
    {
        _gameData.Level = 0;
        _gameData.Money = new Money(100, 3);
        _gameData.Approval = 0f;
        _gameData.Volume = 0.5f;
        _gameData.Target = new Money(1, 12);

        ReaderWriterJSON.SaveToJSON(_gameData);
    }
}
