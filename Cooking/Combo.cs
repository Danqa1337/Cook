using System;
using UnityEngine;

[Serializable]
public class Combo
{
    [SerializeField] public ComboName ComboName;
    [SerializeField] public Criteria Criteria;
    [SerializeField] public int Score;

    public Combo(ComboName comboName, Criteria criteria, int score)
    {
        ComboName = comboName;
        if (comboName != ComboName.Null)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            Score = score;
        }
    }

    public bool CheckCriteria(Dish dish)
    {
        return Criteria.Check(dish);
    }
}