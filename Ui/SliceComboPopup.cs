using DG.Tweening;
using TMPro;
using UnityEngine;

public class SliceComboPopup : MovingPopup
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private int _maxCombo = 6;
    private bool _active = true;

    private void OnEnable()
    {
        SliceComboManager.OnComboEnded += OnComboEnded;
        SliceComboManager.OnComboIncreased += OnCombcoIncreased;
    }

    private void OnDisable()
    {
        SliceComboManager.OnComboEnded -= OnComboEnded;
        SliceComboManager.OnComboIncreased -= OnCombcoIncreased;
    }

    private void OnCombcoIncreased(int value, Vector2 position)
    {
        if (_active)
        {
            Draw(value, position);
        }
    }

    private void OnComboEnded(int value)
    {
        if (_active)
        {
            _active = false;
            var sequence = DOTween.Sequence();
            sequence.Append(RectTransform.DOAnchorPosY(Screen.height, 0.5f));
            sequence.AppendCallback(delegate { Destroy(gameObject); });
            sequence.Play();
        }
    }

    public void Draw(int value, Vector2 position)
    {
        RectTransform.anchoredPosition = position / transform.lossyScale;
        RectTransform.localScale = Vector3.one;
        RectTransform.DOKill();
        RectTransform.DOPunchScale(RectTransform.localScale * 2, 0.2f);
        _text.color = _gradient.Evaluate((float)value / (float)_maxCombo);
        _text.text = "X" + value.ToString();
    }
}