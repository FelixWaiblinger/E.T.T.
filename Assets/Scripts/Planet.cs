using UnityEngine;

public class Planet : MonoBehaviour, ISelectable
{
    public void Select()
    {
        Debug.Log("Planet selected");
    }
}
