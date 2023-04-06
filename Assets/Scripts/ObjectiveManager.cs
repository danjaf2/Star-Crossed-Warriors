using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager> {

    List<IObjective> _objectiveList;

    protected override void Awake() {
        Instance = this;
        _objectiveList = new List<IObjective>();
    }

    public void RegisterObjective(IObjective newObjective) {
        _objectiveList.Add(newObjective);
        newObjective.OnObjectiveComplete += OnObjectiveComplete;
    }

    void OnObjectiveComplete(IObjective completed) {
        _objectiveList.Remove(completed);
        Debug.Log("Completed " + completed.ObjectiveDescription);

        if(_objectiveList.Count > 0) {
            // win the level
        }
    }

    public string GetObjectiveTextList() {
        string textList = "";
        foreach (var objective in _objectiveList) {
            textList += $"- {objective.ObjectiveDescription}\n";
        }
        return textList;
    }
}

public interface IObjective {
    public string ObjectiveDescription { get; }
    public event Action<IObjective> OnObjectiveComplete;

    protected void RegisterAsObjective() { ObjectiveManager.Instance.RegisterObjective(this); }
}