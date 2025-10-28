using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    // References to UI elements
    public GameObject titleLogo;
    public GameObject buttonPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject registerSuccess;
    public GameObject loginSuccess;

    // TMP Input fields
    public TMP_InputField loginUsername;
    public TMP_InputField loginPassword;
    public TMP_InputField registerUsername;
    public TMP_InputField registerPassword;
    public TMP_InputField registerConfirm;

    private void Start()
    {
        // Ensure the initial state is the default screen
        ShowDefaultScreen();
    }

    // Show the default screen with TitleLogo and ButtonPanel
    public void ShowDefaultScreen()
    {
        titleLogo.SetActive(true);
        buttonPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        registerSuccess.SetActive(false);
        loginSuccess.SetActive(false);

        // Clear all input fields
        ClearAllInputs();
    }

    public void ClearAllInputs()
    {
        loginUsername.text = "";
        loginPassword.text = "";
        registerUsername.text = "";
        registerPassword.text = "";
        registerConfirm.text = "";
    }

    // Show the Login panel and hide others
    public void ShowLoginPanel()
    {
        titleLogo.SetActive(false);
        buttonPanel.SetActive(false);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        registerSuccess.SetActive(false);
        loginSuccess.SetActive(false);

        ClearAllInputs();
    }

    // Show the Register panel and hide others
    public void ShowRegisterPanel()
    {
        titleLogo.SetActive(false);
        buttonPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        registerSuccess.SetActive(false);
        loginSuccess.SetActive(false);

        ClearAllInputs();
    }

    public void ShowRegisterSuccess()
    {
        titleLogo.SetActive(false);
        buttonPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        registerSuccess.SetActive(true);
        loginSuccess.SetActive(false);

        ClearAllInputs();
    }

    public void ShowLoginSuccess()
    {
        titleLogo.SetActive(true);
        buttonPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        registerSuccess.SetActive(false);
        loginSuccess.SetActive(true);

        ClearAllInputs();
    }

    // Button actions
    public void OnLoginButtonClicked()
    {
        ShowLoginPanel();
    }

    public void OnRegisterButtonClicked()
    {
        ShowRegisterPanel();
    }

    public void OnBackButtonClicked()
    {
        ShowDefaultScreen();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClicked();
        }
    }
}