using System;
using MBevers;
using UnityEngine;
using UnityEngine.Audio;
using VeiligWerken.Tools;

namespace VeiligWerken
{
    /// <summary>
    ///     This class handles all of the audio in the game.
    ///     <para>Created by Mathias on 17-05-2021</para>
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField, Required] private AudioMixer audioMixer;
        [SerializeField] private SoundClip[] soundClips;

        private void Start()
        {
            foreach (SoundClip soundClip in soundClips)
            {
                if(soundClip.IsSFX) { continue; }

                Play(soundClip.Name);
            }
        }

        public AudioSource Play(string clipName)
        {
            //Get sound clip from array.
            SoundClip soundClip = Array.Find(soundClips, clip => clip.Name == clipName);
            if(soundClip == null) { throw new NullReferenceException($"There is no clip named {clipName} found in the soundClips array."); }

            //Making a new GameObject
            var clipGameObject = new GameObject(clipName);
            clipGameObject.transform.SetParent(CachedTransform);

            //Adding and setting up a audio source component.
            var audioSource = clipGameObject.AddComponent<AudioSource>();
            audioSource.clip = soundClip.AudioClip;
            audioSource.volume = soundClip.Volume;
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

            audioSource.Play();

            if(soundClip.IsSFX) { Destroy(clipGameObject, audioSource.clip.length + 0.1f); }

            //Return audio source.
            return audioSource;
        }
    }
}