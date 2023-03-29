using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Waypoint : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public List<Waypoint> connectedTo = new List<Waypoint>();
    public Vector3 position;
    public bool hasTokenInIt = false;
    public bool isTemporary = false;
    public bool isMoveable = false;

    public float dangerLevel = 0; 

    public Timer existence;

    public bool isSafeZone = false; 
    void Start()
    {
        position = gameObject.transform.position;

        existence = gameObject.AddComponent<Timer>() as Timer;
        existence.timeDefault = 15;
        existence.timeLeft = 0;
        existence.running = false;

        if (isTemporary)
        {
            //StartTimer();
        }

        if (isMoveable)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().Insert(this);
        }

        if (connectedTo == null)
        {
            connectedTo = new List<Waypoint>();
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoveable)
        {
            //DrawConnections();
            //SetCurrentDangerLevel();
        }
        else
        {
           //GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().Insert(this); 
        }
        position = gameObject.transform.position;


    }

  

    public void StartTimer()
    {
        if(existence == null)
        {

            existence = gameObject.AddComponent<Timer>() as Timer;
            existence.timeDefault = 5;
            existence.timeLeft = 0;
            existence.running = false;
        }
        //isTemporary = true;
        //existence.Reset(); 
    }

   /* void OnDrawGizmos()
    {
        if (!isMoveable)
        {
            Handles.Label(transform.position, transform.name);
        }
    }*/

    public void DrawConnections()
    {
      

        foreach (var connection in connectedTo)
        {
            if(connection == null || connection.transform == null) continue;
            if (!connection.isMoveable)
            {
                Debug.DrawLine(transform.position, connection.transform.position, Color.red, 10f);
            }
           
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        foreach (var connection in connectedTo)
        {
            if (connection == null || connection.transform == null) continue;
            if (!connection.isMoveable)
            {
                Gizmos.DrawLine(this.transform.position, connection.transform.position);
            }

        }
    }


    /*public void SetCurrentDangerLevel()
    {
        Chaser[] chasers = GameObject.FindObjectsOfType<Chaser>();
        dangerLevel = 0; 
        foreach(Chaser chaser in chasers)
        {
            Ray ray3 = new Ray(chaser.transform.position, this.transform.position - chaser.transform.position);

            RaycastHit hit;

            if (Physics.Raycast(ray3, out hit, chaser.lineOfSightRadius))
            {

                if (hit.transform.tag != "Wall" && hit.transform.tag != "Ground" && hit.transform.position == this.transform.position)

                {
                    float angle = Vector3.Angle(ray3.direction, chaser.transform.forward);

                    if(Mathf.Abs(angle) < chaser.lineOsSightAngle)
                    {
                        dangerLevel += 10;
                    }
                   

                }
                
            }
            else
            {
                float angle = Vector3.Angle(ray3.direction, chaser.transform.forward);

                if (Mathf.Abs(angle) < chaser.lineOfSightRadius)
                {
                    dangerLevel += 10;
                }
            }
        }
    }*/
}
