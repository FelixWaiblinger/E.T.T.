using UnityEngine;

public class Mockup : MonoBehaviour
{
    public Material Clear;
    public Material Blocked;
    private bool _clear;

    public void SetClear(bool clear)
    {
        if (_clear == clear) return;

        _clear = clear;
        var mat = _clear ? Clear : Blocked;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
    }
}
