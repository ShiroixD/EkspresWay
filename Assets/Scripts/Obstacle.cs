using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float ScrollSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * ScrollSpeed, transform.position.z);
        if (transform.position.y < -10)
            Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {

    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.tag.ToString());
        switch (this.gameObject.tag)
        {
            case "Tooth":
                {
                    Destroy(this.gameObject);
                    break;
                }
            case "Teeth":
                {
                    Destroy(this.gameObject);
                    break;
                }
            default:
                break;

        }
    }

}
