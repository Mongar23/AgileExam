using System;
using MBevers;
using UnityEngine;

namespace Prototyping
{
    public class Testing : ExtendedMonoBehaviour
    {
        [SerializeField] private GameObject serializedGameObject;
        [SerializeField, Required] private GameObject requiredGameObject;


        private void Start()
        {
            bool hasComponent = HasComponent<Transform>();
            var getComponentIfInitialized = GetComponentIfInitialized<Rigidbody>();
        }
    }
}