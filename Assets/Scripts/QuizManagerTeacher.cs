using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class QuizManagerTeacher : MonoBehaviour
{
    [Header("Quiz")]
    public Transform quizContent;
    public GameObject quizItemPrefab;

    [Header("Edit Quiz")]
    public GameObject quizFormPanel;
    public TMP_InputField questionInput;
    public TMP_InputField answerAInput;
    public TMP_InputField answerBInput;
    public TMP_InputField answerCInput;
    public TMP_InputField answerDInput;

    public TMP_Dropdown correctAnswerDropdown;

    private bool isEditMode = false;


    DatabaseReference DBreference;
    private string currentQuizId = "";

    void Start()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        LoadQuiz();
    }

    public void LoadQuiz()
    {
        foreach (Transform child in quizContent)
        {
            Destroy(child.gameObject);
        }

        DBreference.Child("Quiz")
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            foreach (DataSnapshot quiz in snapshot.Children)
            {
                string quizId = quiz.Key;

                string question =
                    quiz.Child("question")
                    .Value.ToString();

                GameObject item =
                    Instantiate(
                        quizItemPrefab,
                        quizContent);

                item.GetComponent<QuizItem>()
                    .SetData(
                        quizId,
                        question,
                        this);
            }
        });
    }

    public void OpenAddQuiz()
    {
        isEditMode = false;
        currentQuizId = "";

        quizFormPanel.SetActive(true);
        questionInput.text = "";

        answerAInput.text = "";
        answerBInput.text = "";
        answerCInput.text = "";
        answerDInput.text = "";

        correctAnswerDropdown.value = 0;
    }

    public void OpenEditQuiz(string quizId)
    {
        isEditMode = true;
        currentQuizId = quizId;

        quizFormPanel.SetActive(true);

        DBreference.Child("Quiz")
        .Child(quizId)
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            questionInput.text =
                snapshot.Child("question").Value.ToString();

            answerAInput.text =
                snapshot.Child("answers").Child("0").Value.ToString();

            answerBInput.text =
                snapshot.Child("answers").Child("1").Value.ToString();

            answerCInput.text =
                snapshot.Child("answers").Child("2").Value.ToString();

            answerDInput.text =
                snapshot.Child("answers").Child("3").Value.ToString();

            correctAnswerDropdown.value =
                int.Parse(snapshot.Child("correctAnswer").Value.ToString());
        });
    }

    public bool ValidateQuiz()
    {
        if (string.IsNullOrEmpty(questionInput.text))
        {
            Debug.Log("Soal tidak boleh kosong");
            return false;
        }

        if (string.IsNullOrEmpty(answerAInput.text) ||
            string.IsNullOrEmpty(answerBInput.text) ||
            string.IsNullOrEmpty(answerCInput.text) ||
            string.IsNullOrEmpty(answerDInput.text))
        {
            Debug.Log("Semua jawaban harus diisi");
            return false;
        }

        return true;
    }

    public void SaveQuiz()
    {
        if (!ValidateQuiz())
            return;

        if (isEditMode)
            UpdateQuiz();
        else
            CreateQuiz();
    }

    public void CreateQuiz()
    {
        string quizId = DBreference.Child("Quiz").Push().Key;

        Dictionary<string, object> answers = new Dictionary<string, object>();
        answers["0"] = answerAInput.text;
        answers["1"] = answerBInput.text;
        answers["2"] = answerCInput.text;
        answers["3"] = answerDInput.text;

        Dictionary<string, object> quizData = new Dictionary<string, object>();
        quizData["question"] = questionInput.text;
        quizData["answers"] = answers;
        quizData["correctAnswer"] = correctAnswerDropdown.value;

        DBreference.Child("Quiz").Child(quizId).UpdateChildrenAsync(quizData);

        quizFormPanel.SetActive(false);
        LoadQuiz();
    }

    public void UpdateQuiz()
    {
        Dictionary<string, object> answers = new Dictionary<string, object>();
        answers["0"] = answerAInput.text;
        answers["1"] = answerBInput.text;
        answers["2"] = answerCInput.text;
        answers["3"] = answerDInput.text;

        Dictionary<string, object> quizData = new Dictionary<string, object>();
        quizData["question"] = questionInput.text;
        quizData["answers"] = answers;
        quizData["correctAnswer"] = correctAnswerDropdown.value;

        DBreference.Child("Quiz").Child(currentQuizId).UpdateChildrenAsync(quizData);

        quizFormPanel.SetActive(false);
        LoadQuiz();
    }

    public void DeleteQuiz(string quizId)
    {
        DBreference.Child("Quiz")
    .Child(quizId)
    .RemoveValueAsync()
    .ContinueWithOnMainThread(task =>
    {
        if (task.IsCompleted)
        {
            Debug.Log("Quiz berhasil dihapus");

            LoadQuiz();
        }
    });
    }

    public void CloseFormPanelQuiz()
    {
        quizFormPanel.SetActive(false);
    }

}
