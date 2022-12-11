using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace BadSyncProgram
{
	class Program
	{
		static string VersionString = "0.0.0";
		static string ProgramName = "BSProgram";
		static int VerbosityLvl = 3;
		static string SourceDir = "";
		static string DestDir = "";
		static void Main(string[] args)
		{
			List<string> FilesInSourceDir = new List<string>();
			List<string> FilesInDestDir = new List<string>();
			List<string> DoubleEntries = new List<string>();
			Title = $"{ProgramName} {VersionString}";
			if(args.Length > 1)
			{
				if(Directory.Exists(args[0]) && Directory.Exists(args[1]) && args[0].EndsWith("/") && args[1].EndsWith("/"))
				{
					SourceDir = args[0];
					DestDir = args[1];
				}
				else
				{
					WriteLine("\n\nOne of the paths passed as an argument is not a valid path. Paths have to end with '/'\nExiting...");
					Environment.Exit(0);
				}
			}
			else
			{
				SourceDir = GetInput("Please input the path to the source directory:");
				DestDir = GetInput("Please input the path to the destination directory:");
			}
			if(SourceDir == DestDir)
			{
				WriteLine("\nSource Directory is equal to Destination Directory\nExiting...");
				Environment.Exit(0);
			}

			FilesInSourceDir = GetAllFilesInDirectory(SourceDir, SourceDir.Length);
			if(VerbosityLvl > 0)
			{
				WriteLine($"Found {FilesInSourceDir.Count} files in Source directory");
			}
			FilesInDestDir = GetAllFilesInDirectory(DestDir, DestDir.Length);
			if(VerbosityLvl > 0)
			{
				WriteLine($"Found {FilesInDestDir.Count} files in Destination directory");
			}
			DoubleEntries = GetDoubleEntries(ref FilesInSourceDir, ref FilesInDestDir);
			if(VerbosityLvl > 0)
			{
				WriteLine($"Found {DoubleEntries.Count} files that are in both directories");
			}
		}

		static string GetInput(string sQuestion)
		{
			WriteLine(sQuestion);
			string Input = ReadLine();
			while(!Directory.Exists(Input) || !Input.EndsWith("/"))
			{
				WriteLine("Input is not a valid directory, please try again:");
				Input = ReadLine();
			}
			return Input;
		}

		static List<string> GetAllFilesInDirectory(string DirectoryPath, int dirLength)
		{
			List<string> FilesInDirectory = new List<string>();
			foreach(string subDir in Directory.GetDirectories(DirectoryPath))
			{
				List<string> subFiles = GetAllFilesInDirectory(subDir, dirLength);
				foreach(string subFile in subFiles)
				{
					FilesInDirectory.Add(subFile);
				}
			}
			foreach(string file in Directory.GetFiles(DirectoryPath))
			{
				FilesInDirectory.Add(file.Substring(dirLength));
			}
			if(VerbosityLvl > 2)
			{
				foreach(string file in FilesInDirectory)
				{
					WriteLine(file);
				}
			}
			return FilesInDirectory;
		}

		static List<string> GetDoubleEntries(ref List<string> SourceFiles, ref List<string> DestFiles)
		{
			List<string> DoubleEntries = new List<string>();
			if(SourceFiles.Count < DestFiles.Count)
			{
				foreach(string SourceFile in SourceFiles)
				{
					if(DestFiles.Contains(SourceFile))
					{
						DoubleEntries.Add(SourceFile);
					}
				}
			}
			else
			{
				foreach(string DestFile in DestFiles)
				{
					if(SourceFiles.Contains(DestFile))
					{
						DoubleEntries.Add(DestFile);
					}
				}
			}
			foreach(string file in DoubleEntries)
			{
				DestFiles.Remove(file);
				SourceFiles.Remove(file);
			}
			if(VerbosityLvl > 2)
			{
				WriteYellowLine("Files only in Source Directory:");
				foreach(string file in SourceFiles)
				{
					WriteLine(file);
				}
				WriteYellowLine("Files only in Destination Directory:");
				foreach(string file in DestFiles)
				{
					WriteLine(file);
				}
				WriteYellowLine("Files in both directories:");
				foreach(string DoubleEntry in DoubleEntries)
				{
					WriteLine(DoubleEntry);
				}
			}
			return DoubleEntries;
		}
		static void WriteYellowLine(string sLine)
		{
			ForegroundColor = ConsoleColor.Yellow;
			WriteLine(sLine);
			ResetColor();
		}
	}
}
