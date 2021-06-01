using System;
using MBevers;
using UnityEngine;

namespace Prototyping
{
    public class Testing : ExtendedMonoBehaviour
    {
        [SerializeField, Range(0.0f, 360.0f)] private float direction;
        [SerializeField] private RectTransform arrow;

        private void Update()
        {
            float directionInRad = (direction + 180.0f) * Mathf.Deg2Rad;
            var windDirection = new Vector2(Mathf.Sin(directionInRad), Mathf.Cos(directionInRad));
            
            arrow.rotation = Quaternion.Euler(0, 0, -direction);
            Debug.DrawRay(arrow.pivot, windDirection * 10.0f, Color.magenta);
        }
    }
}