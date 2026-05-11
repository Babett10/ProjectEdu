using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject profilePanel;
    public TMP_Text usernameText;
    public TMP_Text emailText;

    FirebaseAuth auth;
    DatabaseReference DBreference;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        profilePanel.SetActive(false);
    }

    public void OpenProfile()
    {
        profilePanel.SetActive(true);

        FirebaseUser user = auth.CurrentUser;

        if (user == null)
        {
            Debug.LogError("User null");
            return;
        }

        DBreference.Child("Users").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    usernameText.text = snapshot.Child("username").Value.ToString();
                    emailText.text = snapshot.Child("email").Value.ToString();
                }
            }
            else
            {
                Debug.LogError("Gagal load data.");
            }
        });
    }

    public void CloseProfile()
    {
        profilePanel.SetActive(false);
    }

    public void Logout()
    {
        auth.SignOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirebaseAuth");
    }
}
