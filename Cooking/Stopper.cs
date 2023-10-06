public class Stopper : Device
{
    public override StaticIngredientData IngData => new StaticIngredientData() { effect = EffectName.Stop };

    public override void OnUse()
    {
        Player.instance.Stop();
        PlayerEffects.instance.Clear();
    }
}