using System;

[Serializable]
public class AppriceResult
{
    public readonly int CurrentStars;
    public readonly int MaxStars;
    public readonly int DishScore;
    public readonly float Distance;
    public readonly int FinalValue;

    public static AppriceResult Null;

    public AppriceResult(int currentStars, int maxStars, int dishScore, float distance, int finalValue)
    {
        CurrentStars = currentStars;
        MaxStars = maxStars;
        DishScore = dishScore;
        Distance = distance;
        FinalValue = finalValue;
    }
}
