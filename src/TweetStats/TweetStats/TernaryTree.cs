using System;
using System.Text;
using System.Collections.Generic;

namespace TweetStats
{
	/// <summary>
	/// This class contains the ternary tree implementation
	/// </summary>
	public class TernaryTree
	{
		// the root node
		private static TernaryNode root;

		// a handle to the TweetWriter class
		private static TweetWriter tw;

		/// <summary>
		/// Initializes the TernaryTree class.
		/// </summary>
		static TernaryTree ()
		{
			root = null;	
		}

		/// <summary>
		/// Helper function to insert the word.
		/// </summary>
		public static void InsertWord(StringBuilder word)
		{
			root = InsertWord(root, word, 0);
		}

		/// <summary>
		/// Recursive function to insert the word into the tree
		/// </summary>
		private static TernaryNode InsertWord(TernaryNode node, StringBuilder word, int index)
		{
			// Base Case: end of word
			if (index >= word.Length) {
				return node;
			}

			byte ch = (byte)word [index];

			if (node == null) {
				node = new TernaryNode (ch);
			}

			// If current character of word is smaller than node's character,
			// then insert this word in left subtree of node
			if (ch < node.Character) {
				node.Left = InsertWord(node.Left, word, index);
			}

			// If current character of word is greater than node's character,
			// then insert this word in right subtree of node
			else if (ch > node.Character) {
				node.Right = InsertWord(node.Right, word, index);
			}

			// else, current character of word is same as node's character
			else
			{
				// move on to the next character of the word
				if (index + 1 < word.Length) {
					node.Eq = InsertWord (node.Eq, word, index + 1);
				}

				// mark this node as the last character of the word
				else{
					node.MarkEndOfWord ();
				}
			}

			return node;
		}

		/// <summary>
		/// Helper function to open the file and output the words to the output file
		/// </summary>
		public static void OutputWords(string filePath)
		{
			char[] buffer = new char[AppSettings.MaxTweetLength];

			tw = new TweetWriter ();
			tw.OpenFile (filePath);

			OutputWords (root, buffer, 0);

			tw.CloseFile ();
		}

		/// <summary>
		/// A recursive function to traverse Ternary Search Tree and output the words
		/// </summary>
		private static void OutputWords(TernaryNode node, char[] buffer, int depth)
		{
			if (node != null)
			{
				// First traverse the left subtree
				OutputWords(node.Left, buffer, depth);

				// Store the character of this node
				buffer[depth] = (char)node.Character;

				if (node.Count > 0)
				{
					PrintWord (buffer, depth, node.Count);
				}

				// Traverse the middle subtree
				OutputWords(node.Eq, buffer, depth + 1);

				// Finally Traverse the right subtree
				OutputWords(node.Right, buffer, depth);
			}
		}

		/// <summary>
		/// Prints the word.
		/// </summary>
		private static void PrintWord (char[] buffer, int depth, int nodeCount)
		{
			// build a string from the character buffer 
			StringBuilder str = new StringBuilder();

			for (int i = 0; i <= depth; i++) {
				str.Append (buffer [i]);
			}

			// output the line
			string line = str.ToString().PadRight(30) + nodeCount;
			tw.WriteLine (line);
		}
	}
}

