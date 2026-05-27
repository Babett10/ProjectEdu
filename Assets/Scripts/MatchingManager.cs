using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MatchingManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text resultText;

    [Header("Items")]
    public List<GameObject> draggableItems;

    private float timer = 0f;
    private bool gameFinished = false;

    private int matchedCount = 0;

    void Update()
    {
        if (!gameFinished)
        {
            timer += Time.deltaTime;

            timerText.text =
                "Time : " + timer.ToString("F1") + "s";
        }
    }

    // ✅ Dipanggil saat jawaban cocok
    public void AddMatch(GameObject item)
    {
        matchedCount++;

        if (matchedCount >= draggableItems.Count)
        {
            FinishGame();
        }
    }

    // 🏁 Finish
    void FinishGame()
    {
        gameFinished = true;

        int score = CalculateScore();

        scoreText.text = "Score : " + score;

        resultText.text = "Selesai!";
    }

    // 🎯 Score berdasarkan waktu
    int CalculateScore()
    {
        if (timer <= 10)
            return 100;

        if (timer <= 20)
            return 80;

        if (timer <= 30)
            return 60;

        return 40;
    }

    // 🔄 Reset game
    public void ResetGame()
    {
        timer = 0f;
        matchedCount = 0;
        gameFinished = false;

        resultText.text = "";
        scoreText.text = "";

        ShuffleItems();

        foreach (GameObject item in draggableItems)
        {
            item.GetComponent<DragItem>().enabled = true;
            item.GetComponent<DragItem>().ResetPosition();
        }
    }

    // 🔀 Random posisi item
    void ShuffleItems()
    {
        for (int i = 0; i < draggableItems.Count; i++)
        {
            int randomIndex =
                Random.Range(i, draggableItems.Count);

            Vector3 temp =
                draggableItems[i].transform.position;

            draggableItems[i].transform.position =
                draggableItems[randomIndex].transform.position;

            draggableItems[randomIndex].transform.position =
                temp;
        }
    }
}