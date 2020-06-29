using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private TextMeshProUGUI _pointsText;

    [SerializeField]
    private TextMeshProUGUI _remainingTimeText;

    [SerializeField]
    private TextMeshProUGUI _tapCounter;

    [SerializeField]
    private Image _startButtonIcon;

    [SerializeField]
    private Image _retryButtonIcon;

    [SerializeField]
    private Image _nextButtonIcon;

    [SerializeField]
    private Image _resetButtonIcon;

    [SerializeField]
    private Image _gameOverIcon;

    [SerializeField]
    private Image _successIcon;

    [SerializeField]
    private TextMeshProUGUI _result;

    [SerializeField]
    private TextMeshProUGUI _stageNumber;

    [SerializeField]
    private GameObject _scrollArea;

    [SerializeField]
    private GameObject _pointRecords;

    [SerializeField]
    private GameObject _scrollBar;

    [SerializeField]
    private GameObject _pointRecordPrefab;

    [SerializeField]
    private int _scrollBarMinAmount = 3;

    void Update()
    {
        _pointsText.text = _gameManager.PointsCounter.ToString();
        if (_gameManager.AntiStunTapCounter > -1 && _tapCounter.isActiveAndEnabled)
        {
            _tapCounter.text = _gameManager.AntiStunTapCounter.ToString();
        }
    }

    public void ShowStartUi(int stage)
    {
        _stageNumber.text = stage.ToString();
        _stageNumber.transform.parent.gameObject.SetActive(true);
        _startButtonIcon.gameObject.SetActive(true);
        _resetButtonIcon.gameObject.SetActive(true);
        ReloadPointRecords();
    }

    public void HideStartUi()
    {
        _stageNumber.transform.parent.gameObject.SetActive(false);
        _startButtonIcon.gameObject.SetActive(false);
        _resetButtonIcon.gameObject.SetActive(false);
        _scrollArea.gameObject.SetActive(false);
    }

    public void ShowInGameUi()
    {
        _startButtonIcon.gameObject.SetActive(false);
        _nextButtonIcon.gameObject.SetActive(false);
        _gameOverIcon.gameObject.SetActive(false);
        _retryButtonIcon.gameObject.SetActive(false);
        _successIcon.gameObject.SetActive(false);
        _pointsText.transform.parent.gameObject.SetActive(true);
        _remainingTimeText.transform.parent.gameObject.SetActive(true);
    }

    public void HideInGameUi()
    {
        _pointsText.transform.parent.gameObject.SetActive(false);
        _remainingTimeText.transform.parent.gameObject.SetActive(false);
    }


    public void HideTimeOutUi()
    {
        _stageNumber.transform.parent.gameObject.SetActive(false);
        _result.transform.parent.gameObject.SetActive(false);
        _successIcon.gameObject.SetActive(false);
        _nextButtonIcon.gameObject.SetActive(false);
        _scrollArea.gameObject.SetActive(false);
    }

    public void ShowTimeOutUi(int stage)
    {
        HideAntiStunButton();
        _stageNumber.text = stage.ToString();
        _stageNumber.transform.parent.gameObject.SetActive(true);
        _result.text = _pointsText.text;
        _result.transform.parent.gameObject.SetActive(true);
        _successIcon.gameObject.SetActive(true);
        _nextButtonIcon.gameObject.SetActive(true);
        ReloadPointRecords();
    }


    public void HideGameOverUi()
    {
        _stageNumber.transform.parent.gameObject.SetActive(false);
        _result.transform.parent.gameObject.SetActive(false);
        _gameOverIcon.gameObject.SetActive(false);
        _retryButtonIcon.gameObject.SetActive(false);
        _scrollArea.gameObject.SetActive(false);
    }

    public void ShowGameOverUi(int stage)
    {
        HideAntiStunButton();
        _stageNumber.text = stage.ToString();
        _stageNumber.transform.parent.gameObject.SetActive(true);
        _result.text = _pointsText.text;
        _result.transform.parent.gameObject.SetActive(true);
        _gameOverIcon.gameObject.SetActive(true);
        _retryButtonIcon.gameObject.SetActive(true);
        ReloadPointRecords();
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

    public void ShowAntiStunButton()
    {
        _tapCounter.transform.parent.gameObject.SetActive(true);
    }

    public void HideAntiStunButton()
    {
        _tapCounter.transform.parent.gameObject.SetActive(false);
    }

    private void ReloadPointRecords()
    {
        _scrollArea.gameObject.SetActive(true);
        foreach (Transform child in _pointRecords.transform)
        {
            Destroy(child.gameObject);
        }
        int pointsRecordAmount = _gameManager.PointRecords.Count;
        if (pointsRecordAmount > _scrollBarMinAmount)
            _scrollBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        else
            _scrollBar.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        for (int i = pointsRecordAmount - 1; i >= 0; i--)
        {
            GameObject record = Instantiate(_pointRecordPrefab, _pointRecords.transform);
            GameObject stage = record.transform.Find("Stage").gameObject;
            GameObject points = record.transform.Find("Points").gameObject;

            if (stage != null && points != null)
            {
                stage.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                points.GetComponent<TextMeshProUGUI>().text = _gameManager.PointRecords[i].ToString();
            }
        }
    }
}
