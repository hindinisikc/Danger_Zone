using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;

    // UI References
    public GameObject FullObjectivesView;
    public GameObject ObjectiveItemPrefab;
    public Transform ObjectivesContent;

    public GameObject NewObjectiveNotification; // Notification UI
    public float FadeDuration = 1f;             // Fade in/out duration
    public float NotificationDuration = 3f;     // How long the notification stays visible
    public AudioClip NotificationSound;         // Notification sound

    // List to store objectives data
    public List<ObjectiveData> ObjectivesData;

    private Dictionary<int, BaseObjective> objectives = new Dictionary<int, BaseObjective>();
    private Dictionary<int, GameObject> objectiveItems = new Dictionary<int, GameObject>();

    private CanvasGroup notificationCanvasGroup;
    private AudioSource audioSource;

    private void Awake()
    {
        // Implementing Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (NewObjectiveNotification != null)
        {
            notificationCanvasGroup = NewObjectiveNotification.GetComponent<CanvasGroup>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Register all objectives at the start
        foreach (var data in ObjectivesData)
        {
            RegisterObjective(CreateObjective(data));
        }

        if (NewObjectiveNotification != null)
        {
            NewObjectiveNotification.SetActive(false); // Ensure it's initially hidden
        }
    }

    private BaseObjective CreateObjective(ObjectiveData data)
    {
        GameObject objectiveGO = new GameObject(data.ObjectiveName);
        ConcreteObjective newObjective = objectiveGO.AddComponent<ConcreteObjective>();

        if (newObjective != null)
        {
            newObjective.ObjectiveID = data.ObjectiveID;
            newObjective.ObjectiveName = data.ObjectiveName;
            newObjective.ObjectiveDescription = data.ObjectiveDescription;
            newObjective.ObjectiveGoal = data.ObjectiveGoal;
        }
        else
        {
            Debug.LogError("Failed to create ConcreteObjective!");
        }

        return newObjective;
    }

    public void RegisterObjective(BaseObjective objective)
    {
        if (!objectives.ContainsKey(objective.ObjectiveID))
        {
            objectives.Add(objective.ObjectiveID, objective);

            GameObject objectiveUI = Instantiate(ObjectiveItemPrefab, ObjectivesContent);
            objectiveItems[objective.ObjectiveID] = objectiveUI;

            objectiveUI.SetActive(false);
        }
    }

    public void ActivateObjective(int objectiveID)
    {
        if (objectives.TryGetValue(objectiveID, out var objective))
        {
            objective.ActivateObjective();
            UpdateObjectiveDisplay(objective);

            if (objectiveItems.TryGetValue(objectiveID, out var objectiveUI))
            {
                objectiveUI.SetActive(true);
            }

            // Trigger the notification
            if (NewObjectiveNotification != null)
            {
                StartCoroutine(ShowNotification(objective.ObjectiveName));
            }
        }
    }

    public void CompleteObjective(int objectiveID)
    {
        if (objectives.TryGetValue(objectiveID, out var objective))
        {
            objective.CompleteObjective();
            UpdateObjectiveDisplay(objective);
        }
    }

    public void UpdateObjectiveDisplay(BaseObjective objective)
    {
        if (objectiveItems.TryGetValue(objective.ObjectiveID, out var objectiveUI))
        {
            var objectiveNameText = objectiveUI.transform.Find("ObjectiveNameText").GetComponent<TMP_Text>();
            var objectiveDescriptionText = objectiveUI.transform.Find("ObjectiveDescriptionText").GetComponent<TMP_Text>();
            var objectiveGoalText = objectiveUI.transform.Find("ObjectiveGoalText").GetComponent<TMP_Text>();
            var completionStatusText = objectiveUI.transform.Find("CompletionStatusText").GetComponent<TMP_Text>();

            objectiveNameText.text = objective.ObjectiveName;
            objectiveDescriptionText.text = objective.ObjectiveDescription;
            objectiveGoalText.text = objective.ObjectiveGoal;
            completionStatusText.text = objective.IsCompleted ? "Completed" : "Incomplete";

            completionStatusText.color = objective.IsCompleted ? Color.green : Color.red;
        }
    }

    private IEnumerator ShowNotification(string objectiveName)
    {
        if (notificationCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing from NewObjectiveNotification!");
            yield break;
        }

        // Set the notification text (if it has one)
        var notificationText = NewObjectiveNotification.GetComponentInChildren<TMP_Text>();
        if (notificationText != null)
        {
            notificationText.text = $"New Objective: {objectiveName}";
        }

        // Play the notification sound
        if (audioSource != null && NotificationSound != null)
        {
            audioSource.PlayOneShot(NotificationSound);
        }

        // Activate the notification
        NewObjectiveNotification.SetActive(true);

        // Fade in
        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            notificationCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / FadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        notificationCanvasGroup.alpha = 1;

        // Wait
        yield return new WaitForSeconds(NotificationDuration);

        // Fade out
        elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            notificationCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / FadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        notificationCanvasGroup.alpha = 0;

        // Deactivate the notification
        NewObjectiveNotification.SetActive(false);
    }
}