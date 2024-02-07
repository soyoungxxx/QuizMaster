using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isAnsweringQuestion = true;
    public float fillFraction;
    public bool loadNextQuestion = true;
    float timerValue;

    [SerializeField] float timeToCompleteQuiz = 30f;
    [SerializeField] float timeToShowAnswer = 10f;

    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0)
        {
            timerValue = ChangeValue(isAnsweringQuestion);
            isAnsweringQuestion = !isAnsweringQuestion;
            // 상태 변경

            if (isAnsweringQuestion)
            {
                loadNextQuestion = true;
            }
        }

        else
        {
            fillFraction = timerValue / ChangeValue(!isAnsweringQuestion);
        }
    }

    float ChangeValue(bool isAnsweringQuestion)
    {
        if (isAnsweringQuestion)
        {
            return timeToShowAnswer;
        }
        else return timeToCompleteQuiz;
    }
}
