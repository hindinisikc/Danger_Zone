using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunHistoryUIController : MonoBehaviour
{
    public GameObject runHistoryScrollView;
    public GameObject scoreCriteriaView;
    public GameObject leaderboardScrollView;
    public Button viewCriteriaButton;
    public Button viewLeaderboardButton;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreCriteriaView.SetActive(false);
        leaderboardScrollView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if ScoreCriteriaPanel is active and Escape key is pressed
        if (scoreCriteriaView.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseScoreCriteria();
        }

        // Check if ScoreCriteriaPanel is active and Escape key is pressed
        if (leaderboardScrollView.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLeaderboard();
        }
    }

    public void ViewScoreCriteria()
    {
        runHistoryScrollView.SetActive(false);
        viewCriteriaButton.enabled = false;
        scoreCriteriaView.SetActive(true);
        leaderboardScrollView.SetActive(false);
    }

    public void CloseScoreCriteria()
    {
        runHistoryScrollView.SetActive(true);
        viewCriteriaButton.enabled = true;
        scoreCriteriaView.SetActive(false);
        leaderboardScrollView.SetActive(false);
    }

    public void ViewLeaderboard()
    {
        runHistoryScrollView.SetActive(false);
        viewCriteriaButton.enabled = false;
        scoreCriteriaView.SetActive(false);
        leaderboardScrollView.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        runHistoryScrollView.SetActive(true);
        viewCriteriaButton.enabled = true;
        scoreCriteriaView.SetActive(false);
        leaderboardScrollView.SetActive(false);
    }
}
