using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "CombosDatabase", menuName = "Databases/CombosDatabase")]
public class CombosDatabase : Database<CombosDatabase, CombosTable, CombosTable.Param, Combo>
{
    protected override string enumName => "ComboName";

    [SerializeField]
    public SerializableDictionary<ComboName, Combo> _combos = new SerializableDictionary<ComboName, Combo>();

    public override void StartUp()
    {
    }

    protected override void StartReimport()
    {
        base.StartReimport();
        _combos = new SerializableDictionary<ComboName, Combo>();
    }

    protected override void ProcessParam(CombosTable.Param param)
    {
        var comboName = param.enumName.DecodeCharSeparatedEnumsAndGetFirst<ComboName>();
        var ingredients = CriteriaDecoder.GetCriteria(param.ingredients);
        _combos.Add(comboName, new Combo(comboName, ingredients, param.score));
    }

    public static Combo GetCombo(ComboName comboName)
    {
        return instance._combos[comboName];
    }

    public static Combo[] GetAllCombos()
    {
        return instance._combos.Values.ToArray();
    }

    public static Sprite GetIcon(ComboName comboName)
    {
        var icon = Addressables.LoadAssetAsync<SpriteAtlas>("Assets/Sprites/ComboIcons/ComboIcons.spriteatlasv2").WaitForCompletion().GetSprite(comboName + "ComboIcon");
        return icon;
    }
}