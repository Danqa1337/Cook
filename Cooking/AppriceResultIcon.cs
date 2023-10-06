using TMPro;
using UnityEngine;

public class AppriceResultIcon : MonoBehaviour
{
    [SerializeField] private Star[] _stars;
    [SerializeField] private TextMeshProUGUI _finalResultText;

    public void Draw(AppriceResult appriceResult)
    {
        Clear();

        if (appriceResult != AppriceResult.Null)
        {
            _finalResultText.text = appriceResult.FinalValue.ToString();
            for (int i = 0; i < appriceResult.CurrentStars; i++)
            {
                _stars[i].Draw(true);
            }
        }
        else
        {
            _finalResultText.text = "???";
        }
    }

    public void Clear()
    {
        foreach (var item in _stars)
        {
            item.Draw(false);
        }
    }
}