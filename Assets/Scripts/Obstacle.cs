using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (_gameManager.GameState == GameState.IN_PROGRESS)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _gameManager.ScrollSpeed, transform.position.z);
            if (transform.position.y <= -7.0f)
                Disappear();
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
