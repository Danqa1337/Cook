using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using DG.Tweening;

public class EffectIcon : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private SpriteAtlas _iconsAtlas;
    [SerializeField] private Image _lifeTimeBar;

    public void Draw(EffectName effect, Vector2 vector)
    {
        var sprite = _iconsAtlas.GetSprite(effect.ToString());
        _iconImage.sprite = sprite;
        _iconImage.transform.rotation = Quaternion.LookRotation(Vector3.forward, vector);
        _lifeTimeBar.fillAmount = 1;
    }

    public void Draw(Effect effect)
    {
        transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f);
        var sprite = _iconsAtlas.GetSprite(effect.EffectName.ToString());
        _iconImage.sprite = sprite;

        effect.OnLifeTimeChanged += OnLifeTimeChanged;
        effect.OnEnded += OnEnded;

        void OnLifeTimeChanged(float lifeTime, float maxLifetime)
        {
            var fillAmount = lifeTime >= 0 ? Mathf.Clamp01(lifeTime / maxLifetime) : 1;
            _lifeTimeBar.fillAmount = fillAmount;
        }
        void OnEnded()
        {
            Destroy(gameObject);
        }
    }
}