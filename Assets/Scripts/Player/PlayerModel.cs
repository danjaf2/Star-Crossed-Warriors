using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Quaternion originalRot;
    public Vector3 eulerRot = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        originalRot = transform.rotation; 
        //eulerRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "Player" || transform.tag == "Plane")
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            transform.Rotate(eulerRot); 
        }
        else
        {
            transform.rotation = transform.parent.rotation;
        }
       
    }
}
