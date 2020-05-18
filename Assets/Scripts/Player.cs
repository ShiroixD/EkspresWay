using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SwipeDetector SwipeDetector;

    private void Awake()
    {
        SwipeDetector.OnSwipe += Move;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Move(SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);
    }
}
