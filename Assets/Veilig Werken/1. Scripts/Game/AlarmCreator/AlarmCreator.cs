﻿using System;
using System.Collections.Generic;
using System.IO;
using MBevers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VeiligWerken.Tools;

namespace VeiligWerken.AlarmEditor
{
	/// <summary>
	///     This <c>class</c> is used in the Editor scene to handle the input fields of the alarm creator.
	///     <para>
	///         Created by: Mathias on 6/6/2021.
	///     </para>
	/// </summary>
	public class AlarmCreator : Singleton<AlarmCreator>
	{
		private Button saveButton = null;
		private Dictionary<string, Alarm> alarms;
		private string savePath = string.Empty;
		private TMP_Dropdown typeDropdown = null;
		private TMP_InputField hundredInputField = null;
		private TMP_InputField nameInputField = null;
		private TMP_InputField oneInputField = null;

		protected override void Awake()
		{
			base.Awake();

			// Set the save path of the alarm.
			savePath = Application.persistentDataPath + @"\alarms.json";

			// If the file already exists retrieve its data. 
			try { alarms = JSONHandler.ReadFromJSON<Dictionary<string, Alarm>>(savePath) ?? new Dictionary<string, Alarm>(); }
			catch (FileNotFoundException)
			{
				Debug.LogWarning($"{savePath.Italic()} does not exist yet, alarms is a new <string, alarm> dictionary.");
				alarms = new Dictionary<string, Alarm>();
			}

			// Get UI components.
			typeDropdown = GetComponentInChildren<TMP_Dropdown>();
			saveButton = CachedTransform.Find("SaveButton").GetComponent<Button>();
			oneInputField = CachedTransform.Find("OneInputField").GetComponent<TMP_InputField>();
			nameInputField = CachedTransform.Find("NameInputField").GetComponent<TMP_InputField>();
			hundredInputField = CachedTransform.Find("HundredInputField").GetComponent<TMP_InputField>();

			// Validate input when apply button is clicked. 
			saveButton.onClick.AddListener(ValidateInput);

			//Update alarms view.
			AlarmViewer.Instance.UpdateAlarmView(alarms);
		}

		private void ValidateInput()
		{
			if (string.IsNullOrEmpty(nameInputField.text))
			{
				StartCoroutine(AlarmEditorGeneralUI.Instance.InfoMessageCoroutine("Name has to be filled in.".Color(Color.red)));
				return;
			}

			// Get the value of the name input field as an string.
			string alarmName = nameInputField.text;

			if (string.IsNullOrEmpty(hundredInputField.text))
			{
				StartCoroutine(AlarmEditorGeneralUI.Instance.InfoMessageCoroutine("Hundred has to be filled in.".Color(Color.red)));
				return;
			}

			// Get the value of the hundred input field as an int.
			int hundred = int.Parse(hundredInputField.text);

			if (string.IsNullOrEmpty(oneInputField.text))
			{
				StartCoroutine(AlarmEditorGeneralUI.Instance.InfoMessageCoroutine("One has to be filled in.".Color(Color.red)));
				return;
			}

			// Get the value of the one input field as an int.
			int one = int.Parse(oneInputField.text);

			// Get the drop-down's int value as the alarm type.
			var type = (Alarm.AlarmType) typeDropdown.value;

			// Create a new alarm and update or add it to the alarms set.
			var alarm = new Alarm(hundred, one, type);
			if (alarms.ContainsKey(alarmName)) { alarms[alarmName] = alarm; }
			else { alarms.Add(alarmName, alarm); }

			SaveToJSON();
		}

		private void SaveToJSON()
		{
			try
			{
				JSONHandler.WriteToJSON(savePath, alarms);

				ClearFields();

				AlarmViewer.Instance.UpdateAlarmView(alarms);
				StartCoroutine(AlarmEditorGeneralUI.Instance.InfoMessageCoroutine("Saved successfully!"));
			}
			catch (NullReferenceException exception)
			{
				StartCoroutine(AlarmEditorGeneralUI.Instance.InfoMessageCoroutine(exception.Message.Color(Color.red)));
			}
		}

		public void Clear()
		{
			alarms = new Dictionary<string, Alarm>();
			SaveToJSON();
		}


		private void ClearFields()
		{
			nameInputField.text = string.Empty;
			hundredInputField.text = string.Empty;
			oneInputField.text = string.Empty;
			typeDropdown.value = 1;
		}
	}
}