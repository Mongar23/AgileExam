using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBevers.Menus
{
	public class MenuManager : Singleton<MenuManager>
	{
		private readonly List<Menu> menus = new List<Menu>();

		/// <summary>Counts all the menus that are open and not a HUD menu.</summary>
		public int OpenMenuCount => menus.Count(menu => menu.IsOpen && !menu.IsHUD);

		/// <summary>Returns true if the open menu count is more than 0. Ignores HUD menus.</summary>
		public bool IsAnyOpen => OpenMenuCount >= 1;

		public void RegisterMenu(Menu menu)
		{
			if (menus.Contains(menu)) { return; }

			menus.Add(menu);
		}

		/// <summary>Attempts to open the first menu of the requested menu type that is not opened. Returns the menu that is attempted to open.</summary>
		/// <typeparam name="T">Menu type that has to be opened. Requested type needs to inherit from Delirium.Tools.Menu.</typeparam>
		/// <returns>Menu that is attempted to open.</returns>
		public T OpenMenu<T>() where T : Menu
		{
			T[] menusOfTypeT = menus.OfType<T>().ToArray();

			if (menusOfTypeT.Length == 0) { return null; }

			foreach (T menuOfTypeT in menusOfTypeT)
			{
				if (menuOfTypeT.IsOpen) { continue; }

				menuOfTypeT.Open();
				return menuOfTypeT;
			}

			return null;
		}

		/// <summary>Attempts to close the first menu of the requested menu type that is not closed. Returns the menu that is attempted to close.</summary>
		/// <typeparam name="T">Menu type that has to be closed. Requested type needs to inherit from Delirium.Tools.Menu.</typeparam>
		/// <returns>Menu that is attempted to close.</returns>
		public T CloseMenu<T>() where T : Menu
		{
			T[] menusOfTypeT = menus.OfType<T>().ToArray();

			if (menusOfTypeT.Length == 0) { return null; }

			foreach (T menuOfTypeT in menusOfTypeT)
			{
				if (!menuOfTypeT.IsOpen) { continue; }

				menuOfTypeT.Close();
				return menuOfTypeT;
			}

			return null;
		}

		/// <summary>Attempts to toggle the first menu of the requested menu type. Returns the menu that is attempted to toggle.</summary>
		/// <typeparam name="T">Menu type that has to be toggled. Requested type needs to inherit from Delirium.Tools.Menu.</typeparam>
		/// <returns>Menu that is attempted to toggle.</returns>
		public T ToggleMenu<T>() where T : Menu
		{
			T[] menusOfTypeT = menus.OfType<T>().ToArray();

			if (menusOfTypeT.Length == 0) { return null; }

			foreach (T menuOfTypeT in menusOfTypeT)
			{
				if (!menuOfTypeT.IsOpen)
				{
					menuOfTypeT.Open();
					return menuOfTypeT;
				}

				menuOfTypeT.Close();
				return menuOfTypeT;
			}

			return null;
		}

		/// <summary>Returns the first menu of requested type that is registered to the MenuManager.</summary>
		/// <typeparam name="T">Menu to get. Requested type needs to inherit from Delirium.Tools.Menu.</typeparam>
		/// <returns></returns>
		public T GetMenu<T>() where T : Menu => menus.OfType<T>().FirstOrDefault();

		public void CloseAllMenus()
		{
			foreach (Menu menu in menus)
			{
				menu.Close();
			}
		}
	}
}