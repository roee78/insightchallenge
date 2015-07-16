using System;
using System.IO;
using System.Diagnostics;
namespace TweetStats
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// if the data structure argument was provided, 
			// set the application setting to the provided data structure
			if (args.Length > 0) {

				DataStructures ds;
				Enum.TryParse (args [0], out ds); 
				AppSettings.DataStructure = ds;
			}

			if (AppSettings.DataStructure == DataStructures.HashTable) 
			{
				Console.WriteLine ("Running word count using a hashtable");
			} 
			else if (AppSettings.DataStructure == DataStructures.Trie) 
			{
				Console.WriteLine ("Running word count using a trie");
			} 
			else {
				Console.WriteLine ("Running word count using a ternary tree");
			}
				
			Console.WriteLine ();

			Stopwatch sw = new Stopwatch ();
			sw.Start();

			// begin process of reading the file
			// this will trigger the running median process to begin as well
			TweetReader.ReadFile();

			// when we are done reading the file and storing the words in a data structure,
			// output the words to the output file
			WordManager.OutputWords();
			sw.Stop();

			//Console.WriteLine (sw.Elapsed);
		}
	}
}
