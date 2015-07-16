using System;
using System.Configuration;
using System.IO;

namespace TweetStats
{
	/// <summary>
	/// Enumeration of data structures
	/// </summary>
	public enum DataStructures
	{
		HashTable = 0,
		Trie = 1,
		TernaryTree = 2
	}

	/// <summary>
	/// Application settings
	/// </summary>
	public static class AppSettings
	{
		// Application settings
		private static readonly AppSettingsReader appSettings = new AppSettingsReader ();

		// Data structure to be used during the process. Default is hashtable
		// but can be set with command line argument
		private static DataStructures dataStructure = DataStructures.HashTable;

		// src folder
		private static string srcDirectory;

		// folder from which file will be read
		private static string inputDirectory;

		// folder which output files will be written to
		private static string outputDirectory;

		// output file which contains unique words and number of occurrences
		private static string wordCountFile;

		// output file which contains the running media
		private static string medianFile;

		// the maximum number of characters in a tweet, configurable from app.config
		private static int maxTweetLength;

		// input file of tweets which the process will read from
		private static string inputFileName;

		/// <summary>
		/// Gets the length of the max tweet.
		/// </summary>
		public static int MaxTweetLength{

			get{
				if (maxTweetLength == 0) {
					maxTweetLength = (int)appSettings.GetValue ("maxTweetLength", typeof(int));
				}

				return maxTweetLength;
			}
		}

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		public static string InputFileName{

			get{
				if (string.IsNullOrEmpty(inputFileName)) {
					inputFileName = InputDirectory + (string)appSettings.GetValue ("inputFileName", typeof(string));
				}

				return inputFileName;
			}
		}

		/// <summary>
		/// Gets the word count file.
		/// </summary>
		public static string WordCountFile{

			get{
				if (string.IsNullOrEmpty(wordCountFile)) {
					wordCountFile = OutputDirectory + (string)appSettings.GetValue ("wordCountFile", typeof(string));
				}

				return wordCountFile;
			}
		}

		/// <summary>
		/// Gets the median file.
		/// </summary>
		public static string MedianFile{

			get{
				if (string.IsNullOrEmpty(medianFile)) {
					medianFile = OutputDirectory + (string)appSettings.GetValue ("medianFile", typeof(string));
				}

				return medianFile;
			}
		}
			
		/// <summary>
		/// Gets the src directory.
		/// </summary>
		public static string SrcDirectory{

			get{
				if (string.IsNullOrEmpty(srcDirectory)) {
					
					//returns the correct value whether you run from IDE or command line
					string srcDir = "";

					DirectoryInfo info = Directory.GetParent (Directory.GetCurrentDirectory ());
					if (info.Name == "bin") {
						srcDir = info.Parent.Parent.Parent.Parent.FullName;
						srcDir += Path.DirectorySeparatorChar; 
					}

					srcDirectory = srcDir;
				}
					
				return srcDirectory;
			}
		}

		/// <summary>
		/// Gets the input directory.
		/// </summary>
		public static string InputDirectory{

			get{
				if (string.IsNullOrEmpty(inputDirectory)) {
					inputDirectory = SrcDirectory + "tweet_input" + Path.DirectorySeparatorChar;
				}

				return inputDirectory;
			}
		}

		/// <summary>
		/// Gets the output directory.
		/// </summary>
		public static string OutputDirectory{

			get{
				if (string.IsNullOrEmpty(outputDirectory)) {
					outputDirectory = SrcDirectory + "tweet_output" + Path.DirectorySeparatorChar;
				}

				return outputDirectory;
			}
		}

		/// <summary>
		/// Gets or sets the data structure.
		/// </summary>
		public static DataStructures DataStructure {
			get
			{
				return dataStructure;
			}
			set 
			{
				dataStructure = value;
			}
		}
	}
}

