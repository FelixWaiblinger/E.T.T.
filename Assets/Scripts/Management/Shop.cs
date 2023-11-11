using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private IntEventChannel _shopSelectEvent;
    [SerializeField] private Button[] _shopOptions;
    [SerializeField] private BuildingSO[] _buildingOptions;

    private void Awake()
    {
        for (int i = 0; i < _shopOptions.Length; i++)
        {
            var t = _shopOptions[i].transform;
            // icon
            t.GetChild(0).GetComponent<Image>().sprite = _buildingOptions[i].Icon;
            // name
            t.GetChild(1).GetComponent<TMP_Text>().text = _buildingOptions[i].Name;
            // cost
            t.GetChild(2).GetComponent<TMP_Text>().text = _buildingOptions[i].Cost.ToString();
        }
    }

    public void SelectOption(int option)
    {
        _shopSelectEvent.RaiseIntEvent(option);
    }
}
