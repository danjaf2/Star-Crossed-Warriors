using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody body;
    public Vector3 initVelocity;

    public float radius = 0.5f;

    void Start()
    {  
        body = this.GetComponent<Rigidbody>();
        body.velocity = initVelocity;
    }
}
