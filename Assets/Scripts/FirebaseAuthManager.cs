using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class FirebaseAuthManager : MonoBehaviour
{
    public TMP_InputField usernameField;  // Username field
    public TMP_InputField passwordField;  // Password field
    public TMP_InputField confirmPasswordField;  // Confirm password field
    public TMP_Text messageText;
    public TMP_Text displayName;

    private DatabaseReference databaseRef;
    FirebaseAuth auth;
    FirebaseUser user;

    public LoginUIManager loginUIManager;

    void Start()
    {
        InitializeFirebase();
        HideErrorMessage();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Error;
    }

    // Register method triggered by the Register button
    public void Register()
    {
        string username = usernameField.text;  // Get the username
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            messageText.text = "Please fill in all fields.";
            ShowErrorMessage();
            return;
        }

        if (password != confirmPassword)
        {
            messageText.text = "Passwords do not match.";
            ShowErrorMessage();
            return;
        }

        string email = username + "@yourappdomain.com";

        // Use plain password for Firebase Authentication
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                messageText.text = "Registration canceled.";
                ShowErrorMessage();
                return;
            }

            if (task.IsFaulted)
            {
                messageText.text = "Error: " + task.Exception?.Message;
                ShowErrorMessage();
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;

            UserProfile profile = new UserProfile { DisplayName = username };
            newUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(profileTask =>
            {
                if (profileTask.IsCanceled)
                {
                    messageText.text = "Error setting username.";
                    ShowErrorMessage();
                    return;
                }

                if (profileTask.IsFaulted)
                {
                    messageText.text = "Error: " + profileTask.Exception?.Message;
                    ShowErrorMessage();
                    return;
                }

                // Successfully registered user, now upload user data to Firebase Realtime Database
                string hashedPassword = HashPassword(password);
                UploadUserDataToDatabase(newUser.UserId, username, hashedPassword);

                displayName.text = "Registration successful! Welcome, " + newUser.DisplayName;
                loginUIManager.ShowRegisterSuccess();
                ClearInputFields();
            });
        });
    }

    private void UploadUserDataToDatabase(string userId, string username, string hashedPassword)
    {
        // Create user object with necessary details (store hashed password)
        User newUser = new User(username, hashedPassword);
        string json = JsonUtility.ToJson(newUser);

        // Save to Firebase Realtime Database under the "Users" node
        databaseRef.Child("Users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"User registered and data uploaded successfully for userId: {userId}");
            }
            else
            {
                Debug.LogError("Failed to upload user data: " + task.Exception);
            }
        });
    }

    public class User
    {
        public string username;
        public string password;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    // Hash password using SHA-256
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert password string to byte array
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert the byte array to a hexadecimal string
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();  // Return the hashed password as a string
        }
    }

    private void ShowErrorMessage()
    {
        messageText.gameObject.SetActive(true);
        // Start a coroutine to hide the error message after 3 seconds
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

    // Clear input fields
    private void ClearInputFields()
    {
        usernameField.text = "";
        passwordField.text = "";
        confirmPasswordField.text = "";
    }
}
