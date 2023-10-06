using System.Collections;
using UnityEngine;

public class Grain : Ingredient
{
    public override float Mass => 0.3f;

    public override void OnRejected(Crockery crockery)
    {
        base.OnRejected(crockery);
        Destroy(gameObject);
    }
}