using System;
using UnityEngine;

namespace MBevers
{
	/// <summary>
	/// These are some functions that can enhance the work flow with Unity's MonoBehaviour.
	/// </summary>
	public class MonoBehaviourExtensions : MonoBehaviour
	{
		private Transform cachedTransform = null;
		/// <summary>
		/// Unity cashes the transform at the C++ side of the engine. Every call you make to the transform property involves marshalling.
		/// Which has some overhead. Using this property, the transform is cashed on the C# side of the engine. This gets rid of the overhead.
		/// </summary>
		public Transform CachedTransform
		{
			get
			{
				if (cachedTransform == null)
				{
					cachedTransform = transform;
				}

				return cachedTransform;
			}
		}

		/// <summary>
		/// This property gets the CachedTransform casted to a RectTransform. This is the transform used when working with UI objects.
		/// </summary>
		public RectTransform CachedRectTransform => CachedTransform as RectTransform;

		private static Camera _cachedCamera = null;

		/// <summary>
		/// Unity's Camera.Main function loops through all the objects in the scene for a object with the tag Main Camera.
		/// With this property it will cache the camera and will only loop at initialization or when the previous camera can not be found. 
        /// </summary>
		public Camera CachedCamera
		{
			get
			{
                try
                {
                    return _cachedCamera;
                }
                catch (NullReferenceException)
				{
                    _cachedCamera = Camera.main;
                    return _cachedCamera;
                }
			}
		}

		/// <summary>
		/// Converts the current mouse position to world point. The z is always 0.
		/// </summary>
		public Vector3 MouseToWorldPosition
		{
			get
			{
				Vector3 worldPosition = CachedCamera.ScreenToWorldPoint(Input.mousePosition);
				worldPosition.z = 0;

				return worldPosition;
			}
		}

		/// <summary>
		/// This function checks if a the requested type exist on a gameObject.</summary>
		/// <typeparam name="TComponent">The type you are looking for, it has to inherit from component.</typeparam>
		/// <returns>Returns true if the looked for component exists on the gameObject</returns>
		public bool HasComponent<TComponent>() where TComponent : Component
		{
			var component = GetComponent<TComponent>();
            return component != null;
        }

		///	<summary>
		/// This function returns the requested type if it 
		/// </summary>
		/// <typeparam name="TComponent">The type you are looking for, it has to inherit from component</typeparam>
		/// <returns>Returns the requested type if it exists, otherwise it will throw a NoComponentFoundException</returns>
		public TComponent GetComponentIfInitialized<TComponent>() where TComponent : Component
		{
			var component = GetComponent<TComponent>();

			if (component == null)
			{
				throw new NullReferenceException($"{typeof(TComponent).Name} component is not initialized.");
			}

			return component;
		}
	}
}