using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _departEvent;
    [SerializeField] private VoidEventChannel _titleEvent;
    [SerializeField] private float _sceneChangeDelay;
    private float _timer = 0;
    private int _indexToLoad;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _departEvent.VoidEventRaised += () => ChangeScene(1);
        _titleEvent.VoidEventRaised += () => ChangeScene(0);
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
                UnityEngine.SceneManagement.SceneManager.LoadScene(_indexToLoad);
        }
    }

    void ChangeScene(int index)
    {
        _timer = _sceneChangeDelay;
        _indexToLoad = index;
    }
}
