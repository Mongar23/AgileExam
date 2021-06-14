using System;
using System.Linq;
using MBevers;
using UnityEngine;

namespace VeiligWerken.Tools
{
	[CreateAssetMenu(fileName = "QuizQuestion", menuName = "Veilig Werken/Quiz Question", order = 1)]
	public class QuizQuestion : ScriptableObject
	{
		public const int MAX_ANSWERS = 3;

		[SerializeField, TextArea(5, 100)] private string question;
		[SerializeField] private QuizAnswer[] answers;

		public string Question => question;
		public QuizAnswer[] Answers => answers;

		private void OnValidate()
		{
			if (answers.Length > MAX_ANSWERS)
			{
				Array.Resize(ref answers, MAX_ANSWERS);
				throw new ArgumentException($"A quiz question can not have more than {MAX_ANSWERS.ToString().Bold()} answers");
			}

			if (Answers.Count(quizAnswer => quizAnswer.IsRightAnswer) != 1)
			{
				throw new ArgumentException($"There should be {"1".Bold()} right answer");
			}
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