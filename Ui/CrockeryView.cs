using UnityEngine;

public class CrockeryView : MonoBehaviour
{
    private void OnEnable()
    {
        Kitchen.instance.ActiveCrockery.OnChunkTooBig += ShowPopup;
    }

    private void OnDisable()
    {
        Kitchen.instance.ActiveCrockery.OnChunkTooBig -= ShowPopup;
    }

    private void ShowPopup(Ingredient ingredient)
    {
    }
}