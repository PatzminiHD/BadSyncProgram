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
		static int VerbosityLvl = 1;
		static void Main(string[] args)
		{
			string SourceDir = "", DestDir = "";
			List<string> FilesInSourceDir = new List<string>();
			List<string> FilesInDestDir = new List<string>();
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

			FilesInSourceDir = GetAllFilesInDirectory(SourceDir);
			if(VerbosityLvl > 0)
			{
				WriteLine($"Found {FilesInSourceDir.Count} files in Source Directory");
			}
			FilesInDestDir = GetAllFilesInDirectory(DestDir);
			if(VerbosityLvl > 0)
			{
				WriteLine($"Found {FilesInDestDir.Count} files in Destination Directory");
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

		static List<string> GetAllFilesInDirectory(string DirectoryPath)
		{
			List<string> FilesInDirectory = new List<string>();
			foreach(string subDir in Directory.GetDirectories(DirectoryPath))
			{
				List<string> subFiles = GetAllFilesInDirectory(subDir);
				foreach(string subFile in subFiles)
				{
					FilesInDirectory.Add(subFile);
				}
			}
			foreach(string file in Directory.GetFiles(DirectoryPath))
			{
				FilesInDirectory.Add(file);
			}
			if(VerbosityLvl > 1)
			{
				foreach(string file in FilesInDirectory)
				{
					WriteLine(file);
				}
			}
			return FilesInDirectory;
		}
	}
}
