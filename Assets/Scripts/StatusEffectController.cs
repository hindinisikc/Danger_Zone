using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectController : MonoBehaviour
{
    public PlayerFireMechanic playerFire;
    public PlayerWaterDetection waterDetection;

    public Image burningStatusImage;
    public Image damagedStatusImage;
    public Image drowningStatusImage;
    public Image healedStatusImage;
    public Image smokeStatusImage;

    private Dictionary<string, Image> activeStatusEffects = new Dictionary<string, Image>();

    // Priority Order (lower index = higher priority)
    private List<string> priorityOrder = new List<string>
    {
        "Burning",
        "Drowning",
        "Smoke",
        "Damaged",
        "Healed"
    };

    private bool isDamagingEffectActive = false; // Track if a damaging effect is active

    void Start()
    {
        TurnOffAllStatus();
    }

    void Update()
    {
        UpdateBurningStatus();
        UpdateDrowningStatus();
        UpdateStatusUI();
    }

    public void TurnOffAllStatus()
    {
        burningStatusImage.gameObject.SetActive(false);
        damagedStatusImage.gameObject.SetActive(false);
        drowningStatusImage.gameObject.SetActive(false);
        healedStatusImage.gameObject.SetActive(false);
        smokeStatusImage.gameObject.SetActive(false);

        activeStatusEffects.Clear();
    }

    public void AddStatusEffect(string effectName)
    {
        Debug.Log("Adding Status Effect: " + effectName);

        // If the status effect is already active, don't add it again
        if (activeStatusEffects.ContainsKey(effectName))
        {
            Debug.Log(effectName + " is already active.");
            return; // Skip adding it again
        }

        // If no effects are currently active, add the effect
        if (activeStatusEffects.Count == 0)
        {
            Image effectImage = GetEffectImage(effectName);
            if (effectImage != null)
            {
                activeStatusEffects.Add(effectName, effectImage);
                effectImage.gameObject.SetActive(true); // Turn on the new effect immediately
            }
        }
        else
        {
            // Iterate through the priority list and remove lower-priority effects if a higher-priority one is found
            foreach (string activeEffect in activeStatusEffects.Keys.ToList())
            {
                // If this effect is lower priority than the new one, remove it
                if (priorityOrder.IndexOf(effectName) < priorityOrder.IndexOf(activeEffect))
                {
                    RemoveStatusEffect(activeEffect); // Remove the lower-priority effect
                }
            }

            // Add the new effect after cleaning up lower-priority effects
            Image effectImage = GetEffectImage(effectName);
            if (effectImage != null)
            {
                activeStatusEffects.Add(effectName, effectImage);
                effectImage.gameObject.SetActive(true); // Turn on the new effect
            }
        }
    }

    public void RemoveStatusEffect(string effectName)
    {
        Debug.Log("Removing Status Effect: " + effectName);
        if (activeStatusEffects.ContainsKey(effectName))
        {
            activeStatusEffects[effectName].gameObject.SetActive(false);
            activeStatusEffects.Remove(effectName);
            Debug.Log("Remove Status Effect: " + effectName);
        }

        if (IsDamagingStatus(effectName))
        {
            // When a damaging status is removed, allow DamagedStatus to be added next
            StartCoroutine(QueueDamagedStatus());
        }
    }

    private void UpdateBurningStatus()
    {
        if (playerFire.isBurning)
        {
            Debug.Log("Player is burning.");
            AddStatusEffect("Burning");
        }
        else
        {
            Debug.Log("Player is no longer burning.");
            RemoveStatusEffect("Burning");
        }
    }

    private void UpdateDrowningStatus()
    {
        if (waterDetection.isDrowning)
        {
            Debug.Log("Player is drowning.");
            AddStatusEffect("Drowning");
        }
        else
        {
            Debug.Log("Player is no longer drowning.");
            RemoveStatusEffect("Drowning");
        }
    }

    private bool IsDamagingStatus(string effectName)
    {
        // Check if the effect is one of the damaging statuses
        return effectName == "Burning" || effectName == "Drowning" || effectName == "Smoke";
    }

    private void UpdateStatusUI()
    {
        Debug.Log("Updating Status UI...");
        Debug.Log("Active Status Effects: " + string.Join(", ", activeStatusEffects.Keys));

        bool anyEffectActive = false;
        bool anyDamagingEffectActive = false;

        // Check for active status effects in priority order
        foreach (string effectName in priorityOrder)
        {
            if (activeStatusEffects.ContainsKey(effectName))
            {
                // Check if the effect is damaging
                if (IsDamagingStatus(effectName))
                {
                    anyDamagingEffectActive = true;
                }

                // Only show the highest priority status effect
                activeStatusEffects[effectName].gameObject.SetActive(true);
                Debug.Log($"Displaying status effect: {effectName}");
                anyEffectActive = true;
                break; // Only show the highest priority effect
            }
        }

        // If no damaging status is active, do not show the DamagedStatus
        if (!anyDamagingEffectActive)
        {
            RemoveStatusEffect("Damaged");
        }
    }

    private IEnumerator QueueDamagedStatus()
    {
        // Wait for the damaging effect to end before adding DamagedStatus
        yield return new WaitForSeconds(1f); // Adjust the delay as needed
        // If there are no other damaging effects, then add DamagedStatus
        if (isDamagingEffectActive)
        {
            AddStatusEffect("Damaged");
        }
    }

    private Image GetEffectImage(string effectName)
    {
        Image effectImage = effectName switch
        {
            "Burning" => burningStatusImage,
            "Damaged" => damagedStatusImage,
            "Drowning" => drowningStatusImage,
            "Healed" => healedStatusImage,
            "Smoke" => smokeStatusImage,
            _ => null,
        };

        Debug.Log($"GetEffectImage for {effectName}: {(effectImage != null ? "Found" : "Not Found")}");
        return effectImage;
    }

    // Assuming you have a Coroutine for delaying the healed status disappearance
    private IEnumerator ShowHealedStatusForSeconds(float duration)
    {
        healedStatusImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration); // Show it for the specified duration
        RemoveStatusEffect("Healed");
        healedStatusImage.gameObject.SetActive(false); // Hide it after the time is up
    }

    // Add a duration parameter to ShowHealedStatus
    public void ShowHealedStatus(float duration)
    {
        StartCoroutine(ShowHealedStatusForSeconds(duration)); // Pass the duration to the coroutine
    }

    private IEnumerator AutoRemoveEffect(string effectName, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveStatusEffect(effectName);
        Debug.Log(effectName + " removed automatically after " + delay + " seconds.");
    }
}