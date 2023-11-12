using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "Events/BoolEventChannel")]
public class BoolEventChannel : ScriptableObject
{
    public UnityAction<bool> BoolEventRaised;

    public void RaiseBoolEvent(bool arg)
    {
        BoolEventRaised?.Invoke(arg);
    }
}
