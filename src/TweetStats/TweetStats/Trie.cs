using System;
using System.Text;
using System.Collections.Generic;

namespace TweetStats
{
	/// <summary>
	/// This class contains the trie implementation
	/// </summary>
	public class Trie
	{
		// the root node
		private static TrieNode root;

		// a handle to the TweetWriter class
		private static TweetWriter tw;

		/// <summary>
		/// Initializes the Trie class.
		/// </summary>
		static Trie ()
		{
			root = new TrieNode ();
		}

		/// <summary>
		/// Inserts the word.
		/// </summary>
		public static void InsertWord(StringBuilder word)
		{
			TrieNode node = root;
			int len = word.Length;

			for (int i = 0; i < len; i++) {

				byte ch = (byte)word[i];

				if (!node.ContainsCharacter (ch)) {
					node.AddCharacter (ch);
				} 

				node = node.GetNode (ch);
			}

			// mark this node as the last character of the word
			node.MarkEndOfWord ();
		}

		/// <summary>
		/// Helper function to open the file and output the words to the output file
		/// </summary>
		public static void OutputWords(string filePath){
			int count = 0;
			tw = new TweetWriter ();
			tw.OpenFile (filePath);

			OutputWords (root, ref count, new StringBuilder());

			tw.CloseFile ();
		}

		/// <summary>
		/// A recursive function to traverse Trie and output the words
		/// </summary>
		private static void OutputWords(TrieNode node, ref int count, StringBuilder str)
		{
			if (node == null) {
				return;
			}

			if (node.Count > 0) {
				string line = str.ToString().PadRight(30) + node.Count;
				tw.WriteLine (line);
			}

			// need to sort the characters hash table first before we traverse
			byte[] characters = node.GetSortedCharacters ();

			for (int i = 0; i < characters.Length; i++) {

				TrieNode nextCharacter = node.GetNode (characters [i]);
				str.Append ((char)characters[i]);
				OutputWords (nextCharacter, ref count, str);
				str.Remove (str.Length - 1, 1);
			}
		}
	}
}

