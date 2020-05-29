using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float DinstanceToReplace = 10;
    public float NewPositionOffset = 20f;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (transform.position.y < -DinstanceToReplace)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _gameManager.getScrollSpeed(), transform.position.z);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (gameObject.tag)
            {
                case "Teeth":
                    {
                        _gameManager.IncreasePoints(1);
                        transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
                        break;
                    }
                case "Tooth":
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + NewPositionOffset, transform.position.z);
                        break;
                    }
                case "Thread":
                    {
                        _gameManager.setScrollSpeed(0);
                        _gameManager.Player.GetComponent<Player>().obstacleLockFlag = true;
                        break;
                    }

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
