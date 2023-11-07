using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
public class BuildingSO : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public Money Cost;
    public Building Prefab;
}
