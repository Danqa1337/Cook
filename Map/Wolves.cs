using System.Collections;
using UnityEngine.U2D;

public class Wolves : MapObjectComponent
{
    protected override void OnPlayerEnter()
    {
        Player.instance.Die();
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}