using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentItem : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text classText;
    public TMP_Text emailText;

    public void SetData(string username,
                        string kelas,
                        string email)
    {
        usernameText.text = username;
        classText.text = kelas;
        emailText.text = email;
    }
}
