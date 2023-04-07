using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{

    List<IObjective> _objectiveList;

    protected override void Awake()
    {
        Instance = this;
        _objectiveList = new List<IObjective>();
    }

    public void RegisterObjective(IObjective newObjective)
    {
        _objectiveList.Add(newObjective);
        newObjective.OnObjectiveComplete += OnObjectiveComplete;
    }

    public void OnObjectiveComplete(IObjective completed)
    {
        _objectiveList.Remove(completed);
        Debug.Log("Completed " + completed.ObjectiveDescription);
        try
        {
            //kinda ugly but meh whatavah
            _objectiveList[0].Reference.GetComponent<SimpleObjective>().SetActive();
        }
        catch(ArgumentOutOfRangeException e)
        {
            //Start final kill mothership phase
        }

        if (_objectiveList.Count > 0)
        {
            // win the level
        }
    }

    public string GetObjectiveTextList()
    {
        string textList = "";
        foreach (var objective in _objectiveList)
        {
            textList += $"- {objective.ObjectiveDescription}\n";
        }
        return textList;
    }

    public List<GameObject> GetObjectiveList()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var objective in _objectiveList)
        {
            if (objective.Reference != null)
            {
                list.Add(objective.Reference);
            }
        }
        return list;
    }

}

public interface IObjective
{
    public string ObjectiveDescription { get; }
    public GameObject Reference { get; }

    public event Action<IObjective> OnObjectiveComplete;
    public bool isActiveObjective { get; set; }
    protected void RegisterAsObjective() { ObjectiveManager.Instance.RegisterObjective(this);


    
    }
}