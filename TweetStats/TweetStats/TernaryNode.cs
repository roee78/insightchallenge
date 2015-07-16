using System;

namespace TweetStats
{
	/// <summary>
	/// This class represents a node in the ternary tree implementation
	/// </summary>
	public class TernaryNode
	{
		// the character that corresponds to the current node
		private byte character;

		// the count of words that terminate at the current node
		private int count;

		// the current node's left node
		private TernaryNode left;

		// the current node's middle node
		private TernaryNode eq;

		// the current node's right node
		private TernaryNode right;

		/// <summary>
		/// Gets the character.
		/// </summary>
		public byte Character
		{
			get
			{
				return character;
			}
		}

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set 
			{
				count = value;
			}
		}

		/// <summary>
		/// Gets or sets the left.
		/// </summary>
		public TernaryNode Left
		{
			get
			{
				return left;
			}
			set 
			{
				left = value;
			}
		}

		/// <summary>
		/// Gets or sets the eq.
		/// </summary>
		public TernaryNode Eq
		{
			get
			{
				return eq;
			}
			set 
			{
				eq = value;
			}
		}

		/// <summary>
		/// Gets or sets the right.
		/// </summary>
		public TernaryNode Right
		{
			get
			{
				return right;
			}
			set 
			{
				right = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TernaryNode class.
		/// </summary>
		public TernaryNode (byte ch)
		{
			character = ch;
			count = 0;
			left = null;
			right = null;
			eq = null;
		}

		/// <summary>
		/// Marks the end of a word by incrementing the count
		/// </summary>
		public void MarkEndOfWord()
		{
			count++;
		}
	}
}

