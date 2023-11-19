using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public string Filename { get; private set; } = "Data";
    public int Level;
    public Money Money;
    public Money Target;
    public float Approval;
    public float Volume;
}
