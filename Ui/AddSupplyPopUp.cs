using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddSupplyPopUp : MovingPopup
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;

    public void Draw(SupplyName supplyName, int count)
    {
        _iconImage.sprite = SuppliesDatabase.GetIcon(supplyName);
        _countText.text = "+" + count.ToString();
    }
}