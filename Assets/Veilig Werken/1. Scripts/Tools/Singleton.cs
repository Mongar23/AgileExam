using UnityEngine;

namespace MBevers
{
	/// <summary>
	/// When there should only be one instance of a MonoBehaviourExtensions class it can inherit from Singleton.
	/// It has all the same possibilities as a normal MonoBehaviourExtensions class and it makes sure there is only one of this class in the scene.
	/// </summary>
	/// <typeparam name="T">The MonoBehaviourExtensions class that has to be a Singleton</typeparam>
	public abstract class Singleton<T> : MonoBehaviourExtensions where T : Singleton<T>
	{
		/// <summary>
		/// When this is true and the Singleton is not set up correctly,
		/// all the references to the Instance will attempt to find an object with this Singleton class.
		/// </summary>
		public static bool SearchForInstance { get; set; } = true;

		
		private static T _instance = null;
        /// <summary>
        /// Returns the Singleton Instance of the requested class type.
        /// When no Instance is found, there will be an attempt to find a instance in the scene.
        /// In case there are more then one instances found, an error is thrown and null will be returned.
        /// </summary>
		public static T Instance
		{
			get
			{
				if (IsInitialized)
				{
					return _instance;
				}

				T newInstance = null;

				if (SearchForInstance)
				{
					T[] instances = FindObjectsOfType<T>();

					if (instances.Length == 1)
					{
						newInstance = instances[0];
					}
					if (instances.Length > 1)
					{
						Debug.LogError($"Only one of {typeof(T).Name} can be in the scene, there where {instances.Length} found.");
                        return null;
                    }
				}

				if (newInstance == null)
				{
					var gameObject = new GameObject()
					{
						hideFlags = HideFlags.HideAndDontSave
					};

                    newInstance = gameObject.AddComponent<T>();
                }

				Instance = newInstance;

				return _instance;
			}
			private set => _instance = value;
        }

		/// <summary>
		/// Returns the Instance if it is initialized. Otherwise it will return null.
		/// </summary>
		public static T InstanceIfInitialized => IsInitialized ? Instance : null;

		/// <summary>
		/// Returns whether if the instance is initialized or not.
		/// </summary>
		public static bool IsInitialized => _instance != null;
		
		/// <summary>
		/// The Awake method, called by Unity when the MonoBehaviour is initialized, sets up the SingletonBehavior's unique Instance. 
		/// All classes that inherit from Singleton should call base.Awake() to make sure that the Instance is set up properly.
		/// </summary>
		protected virtual void Awake()
		{
			if (!IsInitialized)
			{
				Instance = this as T;
				return;
			}

            if (Instance == this)
            {
                return;
            }

            Destroy(this);
            Debug.LogWarning($"Trying to instantiate a second instance of {typeof(T).Name}. Additional instance was destroyed!");
        }

		/// <summary>
		/// The OnDestroy method, called by Unity when the MonoBehaviour is destroyed, makes sure the unique instance is also destroyed and set to null.
		/// All classes that inherit from Singleton should call base.OnDestroy() to make sure the instance is cleaned up properly.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if(_instance == this)
			{
				_instance = null;
			}
		}
	}
}