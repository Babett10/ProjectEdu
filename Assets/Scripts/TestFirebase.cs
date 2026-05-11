using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class TestFirebase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var status = task.Result;

                if (status == DependencyStatus.Available)
                {
                    Debug.Log("Firebase Ready!");
                }
                else
                {
                    Debug.LogError("Firebase Error: " + status);
                }
            });
    }

}
