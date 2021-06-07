using System;
using System.Collections.Generic;
using System.IO;
using MBevers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VeiligWerken.Prototyping;

namespace Assets.Prototyping
{
	/// <summary>
	///     <para>
	///         Created by: Mathias on 6/6/2021 3:24:10 PM.
	///     </para>
	/// </summary>
	public class AlarmCreator : ExtendedMonoBehaviour
	{
		public Dictionary<string, Alarm> Alarms { get; private set; }

		private Button applyButton = null;
		private string savePath = string.Empty;
		private TMP_Dropdown typeDropdown = null;
		private TMP_InputField hundredInputField = null;
		private TMP_InputField nameInputField = null;
		private TMP_InputField oneInputField = null;

		private void Awake()
		{
			// Set the save path of the alarm.
			savePath = Application.persistentDataPath + @"\alarms.json";

			// If the file already exists retrieve its data. 
			try { Alarms = JSONManager.Instance.ReadFromJSON<Dictionary<string, Alarm>>(savePath); }
			catch (FileNotFoundException)
			{
				Debug.LogWarning($"{savePath.Italic()} does not exist yet, Alarms is a new string:alarm dictionary.");
				Alarms = new Dictionary<string, Alarm>();
			}

			// Get UI components.
			applyButton = GetComponentInChildren<Button>();
			typeDropdown = GetComponentInChildren<TMP_Dropdown>();
			nameInputField = CachedRectTransform.Find("Name").GetComponent<TMP_InputField>();
			hundredInputField = CachedRectTransform.Find("Hundred").GetComponent<TMP_InputField>();
			oneInputField = CachedRectTransform.Find("One").GetComponent<TMP_InputField>();

			// Validate input when apply button is clicked. 
			applyButton.onClick.AddListener(ValidateInput);
		}

		private void ValidateInput()
		{
			if (string.IsNullOrEmpty(nameInputField.text)) { throw new ArgumentNullException("Name inputfield is empty."); }

			string alarmName = nameInputField.text;

			// Get the value of the hundred input field as an it.
			int hundred = int.Parse(hundredInputField.text);
			if (hundred > 9 || hundred < 0)
			{
				hundredInputField.text = string.Empty;
				throw new ArgumentException("Hundred has an invalid input, please enter a number between 1 and 9.");
			}

			// Get the value of the one input field as an it.
			int one = int.Parse(oneInputField.text);
			if (one > 9 || one < 0)
			{
				oneInputField.text = string.Empty;
				throw new ArgumentException("Hundred has an invalid input, please enter a number between 1 and 9.");
			}

			// Get the drop-down's int value as the alarm type.
			var type = (Alarm.AlarmType) typeDropdown.value;

			// Create a new alarm and update or add it to the alarms set.
			var alarm = new Alarm(hundred, one, type);
			if (Alarms.ContainsKey(alarmName)) { Alarms[alarmName] = alarm; }
			else { Alarms.Add(alarmName, alarm); }

			JSONManager.Instance.WriteToJSON(savePath, Alarms);
		}
	}
}