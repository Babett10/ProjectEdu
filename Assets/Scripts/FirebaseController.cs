using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;
public class FirebaseController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel, notificationPanel;

    [Header("Input Fields")]
    public TMP_InputField loginEmail, loginPassword;
    public TMP_InputField signupEmail, signupPassword, signupCPassword, signupUsername;
    public TMP_InputField forgetPassEmail;

    [Header("Text UI")]
    public TMP_Text notif_Title_Text, notif_Message_Text;
    public TMP_Text profileUserName_Text, profileUserEmail_Text;

    [Header("Other")]
    public Toggle rememberMe;

    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference DBreference;


    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase gagal initialize");
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        user = auth.CurrentUser;

        if (user != null)
        {
            Debug.Log("User masih login: " + user.Email);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("Belum login");
            OpenLoginPanel();
        }
    }

    //Panel Control
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }
    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }


    public void OpenForgetPassword()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    //Register
    public void SignUpUser()
    {
        string email = signupEmail.text.Trim();
        string password = signupPassword.text.Trim();
        string confirm = signupCPassword.text.Trim();
        string username = signupUsername.text.Trim();

        if (email == "" || password == "" || confirm == "" || username == "")
        {
            ShowNotification("Error", "Semua field wajib diisi.");
            return;
        }

        if (password != confirm)
        {
            ShowNotification("Error", "Password tidak sama.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                ShowNotification("Error", "Register gagal.");
                Debug.Log(task.Exception);
                return;
            }

            user = task.Result.User;

            UserProfile profile = new UserProfile
            {
                DisplayName = username
            };

            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(profileTask =>
            {
                string uid = user.UserId;
                DBreference.Child("Users").Child(uid).Child("username").SetValueAsync(username);
                DBreference.Child("Users").Child(uid).Child("email").SetValueAsync(email);
                ShowNotification("Success", "Register berhasil!");
                OpenLoginPanel();
            });
        });
    }

    //Login User
    public void LoginUser()
    {
        string email = loginEmail.text.Trim();
        string password = loginPassword.text.Trim();

        if (email == "" || password == "")
        {
            ShowNotification("Error", "Isi email dan password.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                ShowNotification("Error", "Login gagal.");
                Debug.Log(task.Exception);
                return;
            }

            user = task.Result.User;

            user.ReloadAsync().ContinueWithOnMainThread(reloadTask =>
            {
                SceneManager.LoadScene("MainMenu");
            });
        });
    }

    //Profile
    void ShowProfile(FirebaseUser currentUser)
    {
        profileUserName_Text.text = currentUser.DisplayName;
        profileUserEmail_Text.text = currentUser.Email;

        OpenProfilePanel();
    }

    //LogOut
    public void LogOut()
    {
        auth.SignOut();

        profileUserName_Text.text = "";
        profileUserEmail_Text.text = "";

        OpenLoginPanel();
    }

    //ForgetPassword
    public void ForgetPassword()
    {
        string email = forgetPassEmail.text.Trim();

        if (email == "")
        {
            ShowNotification("Error", "Masukkan email.");
            return;
        }

        auth.SendPasswordResetEmailAsync(email)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                ShowNotification("Error", "Gagal kirim email reset.");
                return;
            }

            ShowNotification("Success", "Email reset password dikirim.");
        });
    }

    //Notification
    void ShowNotification(string title, string msg)
    {
        notif_Title_Text.text = title;
        notif_Message_Text.text = msg;
        notificationPanel.SetActive(true);
    }

    public void CloseNotification()
    {
        notificationPanel.SetActive(false);
    }

    //SaveUserData
    void SaveUserData(string uid, string username, string email)
    {
        DBreference.Child("Users").Child(uid).Child("username").SetValueAsync(username);
        DBreference.Child("Users").Child(uid).Child("email").SetValueAsync(email);
        DBreference.Child("Users").Child(uid).Child("level").SetValueAsync(1);
        DBreference.Child("Users").Child(uid).Child("gold").SetValueAsync(0);
    }

    //Load User Data
    void LoadUserData()
    {
        DBreference.Child("Users").Child(user.UserId)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                string username = snapshot.Child("username").Value.ToString();
                string email = snapshot.Child("email").Value.ToString();

                profileUserName_Text.text = username;
                profileUserEmail_Text.text = email;

                OpenProfilePanel();
            }
        });
    }
}
