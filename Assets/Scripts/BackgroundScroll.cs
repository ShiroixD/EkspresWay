using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float ScrollSpeed = 5f;
    public float DistanceLimit = 34f;
    public GameObject Player;
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        float myPosY = transform.position.y;
        float playerPosY = Player.transform.position.y;

        if (myPosY < playerPosY && Mathf.Abs(myPosY - playerPosY) > DistanceLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3 * DistanceLimit, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * ScrollSpeed, transform.position.z);

        float newPos = Mathf.Repeat(Time.time * ScrollSpeed, 35);
        transform.position = _startPos + Vector3.down * newPos;
    }
}
