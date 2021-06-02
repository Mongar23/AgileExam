using MBevers;
using UnityEngine;

namespace VeiligWerken
{
    /// <summary>
    ///     This class loads all of the objects out of the resources folder and stores them.
    ///     <para>Created by Mathias on 14-05-2021</para>
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        public GameObject PlayerPrefab { get; private set; }
        public GameObject AnswerUIPrefab { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerPrefab = Resources.Load<GameObject>("Player");
            AnswerUIPrefab = Resources.Load<GameObject>("AnswerUI");
        }
    }
}