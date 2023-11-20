using UnityEngine;

public class Accelerator : Building
{
    [SerializeField] private float _factor;

    void Start()
    {
        Info = new BuildingInfo(
            this.ToString(),
            _level.ToString(),
            (_factor * 100).ToString("0.0") + "%",
            UpgradeCost.ToString(),
            transform.GetSiblingIndex()
        );
    }

    void Update()
    {
        
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}
