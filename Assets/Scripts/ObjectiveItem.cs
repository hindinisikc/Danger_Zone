using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveItem : MonoBehaviour
{
    public TextMeshProUGUI ObjectiveNameText;
    public TextMeshProUGUI ObjectiveDescriptionText;
    public TextMeshProUGUI ObjectiveGoalText;
    public TextMeshProUGUI CompletionStatusText;

    public void Setup(BaseObjective objective)
    {
        ObjectiveNameText.text = objective.ObjectiveName;
        ObjectiveDescriptionText.text = objective.ObjectiveDescription;
        ObjectiveGoalText.text = objective.ObjectiveGoal;
        CompletionStatusText.text = objective.CompletionStatus;
    }

    public void UpdateStatus(string newStatus)
    {
        CompletionStatusText.text = newStatus;
    }
}
