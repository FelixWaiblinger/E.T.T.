using System.Collections;
using UnityEngine;
using TMPro;

public abstract class Building : MonoBehaviour, ISelectable, IUpgradable
{
    [SerializeField] private BoolEventChannel _infoEvent;
    [SerializeField] private Transform _upgradeVisuals;
    [SerializeField] private GameObject _infoCanvas;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private GameObject _buildEffect;
    [SerializeField] private GameObject _upgradeEffect;
    private Outline _outline;
    private float _animScale = 0.02f;
    protected int _level = 0;
    protected Money _upgradeCost = new Money(0, 0);
    private bool _selected = false;

    #region SETUP

    void Awake()
    {
        _outline = GetComponent<Outline>();

        StartCoroutine(BuildAnimation());
        Instantiate(_buildEffect, transform.position + transform.up, transform.rotation);
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

    #endregion

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
        _upgradeVisuals.GetChild(_level - 1).gameObject.SetActive(true);
        _stats.text = this.name + "\nLvl. " + (_level + 1).ToString();
        Instantiate(_upgradeEffect, transform.position + transform.up, transform.rotation);
    }

    void ToggleInfo(bool active)
    {
        _infoCanvas.SetActive(active);
    }

    protected abstract BuildingInfo Information();
}
