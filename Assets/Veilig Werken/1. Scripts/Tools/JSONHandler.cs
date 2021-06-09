using System;
using System.IO;
using System.Linq;
using MBevers;
using Newtonsoft.Json;
using UnityEngine;

namespace VeiligWerken.Tools
{
	/// <summary>
	///     This <c>class</c> is a wrapper class to read and write object to <c>JSON</c>.
	///     <para>
	///         Created by: Mathias on 6/4/2021.
	///     </para>
	/// </summary>
	public class JSONHandler
	{
		/// <summary>
		///     Save and object to a <c>JSON</c> file. Note: if a file already exists the current content gets overwritten.
		/// </summary>
		/// <typeparam name="T">Type to save as JSON.</typeparam>
		/// <param name="filePath">Path to write the <paramref name="object" /> to.</param>
		/// <param name="object">Object to write to the <paramref name="filePath" />.</param>
		public static void WriteToJSON<T>(string filePath, T @object)
		{
			// Check if the save path is set correctly.
			if (string.IsNullOrEmpty(filePath)) { throw new NullReferenceException("Save path is not set correctly."); }

			// Convert object to json and save it to a json file.
			string file = JsonConvert.SerializeObject(@object, Formatting.Indented);
			File.WriteAllText(filePath, file);

			// Log file path.
			Debug.Log($"Save successfully to: {filePath.Italic()} !");
		}

		/// <summary>
		///     Load an object from a <c>JSON</c> file.
		/// </summary>
		/// <typeparam name="T">Type to load from <c>JSON</c></typeparam>
		/// <param name="filePath">Path to load <typeparamref name="T" /> from.</param>
		/// <exception cref="FileNotFoundException">Thrown when the <paramref name="filePath" /> is invalid.</exception>
		/// <returns></returns>
		public static T ReadFromJSON<T>(string filePath)
		{
			// If the file doesn't exist, throw an error with the existing files in the directory.
			if (File.Exists(filePath)) { return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath)); }

			string existingFiles = Directory.GetFiles(Application.persistentDataPath)
				.Select(Path.GetFileName)
				.Aggregate(string.Empty, (current, fileName) => current + $"{fileName}\n");

			throw new FileNotFoundException(
				$"There is no file found with name: {filePath.Italic()} Existing files in that directory are:\n{existingFiles}");
		}
	}
}