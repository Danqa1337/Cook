using UnityEngine;
using UnityEngine.UI;

public class StoveView : MonoBehaviour
{
    [SerializeField] private Stove _stove;
    [SerializeField] private Scrollbar _scrollbar;

    private void OnEnable()
    {
        _scrollbar.onValueChanged.AddListener(ListenBar);
    }

    private void OnDisable()
    {
        _scrollbar.onValueChanged.RemoveListener(ListenBar);
    }

    public void ListenBar(float value)
    {
        _stove.ChangeTemperature(value);
    }

    private void Clear()
    {
        _scrollbar.value = 0;
    }
}