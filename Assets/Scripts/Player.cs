using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Model;
    public GameObject Physics;
    public SwipeDetector SwipeDetector;
    private Vector3 _startingPos;
    private Quaternion _startingRot;

    void Start()
    {
        _startingPos = transform.position;
        _startingRot = transform.rotation;
    }

    void Update()
    {
        
    }

    public void TiltToSide(SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);

        if (data.Direction == SwipeDirection.Left)
        {
            Physics.transform.position = new Vector3(_startingPos.x - 1f, _startingPos.y, _startingPos.z);
        }
        else if (data.Direction == SwipeDirection.Right)
        {
            Physics.transform.position = new Vector3(_startingPos.x + 1f, _startingPos.y, _startingPos.z);

        }
    }

    public void ReturnToNormal()
    {
        Debug.Log("Returning to normal");
        Physics.transform.position = new Vector3(_startingPos.x, _startingPos.y, _startingPos.z);
        //Model.transform.position = new Vector3(_startingPos.x, _startingPos.y, _startingPos.z);
        //Model.transform.rotation = new Quaternion(_startingRot.x, _startingRot.y, _startingRot.z, _startingRot.w);
    }
}
