using UnityEngine;

public class PlayerEffectsView : MonoBehaviour
{
    [SerializeField] private EffectIcon _effectIconPrefab;
    [SerializeField] private Transform _layout;

    private void OnEnable()
    {
        PlayerEffects.OnEffectAdded += OnEffectAdded;
    }

    private void OnDisable()
    {
        PlayerEffects.OnEffectRemoved += OnEffectRemoved;
    }

    private void OnEffectAdded(Effect effect)
    {
        var icon = Instantiate(_effectIconPrefab, _layout);
        icon.Draw(effect);
    }

    private void OnEffectRemoved(Effect effect)
    {
    }

    private void Clear()
    {
    }
}