using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;

    public string getQuestion()
    {
        return question;
    }

    public int getCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    public string getAnswer(int index)
    {
        return answers[index];
    }
}