using System.Collections;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    [SerializeField] private VoidEventChannel _buildEvent;
    [SerializeField] private GameObject _buildEffect;

    private int _level = 0;

    IEnumerator BuildAnimation()
    {
        SetOpaqueness(1);

        var scale = transform.localScale;

        for (int i = 0; i < 30; i++)
        {
            transform.localScale += Vector3.right * 0.01f;
            transform.localScale -= Vector3.up * 0.01f;
            transform.localScale += Vector3.forward * 0.01f;
            yield return null;
        }

        for (int i = 0; i < 30; i++)
        {
            transform.localScale -= Vector3.right * 0.01f;
            transform.localScale += Vector3.up * 0.01f;
            transform.localScale -= Vector3.forward * 0.01f;
            yield return null;
        }

        transform.localScale = scale;
    }

    public void Select()
    {
        Debug.Log("Selected");
    }

    public void Build(Transform parent)
    {
        transform.SetParent(parent);

        StartCoroutine(BuildAnimation());
        Instantiate(_buildEffect, transform.position, transform.rotation);
        _buildEvent.RaiseVoidEvent();
    }

    #region HELPER

    void SetOpaqueness(float value)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i)
                     .GetComponent<MeshRenderer>()
                     .material
                     .color += new Color(0, 0, 0, value);
    }

    #endregion
}
