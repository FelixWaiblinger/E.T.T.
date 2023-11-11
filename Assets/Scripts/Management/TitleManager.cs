using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _departEvent;

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
        
    }
}
