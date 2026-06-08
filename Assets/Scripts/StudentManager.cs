using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class StudentManager : MonoBehaviour
{
    public Transform studentContent;
    public GameObject studentItemPrefab;

    DatabaseReference DBreference;

    void Start()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        LoadStudents();
    }

    public void LoadStudents()
    {
        DBreference.Child("Users")
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            foreach (Transform child in studentContent)
            {
                Destroy(child.gameObject);
            }

            DataSnapshot snapshot = task.Result;

            foreach (DataSnapshot user in snapshot.Children)
            {
                string role =
                    user.Child("role").Value.ToString();

                if (role != "siswa")
                    continue;

                string username =
                    user.Child("username").Value.ToString();

                string kelas =
                    user.Child("kelas").Value.ToString();

                string email =
                    user.Child("email").Value.ToString();

                GameObject item =
                    Instantiate(
                        studentItemPrefab,
                        studentContent);

                item.GetComponent<StudentItem>()
                    .SetData(
                        username,
                        kelas,
                        email);
            }
        });
    }
}
