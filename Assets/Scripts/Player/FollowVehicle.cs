using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVehicle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject vehicle;
    public Vector3 offset; 
    void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Plane");
        offset = vehicle.transform.position - transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = vehicle.transform.position - offset;
        transform.rotation = Quaternion.LookRotation(vehicle.transform.forward);
    }
}
