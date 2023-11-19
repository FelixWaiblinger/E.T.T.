using System.Collections;
using UnityEngine;
using TMPro;

public abstract class Building : MonoBehaviour, ISelectable, IUpgradable
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private BoolEventChannel _infoEvent;
    [SerializeField] private Transform _upgradeVisuals;
    [SerializeField] private GameObject _infoCanvas;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private GameObject _buildEffect;
    [SerializeField] private GameObject _upgradeEffect;
    private Outline _outline;
    private float _animScale = 0.02f;
    protected int _level = 1;
    public Money UpgradeCost { get; protected set; }
    private bool _selected = false;

    #region SETUP

    void Awake()
    {
        _outline = GetComponent<Outline>();

        StartCoroutine(BuildAnimation());
        Instantiate(_buildEffect, transform.position + transform.up, transform.rotation);
        
        ComputeUpgradeCost();
    }

    void OnEnable()
    {
        _infoEvent.BoolEventRaised += ToggleInfo;
    }

    void OnDisable()
    {
        _infoEvent.BoolEventRaised -= ToggleInfo;
    }

    void OnMouseEnter()
    {
        _outline.enabled = _selected || true;
    }

    void OnMouseExit()
    {
        _outline.enabled = _selected || false;
    }

    protected abstract BuildingInfo Information();

    IEnumerator BuildAnimation()
    {
        var scale = transform.localScale;

        for (int i = 0; i < 50; i++)
        {
            transform.localScale += Vector3.right * _animScale;
            transform.localScale -= Vector3.up * _animScale;
            transform.localScale += Vector3.forward * _animScale;
            yield return null;
        }

        for (int i = 0; i < 50; i++)
        {
            transform.localScale -= Vector3.right * _animScale;
            transform.localScale += Vector3.up * _animScale;
            transform.localScale -= Vector3.forward * _animScale;
            yield return null;
        }

        transform.localScale = scale;
    }

    #endregion

    public void Select()
    {
        _selected = true;
        _outline.enabled = true;
        GameObject.FindGameObjectWithTag("Info")
                  .GetComponent<RMenu>()
                  .Show(Information());
    }

    public void Deselect()
    {
        _selected = false;
        _outline.enabled = false;
        GameObject.FindGameObjectWithTag("Info")
                  .GetComponent<RMenu>()
                  .Hide();
    }

    public virtual void Upgrade()
    {
        if (_level == 10) return;

        _level++;
        _upgradeVisuals.GetChild(_level - 2).gameObject.SetActive(true);
        ComputeUpgradeCost();

        // UI
        _stats.text = this.ToString() + "\nLvl. " + _level.ToString();
        GameObject.FindGameObjectWithTag("Info").GetComponent<RMenu>().Show(Information());

        // effects
        Instantiate(_upgradeEffect, transform.position + transform.up, transform.rotation);
    }

    public override string ToString()
    {
        return this.GetType().ToString();
    }

    #region HELPER

    void ComputeUpgradeCost()
    {
        var m = 10f * Mathf.Pow(3f, (_level < 6 ? _level : _level - 5) - 1);
        var e = (_gameData.Target.Exponent() - 6) + (_level / 5) * 3;
        UpgradeCost = new Money(m, e);
    }

    void ToggleInfo(bool active)
    {
        _infoCanvas.SetActive(active);
    }

    #endregion

}
