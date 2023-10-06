using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Label _scoreLabel;
    private float _score;

    private void Start()
    {
        _scoreLabel.SetValue(_score);
    }

    private void AddScore(float score)
    {
        _score += score;
        _scoreLabel.SetValue(_score);
    }
}