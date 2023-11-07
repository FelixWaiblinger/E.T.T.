using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "Events/IntEventChannel")]
public class IntEventChannel : ScriptableObject
{
    public UnityAction<int> IntEventRaised;

    public void RaiseIntEvent(int arg)
    {
        IntEventRaised?.Invoke(arg);
    }
}
