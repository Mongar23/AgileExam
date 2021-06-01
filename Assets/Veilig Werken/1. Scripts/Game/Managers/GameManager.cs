using System;
using System.Collections.Generic;
using System.Linq;
using MBevers;
using MBevers.Menus;
using UnityEngine;
using VeiligWerken.Menus;
using VeiligWerken.PathFinding;
using Random = UnityEngine.Random;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 12-05-2021</para>
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Transform[] possibleSpawnPoints;
        
        public Player Player { get; private set; } = null;
        public float WindDirection { get; private set; } = 0.0f;
        public int CorrectAnsweredQuestions { get; set; } = 0;
        public bool HasCompletedQuiz { get; private set; }

        public event Action<Player> PlayerSpawnedEvent;

        protected override void Awake()
        {
            base.Awake();
            WindDirection = Random.Range(0.0f, 360.0f);

            AudioManager.Instance.AlarmSequenceDoneEvent += () =>
            {
                var quizMenu = MenuManager.Instance.OpenMenu<QuizMenu>();
                quizMenu.QuizCompletedEvent += () => HasCompletedQuiz = true;
            };
        }

        private void Start() { AStar.Instance.Grid.GridCreatedEvent += OnGridCreatedEvent; }

        private void OnGridCreatedEvent()
        {
            List<Transform> validSpawnPoints =
                possibleSpawnPoints.Where(possibleSpawnPoint => AStar.Instance.Grid.GetNodeFromWorldPoint(possibleSpawnPoint.position).IsWalkable).ToList();
            Vector3 spawnPosition = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)].position;

            GameObject playerGameObject = Instantiate(ResourceManager.Instance.PlayerPrefab, spawnPosition, Quaternion.identity);
            Player = playerGameObject.GetComponent<Player>();

            PlayerSpawnedEvent?.Invoke(Player);
        }
    }
}