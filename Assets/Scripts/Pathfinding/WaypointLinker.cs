using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WaypointLinker : EditorWindow
{
    public List<Waypoint> waypoints;


    public Object waypointPrefab;
    public Vector3 startingPos = Vector3.zero;
    public int numberRows = 10;
    public int numberColumn =10;
    public int zDepth = 10;
    public float spacing=60;

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
        }else if(GUILayout.Button("Spawn Waypoints"))
        {
            waypointPrefab= (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Waypoints/Waypoint.prefab", typeof(GameObject));
            SpawnWaypoints();
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

    private void SpawnWaypoints()
    {
        waypoints = GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().waypoints;
        waypoints.Clear();
        GameObject parentObject = GameObject.Find("FixedWaypoints");
        numberRows = 10;
        numberColumn = 10;
        zDepth = 10;
        spacing= 60;
        for (int row =0;row<numberRows;row++)
        {
            for (int col = 0; col < numberColumn; col++)
            {
                for (int depth = 0; depth < zDepth; depth++)
                {
                    Vector3 position = new Vector3(startingPos.x + row * spacing, startingPos.y + col * spacing, startingPos.z + depth * spacing);
                    Debug.Log(position);
                    GameObject point = new GameObject();
                    point = PrefabUtility.ConnectGameObjectToPrefab(point, waypointPrefab as GameObject);
                    //GameObject point = ((GameObject)PrefabUtility.InstantiatePrefab(waypointPrefab));
                    point.transform.position = position;
                    point.transform.parent = parentObject.transform;
                    point.gameObject.name = point.gameObject.name + "_" + row+ "_" + col+ "_" + depth;
                    waypoints.Add(point.GetComponent<Waypoint>());
                }
            }
        }
    }



    private void FunctionToRun()
    {
        waypoints = GameObject.FindGameObjectWithTag("Manager").GetComponent<Rooms>().waypoints;
        spacing= 60;
        float distanceLimit = Mathf.Sqrt((Mathf.Pow(60, 2) + Mathf.Pow(60, 2)));
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
                        if(hit.distance < distanceLimit) { 
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