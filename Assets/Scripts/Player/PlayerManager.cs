using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject vehicle; 
    void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Plane"); 
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = vehicle.transform.position;
        //transform.rotation = vehicle.transform.rotation;
    }
}
