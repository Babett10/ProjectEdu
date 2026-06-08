using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizItem : MonoBehaviour
{
    public TMP_Text questionText;

    private string quizId;
    private QuizManagerTeacher manager;

    public void SetData(string id, string question, QuizManagerTeacher quizManager)
    {
        quizId = id;
        manager = quizManager;
        questionText.text = question;
    }

    public void EditQuiz()
    {
        manager.OpenEditQuiz(quizId);
    }

    public void DeleteQuiz()
    {
        manager.DeleteQuiz(quizId);
    }

}
