using System;
using System.IO;
using static System.Console;

namespace BadSyncProgram
{
	class Program
	{
		static string VersionString = "0.0.0";
		static string ProgramName = "BSProgram";
		static void Main(string[] args)
		{
			string SourceDir, DestDir;
			Title = $"{ProgramName} {VersionString}";
			if(args.Length > 1)
			{
				if(Directory.Exists(args[0]) && Directory.Exists(args[1]))
				{
					SourceDir = args[0];
					DestDir = args[1];
				}
				else
				{
					WriteLine("\n\nOne of the paths passed as an argument is not a valid path");
					Environment.Exit(0);
				}
			}
			else
			{
				SourceDir = GetInput("Please input the path to the source directory:");
				DestDir = GetInput("Please input the path to the destination directory:");
			}
		}

		static string GetInput(string sQuestion)
		{
			WriteLine(sQuestion);
			string Input = ReadLine();
			while(!Directory.Exists(Input))
			{
				WriteLine("Input is not a valid directory, please try again:");
				Input = ReadLine();
			}
			return Input;
		}
	}
}
