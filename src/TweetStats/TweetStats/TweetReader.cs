using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TweetStats
{
	/// <summary>
	/// This class is in charge of reading the file
	/// </summary>
	public static class TweetReader
	{
		private const int SIZE = 8192;
		private const string FILE_NOT_FOUND_ERROR = "Error: File not found";
		private const byte NL = (byte)'\n';
		private const byte CR = (byte)'\r';

		/// <summary>
		/// Helper function to read the file.
		/// </summary>
		public static void ReadFile()
		{
			string filePath = AppSettings.InputFileName;
			Console.WriteLine ("Reading text from " + filePath);
			Console.WriteLine ();

			if (!File.Exists (filePath)) {
				Console.WriteLine (FILE_NOT_FOUND_ERROR);
				return;
			}

			long oldMemory = GC.GetTotalMemory (true);
			ReadFile(filePath);
			long newMemory = GC.GetTotalMemory (true);
			//Console.WriteLine(newMemory - oldMemory);
		}

		/// <summary>
		/// Reads the file.
		/// </summary>
		private static void ReadFile(string filePath){

			try
			{
				using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read ))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						StringBuilder word = new StringBuilder();
						char[] chArray = new char[SIZE];

						// read the file a block at a time
						int nRead = sr.ReadBlock(chArray, 0, SIZE);

						while (nRead > 0)
						{
							for (int i = 0; i < nRead; i++){

								//if we see a space and the length of characters > 0, then insert the word
								//and reset length of characters to 0
								if (chArray[i] == ' '){

									if (word.Length > 0){
											
										WordManager.InsertWord(word);
										word.Clear();
									}
								}
								else {

									//if we reach end of line and the length of characters > 0, then insert the word
									//and reset length of characters to 0
									if (chArray[i] == CR || chArray[i] == NL){

										if (word.Length > 0){

											WordManager.InsertWord(word);
											word.Clear();
										}

										// notify WordManager that we've encountered a new line
										if (chArray[i] == NL){

											WordManager.AddNewLine();
										}
									}
									else{
										//this means we've reached a valid character in a word, so append the character
										word.Append(chArray[i]);
									}
								}
							}

							// read next block
							nRead = sr.ReadBlock(chArray, 0, SIZE);
						}
								
						//once we reach the end of the file, there might be one more word which hasn't been handled yet
						if (word.Length > 0){
							WordManager.InsertWord(word);
						}

						// count the last line as a line even though there is no new line character
						WordManager.AddNewLine();
						WordManager.FinishRead();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine ("Error: " + ex.Message);
			}
		}
	}
}

