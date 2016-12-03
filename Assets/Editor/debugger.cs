//Change the variables at the start of the class below to show the path where your logs should be stored
//Guides on how to make this debugger:
//http://www.thegameengineer.com/blog/2014/02/09/unity-11-logging/
//http://www.thegameengineer.com/blog/2014/02/13/unity-12-warnings-errors-and-asserts/
//note that there is another log file at:
//C:\Users\userName\AppData\Local\Unity\Editor\Editor.log
//but it is super detailed and not as easy to read as the ones produced by this script
using UnityEngine;
#if UNITY_EDITOR_WIN
using UnityEditor;
[InitializeOnLoad]
#endif

class MyDebugger
{
	//set this string to be the place where you wish your logs to be made. It should look something like:
	//static public string LogPath = "C:/Users/userName/programming/unity/logs/Unity_log.txt";
	static public string LogPath = "C:/Unity.txt";

	//By default both stack traces and logs will be placed in the file, but if you wish to only have one show, make the other false
	static readonly bool StackTraceOnLog = true;
	static public bool ClearLogOnPlay = true;

	static readonly bool Active = true;

	static MyDebugger()
	{
		LogPath = Application.dataPath + "/Unity_Logfile.txt";
		System.IO.File.AppendAllText(LogPath, "Log started.");

		if (Active)
		{
			Application.logMessageReceivedThreaded += WriteUnityLog;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void ClearOnPlay()
	{
		if (ClearLogOnPlay)
		{
			ClearLog();
		}
	}

	public static void ClearLog()
	{
		System.IO.File.WriteAllText(LogPath, "");
	}

	private static void WriteUnityLog(string logString, string stackTrace, LogType type)
	{
		System.IO.File.AppendAllText(LogPath, type.ToString() + " : " + logString + '\n');
		if (type == LogType.Log && !StackTraceOnLog)
		{
			return;
		}
		System.IO.File.AppendAllText(LogPath, stackTrace + "\n");
	}
}