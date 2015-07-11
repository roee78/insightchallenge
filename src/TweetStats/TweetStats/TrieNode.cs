using System;
using System.Collections.Generic;

namespace TweetStats
{
	/// <summary>
	/// This class represents a node in the trie implementation
	/// </summary>
	public class TrieNode
	{
		/// <summary>
		// the count of words that terminate at the current node
		/// </summary>
		private int count;

		/// <summary>
		/// Dictionary of child nodes, where the key is the character in the word
		/// </summary>
		private Dictionary<byte, TrieNode> characters;

		/// <summary>
		/// public member which returns the count
		/// </summary>
		public int Count
		{
			get 
			{
				return count;
			}
		}

		/// <summary>
		/// Gets the characters.
		/// </summary>
		public Dictionary<byte, TrieNode> Characters
		{
			get
			{
				return characters;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TrieNode class.
		/// </summary>
		public TrieNode ()
		{
			characters = new Dictionary<byte, TrieNode> ();
			count = 0;
		}

		/// <summary>
		/// Returns the child Trie node based on the character passed in
		/// </summary>
		public TrieNode GetNode (byte ch)
		{
			return characters [ch];
		}

		/// <summary>
		/// Returns true if the node's characters dictionary contains
		/// the character passed in. Otherwise, returns false.
		/// </summary>
		public bool ContainsCharacter(byte ch)
		{
			return characters.ContainsKey (ch);
		}

		/// <summary>
		/// Adds a new character to the characters dictionary
		/// </summary>
		/// <param name="ch">Ch.</param>
		public void AddCharacter(byte ch)
		{
			characters.Add (ch, new TrieNode ());
		}

		/// <summary>
		/// Marks the end of a word by incrementing the count
		/// </summary>
		public void MarkEndOfWord()
		{
			count++;
		}

		/// <summary>
		/// sorts the character hash table keys and returns them
		/// </summary>
		public byte[] GetSortedCharacters ()
		{
			byte[] keys = new byte[characters.Count];
			characters.Keys.CopyTo (keys, 0);

			Array.Sort (keys);
			return keys;
		}

	}
}

