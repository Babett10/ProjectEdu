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

    // Posisi spawn asli item
    private List<Vector3> originalPositions =
        new List<Vector3>();

    void Start()
    {
        // Simpan posisi awal item
        foreach (GameObject item in draggableItems)
        {
            originalPositions.Add(item.transform.position);
        }

        ResetGame();
    }

    void Update()
    {
        if (!gameFinished)
        {
            timer += Time.deltaTime;

            timerText.text =
                "Time : " + timer.ToString("F1") + "s";
        }
    }

    public void AddMatch(GameObject item)
    {
        matchedCount++;

        if (matchedCount >= draggableItems.Count)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        gameFinished = true;

        int score = CalculateScore();

        scoreText.text = "Score : " + score;
        resultText.text = "Selesai!";
    }

    int CalculateScore()
    {
        if (timer <= 10) return 100;
        if (timer <= 20) return 80;
        if (timer <= 30) return 60;

        return 40;
    }

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
            DragItem dragItem =
                item.GetComponent<DragItem>();

            dragItem.ResetItem();
        }
    }

    void ShuffleItems()
    {
        // Copy posisi asli
        List<Vector3> availablePositions =
            new List<Vector3>(originalPositions);

        foreach (GameObject item in draggableItems)
        {
            int randomIndex =
                Random.Range(0, availablePositions.Count);

            Vector3 randomPos =
                availablePositions[randomIndex];

            item.transform.position = randomPos;

            item.GetComponent<DragItem>()
                .SetStartPosition(randomPos);

            availablePositions.RemoveAt(randomIndex);
        }
    }
}