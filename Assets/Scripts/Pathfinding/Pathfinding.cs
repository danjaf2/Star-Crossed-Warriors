using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    private Rooms graph;
    public delegate float Heuristic(Transform start, Transform end);
    public Waypoint startNode;
    public Waypoint goalNode;

    public Heuristic h;

    public Heuristic evaderH; 


    public GameObject waypointPrefab;

    public bool addAuxiliaryNodes = false; 

    //public RoundManager roundManager;

    public float Distance(Transform startNode, Transform goalNode)
    {
        return (goalNode.position - startNode.position).magnitude;

    }

    public float DistanceSeeker(Transform startNode, Transform goalNode)
    {
        return (goalNode.position - startNode.position).magnitude + (startNode.gameObject.GetComponent<Waypoint>().dangerLevel); //Avoids dangerous rooms as much as possible

    }

    // Start is called before the first frame update
    void Start()
    {
        //Heuristics defined:
        h = Distance;
        evaderH = DistanceSeeker; 


        graph = gameObject.GetComponent<Rooms>();
        //roundManager = gameObject.GetComponent<RoundManager>();

    }

    // Update is called once per frame
    void Update()
    {
        graph = gameObject.GetComponent<Rooms>();
    }

    public List<Waypoint> FindPath(Waypoint start, Waypoint goal)
    {
        return FindPathWrapped(start, goal, h); 
    }

    public List<Waypoint> FindPath(Waypoint start, Waypoint goal, float x)
    {
        if(x == 0)
        {
            return FindPathWrapped(start, goal, h); //Used in regular pathfinding
        }
        else if(x == 1)
        {
            return FindPathWrapped(start, goal, evaderH); //Used in regular pathfinding
        }
        return FindPathWrapped(start, goal, h); //Used in regular pathfinding

    }


    public List<Waypoint> FindPathWrapped(Waypoint start, Waypoint goal, Heuristic hvalue)
    {

        graph = gameObject.GetComponent<Rooms>();

        //Debug.Log("FINDING PATH"); 
        Heuristic heuristic = hvalue;
        bool isAdmissible = true;
        isAdmissible = true; 
        if (graph == null) {
            //Debug.Log("Graph is NULL"); 
            return new List<Waypoint>(); }
        if (heuristic == null) heuristic = (Transform s, Transform e) => 0;

        if (start == null || goal == null)
        {
            return new List<Waypoint>();
        }

        List<Waypoint> path = null;
        bool solutionFound = false;

        Dictionary<Waypoint, float> gnDict = new Dictionary<Waypoint, float>();
        gnDict.Add(start, default);

        Dictionary<Waypoint, float> fnDict = new Dictionary<Waypoint, float>();
        fnDict.Add(start, heuristic(start.transform, goal.transform) + gnDict[start]);

        Dictionary<Waypoint, Waypoint> pathDict = new Dictionary<Waypoint, Waypoint>();
        pathDict.Add(start, null);


        List<Waypoint> openList = new List<Waypoint>();
        openList.Add(start);

        HashSet<Waypoint> closedSet = new HashSet<Waypoint>();

        while (openList.Count > 0)
        {

            Waypoint current = openList[openList.Count - 1];
            openList.RemoveAt(openList.Count - 1);

            closedSet.Add(current);


            if (current == goal && isAdmissible)
            {
                solutionFound = true;
                break;
            }
            else if (closedSet.Contains(goal))
            {
                // early exit strategy if heuristic is not admissible (try to avoid this if possible)
                float gGoal = gnDict[goal];
                bool pathIsTheShortest = true;

                foreach (Waypoint entry in openList)
                {
                    if (gGoal > gnDict[entry])
                    {
                        pathIsTheShortest = false;
                        break;
                    }
                }

                if (pathIsTheShortest) break;
            }

            List<Waypoint> neighbors = graph.GetNeighbors(current);

            float g_next = 0;
            foreach (Waypoint n in neighbors)
            {

                float movement_cost = (current.position - n.position).magnitude;
                //float movement_cost = 1; 

                if (closedSet.Contains(n) || (n.isMoveable && n != goal))
                {
                    continue;
                }


                g_next = gnDict[current] + movement_cost;

                if (!gnDict.ContainsKey(n) || (g_next < gnDict[n]))
                {
                    pathDict[n] = current;
                    gnDict[n] = g_next;

                    if (goal == null || goal.transform == null|| n == null)
                    {
                        return new List<Waypoint>(); 
                    }
                    if(n.transform == null)
                    {
                        continue; 
                    }
                    float hResult = heuristic(n.transform, goal.transform); 
                    fnDict[n] = g_next + hResult;
                    QueueInsert(openList, fnDict, n);


                }



            }
        }


        if (!solutionFound && closedSet.Contains(goal))
        {
            solutionFound = true;
        }
           

        if (solutionFound)
        {

            path = new List<Waypoint>();
            Waypoint newCurrent = goal;

            while (newCurrent != startNode)
            {
                if(newCurrent != null)
                {
                    path.Add(newCurrent);
                   
                }
                newCurrent = pathDict[newCurrent];


            }

            if (startNode != null)
            {
                path.Add(startNode);
            }

           


            path.Reverse();
        }

       // Debug.Log("SOLUTION FOUND? " + solutionFound +"Count: " + path.Count); 

        
        return DrawPath(path);
    }

    public List<Waypoint> DrawPath(List<Waypoint> pathTraced)
    {
        if (pathTraced == null || pathTraced.Count < 1)
        {
            return new List<Waypoint>();
        }
        //Debug.Log("A");

        /*foreach (Waypoint next in pathTraced)
        {
            Debug.Log(next); 
        
        }*/

       

        Waypoint currentPosition = pathTraced[0];
      
        //Debug.Log(currentPosition.position);
        foreach (Waypoint next in pathTraced)
        {
            //Debug.Log(currentPosition.position);
            //Debug.Log(next.position);


            //Debug.DrawLine(currentPosition.position, next.position, Color.blue, 5f);

            Ray check = new Ray(currentPosition.position, (next.position - currentPosition.position));
            RaycastHit hit;

            if (Physics.Raycast(check, out hit, (next.position - currentPosition.position).magnitude))
            {
                if(hit.transform.tag == "Wall" || hit.transform.tag == "Ground")
                {
                    return new List<Waypoint>(); //Prevent miscalculations due to moveable waypoints
                }
            }
            currentPosition = next; 
        }

        return pathTraced; 
    }

    private void QueueInsert(List<Waypoint> pqList, Dictionary<Waypoint, float> fnDict, Waypoint node)
    {
        if (pqList.Count == 0)
            pqList.Add(node);
        else
        {
            for (int i = pqList.Count - 1; i >= 0; --i)
            {
                if (fnDict[pqList[i]] > fnDict[node])
                {
                    pqList.Insert(i + 1, node);
                    break;
                }
                else if (i == 0)
                    pqList.Insert(0, node);
            }
        }
    }

}
