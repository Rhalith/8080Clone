using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    #region Other Components
    [SerializeField] private TMP_Text _scoreText;
    #endregion
    #region Variables
    private int _score;
    #endregion
    private void Start()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
    }
    public void AddScore(int score)
    {
        _score += score;
        _scoreText.text = _score.ToString();
    }
}
