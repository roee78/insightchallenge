using System;
using System.IO;
using System.Text;
namespace TweetStats
{
	/// <summary>
	/// This class in charge of writing to files
	/// </summary>
	public class TweetWriter
	{
		private const string FILE_NOT_FOUND_ERROR = "Error: File not found";
		private FileStream fs;
		private StreamWriter sw;

		/// <summary>
		/// Initializes a new instance of the TweetWriter class.
		/// </summary>
		public TweetWriter ()
		{
		}

		/// <summary>
		/// Opens the file.
		/// </summary>
		public void OpenFile(string filePath){

			try 
			{
				fs = File.Open (filePath, FileMode.Create, FileAccess.Write);
				sw = new StreamWriter(fs);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message); 
			}
		}

		/// <summary>
		/// Writes the line.
		/// </summary>
		public void WriteLine(string line){

			try
			{
				sw.WriteLine(line);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message); 
			}
		}

		/// <summary>
		/// Closes the file.
		/// </summary>
		public void CloseFile()
		{
			if (sw != null) 
			{
				sw.Close ();
				sw.Dispose ();
				sw = null;
			}

			if (fs != null) 
			{
				fs.Close ();
				fs.Dispose ();
				fs = null;
			}
		}
	}
}

