using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEventChannel", menuName = "Events/FloatEventChannel")]
public class FloatEventChannel : ScriptableObject
{
    public UnityAction<float> FloatEventRaised;

    public void RaiseFloatEvent(float arg)
    {
        FloatEventRaised?.Invoke(arg);
    }
}
