using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MBevers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using VeiligWerken.AlarmEditor;
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

		public bool IsAlarmSequencePlaying { get; private set; } = false;

		private Dictionary<string, Alarm> loadedAlarms;

		protected override void Awake()
		{
			base.Awake();

			if (IsPendingDestroy) { return; }

			DontDestroyOnLoad(this);

			foreach (SoundClip soundClip in soundClips)
			{
				if (soundClip.IsSFX) { continue; }

				Play(soundClip.Name);
			}

			SceneManager.sceneLoaded += OnSceneLoaded;
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
		public AudioSource Play(string clipName)
		{
			// Get sound clip from array.
			SoundClip soundClip = Array.Find(soundClips, clip => clip.Name == clipName);
			if (soundClip == null) { throw new NullReferenceException($"There is no clip named {clipName.Bold()} found in the soundClips array."); }

			// Throw an ArgumentException when the sound clip is not a SFX and there is already an child object with the same name.
			if (!soundClip.IsSFX)
			{
				if (CachedTransform.Cast<Transform>().Any(child => child.name == clipName))
				{
					throw new ArgumentException(
						$"There can only be one instance of the soundClip named {clipName.Bold()}. When I should have more instances it should be an SFX");
				}
			}

			// Make a new game object named 'clipName', and set it as a child of this object. 
			var clipGameObject = new GameObject(clipName);
			clipGameObject.transform.SetParent(CachedTransform);

			// Adding and setting up a audio source component.
			var audioSource = clipGameObject.AddComponent<AudioSource>();
			audioSource.clip = soundClip.AudioClip;
			audioSource.volume = soundClip.Volume;
			audioSource.loop = !soundClip.IsSFX;
			audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

			// Play the audio
			audioSource.Play();

			// If the sound clip is a SFX destroy the game object when it is done playing.
			if (soundClip.IsSFX) { Destroy(clipGameObject, audioSource.clip.length + 0.1f); }

			// Return audio source.
			return audioSource;
		}


		/// <summary>
		///     This <c>coroutine</c> is used to play an alarm sequence. It makes sure there is enough time between the alarm beeps.
		/// </summary>
		public IEnumerator PlayAlarmSequence(Alarm alarm)
		{
			string clipName = (int)alarm.Type == 1 ? "Alarm Beep" : "Alarm Ring";
			IsAlarmSequencePlaying = true;

			AudioSource beep;

			// Play as many alarm beeps as specified in the alarm as hundred for the first number.
			for (var i = 0; i < alarm.Hundred; i++)
			{
				beep = Play(clipName);
				yield return new WaitForSeconds(beep.clip.length);
			}

			// Wait another half a second before section of the alarm is played so the total wait time is 1 second.
			yield return new WaitForSeconds(TIME_BETWEEN_ALARM_SECTIONS);

			beep = Play(clipName);

			// Wait for the next alarm section.
			yield return new WaitForSeconds(beep.clip.length + TIME_BETWEEN_ALARM_SECTIONS);

			// Play as many alarm beeps as specified in the alarm as One for the last number.
			for (var i = 0; i < alarm.One; i++)
			{
				beep = Play(clipName);
				yield return new WaitForSeconds(beep.clip.length);
			}

			IsAlarmSequencePlaying = false;
			AlarmSequenceDoneEvent?.Invoke();
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
		{
			if (scene.buildIndex != 1) { return; }

			try
			{
				loadedAlarms = JSONHandler.ReadFromJSON<Dictionary<string, Alarm>>(Application.persistentDataPath + @"\alarms.json") ??
				               new Dictionary<string, Alarm>();

				Alarm randomAlarm = loadedAlarms.RandomValue();
				StartCoroutine(PlayAlarmSequence(randomAlarm));
			}
			catch (FileNotFoundException exception)
			{
				loadedAlarms = new Dictionary<string, Alarm>();
				StartCoroutine(PlayAlarmSequence(new Alarm(2, 1, Alarm.AlarmType.Horns)));
				Debug.LogError(exception);
			}
		}

		public event Action AlarmSequenceDoneEvent;
	}
}