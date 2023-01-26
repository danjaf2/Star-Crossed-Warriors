using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{

    [SerializeField] public Vector3 trueNorth = new Vector3(0, 0, 100000);
    [SerializeField] public GameObject plane;
    [SerializeField] public float compassSpeed = 500.0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 direction = trueNorth - plane.transform.forward;
        //Vector3 newDirection = Vector3.RotateTowards(plane.transform.forward, new Vector3(direction.x, 0, direction.z), compassSpeed*Time.deltaTime, 0.0f);
        //float angleOfRotation = Vector2.SignedAngle(Vector2.up, direction);
        //transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);

        trueNorth.z = plane.transform.eulerAngles.y;
        transform.localEulerAngles = trueNorth;
    }
}
