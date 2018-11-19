using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManegerToScore : MonoBehaviour
{
    public TextMesh TextScore;
    public TextMesh YourScore;
    public TextMesh YourTime;
    public TextMesh NeedScore;
    public TextMesh TextLoseOrWin;
    private int _textToManager = 0;
    public int MinTime = 10;
    public int MaxTime = 30;
    public int MinScore = 500;
    public int MaxScore = 1001;
    public int StepTime = 5;
    public int StepScore = 50;
    private float _timeToLose = 0;
    private int _scoreToWin = 0;
    public GameObject WinPanel;
    public bool IsOwer = false;
    private string _textWin = "Congratulations! \n \t You won!";
    private string _textLose = "Oops!You Lose! \n \t  Play again?";


    void Start ()
    {
        GetRandomTimeAndScore();
        NeedScore.text = _scoreToWin.ToString();
    }
	
	void Update ()
    {
        if (IsOwer == false)
        {
            ScoreTime();
        }
    }

    void GetRandomTimeAndScore()
    {
        int _ScoreToWinRandGeneric = Random.Range(MinScore, MaxScore);
        float _ScoreSteps = Mathf.Floor(_ScoreToWinRandGeneric / StepScore);
        _scoreToWin = (int)_ScoreSteps * StepScore;
        //550
        //_ScoreSteps =(550 / 50)
        int _timeToLoseRandGeneric = Random.Range(MinTime, MaxTime);
        float _TimeSteps = Mathf.Floor(_timeToLoseRandGeneric / StepTime);
        _timeToLose = _TimeSteps * StepTime;
    }

    private void ScoreTime()
    {
        _timeToLose -= Time.deltaTime;
        float resTime = ((int)(_timeToLose * 100)) / 100;
        if (_textToManager < _scoreToWin && _timeToLose >= 0)
        {
            TextScore.text = _textToManager.ToString();
            YourTime.text = resTime.ToString();
        }
        else
        {
            IsOwer = true;
            YourScore.text = _textToManager.ToString();
            WinPanel.gameObject.SetActive(true);
            if(_timeToLose <= 0)
            {
                TextLoseOrWin.text = _textLose;
            }
            if(_textToManager >= _scoreToWin)
            {
                TextLoseOrWin.text = _textWin;
            }
        }
    }

    public void ManagerScore(int ScoreToCount)
    {
        _textToManager = ScoreToCount;
    }
}
