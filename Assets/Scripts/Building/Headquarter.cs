using UnityEngine;

public class Headquarter : Building
{
    [SerializeField] private float _boost;
    
    void Start()
    {
        Info = new BuildingInfo(
            this.ToString(),
            _level.ToString(),
            (_boost * 100).ToString("0.0") + "%",
            UpgradeCost.ToString(),
            transform.GetSiblingIndex()
        );
    }

    void Update()
    {
        
    }
}
