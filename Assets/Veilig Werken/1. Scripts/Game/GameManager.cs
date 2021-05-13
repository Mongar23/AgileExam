using MBevers;
using UnityEngine;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 12-05-2021</para>
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public float WindDirection { get; private set; } = 0.0f;

        protected override void Awake()
        {
            base.Awake();
            WindDirection = Random.Range(0.0f, 360.0f);
        }
    }
}