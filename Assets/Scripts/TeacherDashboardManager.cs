using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class TeacherDashboardManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject studentPanel;
    public GameObject materiPanel;
    public GameObject quizPanel;
    public GameObject historyPanel;

    void Start()
    {
        OpenStudentPanel();
    }

    public void OpenStudentPanel()
    {
        HideAllPanels();
        studentPanel.SetActive(true);
    }

    public void OpenMaterialPanel()
    {
        HideAllPanels();
        materiPanel.SetActive(true);
    }

    public void OpenQuizPanel()
    {
        HideAllPanels();
        quizPanel.SetActive(true);

    }

    public void OpenHistoryPanel()
    {
        HideAllPanels();
        historyPanel.SetActive(true);
    }

    void HideAllPanels()
    {
        studentPanel.SetActive(false);
        materiPanel.SetActive(false);
        quizPanel.SetActive(false);
        historyPanel.SetActive(false);
    }
    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
