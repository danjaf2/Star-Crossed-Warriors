using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.GraphicsBuffer;

public class Rooms : MonoBehaviour
{
    [SerializeField] public List<Waypoint> waypoints;

    [SerializeField] public List<Waypoint> tokenWaypointLocations;

    [SerializeField] public List<Waypoint> safeZones;

    public Timer longerUpdate;

    // Start is called before the first frame update
    void Start()
    {
        //WaypointLinker linker= new WaypointLinker();
        //linker.FunctionToRun();
        longerUpdate = gameObject.AddComponent<Timer>() as Timer;
        longerUpdate.timeDefault = 2;
        longerUpdate.timeLeft = 0;
        longerUpdate.running = false;
        longerUpdate.Reset();

    }

    // Update is called once per frame
    void Update()
    {

        if ((!longerUpdate.running) || true)
        {
            for (var i = waypoints.Count - 1; i > -1; i--)
            {
                if (waypoints[i] == null && waypoints[i].isMoveable)
                {
                    waypoints.RemoveAt(i);
                    continue;
                }
                else if (waypoints[i].transform == null && waypoints[i].isMoveable)
                {
                    waypoints.RemoveAt(i);
                    continue;
                }

            }

            UpdateConnections();
            longerUpdate.Reset(); 
        }
       
    }

    public void UpdateConnections()
    {
        var listCopy = new List<Waypoint>(waypoints);
        foreach (Waypoint w in listCopy)
        {
            if (w.isMoveable)
            {
                if (waypoints.Contains(w))
                {
                    RemoveWaypoint(w);
                    Remove(w); 
                }

                InsertNewWaypoint(w);
            }

        }
    }


    public List<Waypoint> GetNeighbors(Waypoint current)
    {
        return current.connectedTo;
    }

    public void InsertNewWaypoint(Waypoint current)
    {
       
        if (current == null || current.transform == null || !current.isMoveable) return;

        if (current.isMoveable)
        {
            current.connectedTo = new List<Waypoint>();

            current.position = current.transform.position;
        }



        foreach (Waypoint w in waypoints)
        {

            if (w == null)
            {
                continue;
            }

            if (w.transform == null || w.isMoveable)
            {
               w.connectedTo.Remove(current);
               current.connectedTo.Remove(w);
               continue;
            }

            w.position = w.transform.position; 

            Ray ray3 = new Ray(current.position, w.position - current.position);

            RaycastHit hit;

            if (Physics.Raycast(ray3, out hit, (w.position - current.position).magnitude))
            {

                if (hit.transform.tag != "Wall" && hit.transform.tag != "Ground" && hit.transform.position == w.position)

                {
                    //Debug.DrawLine(current.position, w.position, Color.green, 12f);
                    if (!w.connectedTo.Contains(current))
                    {
                        w.connectedTo.Add(current);
                    }

                    if (!current.connectedTo.Contains(w))
                    {
                        current.connectedTo.Add(w);

                    }


                    continue;
                    

                }
                else
                {
                    //Debug.DrawLine(current.position, hit.point, Color.red, 12f);
                    w.connectedTo.Remove(current);
                    current.connectedTo.Remove(w);
                    continue;

                }
            }
            else
            {
                //Debug.DrawLine(current.position, w.position, Color.black, 12f);
                if (!w.connectedTo.Contains(current))
                {
                    w.connectedTo.Add(current);
                }

                if (!current.connectedTo.Contains(w))
                {
                    current.connectedTo.Add(w);

                }
            }

          
        




       
    }

        waypoints.Add(current);
    }

    public void Insert(Waypoint current)
    {
        if (current == null || current.transform == null || !current.isMoveable) return;
        if (!waypoints.Contains(current))
        {
            waypoints.Add(current);
        }
    }
    public void Remove(Waypoint current)
    {
        if (current == null || current.transform == null || !current.isMoveable) return;
        if (waypoints.Contains(current))
        {
            
            waypoints.Remove(current);

        }
    }
    public void RemoveWaypoint(Waypoint current)
    {
        
        if (current == null || current.transform == null || !current.isMoveable) return;

        foreach (Waypoint w in waypoints)
        {
            if (w.connectedTo.Contains(current))
            {
                w.connectedTo.Remove(current); 
            }

            if (current.connectedTo.Contains(w))
            {
                current.connectedTo.Remove(w);
            }
        }

        waypoints.Remove(current);

    }

    



}
