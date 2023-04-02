using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Vector3 initVelocity;
    public Vector3 velocity;
    public float mass;


    public float radius = 0.5f;

    void Start()
    {
        velocity = initVelocity;
    }

    private void Update()
    {
        this.transform.position += velocity * Time.deltaTime;
    }
}
