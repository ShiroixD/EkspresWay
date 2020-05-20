using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _startingPos;
    private Quaternion _startingRot;

    void Start()
    {
        Input.multiTouchEnabled = false;
        _startingPos = transform.position;
        _startingRot = transform.rotation;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2 || Input.GetKeyDown("A"))
                {
                    transform.position = new Vector3(_startingPos.x - 0.3f, _startingPos.y, _startingPos.z);
                    transform.rotation = new Quaternion(_startingRot.x, _startingRot.y + 70.0f, _startingRot.z, _startingRot.w);
                }
                else if (touch.position.x > Screen.width / 2 || Input.GetKeyDown("D"))
                {
                    transform.position = new Vector3(_startingPos.x + 0.3f, _startingPos.y, _startingPos.z);
                    transform.rotation = new Quaternion(_startingRot.x, _startingRot.y - 70.0f, _startingRot.z, _startingRot.w);

                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                transform.position = _startingPos;
                transform.rotation = _startingRot;
            }
        }
    }
}
