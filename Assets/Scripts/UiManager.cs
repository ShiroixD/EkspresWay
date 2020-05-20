using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameManager GameManager;
    public Text PointsText;

    void Start()
    {
        
    }

    void Update()
    {
        PointsText.text = GameManager.GetCurrentPoints().ToString();
    }
}
