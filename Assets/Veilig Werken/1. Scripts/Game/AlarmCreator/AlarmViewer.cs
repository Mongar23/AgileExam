using System.Collections.Generic;
using MBevers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VeiligWerken.AlarmEditor
{
	/// <summary>
	///     <para>
	///         Created by: Mathias on 6/8/2021.
	///     </para>
	/// </summary>
	public class AlarmViewer : Singleton<AlarmViewer>
	{
		private const float ALARM_PREFAB_HEIGHT = 125.0f;

		private GridLayoutGroup existingAlarmsGroup = null;

		protected override void Awake()
		{
			base.Awake();

			GetComponentInChildren<Button>().onClick.AddListener(AlarmCreator.Instance.Clear);
		}

		public void UpdateAlarmView(Dictionary<string, Alarm> alarms)
		{
			if (existingAlarmsGroup == null) { existingAlarmsGroup = GetComponentInChildren<GridLayoutGroup>(); }

			//Clear existing view.
			foreach (Transform child in existingAlarmsGroup.transform) { Destroy(child.gameObject); }

			if (alarms == null) { return; }

			//Set content height.
			var contentRectTransform = existingAlarmsGroup.transform as RectTransform;
			contentRectTransform.sizeDelta = new Vector2(contentRectTransform.rect.width,
				(float) ((ALARM_PREFAB_HEIGHT + ALARM_PREFAB_HEIGHT * 0.1) * alarms.Count));

			// Instantiate a alarm view for each item in the alarms dictionary.
			foreach (KeyValuePair<string, Alarm> kvpAlarm in alarms)
			{
				GameObject alarmUI = Instantiate(ResourceManager.Instance.ExistingAlarmUIPrefab, existingAlarmsGroup.transform);
				alarmUI.name = kvpAlarm.Key;

				//Set name, number and type.
				var alarmUITransform = alarmUI.transform as RectTransform;
				alarmUITransform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(kvpAlarm.Key);
				alarmUITransform.Find("NumberText")
					.GetComponent<TextMeshProUGUI>()
					.SetText($"Reporter number: {kvpAlarm.Value.Hundred}1{kvpAlarm.Value.One}");
				alarmUITransform.Find("TypeText").GetComponent<TextMeshProUGUI>().SetText(kvpAlarm.Value.Type.ToString());
			}
		}
	}
}