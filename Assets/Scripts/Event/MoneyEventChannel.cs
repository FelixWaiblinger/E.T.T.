using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoneyEventChannel", menuName = "Events/MoneyEventChannel")]
public class MoneyEventChannel : ScriptableObject
{
    public UnityAction<Money> MoneyEventRaised;

    public void RaiseMoneyEvent(Money arg)
    {
        MoneyEventRaised?.Invoke(arg);
    }
}
