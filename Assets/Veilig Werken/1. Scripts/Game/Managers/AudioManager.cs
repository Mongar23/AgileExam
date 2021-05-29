using System;
using System.Collections;
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
        private const float TIME_BETWEEN_ALARM_SECTIONS = 0.5f;

        [SerializeField, Required] private AudioMixer audioMixer;
        [SerializeField] private SoundClip[] soundClips;

        public Action AlarmSequenceDoneEvent;

        protected override void Awake()
        {
            base.Awake();

            foreach (SoundClip soundClip in soundClips)
            {
                if(soundClip.IsSFX) { continue; }

                Play(soundClip.Name);
            }

            StartCoroutine(PlayAlarmSequence(3, 2));
        }

        /// <summary>
        ///     The <c>Play</c> method plays a sound based on the <paramref name="clipName" />.
        /// </summary>
        /// <param name="clipName">Name of the <see cref="SoundClip" /> that should be played.</param>
        /// <returns>Returns a <see cref="AudioSource" /> created based on the settings defined in the <see cref="SoundClip" />.</returns>
        /// <exception cref="NullReferenceException">
        ///     Thrown when there is no <see cref="SoundClip" /> in the soundClips array named
        ///     <paramref name="clipName" />.
        /// </exception>
        //TODO: make sure a non SFX sound can't be played twice.
        public AudioSource Play(string clipName)
        {
            // Get sound clip from array.
            SoundClip soundClip = Array.Find(soundClips, clip => clip.Name == clipName);
            if(soundClip == null) { throw new NullReferenceException($"There is no clip named {clipName.Bold()} found in the soundClips array."); }

            // Making a new GameObject
            var clipGameObject = new GameObject(clipName);
            clipGameObject.transform.SetParent(CachedTransform);

            // Adding and setting up a audio source component.
            var audioSource = clipGameObject.AddComponent<AudioSource>();
            audioSource.clip = soundClip.AudioClip;
            audioSource.volume = soundClip.Volume;
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

            // Play the audio
            audioSource.Play();

            // If the sound clip is a SFX destroy the game object when it is done playing.
            if(soundClip.IsSFX) { Destroy(clipGameObject, audioSource.clip.length + 0.1f); }

            // Return audio source.
            return audioSource;
        }

        private IEnumerator PlayAlarmSequence(int hundred, int one)
        {
            AudioSource beep;

            // Play as many alarm beeps as specified in the alarm as hundred for the first number.
            for (var i = 0; i < hundred; i++)
            {
                beep = Play("Alarm Beep");
                yield return new WaitForSeconds(beep.clip.length);
            }

            // Wait another half a second before section of the alarm is played so the total wait time is 1 second.
            yield return new WaitForSeconds(TIME_BETWEEN_ALARM_SECTIONS);

            beep = Play("Alarm Beep");

            // Wait for the next alarm section.
            yield return new WaitForSeconds(beep.clip.length + TIME_BETWEEN_ALARM_SECTIONS);

            // Play as many alarm beeps as specified in the alarm as One for the last number.
            for (var i = 0; i < one; i++)
            {
                beep = Play("Alarm Beep");
                yield return new WaitForSeconds(beep.clip.length);
            }

            AlarmSequenceDoneEvent?.Invoke();
        }
    }
}