using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vehicle : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject plane;
    [SerializeField] public float roll;
    [SerializeField] public float pitch;
    [SerializeField] public float yaw;
    [SerializeField] public float throttle;
    [SerializeField] public float throttleIncrement;
    [SerializeField] public float brakeIncrement;
    [SerializeField] public float maxThrottle = 200.0f;
    [SerializeField] public float sensitivity;
    [SerializeField] public float thrust = 5.0f;
    [SerializeField] public float maxThrust = 5.0f;


    [SerializeField] public float sensitivityScale = 10.0f;
    [SerializeField] public float rotationMaxDegrees = 30.0f;

    [SerializeField] public string rollAxis = "Roll";
    [SerializeField] public string pitchlAxis = "Pitch";
    [SerializeField] public string yawAxis = "Yaw";
    [SerializeField] public string throttleKey = "Mouse0";
    [SerializeField] public string brakeKey = "Mouse1";

    [SerializeField] public float collisionCrashIndex = 500.0f;

    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject screenBoundaries;

    [SerializeField] public AudioSource audioSource;


    [SerializeField] private bool brakesOn = false;



    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;


    }

    // Update is called once per frame
    void Update()
    {
        //ControllerProcessing();


    }

    private void FixedUpdate()
    {
        if (brakesOn)
        {
            rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
            return;
        }

        float adjustedSensitivity = getAdjustedSensitivity();

        Debug.DrawRay(transform.position, transform.forward * 10,Color.red);
        rb.AddForce(transform.forward * throttle * adjustedSensitivity);
     
        //rb.AddTorque(transform.up * yaw * adjustedSensitivity);
       // rb.AddTorque(transform.right * pitch * adjustedSensitivity);
        //rb.AddTorque(-transform.forward * roll * adjustedSensitivity);
        rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
        //Debug.Log(transform.position); 
    }

    public float getAdjustedSensitivity()
    {
        return (rb.mass / sensitivityScale) * sensitivity;
    }



    public void ControllerProcessing(Vector3 translation, Quaternion rotation, bool throttleOn, bool brakingOn)
    {
        if (Time.timeScale != 1 || Globals.gameOver)
        {
            return;
        }

       

        //pitch = (int) Mathf.Clamp(((rotation).eulerAngles.x), -1, 1);
        //roll = (int) Mathf.Clamp(((rotation).eulerAngles.z), -1, 1);
        //yaw = -(int) Mathf.Clamp(((rotation).eulerAngles.y), -1, 1);

        rb.rotation = Quaternion.RotateTowards(transform.rotation,rotation.normalized, rotationMaxDegrees);

        // Debug.Log(roll);
        // Debug.Log(pitch)
        //Debug.Log(yaw);

        if (brakingOn)
        {
            Brake();
        }
        else if (throttleOn)
        {
            Throttle(); 
        }
        if (throttle < 0)
        {
            throttle = 0;
        }
    }

    public void Throttle()
    {
        //Debug.Log("Throttle on");
        throttle += throttleIncrement;

        if (throttle >= maxThrottle)
        {
            throttle = maxThrottle;
        }
    }

    public void Brake()
    {
        //Debug.Log("Brakes on");
        throttle -= brakeIncrement;

        if (throttle < 0)
        {
            throttle = 0;
        }
    }
}
