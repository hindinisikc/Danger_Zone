using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseLogout : MonoBehaviour
{
    private DatabaseReference databaseRef;
    private FirebaseAuth auth;
    private FirebaseUser currentUser;

    public void LogOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("User logged out successfully.");
        }
        else
        {
            Debug.LogWarning("No user is currently logged in.");
        }
    }
}
