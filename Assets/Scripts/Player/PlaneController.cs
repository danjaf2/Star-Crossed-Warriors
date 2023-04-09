using AI;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaneController : NetworkBehaviour
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


    [SerializeField] public float rotationSpeed = 1;


    [SerializeField] public GameObject particlePrefab;

    [SerializeField] public PlayerManager manager; 

    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        manager = GetComponent<PlayerManager>(); 



    }

    // Update is called once per frame
    void Update()
    {

        if (!IsOwner)
        {
            return;
        }
        if (manager != null)
        {
            if(manager.playerClass != null)
            {
                if(manager.playerClass.isStunned)
                {
                    rb.velocity = Vector3.zero;
                    throttle = 0;
                    Debug.Log("Player is stunned"); 
                    return; 
                }
            }
        }


        ControllerProcessing();
       
        if(Input.GetKeyDown(KeyCode.H))
        {
            callForHelp();
        }


        if (brakesOn)
        {
            rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
            return;
        }

        float adjustedSensitivity = getAdjustedSensitivity();

        Debug.DrawRay(transform.position, transform.forward * 50, Color.magenta);
        Debug.DrawRay(transform.position, transform.right * 50, Color.red);
        Debug.DrawRay(transform.position, transform.up * 50, Color.green);
       // Debug.Log("Applying forces");
        //Debug.Log(transform.up * yaw * adjustedSensitivity);
        rb.AddForce(transform.forward * throttle * adjustedSensitivity);
        rb.AddTorque(transform.up * yaw * adjustedSensitivity * rotationSpeed);
        rb.AddTorque(transform.right * pitch * adjustedSensitivity * rotationSpeed);
        rb.AddTorque(-transform.forward * roll * adjustedSensitivity * rotationSpeed);
        //rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
        //Debug.Log(transform.position);
    }

    private void FixedUpdate()
    {
       
    }

    public float getAdjustedSensitivity()
    {
        return (rb.mass / sensitivityScale) * sensitivity;
    }
    public void BoostForward(float boostMultiplier)
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(transform.forward * boostMultiplier * 10f, ForceMode.Impulse);

        //throttle = boostMultiplier;
        //Debug.Log("Throttle: " + throttle);
        //Debug.Log("Vehicle moved foward");
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
                        if (ally != null)
                        {
                        friend._root.SetData("AllyPingedLocation", point);
                        Debug.Log("Alerted Ally");

                        }
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

       // Debug.Log(roll);
        //Debug.Log(pitch);
       // Debug.Log(yaw);

       
        if (Input.GetButton(throttleKey))
        {
           //Debug.Log("Throttle on"); 
            throttle += throttleIncrement; 

            if(throttle >=maxThrottle)
            {
                throttle = maxThrottle; 
            }

            particlePrefab.SetActive(true); 
        }

        else if (Input.GetButton(brakeKey))
        {
           //Debug.Log("Brakes on");
            throttle -= throttleIncrement;

            if (throttle < 0)
            {
                throttle = 0;
            }
            particlePrefab.SetActive(false);
        }

        else
        {
           // throttle -= 0.0098f;

            if (throttle < 0)
            {
                throttle = 0;
            }
            particlePrefab.SetActive(false);
        }
    }
}
