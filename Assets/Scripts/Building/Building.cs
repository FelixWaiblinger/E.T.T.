using System.Collections;
using UnityEngine;
using TMPro;

public abstract class Building : MonoBehaviour, ISelectable, IUpgradable
{
    public BuildingInfo Info { get; protected set; }
    public Money UpgradeCost { get; protected set; }

    [SerializeField] private GameData _gameData;
    [SerializeField] private BuildingSO _buildData;
    [SerializeField] private BoolEventChannel _infoEvent;
    [SerializeField] private Transform _upgradeVisuals;
    [SerializeField] private GameObject _infoCanvas;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private GameObject _buildEffect;
    [SerializeField] private GameObject _upgradeEffect;
    [SerializeField] private AudioClip _buildSound;
    [SerializeField] private AudioClip _upgradeSound;
    [SerializeField] private AudioClip _removeSound;
    private Outline _outline;
    private float _animScale = 0.02f;
    protected int _level = 1;
    private bool _selected = false;

    #region SETUP

    void Awake()
    {
        _outline = GetComponent<Outline>();

        StartCoroutine(BuildAnimation());

        Instantiate(
            _buildEffect,
            transform.position + transform.up,
            transform.rotation
        );

        AudioSource.PlayClipAtPoint(
            _buildSound,
            transform.position,
            _gameData.Volume
        );
        
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

    void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(
            _removeSound,
            transform.position,
            _gameData.Volume
        );
    }

    IEnumerator BuildAnimation()
    {
        var scale = transform.localScale;

        for (int i = 0; i < 15; i++)
        {
            transform.localScale += Vector3.right * _animScale;
            transform.localScale -= Vector3.up * _animScale;
            transform.localScale += Vector3.forward * _animScale;
            yield return null;
        }

        for (int i = 0; i < 15; i++)
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
                  .Show(Info);
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

        // effects
        Instantiate(
            _upgradeEffect,
            transform.position + transform.up,
            transform.rotation
        );
        
        AudioSource.PlayClipAtPoint(
            _upgradeSound,
            transform.position,
            _gameData.Volume
        );
    }

    public override string ToString()
    {
        return this.GetType().ToString();
    }

    #region HELPER

    protected void CreateInfo(string buildingSpecificInfo)
    {
        Info = new BuildingInfo(
            this.ToString(),
            _level.ToString(),
            buildingSpecificInfo,
            UpgradeCost.ToString(),
            transform.GetSiblingIndex()
        );
    }

    void ComputeUpgradeCost()
    {
        var m = _buildData.Cost.Mantissa() * Mathf.Pow(3f, (_level < 6 ? _level : _level - 5) - 1);
        var e = (_gameData.Target.Exponent() - 6) + (_level / 5) * 3;
        UpgradeCost = new Money(m, e);
    }

    void ToggleInfo(bool active)
    {
        _infoCanvas.SetActive(active);
    }

    #endregion

}
