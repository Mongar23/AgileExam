using MBevers;
using MBevers.Menus;
using UnityEngine;
using VeiligWerken.Menus;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 13-05-2021</para>
    /// </summary>
    public class Player : ExtendedMonoBehaviour
    {
        private void Update()
        {
            if(Input.GetButtonDown("Open map")) { MenuManager.Instance.OpenMenu<FloorPlanMenu>(); }
        }
    }
}