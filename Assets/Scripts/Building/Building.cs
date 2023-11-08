using System.Collections;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject _buildEffect;
    private Outline _outline;
    private float _animScale = 0.02f;
    protected int _level = 0;

    void Awake()
    {
        _outline = GetComponent<Outline>();

        StartCoroutine(BuildAnimation());
        Instantiate(_buildEffect, transform.position + transform.up, transform.rotation);
    }

    IEnumerator BuildAnimation()
    {
        SetOpaqueness(1);

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
        _outline.enabled = true;
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
