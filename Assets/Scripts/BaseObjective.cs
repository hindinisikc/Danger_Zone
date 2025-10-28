using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType { Collection, Location, Puzzle }

[System.Serializable]
public abstract class BaseObjective : MonoBehaviour
{
    public int ObjectiveID;
    public string ObjectiveName;
    public string ObjectiveDescription;
    public ObjectiveType Type;
    public string ObjectiveGoal;
    public bool IsActive;
    public bool IsCompleted;

    // Property to represent the completion status
    public string CompletionStatus
    {
        get
        {
            if (IsCompleted)
                return "Completed";
            return "Incomplete";
        }
    }

    // Method for activating the objective
    public virtual void ActivateObjective()
    {
        IsActive = true;
        Debug.Log(ObjectiveName + " activated.");
    }

    // Method for completing the objective
    public virtual void CompleteObjective()
    {
        IsCompleted = true;
        Debug.Log(ObjectiveName + " completed.");
    }
}