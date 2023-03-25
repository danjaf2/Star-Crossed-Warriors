using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] public GameObject target;
    [SerializeField] public Vector3 positionFromTarget;
    bool _camMode;
    // Start is called before the first frame update
    void Start()
    {
        //positionFromTarget = new Vector3(0, 0, 0); 
        _camMode = transform.tag != "Radar";
    }

    // Update is called once per frame
    void Update()
    {
        if(_camMode)
        {
            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation;
        }
        else
        {
            transform.position = target.transform.position + positionFromTarget;
            //transform.position = target.GetComponent<Rigidbody>().centerOfMass + positionFromTarget;
            transform.rotation = Quaternion.Euler(90, 0, target.transform.rotation.eulerAngles.z);
        }
    }
}
