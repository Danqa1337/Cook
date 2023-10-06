using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboView : MonoBehaviour
{
    [SerializeField] private ComboIcon _comboIconPrefab;

    private void OnEnable()
    {
        Kitchen.instance.ActiveCrockery.OnComboActivated += DrawCombo;
    }

    private void OnDisable()
    {
        Kitchen.instance.ActiveCrockery.OnComboActivated -= DrawCombo;
    }

    private void DrawCombo(Combo combo)
    {
        Debug.Log("Combo " + combo.ComboName + " Activated!");
        var icon = Instantiate(_comboIconPrefab.gameObject, transform).GetComponent<ComboIcon>();
        icon.DrawCombo(combo.ComboName, true);
        icon.transform.DOShakeScale(1).OnComplete(delegate { icon.transform.DOMove(transform.position, 2).OnComplete(delegate { Destroy(icon.gameObject); }); });
    }
}

public static class TransformExtensions
{
    public static Tweener DoWait(this Transform transform, float time)
    {
        return transform.DOMove(transform.position, time);
    }
}