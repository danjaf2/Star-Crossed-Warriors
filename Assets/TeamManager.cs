using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public  List<GameObject> alliesList = new List<GameObject>();
    public  List<GameObject> enemyList = new List<GameObject>();
    // Start is called before the first frame update

    protected override void Awake()
    {
        Instance = this;
        alliesList = new List<GameObject>();
        enemyList = new List<GameObject>();
    }
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject e in enemies)
        {
            if (e.layer == LayerMask.NameToLayer("Character"))
            {
                enemyList.Add(e);
            }
        }
        foreach (GameObject e in allies)
        {
            if (e.layer == LayerMask.NameToLayer("Character"))
            {
                alliesList.Add(e);
            }
        }
    }

    public void AddToListAllies(GameObject obj)
    {
        alliesList.Add (obj);
    }

   
}
