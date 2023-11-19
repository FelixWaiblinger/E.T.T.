using UnityEngine;

public class Planet : MonoBehaviour // , ISelectable
{
    private Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
    }

    void OnMouseEnter()
    {
        Debug.Log("Enter");
        _outline.enabled = true;
    }

    void OnMouseExit()
    {
        Debug.Log("Exit");
        _outline.enabled = false;
    }

    public void Select()
    {
        Debug.Log("Planet selected");
    }
}
