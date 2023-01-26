using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public float collisionCrashIndex = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Player")
        {
            return; 
        }
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        Debug.Log("Collision - " + collisionForce.magnitude);
        if (collisionForce.magnitude > collisionCrashIndex)
        {
           
            Globals.gameOver = true;

        }

    }

}
