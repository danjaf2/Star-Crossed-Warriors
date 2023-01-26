using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        ControllerProcessing();
    }

    private void FixedUpdate()
    {
        if (brakesOn)
        {
            rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
            return; 
        }
        rb.AddForce(transform.forward * thrust * throttle);
        float adjustedSensitivity = getAdjustedSensitivity();
        rb.AddTorque(transform.up * yaw * adjustedSensitivity);
        rb.AddTorque(transform.right * pitch * adjustedSensitivity);
        rb.AddTorque(-transform.forward * roll * adjustedSensitivity);
        rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * 0.001f);
        Debug.Log(transform.position); 
    }

    public float getAdjustedSensitivity()
    {
        return (rb.mass / sensitivityScale) * sensitivity;
    }

    public void ThereminControllerProcessing()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log(mousePos.x);
        Debug.Log(mousePos.y);

        roll = mousePos.x * 2 - 1.0f;
        pitch = mousePos.y * 2 - 1.0f;

        //audioSource.panStereo = mousePos.x*2 - 1.0f;
       // audioSource.pitch = mousePos.y * 2 + 1.0f; 
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

        if (Input.GetKey(KeyCode.Mouse2))
        {
            ThereminControllerProcessing();
        }
        else
        {
            //audioSource.panStereo =0;
            //audioSource.pitch =1.5f;
        }

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
