using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _approvalText;
    [SerializeField] private Image _approvalBar;
    [SerializeField] private MoneyEventChannel _moneyEvent;
    [SerializeField] private FloatEventChannel _approvalEvent;
    private Money _money = new Money(10, 3); // 10k start money
    private float _approval = 0.5f;

    [SerializeField] private BuildingSO[] _buildingOptions;
    [SerializeField] private IntEventChannel _buildEvent;
    private Transform _allBuildings;

    [SerializeField] private Asteroid[] _asteroids;
    [SerializeField] private float _spawnTimer;
    private float _timer = 0;

    #region SETUP

    private void OnEnable()
    {
        _moneyEvent.MoneyEventRaised += UpdateMoney;
        _approvalEvent.FloatEventRaised += UpdateApproval;
        _buildEvent.IntEventRaised += Build;
    }

    private void OnDisable()
    {
        _moneyEvent.MoneyEventRaised -= UpdateMoney;
        _approvalEvent.FloatEventRaised -= UpdateApproval;
        _buildEvent.IntEventRaised -= Build;
    }
    
    void Start()
    {
        _allBuildings = GameObject.FindGameObjectWithTag("Buildings").transform;
        _moneyText.text = _money.ToString();
        _approvalText.text = (_approval * 100).ToString("0.0") + "%";
        _approvalBar.fillAmount = _approval;

    }

    #endregion

    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else
        {
            CreateAsteroid();
            _timer = _spawnTimer;
        }
    }

    void UpdateMoney(Money amount)
    {
        if (amount > new Money(0, 0))
            _money = _money + (amount * _approval);
        else if (_money > amount)
            _money = _money - amount;

        _moneyText.text = _money.ToString();
    }

    void UpdateApproval(float amount)
    {
        _approval = Mathf.Clamp01(_approval + amount);
        _approvalBar.fillAmount = _approval;
        _approvalText.text = (_approval * 100).ToString("0.0") + "%";
    }

    void Build(int option)
    {
        if (_money < -_buildingOptions[option].Cost)
        {
            Debug.Log("Cannot afford that");
            return;
        }
        else UpdateMoney(_buildingOptions[option].Cost);

        var mockup = GameObject.FindGameObjectWithTag("Mockup").transform;

        var building = Instantiate(
            _buildingOptions[option].Prefab,
            mockup.position,
            mockup.rotation
        );
        building.transform.SetParent(_allBuildings);
    }

    void CreateAsteroid()
    {
        var index = Random.Range(0, _asteroids.Length);
        var valueM = Random.Range(1f, 500f);
        var valueE = Random.Range(_money.Exponent() - 2, _money.Exponent());
        var start = Random.onUnitSphere * 25;
        var direction = -start; // TODO
        var speed = Random.Range(0.1f, 0.8f);

        var a = Instantiate(_asteroids[index], start, Quaternion.identity);
        a.Init(new Money(valueM, valueE), direction, speed);
    }
}
