using System.Collections;
using UnityEngine;
using TMPro;

public abstract class Building : MonoBehaviour, ISelectable, IUpgradable
{
    [SerializeField] private Transform _upgradeVisuals;
    [SerializeField] private VoidEventChannel _toggleInfoEvent;
    [SerializeField] private GameObject _infoCanvas;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private GameObject _buildEffect;
    [SerializeField] private GameObject _upgradeEffect;
    private Outline _outline;
    private float _animScale = 0.02f;
    protected int _level = 0;

    #region SETUP

    void Awake()
    {
        _outline = GetComponent<Outline>();

        StartCoroutine(BuildAnimation());
        Instantiate(_buildEffect, transform.position + transform.up, transform.rotation);
    }

    void OnEnable()
    {
        _toggleInfoEvent.VoidEventRaised += ToggleInfo;
    }

    void OnDisable()
    {
        _toggleInfoEvent.VoidEventRaised -= ToggleInfo;
    }

    void OnMouseEnter()
    {
        _outline.enabled = true;
    }

    void OnMouseExit()
    {
        _outline.enabled = false;
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
        Debug.Log("Selected");
    }

    public virtual void Upgrade()
    {
        if (_level == 10) return;

        _level++;
        _upgradeVisuals.GetChild(_level - 1).gameObject.SetActive(true);
        _stats.text = this.name + "\nLvl. " + (_level + 1).ToString();
        Instantiate(_upgradeEffect, transform.position + transform.up, transform.rotation);
    }

    void ToggleInfo()
    {
        _infoCanvas.SetActive(!_infoCanvas.activeSelf);
    }
}
