using MBevers;
using MBevers.Menus;
using UnityEngine;
using VeiligWerken.Menus;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 17-05-2021</para>
    /// </summary>
    public class Shelter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.HasComponent<Player>()) { return; }

            MenuManager.Instance.OpenMenu<FinishedMenu>();
            Debug.Log("Player has reached the shelter");
        }
    }
}