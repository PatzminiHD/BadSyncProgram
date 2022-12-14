using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace BadSyncProgram
{
	class Program
	{
		static string VersionString = "0.1.0";
		static string ProgramName = "BSProgram";
		static int VerbosityLvl = 0;
		static bool CompDoubleEntries = false;
		static bool ShowProgress = false;
		static bool DeleteExtraFiles = false;
		static string SourceDir = "";
		static string DestDir = "";

		static void Main(string[] args)
		{
			List<string> FilesInSourceDir = new List<string>();
			List<string> FilesInDestDir = new List<string>();
			List<string> DoubleEntries = new List<string>();
			List<string> CmdArgs = new List<string>();
			Title = $"{ProgramName} {VersionString}";
			GetArgs(args);
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
				WriteLine("\n\nNot enough arguments given\nExiting...");
				Environment.Exit(0);
			}
			if(SourceDir == DestDir)
			{
				WriteLine("\nSource Directory is equal to Destination Directory\nExiting...");
				Environment.Exit(0);
			}

			WriteYellowLine("Searching Files in Source Directory...");
			FilesInSourceDir = GetAllFilesInDirectory(SourceDir, SourceDir.Length);
			if(VerbosityLvl > 0)
			{
				WriteLine($"\nFound {FilesInSourceDir.Count} file(s) in Source directory");
			}

			WriteYellowLine("Searching Files in Destination Directory...");
			FilesInDestDir = GetAllFilesInDirectory(DestDir, DestDir.Length);
			if(VerbosityLvl > 0)
			{
				WriteLine($"\nFound {FilesInDestDir.Count} file(s) in Destination directory");
			}

			WriteYellowLine("Searching for files that exist in both directories...");
			DoubleEntries = GetDoubleEntries(ref FilesInSourceDir, ref FilesInDestDir);
			if(VerbosityLvl > 0)
			{
				WriteLine($"\nFound {DoubleEntries.Count} file(s) that are in both directories");
			}
			if(CompDoubleEntries)
			{
				WriteYellowLine("Comparing duplicate files...");
				DoubleEntries = CompareDoubleEntries(DoubleEntries);
				if(VerbosityLvl > 0)
				{
					WriteLine($"\nFound {DoubleEntries.Count} file(s) that are in both dirs and different");
				}
			}
			WriteYellowLine($"\nCopying {FilesInSourceDir.Count} Files...");
			CopyFiles(FilesInSourceDir);
		}

		static void CreateSubDir(string filepath)
		{
			Directory.CreateDirectory(filepath.Substring(0, filepath.LastIndexOf('/')));
		}
		
		static void CopyFiles(List<string> files)
		{
			int filesLength = files.Count;
			WriteLine();
			for(int i = 0; i < filesLength; i++)
			{
				CreateSubDir(DestDir + files[i]);
				File.Copy(SourceDir + files[i], DestDir + files[i]);
				if(VerbosityLvl > 2 && ShowProgress)
				{
					SetCursorPosition(0, CursorTop - 1);
					Write($"Copying {files[i]}...");
					for(int j = CursorLeft; j < WindowWidth; j++)
					{
						Write(' ');
					}
					Write($"\n{i * 100 / ((double)filesLength - 1)}%");
					for(int j = CursorLeft; j < WindowWidth; j++)
					{
						Write(' ');
					}
				}
				else if(ShowProgress)
				{
					SetCursorPosition(0, CursorTop);
					Write($"{i * 100 / ((double)filesLength - 1)}%");
				}
				else if(VerbosityLvl > 2)
				{
					SetCursorPosition(0, CursorTop);
					Write($"Copying {files[i]}...");
				}
			}
		}

		static void WriteSpaceToLineEnd()
		{
			for(int i = CursorLeft; i < WindowWidth - 1; i++)
			{
				Write(' ');
			}
		}

		static void DeleteFiles(List<string> files)
		{
			int filesLength = files.Count;
			for(int i = 0; i < filesLength; i++)
			{
				File.Delete(DestDir + files[i]);
				if(VerbosityLvl > 2 && ShowProgress)
				{
					SetCursorPosition(0, CursorTop - 1);
					Write($"Deleting {files[i]}...");
					WriteSpaceToLineEnd();
					Write($"\n{i * 100 / ((double)filesLength - 1)}%");
					WriteSpaceToLineEnd();
				}
				else if(ShowProgress)
				{
					SetCursorPosition(0, CursorTop);
					Write($"{i * 100 / ((double)filesLength - 1)}%");
					WriteSpaceToLineEnd();
				}
				else if(VerbosityLvl > 2)
				{
					SetCursorPosition(0, CursorTop);
					Write($"Copying {files[i]}...");
					WriteSpaceToLineEnd();
				}
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
				if(VerbosityLvl > 2)
				{
					SetCursorPosition(0, CursorTop);
					Write($"Found {file}...");
					WriteSpaceToLineEnd();
				}
			}
			return FilesInDirectory;
		}

		static List<string> GetDoubleEntries(ref List<string> SourceFiles, ref List<string> DestFiles)
		{
			List<string> DoubleEntries = new List<string>();
			int SourceFilesLength = SourceFiles.Count;
			int DestFilesLength = DestFiles.Count;
			if(ShowProgress)
			{
				WriteLine("Searching for double entries: ");
			}
			if(SourceFiles.Count < DestFiles.Count)
			{
				for(int i = 0; i < SourceFiles.Count; i++)
				{
					if(DestFiles.Contains(SourceFiles[i]))
					{
						DoubleEntries.Add(SourceFiles[i]);
					}
					if(VerbosityLvl > 2 && ShowProgress)
					{
					SetCursorPosition(0, CursorTop - 1);
					Write($"Checking {SourceFiles[i]}...");
					WriteSpaceToLineEnd();
					Write($"\n{i * 100 / ((double)SourceFilesLength - 1)}%");
					WriteSpaceToLineEnd();
					}
					else if(ShowProgress)
					{
						SetCursorPosition(0, CursorTop);
						Write($"{i * 100 / ((double)SourceFilesLength - 1)}%");
						WriteSpaceToLineEnd();
					}
					else if(VerbosityLvl > 2)
					{
						SetCursorPosition(0, CursorTop);
						Write($"Checking {SourceFiles[i]}...");
						WriteSpaceToLineEnd();
					}
				}
			}
			else
			{
				for(int i = 0; i < DestFiles.Count; i++)
				{
					if(SourceFiles.Contains(DestFiles[i]))
					{
						DoubleEntries.Add(DestFiles[i]);
					}
					if(VerbosityLvl > 2 && ShowProgress)
					{
						SetCursorPosition(0, CursorTop - 1);
						Write($"Checking {DestFiles[i]}...");
						WriteSpaceToLineEnd();
						Write($"\n{i * 100 / ((double)DestFilesLength - 1)}%");
						WriteSpaceToLineEnd();
					}
					else if(ShowProgress)
					{
						SetCursorPosition(0, CursorTop);
						Write($"{i * 100 / ((double)DestFilesLength - 1)}%");
						WriteSpaceToLineEnd();
					}
					else if(VerbosityLvl > 2)
					{
						SetCursorPosition(0, CursorTop);
						Write($"Checking {DestFiles[i]}...");
						WriteSpaceToLineEnd();
					}
				}
			}
			foreach(string file in DoubleEntries)
			{
				DestFiles.Remove(file);
				SourceFiles.Remove(file);
			}
			return DoubleEntries;
		}
		static void WriteYellowLine(string sLine)
		{
			ForegroundColor = ConsoleColor.Yellow;
			WriteLine(sLine);
			ResetColor();
		}
		static void ShowHelpAndExit()
		{
			WriteLine($"{ProgramName} version {VersionString}\n");
			WriteYellowLine("USAGE: bsprog [Source Dir] [Destination Dir] [OPTIONS]\n");
			WriteYellowLine("Options:");
			WriteLine("-h    Show this help and exit");
			WriteLine("-p    Show the progress while manipulating files");
			WriteLine("-v    Increase the verbosity Level");
			WriteLine("-c    Compare files existing in both directories");
			WriteLine("-d    Delete files existing in Destination Dir");
			WriteLine("                         but not in Source Dir\n");
			WriteYellowLine("Paths have to end with '/'");
			WriteYellowLine("For example:\n");
			WriteLine("bsprog /home/user/ /mnt/backup/userdir/ -p -vvv -c -d\n");
			WriteLine("Shows the progress, compares files that exist in both directories,");
			WriteLine("deletes files that are in Destination Dir but not in Source Dir");
			WriteLine("and sets the verbosity Level to 3\n");
			WriteYellowLine("Some Additional valid usages:\n");
			WriteLine("bsprog /path/to/source/ /path/to/dest/ -pvvvcd");
			WriteLine("bsprog /path/to/source/ /path/to/dest/ --progress --delete-extra -c -v");
			WriteLine("bsprog /path/to/source/ /path/to/dest/");
			Environment.Exit(0);

		}
		static void GetArgs(string[] args)
		{
			try
			{
				if(args.Length < 1)
				{
					WriteLine("Not enough arguments\nExiting...");
					Environment.Exit(0);
				}
				if(args[0] == "-h" || args[0] == "--help")
				{
					ShowHelpAndExit();
				}
				for(int i = 0; i < args.Length; i++)
				{
					if(args[i].StartsWith("-"))
					{
						args[i] = args[i].Substring(1);
						foreach(char chr in args[i])
						{
							switch(chr)
							{
								case 'v':
									VerbosityLvl++;
									break;
								case 'c':
									CompDoubleEntries = true;
									break;
								case 'p':
									ShowProgress = true;
									break;
								case 'd':
									DeleteExtraFiles = true;
									break;
								default:
									WriteLine($"Unknown Argument \"-{chr}\"\nExiting...");
									Environment.Exit(0);
									break;
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				WriteLine($"Error parsing arguments:\n{e}\nExiting...");
				Environment.Exit(0);
			}
		}
		static List<string> CompareDoubleEntries(List<string> CurrDoubleEntries)
		{
			List<string> DoubleEntriesToCopy = new List<string>();
			foreach(string entry in CurrDoubleEntries)
			{
				if(VerbosityLvl > 2)
				{
					SetCursorPosition(0, CursorTop);
					Write($"Comparing {entry} ...");
				}
				if(!AreFilesTheSame(SourceDir + entry, DestDir + entry))
				{
					DoubleEntriesToCopy.Add(entry);
					if(VerbosityLvl > 2)
					{
						Write("not identical");
						WriteSpaceToLineEnd();
					}
				}
				else if(VerbosityLvl > 2)
				{
					Write("indentical");
					WriteSpaceToLineEnd();
				}
			}
			return DoubleEntriesToCopy;
		}

		static bool AreFilesTheSame(string FirstFilePath, string SecondFilePath)
		{
			FileStream fs1 = new FileStream(FirstFilePath, FileMode.Open);
			FileStream fs2 = new FileStream(SecondFilePath, FileMode.Open);

			if(fs1.Length != fs2.Length)
			{
				fs1.Close();
				fs2.Close();

				return false;
			}
			FileInfo fi1 = new FileInfo(FirstFilePath);
			FileInfo fi2 = new FileInfo(SecondFilePath);


			byte[] firstHash = MD5.Create().ComputeHash(fi1.OpenRead());
		        byte[] secondHash = MD5.Create().ComputeHash(fi2.OpenRead());

	        	for (int i=0; i<firstHash.Length; i++)
		        {
				if (firstHash[i] != secondHash[i])
				return false;
			}
			return true;			
		}
	}
}
