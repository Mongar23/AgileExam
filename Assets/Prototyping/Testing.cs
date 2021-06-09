using System;
using System.Collections.Generic;
using System.Linq;
using MBevers;
using TMPro;
using UnityEngine;

namespace Prototyping
{
	public class Testing : ExtendedMonoBehaviour
	{
		private void Start()
		{
			string str = GetParentTypes(typeof(TextMeshProUGUI)).Aggregate(string.Empty, (current, type) => current + type.Name + "\n");
			Debug.Log(str);
		}

		private static IEnumerable<Type> GetParentTypes(Type type)
		{
			if (type == null) { throw new NullReferenceException("\"type\" parameter is null."); }

			// Return all interfaces.
			foreach (Type @interface in type.GetInterfaces()) { yield return @interface; }

			// Return all base types.
			Type currentBaseType = type.BaseType;
			while (currentBaseType != null)
			{
				yield return currentBaseType;
				currentBaseType = currentBaseType.BaseType;
			}
		}
	}
}