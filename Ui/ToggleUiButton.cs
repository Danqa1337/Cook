using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleUiButton : MonoBehaviour
{
    [SerializeField] private UIName _uIName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Toggle);
    }

    private void Toggle()
    {
        UiManager.instance.ShowUI(_uIName);
    }
}