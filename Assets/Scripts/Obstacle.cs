using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float ScrollSpeed = 5f;
    public float DinstanceToReplace = 10;
    public float NewPositionOffset = 20f;
    private GameManager _gameManager;
    private GameObject _player;

    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (transform.position.y < -DinstanceToReplace)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * ScrollSpeed, transform.position.z);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Teeth")
            {
                _gameManager.IncreasePoints(1);
                transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
            }
        }
    }
}
