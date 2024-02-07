using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using System.Security.Cryptography.X509Certificates;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    QuestionSO currentQuestion;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnswerdEarly = true;

    [Header("Button Colors")]
    [SerializeField] Color defaultAnswerColor, correctAnswerColor;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scroing")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    public bool isComplete = false;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            hasAnswerdEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnswerdEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void DisplayAnswer(int index)
    {
        correctAnswerIndex = currentQuestion.getCorrectAnswerIndex();
        Image buttonImage;

        if (index == currentQuestion.getCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            scoreKeeper.IncrementCorrectAnswers();
        }

        else
        {
            string correctAnswer = currentQuestion.getAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was " + correctAnswer;

            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        }
        buttonImage.color = correctAnswerColor;
    }

    public void OnAnswerSelected(int index)
    {
        hasAnswerdEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score : " + scoreKeeper.CalculateScore() + "%";

        if (progressBar.value == progressBar.maxValue)
        {
            isComplete = true;
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonColor();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.getQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            string answer = currentQuestion.getAnswer(i);

            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = answer;
        }
    }

    void SetDefaultButtonColor()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image image = answerButtons[i].GetComponent<Image>();
            image.color = defaultAnswerColor;
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
}
