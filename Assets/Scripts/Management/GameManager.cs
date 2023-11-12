using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;

    [Header("Events")]
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private FloatEventChannel _approvalEvent;
    [SerializeField] private IntEventChannel _buildEvent;
    [SerializeField] private VoidEventChannel _departEvent;

    [Header("UI")]
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _approvalText;
    [SerializeField] private Image _approvalBar;
    [SerializeField] private GameObject _departButton;
    private Money _money, _targetMoney;
    private float _approval = 0.5f;

    [Header("Planet")]
    [SerializeField] private Transform _planet;
    [SerializeField] private GameObject[] _planetTypes;
    private List<GameObject> _moons = new List<GameObject>();

    [Header("Building")]
    [SerializeField] private GameObject _denyEffect;
    [SerializeField] private BuildingSO[] _buildingOptions;
    private Transform _allBuildings;

    [Header("Asteroids")]
    [SerializeField] private float _spawnTimer;
    [SerializeField] private Asteroid[] _asteroids;
    private float _timer;

    #region SETUP

    void Awake()
    {
        ReaderWriterJSON.LoadFromJSON<GameData>(ref _gameData);

        var index = Random.Range(0, _planetTypes.Length);
        var numMoons = Mathf.Clamp(_gameData.Level, 0, 6);

        var p = Instantiate(_planetTypes[index], _planet);
        p.transform.localScale = new(8, 8, 8);

        for (int i = 0; i < numMoons; i++)
        {
            var idx = Random.Range(0, _planetTypes.Length);
            var moon = Instantiate(_planetTypes[idx], _planet);
            moon.AddComponent(typeof(Moon));
            moon.transform.position = Random.onUnitSphere * 15f;
            moon.transform.localScale = new(2, 2, 2);
            _moons.Add(moon);
        }

        _money = _gameData.Money;
        _targetMoney = new Money(1f, _money.Exponent() + 6);
        _approval = _gameData.Approval;
    }

    private void OnEnable()
    {
        _moneyEvent.MoneyEventRaised += UpdateMoney;
        _approvalEvent.FloatEventRaised += UpdateApproval;
        _buildEvent.IntEventRaised += Build;
        _departEvent.VoidEventRaised += Depart;
    }

    private void OnDisable()
    {
        _moneyEvent.MoneyEventRaised -= UpdateMoney;
        _approvalEvent.FloatEventRaised -= UpdateApproval;
        _buildEvent.IntEventRaised -= Build;
        _departEvent.VoidEventRaised -= Depart;
    }
    
    void Start()
    {
        _allBuildings = GameObject.FindGameObjectWithTag("Buildings").transform;
        _moneyText.text = _money.ToString();
        _approvalText.text = (_approval * 100).ToString("0.0") + "%";
        _approvalBar.fillAmount = _approval;
        _timer = _spawnTimer;
    }

    #endregion

    void Update()
    {
        CreateAsteroid();
    }

    #region EVENT

    void UpdateMoney(Money amount)
    {
        if (amount > new Money(0, 0))
            _money = _money + (amount * _approval);
        else if (_money > amount)
            _money = _money - amount;

        _moneyText.text = _money.ToString();

        if (_money > _targetMoney) _departButton.SetActive(true);
    }

    void UpdateApproval(float amount)
    {
        _approval = Mathf.Clamp01(_approval + amount);
        _approvalBar.fillAmount = _approval;
        _approvalText.text = (_approval * 100).ToString("0.0") + "%";
    }

    void Build(int option)
    {
        var mockup = GameObject.FindGameObjectWithTag("Mockup").transform;

        if (_money < -_buildingOptions[option].Cost)
        {
            Instantiate(
                _denyEffect,
                mockup.transform.position + mockup.transform.up,
                mockup.transform.rotation
            );

            return;
        }
        else UpdateMoney(_buildingOptions[option].Cost);


        var building = Instantiate(
            _buildingOptions[option].Prefab,
            mockup.position,
            mockup.rotation
        );
        building.transform.SetParent(_allBuildings);
    }

    void Depart()
    {
        ReaderWriterJSON.SaveToJSON(_gameData);
    }

    #endregion

    #region HELPER

    void CreateAsteroid()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        var index = Random.Range(0, _asteroids.Length);
        var speed = Random.Range(3f, 8f);
        var start = Random.onUnitSphere * 25;
        var valueM = Mathf.Clamp(
            Random.Range(_money.Mantissa() * 0.2f, _money.Mantissa()),
            1f,
            999f
        );
        var valueE = Mathf.Clamp(
            Random.Range(_money.Exponent() - 2, _money.Exponent() + (int)(speed / 4)),
            0,
            int.MaxValue
        );

        var a = Instantiate(_asteroids[index], start, Quaternion.LookRotation(-start));
        a.transform.Rotate(new(10f, 10f, 0f), Space.Self);
        a.Init(new Money(valueM, valueE), a.transform.forward, speed);

        _timer = _spawnTimer;
    }

    #endregion
}
