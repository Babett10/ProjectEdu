using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text questionText;
    public TMP_Text resultText;

    public Button[] answerButtons;
    public TMP_Text[] answerTexts;

    [Header("Questions")]
    public List<QuestionData> questions = new List<QuestionData>();

    private int currentQuestionIndex = 0;
    private int score = 0;

    FirebaseAuth auth;
    DatabaseReference DBreference;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        LoadQuestionFromDatabase();
    }

    // 🔄 Reset quiz
    public void ResetQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;

        ShuffleQuestions();

        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(true);
            btn.interactable = true;
        }

        resultText.text = "";

        ShowQuestion();
    }

    void LoadQuestionFromDatabase()
    {
        DBreference.Child("Quiz")
    .GetValueAsync()
    .ContinueWithOnMainThread(task =>
    {
        if (task.IsFaulted)
        {
            Debug.LogError(task.Exception);
            return;
        }

        questions.Clear();

        DataSnapshot snapshot = task.Result;

        foreach (DataSnapshot quizSnapshot in snapshot.Children)
        {
            QuestionData question = new QuestionData();

            question.id = quizSnapshot.Key;

            question.question =
                quizSnapshot.Child("question").Value.ToString();

            question.correctAnswer =
                int.Parse(
                    quizSnapshot.Child("correctAnswer")
                    .Value.ToString());

            question.answers = new List<string>();

            foreach (DataSnapshot answer in
                     quizSnapshot.Child("answers").Children)
            {
                question.answers.Add(
                    answer.Value.ToString());
            }

            questions.Add(question);
        }

        ShuffleQuestions();
        ShowQuestion();
    });
    }

    // 🔀 Random urutan soal
    void ShuffleQuestions()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            QuestionData temp = questions[i];
            int randomIndex = Random.Range(i, questions.Count);

            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }

    // 📌 Tampilkan soal
    void ShowQuestion()
    {
        // Quiz selesai
        if (currentQuestionIndex >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        QuestionData q = questions[currentQuestionIndex];

        questionText.text = q.question;
        resultText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;

            answerTexts[i].text = q.answers[i];

            int index = i;

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    // ✅ Cek jawaban
    void CheckAnswer(int selectedIndex)
    {
        QuestionData q = questions[currentQuestionIndex];

        // Disable tombol biar ga spam klik
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        // Jawaban benar
        if (selectedIndex == q.correctAnswer)
        {
            score++;

            resultText.text = "Benar!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Salah!";
            resultText.color = Color.red;
        }

        // Delay lanjut soal
        Invoke(nameof(NextQuestion), 1.2f);
    }

    // ➡️ Soal berikutnya
    void NextQuestion()
    {
        currentQuestionIndex++;
        ShowQuestion();
    }

    // 🏁 Quiz selesai
    void FinishQuiz()
    {
        questionText.text =
            "Quiz selesai!\n\nScore: " +
            score + " / " + questions.Count;

        // Pesan hasil
        if (score >= questions.Count * 0.8f)
        {
            resultText.text = "Hebat!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Coba lagi!";
            resultText.color = Color.yellow;
        }

        // Sembunyikan tombol jawaban
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        SaveQuizResults();
    }

    void SaveQuizResults()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user == null)
        {
            Debug.LogError("User tidak ditemukan");
            return;
        }

        int percentage = Mathf.RoundToInt(
            ((float)score / questions.Count) * 100);

        string resultId =
            DBreference.Child("QuizResults")
            .Child(user.UserId)
            .Push()
            .Key;

        DBreference.Child("Users")
        .Child(user.UserId)
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                string username =
                    snapshot.Child("username").Value.ToString();

                string kelas =
                    snapshot.Child("kelas").Value.ToString();

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("username")
                .SetValueAsync(username);

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("kelas")
                .SetValueAsync(kelas);

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("score")
                .SetValueAsync(score);

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("totalQuestion")
                .SetValueAsync(questions.Count);

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("percentage")
                .SetValueAsync(percentage);

                DBreference.Child("QuizResults")
                .Child(user.UserId)
                .Child(resultId)
                .Child("date")
                .SetValueAsync(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                Debug.Log("Quiz Result Saved");
            }
        });
    }
}