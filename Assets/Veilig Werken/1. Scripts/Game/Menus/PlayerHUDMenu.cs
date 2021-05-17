using MBevers.Menus;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace VeiligWerken.Menus
{
    /// <summary>
    /// 
    /// <para>Created by Mathias on 12-05-2021</para>
    /// </summary>
    public class PlayerHUDMenu : Menu
    {
        private RectTransform compassArrow = null;

        public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
        public override bool CanBeClosed() => true;

        protected override void Start()
        {
            IsHUD = true;
            
            base.Start();

            compassArrow = Content.Find("Compass").Find("Arrow") as RectTransform;
            Debug.Assert(compassArrow != null, "compassArrow is null");
            compassArrow.rotation = Quaternion.Euler(Vector3.forward * GameManager.Instance.WindDirection);
        }
    }
}