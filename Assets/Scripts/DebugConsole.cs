using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro for UI elements

public class DebugConsole : MonoBehaviour
{
    public GameObject consoleUI; // Reference to the UI panel for the console
    public TMP_InputField inputField; // Input field for commands
    public TextMeshProUGUI outputText; // Output text area to display command results
    public Player player;
    public ObjectivesManager objectivesManager;
    private MistakeTracker mistakeTracker;
    private NPCQuestTracker NPCQuestTracker;
    private TPWaypoint TPWaypoint;
    private bool isConsoleOpen;
    private float keyPressTimeout = 0.5f; // Time in seconds allowed between each key in the sequence
    private float lastKeyPressTime;       // Tracks time of last key press
    private int keyPressIndex = 0;        // Tracks the current step in the F+D+A sequence

    public UIController UIController;
    private GameDataManager gameDataManager;
    public ScrollRect consoleScrollRect;  // Reference to the Scroll Rect of the Console

    public GameOverScreen gameOverScreen; // Reference to the Game Over Screen if game is over

    private void Start()
    {
        consoleUI.SetActive(false);
        isConsoleOpen = false;
        Debug.Log("Is Console Open: " + isConsoleOpen);

        // Try to find MistakeTracker in the scene
        mistakeTracker = FindObjectOfType<MistakeTracker>();

        if (mistakeTracker == null)
        {
            Debug.LogError("MistakeTracker not found in the scene. Please add it.");
        }

        // Try to find NPCQuestTracker in the scene
        NPCQuestTracker = FindObjectOfType<NPCQuestTracker>();

        if (NPCQuestTracker == null)
        {
            Debug.LogError("NPCQuestTracker not found in the scene. Please add it.");
        }

        TPWaypoint = FindObjectOfType<TPWaypoint>();
        if (TPWaypoint == null)
        {
            Debug.LogError("TPWaypoint component not found in the scene. Please add it.");
        }
    }

    private void Update()
    {
        // Check for the key sequence F, D, A in order with a timeout
        if (Time.time - lastKeyPressTime > keyPressTimeout)
        {
            keyPressIndex = 0; // Reset if too much time passed since the last key press
        }

        if (keyPressIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            keyPressIndex = 1;
            lastKeyPressTime = Time.time; // Update last press time
            Debug.Log("First Key Pressed");
        }
        else if (keyPressIndex == 1 && Input.GetKeyDown(KeyCode.D))
        {
            keyPressIndex = 2;
            lastKeyPressTime = Time.time;
            Debug.Log("Second Key Pressed");
        }
        else if (keyPressIndex == 2 && Input.GetKeyDown(KeyCode.A) && !gameOverScreen.IsGameOver())
        {
            keyPressIndex = 0; // Sequence complete, reset index
            ToggleConsole();    // Open or close the console
            Debug.Log("Console Toggled");

            // Toggle waypoints visibility based on console state
            TPWaypoint.ToggleTPWaypoints(isConsoleOpen);
        }

        // Reset if any other key is pressed
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.F) && !Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.A))
        {
            keyPressIndex = 0;
        }

        // Check for Escape key to close the console
        if (consoleUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleConsole(); // Close the console
        }

        // Check if Enter key is pressed in the InputField
        if (consoleUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmit(); // Call OnSubmit when Enter is pressed
        }
    }

    public bool GetIsConsoleOpen()
    {
        return consoleUI.activeSelf; // Return whether the console is open
    }

    private void ToggleConsole()
    {
        isConsoleOpen = !isConsoleOpen; // Toggle internal variable
        consoleUI.SetActive(isConsoleOpen); // Toggle visibility of console UI

        if (isConsoleOpen)
        {
            inputField.Select(); // Focus the input field when the console opens
            TPWaypoint.ToggleTPWaypoints(true); // Show waypoints when console opens
        }
        else
        {
            TPWaypoint.ToggleTPWaypoints(false); // Hide waypoints when console closes
        }

        if (consoleUI.activeSelf)
        {
            UIController.CloseHUD();
            Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
            Cursor.visible = true;
        }
        else
        {
            UIController.OpenHUD();
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor back to the center
            Cursor.visible = false;
        }
    }

    public void OnSubmit()
    {
        string command = inputField.text; // Get the command from the input field
        ExecuteCommand(command);           // Execute the command
        inputField.text = "";              // Clear the input field

        // Re-focus on the input field to allow immediate typing
        inputField.ActivateInputField();
    }


    private void ExecuteCommand(string command)
    {
        string[] splitCommand = command.Split('(');
        string methodName = splitCommand[0].Trim();

        if (splitCommand.Length > 1)
        {
            string parameters = splitCommand[1].Trim(')', ' ');

            switch (methodName)
            {
                case "TakeDamage":
                    if (float.TryParse(parameters, out float damageAmount))
                    {
                        player.TakeDamage(damageAmount);
                        UpdateOutput($"Player took {damageAmount} damage. Current health: {player.GetCurrentHealth()}");
                    }
                    else
                    {
                        UpdateOutput("Invalid value provided for TakeDamage. Use TakeDamage(amount).");
                    }
                    break;

                case "AddHealth":
                    if (float.TryParse(parameters, out float healthAmount))
                    {
                        player.AddHealth(healthAmount);
                        UpdateOutput($"Player gained {healthAmount} health. Current health: {player.GetCurrentHealth()}");
                    }
                    else
                    {
                        UpdateOutput("Invalid value provided for AddHealth. Use AddHealth(amount).");
                    }
                    break;

                case "TP":
                    // Check if parameter is a waypoint name or coordinates
                    if (parameters.Contains(","))
                    {
                        // TP(x, y, z) logic
                        string[] parameterValues = parameters.Split(',');

                        if (parameterValues.Length == 3 &&
                            float.TryParse(parameterValues[0].Trim(), out float x) &&
                            float.TryParse(parameterValues[1].Trim(), out float y) &&
                            float.TryParse(parameterValues[2].Trim(), out float z))
                        {
                            player.Teleport(new Vector3(x, y, z));
                            UpdateOutput($"Player teleported to ({x}, {y}, {z}).");
                        }
                        else
                        {
                            UpdateOutput("Invalid parameters for TP. Use TP(x, y, z).");
                        }
                    }
                    else
                    {
                        // Attempt to teleport to a waypoint
                        TPWaypoint waypoint = FindWaypoint(parameters);
                        if (waypoint != null)
                        {
                            player.Teleport(waypoint.transform.position);
                            UpdateOutput($"Player teleported to waypoint: {waypoint.WaypointName}.");
                        }
                        else
                        {
                            UpdateOutput($"No waypoint found with the name '{parameters}'.");
                        }
                    }
                    break;

                case "ObjAct":
                    if (int.TryParse(parameters, out int objectiveIDAct))
                    {
                        ActivateObjective(objectiveIDAct);
                        UpdateOutput($"Objective {objectiveIDAct} activated.");
                    }
                    else
                    {
                        UpdateOutput("Invalid objective ID for ObjAct. Use ObjAct(objectiveID).");
                    }
                    break;

                case "ObjComp":
                    if (int.TryParse(parameters, out int objectiveIDComp))
                    {
                        CompleteObjective(objectiveIDComp);
                        UpdateOutput($"Objective {objectiveIDComp} completed.");
                    }
                    else
                    {
                        UpdateOutput("Invalid objective ID for ObjComp. Use ObjComp(objectiveID).");
                    }
                    break;

                case "AddMistake":
                    if (int.TryParse(parameters, out int mistakeCount))
                    {
                        if (mistakeTracker != null)
                        {
                            mistakeTracker.AddMistakes(mistakeCount);
                            UpdateOutput($"Added {mistakeCount} mistakes.");
                        }
                        else
                        {
                            UpdateOutput("MistakeTracker is not assigned.");
                        }
                    }
                    else
                    {
                        UpdateOutput("Invalid input for AddMistake. Use AddMistake(number).");
                    }
                    break;

                case "AddNPCQ":
                    if (int.TryParse(parameters, out int NPCQCount))
                    {
                        if (NPCQuestTracker != null)
                        {
                            NPCQuestTracker.AddNPCQuestCompleted(NPCQCount);
                            UpdateOutput($"Added {NPCQCount} Completed NPC Quests. ");
                        }
                        else
                        {
                            UpdateOutput("NPCQuestTracker is not assigned");
                        }
                    }
                    else
                    {
                        UpdateOutput("Invalid input for AddNPCQ. Use AddNPCQ(number).");
                    }
                    break;

                case "Help":
                    UpdateOutput("Command List:");
                    UpdateOutput("TakeDamage(number) subtract certain number of Health from PlayerHP");
                    UpdateOutput("AddHealth(number) add certain number of Health to PlayerHP");
                    UpdateOutput("TP(x, y ,z) to teleport to specified coordinates");
                    UpdateOutput("TP(WaypointCode) to teleport to specified waypoint");
                    UpdateOutput("ObjAct(number) to activate certain ObjectiveID");
                    UpdateOutput("ObjComp(number) to complete certain ObjectiveID");
                    UpdateOutput("AddMistake(number) to add certain number of mistake to MistakeCount");
                    UpdateOutput("AddNPCQ(number) to add certain number of NPC Quests Completed");
                    break;

                case "Die":
                    player.Die();
                    break;

                default:
                    UpdateOutput("Invalid Command.");
                    break;
            }
        }
        else
        {
            UpdateOutput("No value provided.");
        }
    }


    private TPWaypoint FindWaypoint(string waypointName)
    {
        TPWaypoint[] waypoints = FindObjectsOfType<TPWaypoint>();
        foreach (TPWaypoint waypoint in waypoints)
        {
            if (waypoint.WaypointName.Equals(waypointName, System.StringComparison.OrdinalIgnoreCase))
            {
                return waypoint;
            }
        }
        return null;
    }

    private void ActivateObjective(int objectiveID)
    {
        ObjectivesManager.Instance.ActivateObjective(objectiveID); // Access as a static property
    }

    private void CompleteObjective(int objectiveID)
    {
        ObjectivesManager.Instance.CompleteObjective(objectiveID); // Access as a static property
    }

    public void UpdateOutput(string message)
    {
        outputText.text += message + "\n";  // Append the message

        // Force the scroll to the bottom
        Canvas.ForceUpdateCanvases();
        consoleScrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }
}
