using System;
using System.Collections.Generic;
using System.Text;

namespace TweetStats
{
	/// <summary>
	/// This class contains the hash table implementation
	/// </summary>
	public static class Dictionary
	{
		
		// A hashtable of unique words.
		private static Dictionary<string, int> uniqueWords;

		/// <summary>
		/// Initializes the Dictionary class.
		/// </summary>
		static Dictionary ()
		{
			uniqueWords = new Dictionary<string, int>();
		}

		/// <summary>
		/// Inserts the word into the hash table
		/// </summary>
		public static void InsertWord(StringBuilder word)
		{
			string str = word.ToString ();

			if (uniqueWords.ContainsKey (str)) {
				uniqueWords [str]++;
			} else {
				uniqueWords.Add (str, 1);
			}
		}

		/// <summary>
		/// Outputs the words to the output file
		/// </summary>
		public static void OutputWords(string filePath){

			string[] sortedKeys = new string[uniqueWords.Count];
			uniqueWords.Keys.CopyTo (sortedKeys, 0);

			// we need to sort the keys first
			Array.Sort (sortedKeys);

			// open the output file and write to it line by line
			TweetWriter tw = new TweetWriter ();
			tw.OpenFile (filePath);

			for (int i = 0; i < sortedKeys.Length; i++) {

				string key = sortedKeys [i];
				string line = key.PadRight(30) + uniqueWords [key];
				tw.WriteLine (line);
			}

			tw.CloseFile ();
		}
	}
}

