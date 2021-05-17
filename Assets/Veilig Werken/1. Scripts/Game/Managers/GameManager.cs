﻿using MBevers;
using UnityEngine;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 12-05-2021</para>
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Transform[] possibleSpawnPoints;
        
        public Player Player { get; private set; }
        public float WindDirection { get; private set; } = 0.0f;

        protected override void Awake()
        {
            base.Awake();
            WindDirection = Random.Range(0.0f, 360.0f);

            GameObject playerGameObject = Instantiate(ResourceManager.Instance.PlayerPrefab, Vector3.zero, Quaternion.identity);
            Player = playerGameObject.GetComponent<Player>();
        }
    }
}