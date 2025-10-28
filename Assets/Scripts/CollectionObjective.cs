using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjective : BaseObjective
{
    public int RequiredAmount;
    public int CurrentAmount;

    // Override for completing collection objectives
    public override void CompleteObjective()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            base.CompleteObjective();
        }
    }

    // Method to increment collection count
    public void IncrementAmount()
    {
        if (IsActive && !IsCompleted)
        {
            CurrentAmount++;
            if (CurrentAmount >= RequiredAmount)
                CompleteObjective();
        }
    }
}
