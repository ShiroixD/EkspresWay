using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float DistanceLimit = 34f;
    private GameManager _gameManager;
    private Vector3 _startPos;

    void Awake()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _startPos = transform.position;
    }

    void Update()
    {
        float myPosY = transform.position.y;
        float playerPosY = _gameManager.Player.transform.position.y;

        if (myPosY < playerPosY && Mathf.Abs(myPosY - playerPosY) > DistanceLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3 * DistanceLimit, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _gameManager.getScrollSpeed(), transform.position.z);

        float newPos = Mathf.Repeat(Time.time * _gameManager.getScrollSpeed(), 35);
        transform.position = _startPos + Vector3.down * newPos;
    }
}
