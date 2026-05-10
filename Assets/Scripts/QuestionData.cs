using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    public string id;
    public string question;
    public List<string> answers;
    public int correctAnswer;
}