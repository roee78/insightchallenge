using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TweetStats
{
	/// <summary>
	/// This class is in charge of routing the various operations to thhe data structure that was chosen
	/// </summary>
	public static class WordManager
	{
		/// <summary>
		/// A set of unique words in A line.
		/// </summary>
		private static HashSet<string> uniqueWordsInALine;

		/// <summary>
		/// The handle to the median class.
		/// </summary>
		private static TweetMedian median;

		/// <summary>
		/// Initializes the WordManager class.
		/// </summary>
		static WordManager ()
		{
			uniqueWordsInALine = new HashSet<string> ();
			median = new TweetMedian ();
		}

		/// <summary>
		/// Routes inserting the word to the data structure chosen.
		/// </summary>
		public static void InsertWord(StringBuilder word)
		{
			// insert word into unique words in a line set
			uniqueWordsInALine.Add(word.ToString());

			// insert/update word in chosen data structure
			if (AppSettings.DataStructure == DataStructures.HashTable) 
			{
				Dictionary.InsertWord (word);
			}
			else if (AppSettings.DataStructure == DataStructures.Trie) 
			{
				Trie.InsertWord (word);
			}
			else
			{
				TernaryTree.InsertWord (word);
			}
		}

		/// <summary>
		/// Adds the new line in the median class
		/// </summary>
		public static void AddNewLine()
		{
			if (uniqueWordsInALine.Count > 0) 
			{
				median.AddNewLine (uniqueWordsInALine.Count);
				uniqueWordsInALine.Clear ();
			}
		}

		/// <summary>
		/// Signals that we are done reading the file
		/// </summary>
		public static void FinishRead ()
		{
			median.End ();
		}

		/// <summary>
		/// Routes outputting the words to the data structure chosen.
		/// </summary>
		public static void OutputWords(){

			string filePath = AppSettings.WordCountFile;

			Console.WriteLine ("Writing word count to " + filePath);
			Console.WriteLine ();

			//output words from the chosed data structure
			if (AppSettings.DataStructure == DataStructures.HashTable) 
			{
				Dictionary.OutputWords (filePath);
			}
			else if (AppSettings.DataStructure == DataStructures.Trie) 
			{
				Trie.OutputWords (filePath);
			}
			else
			{
				TernaryTree.OutputWords (filePath);
			}
		}
	}
}

