using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaypointLinker : EditorWindow
{
    public List<Waypoint> waypoints;
    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<WaypointLinker>("Edit Mode Functions");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Connect Waypoints"))
        {
            FunctionToRun();
        }
        else if (GUILayout.Button("Clear Waypoints"))
        {
            ClearWaypoints();
        }
    }

    private void ClearWaypoints()
    {
        waypoints = GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().waypoints;
        foreach (var w in waypoints)
        {
            w.connectedTo = new List<Waypoint>();
        }
    }



        private void FunctionToRun()
    {
        waypoints = GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().waypoints;
        foreach (var w in waypoints)
        {

            foreach(var current in waypoints)
            {
                if (w == null)
                {
                    continue;
                }

                if (w.transform == null || w.isMoveable)
                {
                    // w.connectedTo.Remove(current);
                    // current.connectedTo.Remove(w);
                    continue;
                }

                w.position = w.transform.position;

                Ray ray3 = new Ray(current.position, w.position - current.position);

                RaycastHit hit;

                if (Physics.Raycast(ray3, out hit, (w.position - current.position).magnitude))
                {

                    if (hit.transform.tag != "Wall" && hit.transform.tag != "Ground" &&hit.transform.position == w.position)

                    {
                        Debug.DrawLine(current.position, hit.point, Color.cyan, 9f);

                        if (!w.connectedTo.Contains(current))
                        {
                            w.connectedTo.Add(current);
                        }

                        if (!current.connectedTo.Contains(w))
                        {
                            //current.connectedTo.Add(w);

                        }


                        continue;

                    }
                    else
                    {
                        if (w.connectedTo.Contains(current))
                        {
                            w.connectedTo.Remove(current);
                        }

                        if (current.connectedTo.Contains(w))
                        {
                            current.connectedTo.Remove(w);

                        }
                        continue; 
                    }
                }
                else
                {
                    Debug.DrawLine(current.position, w.position, Color.blue, 9f);
                    if (!w.connectedTo.Contains(current))
                    {
                        w.connectedTo.Add(current);
                    }

                    if (!current.connectedTo.Contains(w))
                    {
                        //current.connectedTo.Add(w);

                    }
                }

            }
        }

        Debug.Log("The function ran.");
    }
}