using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    void Start()
    {
        LoadDummyQuestions();

        // 🔥 Acak soal
        ShuffleQuestions();

        ShowQuestion();
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

    // 🔥 Dummy data sementara
    void LoadDummyQuestions()
    {
        questions.Add(new QuestionData
        {
            id = "1",
            question = "Apa yang dimaksud dengan literasi digital?",
            answers = new List<string>
            {
                "Kemampuan menggunakan perangkat elektronik saja",
                "Kemampuan membaca dan menulis di internet",
                "Kemampuan memahami, menggunakan, dan mengevaluasi informasi digital",
                "Kemampuan bermain media sosial"
            },
            correctAnswer = 2
        });

        questions.Add(new QuestionData
        {
            id = "2",
            question = "Apa tujuan utama dari literasi digital?",
            answers = new List<string>
            {
                "Menjadi terkenal di media sosial",
                "Menghindari penggunaan teknologi",
                "Menggunakan teknologi secara bijak dan bertanggung jawab",
                "Menguasai semua aplikasi"
            },
            correctAnswer = 2
        });

        questions.Add(new QuestionData
        {
            id = "3",
            question = "Apa yang dimaksud dengan jejak digital (digital footprint)?",
            answers = new List<string>
            {
                "Data yang tersimpan di komputer saja",
                "Aktivitas pengguna di dunia digital yang terekam",
                "File yang dihapus dari perangkat",
                "Aplikasi yang digunakan"
            },
            correctAnswer = 1
        });

        questions.Add(new QuestionData
        {
            id = "4",
            question = "Manakah yang termasuk pelanggaran etika digital?",
            answers = new List<string>
            {
                "Mengutip sumber dengan benar",
                "Menggunakan kata-kata sopan",
                "Menyebarkan konten tanpa izin pemilik",
                "Memberikan kritik yang membangun"
            },
            correctAnswer = 2
        });

        questions.Add(new QuestionData
        {
            id = "5",
            question = "Jika seseorang menemukan informasi yang meragukan di internet, tindakan yang tepat adalah…",
            answers = new List<string>
            {
                "Menyebarkannya agar cepat viral",
                "Menghapus akun media sosial",
                "Memverifikasi kebenaran informasi tersebut",
                "Mengabaikan semua informasi"
            },
            correctAnswer = 2
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
    }
}