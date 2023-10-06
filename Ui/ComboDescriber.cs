using UnityEngine;

public class ComboDescriber : MonoBehaviour
{
    [SerializeField] private Label _nameLabel;
    [SerializeField] private IngredientsListUI _ingredientsList;

    private void OnEnable()
    {
        CombosScreen.OnIconClicked += Describe;
    }

    private void OnDisable()
    {
        CombosScreen.OnIconClicked -= Describe;
    }

    private void Describe(ComboName comboName)
    {
        if (comboName != ComboName.Null && DataHolder.CurrentData.OpenedCombos.Contains(comboName))
        {
            var combo = CombosDatabase.GetCombo(comboName);
            _nameLabel.SetText(comboName.ToString());
            _ingredientsList.DrawIngredients(combo.Criteria.Ingredients);
        }
        else
        {
            _nameLabel.SetText("???");
        }
    }
}