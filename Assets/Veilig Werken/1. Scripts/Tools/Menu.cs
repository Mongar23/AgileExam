using System;
using UnityEngine;

namespace MBevers.Menus
{
	public abstract class Menu : MonoBehaviour
	{
		public bool IsOpen { get; private set; }
		public bool IsHUD { get; protected set; } = false;

		public event Action Opened;
		public event Action Closed;

		protected Transform Content { get; private set; }

		protected virtual void Start()
		{
			if (transform.childCount > 1) { throw new NotSupportedException($"The only child of {GetType().Name} should be the \"Content\" GameObject."); }

			Content = transform.GetChild(0);
			if (IsHUD) { Open(); }
			else { Close(); }

			MenuManager.Instance.RegisterMenu(this);
		}

		public abstract bool CanBeOpened();
		public abstract bool CanBeClosed();

		
		/// <summary>Tries to open the Menu, when the menu meets the opening conditions the Opened event is invoked().</summary>
		public void Open()
		{
			if (!CanBeOpened())
			{
				Debug.LogWarning($"{GetType().Name} does not meet the opening criteria");
				return;
			}

			Content.gameObject.SetActive(true);
			IsOpen = true;
			Opened?.Invoke();
		}

		/// <summary>Tries to close the Menu, when the menu meets the closing conditions the Closed event is invoked().</summary>
		public void Close()
		{
			if (!CanBeClosed())
			{
				Debug.LogWarning($"{GetType().Name} does not meet the closing criteria");
				return;
			}

			Content.gameObject.SetActive(false);
			IsOpen = false;
			Closed?.Invoke();
		}
	}
}