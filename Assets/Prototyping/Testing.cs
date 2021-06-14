using MBevers;
using TMPro;
using UnityEngine;

namespace Prototyping
{
	public class Testing : ExtendedMonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private Color color;
		private string textValue;

		private void OnValidate()
		{
			if (text == null) { return; }

			textValue = $"Color: #{ColorUtility.ToHtmlStringRGB(color).Color(color)}\n r:{color.r}, g:{color.g}, b:{color.b}, a:{color.a}";
			text.SetText(textValue);
		}
	}
}