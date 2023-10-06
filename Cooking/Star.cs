using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private Image _complitedStar;
    public void Draw(bool value)
    {
        _complitedStar.enabled = value;
    }
}