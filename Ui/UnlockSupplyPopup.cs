using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSupplyPopup : MovingPopup
{
    [SerializeField] private Image _icon;

    public void Draw(SupplyName supplyName)
    {
        _icon.sprite = SuppliesDatabase.GetIcon(supplyName);
    }

    public override Sequence Launch()
    {
        var transform = GetComponent<RectTransform>();
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(delegate { _icon.color = Color.black; });
        sequence.AppendCallback(delegate { transform.DORotate(new Vector3(0, 0, 360) * 5, 0.5f).From(); });
        sequence.Append(transform.DOScale(0, 0.5f).From());
        sequence.Append(transform.DOShakeScale(0.3f));
        sequence.AppendInterval(0.1f);
        sequence.AppendCallback(delegate { _icon.color = Color.white; });
        sequence.AppendInterval(0.8f);
        sequence.Append(transform.DOScale(0, 0.5f));
        return sequence;
    }
}