using System;
using System.IO;
using System.Linq;
using MBevers;
using Newtonsoft.Json;
using UnityEngine;

namespace VeiligWerken.Prototyping
{
	/// <summary>
	///     This <c>class</c> is used to manage the save and load <see cref="Alarm" />s from JSON.
	///     <para>
	///         Created by: Mathias on 6/4/2021.
	///     </para>
	/// </summary>
	public class JSONManager : Singleton<JSONManager>
	{
		public void WriteToJSON<T>(string savePath, T @object)
		{
			// Check if the save path is set correctly.
			if (string.IsNullOrEmpty(savePath)) { throw new NullReferenceException("Save path is not set correctly."); }

			// Convert object to json and save it to a json file.
			string file = JsonConvert.SerializeObject(@object, Formatting.Indented);
			File.WriteAllText(savePath, file);

			// Log file path.
			Debug.Log($"Save successfully to: {savePath.Italic()} !");
		}

		public T ReadFromJSON<T>(string savePath)
		{
			// If the file doesn't exist, throw an error with the existing files in the directory.
			if (File.Exists(savePath)) { return JsonConvert.DeserializeObject<T>(File.ReadAllText(savePath)); }

			string existingFiles = Directory.GetFiles(Application.persistentDataPath)
				.Select(Path.GetFileName)
				.Aggregate(string.Empty, (current, fileName) => current + $"{fileName}\n");

			throw new FileNotFoundException(
				$"There is no file found with name: {savePath.Italic()} Existing files in that directory are:\n{existingFiles}");
		}
	}
}