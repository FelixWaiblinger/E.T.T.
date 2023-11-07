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
    private Money _money = new Money(10, 4);
    private float _approval = 1;

    [SerializeField] private BuildingSO[] _buildingOptions;
    [SerializeField] private IntEventChannel _buildEvent;
    private Transform _allBuildings;

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
        
    }

    void UpdateMoney(Money amount)
    {
        if (amount > new Money(0, 0))
            _money = _money + amount;
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
        var mockup = GameObject.FindGameObjectWithTag("Mockup").transform;

        var building = Instantiate(
            _buildingOptions[option].Prefab,
            mockup.position,
            mockup.rotation
        );
        building.transform.SetParent(_allBuildings);
    }
}
