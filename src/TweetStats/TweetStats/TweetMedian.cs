using System;
using System.Collections.Generic;
using System.IO;

namespace TweetStats
{
	/// <summary>
	/// This class is charge of calculating the running median
	/// </summary>
	public class TweetMedian
	{
		// 
		//private HashSet<List<byte>> uniqueWords;

		//
		private ulong[] medianArray;

		// 
		private ulong numberOfElements;

		// a handle to the TweetWriter class
		private TweetWriter tw;

		/// <summary>
		/// Initializes a new instance of the TweetMedian class.
		/// </summary>
		public TweetMedian ()
		{
			medianArray = new ulong[AppSettings.MaxTweetLength / 2];
			Begin();
		}

		/// <summary>
		/// TweetMedian Destructor"/>
		/// </summary>
		~TweetMedian()
		{
			End ();
		}

		/// <summary>
		/// Open file for writing
		/// </summary>
		private void Begin()
		{
			string filePath = AppSettings.MedianFile;
			Console.WriteLine ("Writing medians to " + filePath);
			Console.WriteLine ();
			tw = new TweetWriter ();
			tw.OpenFile (filePath);
		}

		/// <summary>
		/// Close the file
		/// </summary>
		private void End()
		{
			tw.CloseFile ();
		}

		/// <summary>
		/// Increments the count of array value, where index is the number of unique words in a line - 1
		/// </summary>
		public void AddNewLine(int numOfUniqueWords){

			medianArray [numOfUniqueWords - 1]++;
			numberOfElements++;

			// output median to file
			OutputMedian ();
		}

		/// <summary>
		/// Outputs the median to the output file
		/// </summary>
		private void OutputMedian()
		{
			float median = GetMedian ();

			//append new median to output file
			tw.WriteLine (median.ToString("0.00"));
		}

		/// <summary>
		/// Caclulates the running median.
		/// </summary>
		private float GetMedian()
		{
			float median = 0;
			ulong middleIndex;

			if (numberOfElements == 1) {
				middleIndex = 1;
			} else {
				middleIndex = (numberOfElements + 1) / 2;
			}

			ulong secondIndex = middleIndex + 1;
			ulong currentNumOfElements = 0;
				
			//traverse array to the middle element
			for (int i = 0; i < medianArray.Length; i++){

				currentNumOfElements += medianArray [i];

				if (currentNumOfElements >= middleIndex) {

					if (median == 0) {
						median = (float)(i+1);
					}

					//if array length is even, get average of 2 middle elements
					if (numberOfElements % 2 == 0) {

						if (currentNumOfElements >= secondIndex) {
							median = (median + (float)(i+1)) / 2;
							break;
						}
					
					//else it is odd, so return the middle element
					} else {
						break;
					}
				}
			}

			return median;
		}
	}
}

