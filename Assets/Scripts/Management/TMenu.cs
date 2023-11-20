using System.Collections;
using UnityEngine;

public class TMenu : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private GameObject _sMenu;
    [SerializeField] private RectTransform _transition;
    [SerializeField] private float _transitionSpeed;

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator Depart()
    {
        yield return new WaitForSeconds(1.3f);

        _transition.gameObject.SetActive(true);
        var speed = _transitionSpeed;

        while (_transition.localScale.magnitude < 1000)
        {
            _transition.localScale += Vector3.one * speed * Time.deltaTime;
            speed *= 1.1f;
            yield return null;
        }
    }

    public void Play()
    {
        _departEvent.RaiseVoidEvent();

        StartCoroutine(Depart());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        _sMenu.SetActive(true);
    }
}
