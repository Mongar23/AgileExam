using MBevers;
using MBevers.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace VeiligWerken.Menus
{
    /// <summary>
    ///     <para>Created by Mathias on 13-05-2021</para>
    /// </summary>
    public class FloorPlanMenu : Menu
    {
        [SerializeField, Required] private SpriteRenderer worldMap;
        [SerializeField, Required] private Image uiMap;
        private RectTransform youAreHereMarker = null;
        private Vector2 playerPosition01 = new Vector2();

        protected override void Start()
        {
            base.Start();

            Opened += OnOpened;
            Closed += () => MenuManager.Instance.OpenMenu<PlayerHUDMenu>();

            youAreHereMarker = uiMap.rectTransform.Find("YouAreHereArrow") as RectTransform;
            Content.GetComponentInChildren<Button>()?.onClick.AddListener(Close);
        }

        public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
        public override bool CanBeClosed() => true;

        private void OnOpened()
        {
            MenuManager.Instance.CloseMenu<PlayerHUDMenu>();

            // Get the player position.
            Vector3 playerPosition = GameManager.Instance.Player.CachedTransform.position;

            // Calculate relative to the world.
            var worldMapSize = new Vector2(worldMap.sprite.rect.width * 0.1f, worldMap.sprite.rect.height * 0.1f);
            playerPosition01.x = (playerPosition.x + worldMapSize.x * 0.5f) / worldMapSize.x;
            playerPosition01.y = (playerPosition.y + worldMapSize.y * 0.5f) / worldMapSize.y;

            // Add the you are here marker to the UI version of the map.
            Vector2 uiMapSize = uiMap.rectTransform.sizeDelta;
            youAreHereMarker.anchoredPosition = new Vector2(uiMapSize.x * playerPosition01.x - uiMapSize.x * 0.5f, uiMapSize.y * playerPosition01.y - uiMapSize.y * 0.5f);
        }
    }
}