using UnityEngine;

namespace VeiligWerken.Tools
{
    /// <summary>
    ///     <para>Created by Mathias on 25-05-2021</para>
    /// </summary>
    public class Alarm
    {
        public bool IsVerySeriousIncident { get; }
        public int Hundred { get; }
        public int One { get; }
        public int Ten { get; }


        public Alarm()
        {
            Hundred = Random.Range(1, 4);
            Ten = 1;
            One = Random.Range(1, 4);

            //There is a 5% chance the incident is very serious.
            IsVerySeriousIncident = Random.Range(0.0f, 100.0f) <= 5.0f;
        }
    }
}