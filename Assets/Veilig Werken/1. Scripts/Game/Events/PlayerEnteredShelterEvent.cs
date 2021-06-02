using UnityEngine.Events;

namespace VeiligWerken.Events
{
    /// <summary>
    ///     This <c>event</c> is invoked when a player has entered a shelter. The event returns a true when the nearest and
    ///     safest shelter has been entered.
    ///     <para>Created by Mathias on 31-05-2021</para>
    /// </summary>
    public class PlayerEnteredShelterEvent : UnityEvent<bool> { }
}