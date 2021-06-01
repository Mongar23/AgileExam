using System;
using MBevers;
using MBevers.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VeiligWerken.Tools;

namespace VeiligWerken.Menus
{
    /// <summary>
    ///     <para>Created by Mathias on 25-05-2021</para>
    /// </summary>
    public class QuizMenu : Menu
    {
        [SerializeField] private QuizQuestion[] quizQuestions;

        public event Action QuizCompletedEvent;

        private int answeredQuestions = 0;
        private int correctAnsweredQuestions = 0;

        protected override void Start()
        {
            base.Start();
            Opened += OnOpened;
        }

        private void Update()
        {
            if(!Input.GetButton("Submit")) { return; }

            if(!Content.GetChild(0).gameObject.activeInHierarchy) { return; }

            QuizCompletedEvent?.Invoke();
            Close();
        }


        protected override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen && answeredQuestions != quizQuestions.Length;
        protected override bool CanBeClosed() => true;

        private void OnOpened()
        {
            Content.GetChild(0).gameObject.SetActive(false);
            Content.GetChild(1).gameObject.SetActive(true);

            UpdateQuestions();
        }

        private void UpdateQuestions()
        {
            // Set the title with the right question number.
            var titleText = Content.FindInAllChildren("Title").GetComponent<TextMeshProUGUI>();
            titleText.SetText($"Question #{answeredQuestions + 1}");

            // Update the question text.
            var questionText = Content.FindInAllChildren("QuestionText").GetComponent<TextMeshProUGUI>();
            questionText.SetText(quizQuestions[answeredQuestions].Question);

            // Destroy all existing answers in the answer's parent object.
            var answersRectTransform = Content.FindInAllChildren("Answers") as RectTransform;
            foreach (Transform child in answersRectTransform)
            {
                child.GetComponent<QuizAnswerUI>().AnswerClickedEvent -= OnAnswerClicked;
                Destroy(child.gameObject);
            }

            // Update GridLayoutGroup to work with the current screen size.
            var answersGridLayoutGroup = answersRectTransform.GetComponent<GridLayoutGroup>();
            Rect answersSize = answersRectTransform.rect;
            answersGridLayoutGroup.cellSize = new Vector2(answersSize.width, answersSize.height / 4 * 90 / 100);
            answersGridLayoutGroup.spacing = Vector2.up * answersSize.height / 4 * 10 / 100;

            // Add for every answer in the current quiz question a new answer prefab.
            foreach (QuizQuestion.QuizAnswer quizAnswer in quizQuestions[answeredQuestions].Answers)
            {
                GameObject answerUI = Instantiate(ResourceManager.Instance.AnswerUIPrefab, answersRectTransform);

                // Initialize the quiz answer UI component.
                var quizAnswerUI = answerUI.GetComponent<QuizAnswerUI>();
                quizAnswerUI.Initialize(quizAnswer.IsRightAnswer);
                quizAnswerUI.AnswerClickedEvent += OnAnswerClicked;

                // Get the text component and set it to the answer.
                var answerText = answerUI.GetComponentInChildren<TextMeshProUGUI>();
                answerText.SetText(quizAnswer.Answer);
            }
        }

        private void OnAnswerClicked(bool correctAnswerClicked)
        {
            // Increase correct answered questions if the correct answer is clicked.
            if(correctAnswerClicked) { correctAnsweredQuestions++; }

            // Increase the answered questions amount.
            answeredQuestions++;

            // Update the there a questions left to answer, update the questions and return.
            if(answeredQuestions < quizQuestions.Length)
            {
                UpdateQuestions();
                return;
            }

            // Send the correct answered questions to the game manager. And switch screens. 
            GameManager.Instance.CorrectAnsweredQuestions = correctAnsweredQuestions;
            Content.GetChild(0).gameObject.SetActive(true);
            Content.GetChild(1).gameObject.SetActive(false);

            //Update score text.
            /*You have awnsered all questions. Your score is 7/10.*/
            var scoreText = Content.FindInAllChildren("ScoreText").GetComponent<TextMeshProUGUI>();
            scoreText.SetText($"You have awnsered all questions. Your score is {$"{correctAnsweredQuestions} / {quizQuestions.Length}".Bold()}");
        }
    }
}