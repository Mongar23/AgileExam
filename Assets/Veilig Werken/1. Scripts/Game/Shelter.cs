using System;
using System.Linq;
using MBevers;
using MBevers.Menus;
using UnityEngine;
using VeiligWerken.Menus;

namespace VeiligWerken
{
    /// <summary>
    ///     <para>Created by Mathias on 17-05-2021</para>
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Shelter : ExtendedMonoBehaviour
    {
        public int PathCost { get; set; } = int.MaxValue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.HasComponent<Player>()) { return; }

            var finishedMenu = MenuManager.Instance.OpenMenu<FinishedMenu>();
            finishedMenu.PlayerEnteredShelterEvent.Invoke(GetCorrectShelter() == this);
        }

        private static Shelter GetCorrectShelter()
        {
            Shelter[] shelters = FindObjectsOfType<Shelter>();
            int lowestPathCost = shelters.Select(shelter => shelter.PathCost).Prepend(int.MaxValue).Min();

            return Array.Find(shelters, shelter => shelter.PathCost == lowestPathCost);
        }
    }
}