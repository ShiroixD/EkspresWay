using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerObject;

    [SerializeField]
    private float _distanceLimitToMoveUp = 34f;

    private GameManager _gameManager;
    private Vector3 _startPosition;

    void Awake()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _startPosition = transform.position;
    }

    void Update()
    {
        float myPosY = transform.position.y;
        float playerPosY = _playerObject.transform.position.y;

        if (myPosY < playerPosY && Mathf.Abs(myPosY - playerPosY) > _distanceLimitToMoveUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3 * _distanceLimitToMoveUp, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _gameManager.ScrollSpeed, transform.position.z);
        float newPos = Mathf.Repeat(Time.time * _gameManager.ScrollSpeed, 35);
        transform.position = _startPosition + Vector3.down * newPos;
    }
}
