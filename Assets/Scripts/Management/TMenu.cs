using UnityEngine;

public class TMenu : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private GameObject _sMenu;

    public void Play()
    {
        _departEvent.RaiseVoidEvent();
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
