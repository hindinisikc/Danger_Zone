using System.Collections;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class FirebaseAuthLoginManager : MonoBehaviour
{
    public TMP_InputField usernameField;  // Username field
    public TMP_InputField passwordField;  // Password field
    public TMP_Text messageText;
    public TMP_Text displayName;

    FirebaseAuth auth;

    public LoginUIManager loginUIManager;
    public MenuSelector menuSelector;

    void Start()
    {
        InitializeFirebase();
        HideErrorMessage();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Error;
    }

    // Login method triggered by the Login button
    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Please fill in both fields.";
            ShowErrorMessage();
            return;
        }

        string email = username + "@yourappdomain.com";

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                messageText.text = "Login canceled.";
                ShowErrorMessage();
                return;
            }

            if (task.IsFaulted)
            {
                messageText.text = "Error: " + task.Exception?.Message;
                ShowErrorMessage();
                return;
            }

            FirebaseUser user = task.Result.User;

            displayName.text = "Login successful! Welcome, " + user.DisplayName;
            Debug.Log(displayName);
            ClearInputFields();

            loginUIManager.ShowLoginSuccess();

            // Start the coroutine for delayed menu navigation
            StartCoroutine(DelayedLoginSuccess(menuSelector, 1.5f)); // n seconds delay
        });
    }

    private void ShowErrorMessage()
    {
        messageText.gameObject.SetActive(true);
        // Start a coroutine to hide the error message after n seconds
        StartCoroutine(HideErrorMessageWithDelay());
    }

    private IEnumerator HideErrorMessageWithDelay()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Hide the error message
        HideErrorMessage();
    }

    private void HideErrorMessage()
    {
        messageText.gameObject.SetActive(false);
    }

    public IEnumerator DelayedLoginSuccess(MenuSelector menuSelector, float delay)
    {
        yield return new WaitForSeconds(delay);
        menuSelector.LoginSuccess();
    }

    // Clear input fields
    private void ClearInputFields()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
}
