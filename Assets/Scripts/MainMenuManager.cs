using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject profilePanel;
    public TMP_Text usernameText;
    public TMP_Text emailText;
    public TMP_Text kelasText;

    public GameObject teacherPageButton;

    FirebaseAuth auth;
    DatabaseReference DBreference;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        profilePanel.SetActive(false);
        LoadUserData();
    }

    void LoadUserData()
    {
        FirebaseUser user = auth.CurrentUser;
        DBreference.Child("Users").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string role = snapshot.Child("role").Value.ToString();
                if (role == "guru")
                {
                    teacherPageButton.SetActive(true);
                }
                else
                {
                    teacherPageButton.SetActive(false);
                }
            }
        });
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
                    kelasText.text = snapshot.Child("kelas").Value.ToString();
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
        SceneManager.LoadScene("FirebaseAuth");
    }

    public void TeacherPage()
    {
        SceneManager.LoadScene("TeacherPage");
    }
}
