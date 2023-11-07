using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Events/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public UnityAction VoidEventRaised;

    public void RaiseVoidEvent()
    {
        VoidEventRaised?.Invoke();
    }
}
