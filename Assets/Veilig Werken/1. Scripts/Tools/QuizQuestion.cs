using System;
using MBevers;
using UnityEngine;

namespace VeiligWerken.Tools
{
    [CreateAssetMenu(fileName = "QuizQuestion", menuName = "Veilig Werken/Quiz Question", order = 0)]
    public class QuizQuestion : ScriptableObject
    {
        private const int MAX_ANSWERS = 4;

        [SerializeField, TextArea(5, 100)] private string question;
        [SerializeField] private QuizAnswer[] answers;

        public string Question => question;
        public QuizAnswer[] Answers => answers;

        private void OnValidate()
        {
            if(answers.Length <= MAX_ANSWERS) { return; }

            Array.Resize(ref answers, MAX_ANSWERS);
            throw new ArgumentException($"A quiz question can not have more than {MAX_ANSWERS.ToString().Bold()} answers");
        }

        [Serializable]
        public struct QuizAnswer
        {
            [SerializeField] private bool isRightAnswer;
            [SerializeField, TextArea(5, 100)] private string answer;

            public bool IsRightAnswer => isRightAnswer;
            public string Answer => answer;
        }
    }
}