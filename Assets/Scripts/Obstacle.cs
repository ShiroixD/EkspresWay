using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _dinstanceToReplace = 10;
    [SerializeField] private float _newPositionOffset = 20f;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (_gameManager.GameState == GameState.IN_PROGRESS)
        {
            if (transform.position.y < -_dinstanceToReplace)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _newPositionOffset, transform.position.z);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _gameManager.ScrollSpeed, transform.position.z);
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (gameObject.tag != "Thread")
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _newPositionOffset, transform.position.z);
            }
        }
    }
}
