using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _remainingTimeText;
    [SerializeField] private Image _startButtonIcon;
    [SerializeField] private Image _retryButtonIcon;
    [SerializeField] private Image _gameOverIcon;
    [SerializeField] private Image _successIcon;

    void Start()
    {
        
    }

    void Update()
    {
        _pointsText.text = _gameManager.PointsCounter.ToString();
    }

    public void ShowStartUi()
    {
        _startButtonIcon.gameObject.SetActive(true);
    }

    public void HideStartUi()
    {
        _startButtonIcon.gameObject.SetActive(false);
    }

    public void ShowInGameUi()
    {
        _startButtonIcon.gameObject.SetActive(false);
        _gameOverIcon.gameObject.SetActive(false);
        _retryButtonIcon.gameObject.SetActive(false);
        _pointsText.gameObject.SetActive(true);
        _remainingTimeText.gameObject.SetActive(true);
    }

    public void HideInGameUi()
    {
        _pointsText.gameObject.SetActive(false);
        _remainingTimeText.gameObject.SetActive(false);
    }

    public void HideGameOverUi()
    {
        _gameOverIcon.gameObject.SetActive(false);
        _retryButtonIcon.gameObject.SetActive(false);
    }


    public void ShowGameOverUi()
    {
        _gameOverIcon.gameObject.SetActive(true);
        _retryButtonIcon.gameObject.SetActive(true);
    }

    public string FormatTime(float timeSec)
    {
        string result = "";
        int minutes = (int)timeSec / 60;
        result += minutes.ToString() + ":";
        int seconds = (int)timeSec - minutes * 60;
        if (seconds < 10)
        {
            result += "0";
        }
        result += seconds.ToString();
        return result;
    }

    public void SetRemainingTime(float time)
    {
        if (time <= 0)
            _remainingTimeText.text = FormatTime(0);
        else
            _remainingTimeText.text = FormatTime(time);
    }
}
