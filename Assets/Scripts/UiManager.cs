using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameManager GameManager;
    public TextMeshProUGUI PointsText;

    void Start()
    {
        
    }

    void Update()
    {
        PointsText.text = GameManager.GetCurrentPoints().ToString();
    }
}
