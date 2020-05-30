using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _pointsText;

    void Start()
    {
        
    }

    void Update()
    {
        _pointsText.text = _gameManager.PointsCounter.ToString();
    }
}
