using UnityEngine;

public class EffectView : MonoBehaviour
{
    [SerializeField] private EffectIcon _effectIcon;
    [SerializeField] private Canvas _canvas;

    private void OnEnable()
    {
        SuppliesHandler.OnSupplyShown += Draw;
        SuppliesHandler.OnSupplyHidden += Hide;
    }

    private void OnDisable()
    {
        SuppliesHandler.OnSupplyShown -= Draw;
        SuppliesHandler.OnSupplyHidden -= Hide;
    }

    private void Start()
    {
        Hide();
    }

    private void Show()
    {
        _canvas.enabled = true;
    }

    private void Hide()
    {
        _canvas.enabled = false;
    }

    private void Draw(SupplyName supplyName)
    {
        var supplyPrefab = SuppliesDatabase.GetSupplyPrefab(supplyName);

        var ingredientData = supplyPrefab.IngData;

        _effectIcon.Draw(ingredientData.effect, ingredientData.movement);

        Show();
    }
}