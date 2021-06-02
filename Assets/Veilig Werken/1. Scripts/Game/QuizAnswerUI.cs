using System;
using MBevers;
using UnityEngine.EventSystems;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 26-05-2021</para>
    /// </summary>
    public class QuizAnswerUI : ExtendedMonoBehaviour, IPointerClickHandler
    {
        private bool isCorrect = false;

        public void OnPointerClick(PointerEventData eventData) { AnswerClickedEvent?.Invoke(isCorrect); }

        public event Action<bool> AnswerClickedEvent;

        public void Initialize(bool isCorrect) { this.isCorrect = isCorrect; }
    }
}