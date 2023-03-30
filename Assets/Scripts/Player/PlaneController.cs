using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject plane;
    [SerializeField] public float roll;
    [SerializeField] public float pitch;
    [SerializeField] public float yaw;
    [SerializeField] public float throttle;
    [SerializeField] public float throttleIncrement;
    [SerializeField] public float maxThrottle = 200.0f;
    [SerializeField] public float sensitivity;
    [SerializeField] public float thrust = 5.0f;
    [SerializeField] public float maxThrust = 5.0f;

    [SerializeField] public float sensitivityScale = 10.0f;

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


    [SerializeField] public float pingRange = 600f;
    [SerializeField] public LayerMask waypointMask;


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
       

    }

    // Update is called once per frame
    void Update()
    {
        ControllerProcessing();
       
        if(Input.GetKeyDown(KeyCode.H))
        {
            callForHelp();
        }
    }

    private void FixedUpdate()
    {
        if (brakesOn)
        {
            rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
            return;
        }
       
        float adjustedSensitivity = getAdjustedSensitivity();

        Debug.DrawRay(transform.position, transform.forward * 10,Color.red, 10f);
        Debug.DrawRay(transform.position, transform.right * 10, Color.blue, 10f);
        Debug.Log("Applying forces");
        Debug.Log(transform.up * yaw * adjustedSensitivity);
        rb.AddForce(transform.forward * throttle * adjustedSensitivity);
        rb.AddTorque(transform.up * yaw * adjustedSensitivity);
        rb.AddTorque(transform.right * pitch * adjustedSensitivity);
        rb.AddTorque(-transform.forward * roll * adjustedSensitivity);
        rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
        //Debug.Log(transform.position); 
    }

    public float getAdjustedSensitivity()
    {
        return (rb.mass / sensitivityScale) * sensitivity;
    }

    public void callForHelp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1000, waypointMask);
        if (hitColliders.Length > 0)
        {
            //Select closest hit waypoint
            if (GetClosestWaypoint(hitColliders).TryGetComponent<Waypoint>(out Waypoint point))
            {

                //Should be changed latter
                foreach (GameObject ally in TeamManager.alliesList)
                {
                    if (ally.TryGetComponent<BaseAlly>(out BaseAlly friend))
                    {
                        friend._root.SetData("AllyPingedLocation", point);
                        Debug.Log("Alerted Ally");
                    }
                }
            }
        }
    }

    Collider GetClosestWaypoint(Collider[] points)
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = this.transform.position;
        foreach (Collider t in points)
        {
            float dist = Vector3.Distance(t.transform.position, this.transform.position);
            if (dist < minDist)
            {

                tMin = t;
                print("SET");
                minDist = dist;
            }
        }
        print(tMin);
        return tMin;
    }

    public void ControllerProcessing()
    {
        if (Time.timeScale != 1 || Globals.gameOver)
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); 
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            brakesOn = !brakesOn;
        }

        roll = Input.GetAxis(rollAxis);
        pitch = Input.GetAxis(pitchlAxis);
        yaw = Input.GetAxis(yawAxis);

        Debug.Log(roll);
        Debug.Log(pitch);
        Debug.Log(yaw);

       
        if (Input.GetButton(throttleKey))
        {
           Debug.Log("Throttle on"); 
            throttle += throttleIncrement; 

            if(throttle >=maxThrottle)
            {
                throttle = maxThrottle; 
            }
        }

        else if (Input.GetButton(brakeKey))
        {
           Debug.Log("Brakes on");
            throttle -= throttleIncrement;

            if (throttle < 0)
            {
                throttle = 0;
            }
        }

        else
        {
           // throttle -= 0.0098f;

            if (throttle < 0)
            {
                throttle = 0;
            }
        }
    }
}
