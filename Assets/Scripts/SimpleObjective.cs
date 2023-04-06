using System;
using System.Collections;
using UnityEngine;

public class SimpleObjective : MonoBehaviour, IObjective {
    // This component is meant to be a simple way to make something into an objective.
    // Ex: Mothership is has a SimpleObjective component on it. On death, it calls the the CompleteObjective method.

    public string ObjectiveText = "SAMPLE OBJECTIVE";
    public string ObjectiveDescription => ObjectiveText;

    public event Action<IObjective> OnObjectiveComplete;

    private void Awake() {
        ObjectiveManager.Instance.RegisterObjective(this);
    }

    public void CompleteObjective() {
        OnObjectiveComplete(this);
    }
}