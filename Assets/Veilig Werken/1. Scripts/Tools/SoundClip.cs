using System;
using MBevers;
using UnityEngine;

namespace VeiligWerken.Tools
{
    /// <summary>
    ///     <para>Created by Mathias on 17-05-2021</para>
    /// </summary>
    [CreateAssetMenu(fileName = "SoundClip", menuName = "Veilig Werken/Sound Clip")]
    public class SoundClip : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField, Required] private AudioClip audioClip;
        [SerializeField, Range(0.0f, 1.0f)] private float volume;
        [SerializeField] private bool isSFX;

        public string Name => name;
        public AudioClip AudioClip => audioClip;
        public float Volume => volume;
        public bool IsSFX => isSFX;
    }
}