using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Reflection;

namespace Unity_Accessibility
{
	[InitializeOnLoad]
	[ExecuteInEditMode]
	public class Unity_GUI_Accessibility
	{
		[DllImport("nvdaControllerClient64.dll")]
		public static extern int nvdaController_testIfRunning();

		[DllImport("nvdaControllerClient64.dll", CharSet = CharSet.Auto)]
		public static extern int nvdaController_speakText(string text);

		[DllImport("nvdaControllerClient64.dll")]
		public static extern int nvdaController_cancelSpeech();


		//static int hashVal = "Unity_GUI_Accessibility".GetHashCode();
		//static int ID = -1;

		static bool IsEnabled = true;
		//static bool initialized = false;

		static Object m_CurrentObject = null;
		static int compileErrorCounter = 0;
		//static bool isCompiling = false;
		static bool isPlaying = false;

		static AudioClip errorSound = null;
		static AudioClip enterPlayModeSound = null;
		static AudioClip leavePlayModeSound = null;

		//////////////////////////////////////////////////////////////////////////

		//////////////////////////////////////////////////////////////////////////

		[MenuItem("Tools/GUI Accessibility/Enable", false, 1)]
		static void EnableTool()
		{
			IsEnabled = true;
			SavePreferences();
		}

		[MenuItem("Tools/GUI Accessibility/Enable", true, 2)]
		static bool ValidateEnableTool()
		{
			return !IsEnabled;
		}

		//////////////////////////////////////////////////////////////////////////

		[MenuItem("Tools/GUI Accessibility/Disable", false, 3)]
		static void DisableTool()
		{
			IsEnabled = false;
			SavePreferences();
		}

		[MenuItem("Tools/GUI Accessibility/Disable", true, 4)]
		static bool ValidateDisableTool()
		{
			return IsEnabled;
		}

		//////////////////////////////////////////////////////////////////////////


		//////////////////////////////////////////////////////////////////////////

		static Unity_GUI_Accessibility()
		{
			// Load Editor preferences
			LoadPreferences();

			Application.logMessageReceivedThreaded += OnUnityLogCallback;
			SceneView.onSceneGUIDelegate += OnSceneViewCallback;

			// Load sound files
			string errorSoundFile = "Assets/Plugins/Unity Accessibility/Sounds/error_sound.wav";
			errorSound = AssetDatabase.LoadAssetAtPath(errorSoundFile, typeof(AudioClip)) as AudioClip;
			if (errorSound == null)
				Debug.LogError("Cannot load audio file: " + errorSoundFile);

			string enterPlayModeSoundFile = "Assets/Plugins/Unity Accessibility/Sounds/chime_up.wav";
			enterPlayModeSound = AssetDatabase.LoadAssetAtPath(enterPlayModeSoundFile, typeof(AudioClip)) as AudioClip;
			if (enterPlayModeSound == null)
				Debug.LogError("Cannot load audio file: " + enterPlayModeSoundFile);

			string leavePlayModeSoundFile = "Assets/Plugins/Unity Accessibility/Sounds/chime_down.wav";
			leavePlayModeSound = AssetDatabase.LoadAssetAtPath(leavePlayModeSoundFile, typeof(AudioClip)) as AudioClip;
			if (leavePlayModeSound == null)
				Debug.LogError("Cannot load audio file: " + leavePlayModeSoundFile);

			/*
						if (!initialized)
						{
							Application.logMessageReceivedThreaded += OnUnityLogCallback;
							SceneView.onSceneGUIDelegate += OnSceneViewCallback;
							errorSound = AssetDatabase.LoadAssetAtPath("Plugins/Unity Accessibility/Sounds/error_sound", typeof(AudioClip)) as AudioClip;

							initialized = true;
						}
			*/

			/*
					if (IsEnabled)
						Debug.Log("Unity GUI Accessibility loaded - use Ctrl-Shift Click to place objects.");
					else
						Debug.Log("Unity GUI Accessibility inactive - Enable through the Tools menu.");
			*/

			Debug.Log("NVDA Screen Reader found? " + (nvdaController_testIfRunning() == 0).ToString());

			//ID = GUIUtility.GetControlID(hashVal, FocusType.Passive);
		}

		//////////////////////////////////////////////////////////////////////////

		private static void OnUnityLogCallback(string logString, string stackTrace, LogType type)
		{
			if (!IsEnabled)
				return;

			// Say errors out loud
			if (type == LogType.Error)
			{
				PlayClip(errorSound);

				Unity_GUI_Accessibility.nvdaController_cancelSpeech();
				string errorText = "Error";

				// See if we can determine the type of error
				int errorIndex = logString.IndexOf(": error");
				if (errorIndex > 0)
				{
					string leftOfError = logString.Substring(0, errorIndex);

					int nextColonIndex = logString.IndexOf(":", errorIndex + 1);
					string errorCode = logString.Substring(errorIndex + 8, nextColonIndex - errorIndex - 8);
					string errorMessage = logString.Substring(nextColonIndex + 1, logString.Length - nextColonIndex - 1);

					int firstBracketIndex = leftOfError.IndexOf("(");
					if (firstBracketIndex > 0)
					{
						string fileName = leftOfError.Substring(0, firstBracketIndex);
						errorText += " in file " + fileName + ". ";
						int commaIndex = leftOfError.LastIndexOf(",");
						string lineString = leftOfError.Substring(firstBracketIndex + 1, commaIndex - firstBracketIndex - 1);
						errorText += " Line " + lineString + ". ";
						errorText += " Error Code " + errorCode + ". ";
						errorText += " Error Message: " + errorMessage + ". ";
					}
				}
				else
				{
					errorText += ": Please see log file for details.";
				}
				Unity_GUI_Accessibility.nvdaController_speakText(errorText);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		static void LoadPreferences()
		{
			IsEnabled = EditorPrefs.GetBool("Unity_Accessibility_IsEnabled", true);
		}

		//////////////////////////////////////////////////////////////////////////

		static void SavePreferences()
		{
			EditorPrefs.SetBool("Unity_Accessibility_IsEnabled", IsEnabled);
		}

		//////////////////////////////////////////////////////////////////////////

		static public void StateChange()
		{
			//Debug.Log("State Change Called");
			if (isPlaying && !EditorApplication.isPlaying)
			{
				PlayClip(leavePlayModeSound);
				//Debug.Log("Entering Game Mode");
				// 						nvdaController_cancelSpeech();
				// 						nvdaController_speakText("Stopping Game Mode.");
				isPlaying = false;
				EditorApplication.playmodeStateChanged -= StateChange;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		static void OnSceneViewCallback(SceneView sceneView)
		{
			//Debug.Log(Event.current + " modifiers: " + (int)Event.current.modifiers);

			if (!IsEnabled)
				return;

			//Event e = Event.current;

			if (Selection.activeObject != m_CurrentObject)
			{
				if (Selection.activeObject != null)
				{
					// Read out the name of the current object
					GameObject go = Selection.activeObject as GameObject;

					List<string> textToSpeak = new List<string>();
					textToSpeak.Add(Selection.activeObject.name);
					if (go != null)
					{
						// automatically collapse any selected item
						var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
						var methodInfo = type.GetMethod("SetExpandedRecursive");
						EditorApplication.ExecuteMenuItem("Window/Hierarchy");
						var window = EditorWindow.focusedWindow;
						methodInfo.Invoke(window, new object[] { go.GetInstanceID(), false });

						if (go.transform.childCount > 0)
							textToSpeak.Add(go.transform.childCount + " sub object" + (go.transform.childCount > 1 ? "s." : "."));
						if (go.transform.parent != null)
							textToSpeak.Add("Parent object is " + go.transform.parent.name);

						// Add a pause
						textToSpeak.Add("^");
						textToSpeak.Add("^");
						textToSpeak.Add("^");
						textToSpeak.Add("^");
						textToSpeak.Add("^");
						textToSpeak.Add("^");

						// Add additional instructions
						if (go.transform.childCount > 0)
							textToSpeak.Add("Use right arrow key to expand children.");
						if (go.transform.parent != null)
							textToSpeak.Add("Use left arrow key to jump to parent object.");

						if (!Application.isPlaying)
							textToSpeak.Add("Use Alt Shift N to create a new child object.");
					}


					// Say everything
					nvdaController_cancelSpeech();
					foreach (string sentence in textToSpeak)
						nvdaController_speakText(sentence);

					// Also copy the name of the current object in the clipboard
					// as some screen readers can work with that.
					//GUIUtility.systemCopyBuffer = Selection.activeObject.name;

					//Debug.Log(Selection.activeObject.name + " selected");
				}
			}
			m_CurrentObject = Selection.activeObject;

			//Debug.Log("GUIUtility.keyboardControl = " + GUIUtility.keyboardControl);
			//Debug.Log("Highlighter.activeText = " + Highlighter.activeText);

			/*
						// Count compile errors currently in the console
						var logEntries = typeof(EditorWindow).Assembly.GetType("UnityEditorInternal.LogEntries");
						logEntries.GetMethod("Clear").Invoke(new object(), null);
						int errorCount = (int)logEntries.GetMethod("GetCount").Invoke(new object(), null);
			*/

			/*
						if (!isCompiling && EditorApplication.isCompiling)
						{
							isCompiling = true;
							//				nvdaController_cancelSpeech();
							nvdaController_speakText("Compiling Code Changes. Please Wait. ");
						}
						if (isCompiling && !EditorApplication.isCompiling)
						{
							isCompiling = false;
							//				nvdaController_cancelSpeech();
							nvdaController_speakText("Compiling Finished. " + errorCount.ToString("0") + " errors.");
						}
			*/

			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				//Debug.Log("Trying to enter play mode");
				if (!EditorApplication.isPlaying)
				{
					// Count compile errors currently in the console
					var logEntries = typeof(EditorWindow).Assembly.GetType("UnityEditorInternal.LogEntries");
					logEntries.GetMethod("Clear").Invoke(new object(), null);
					int errorCount = (int)logEntries.GetMethod("GetCount").Invoke(new object(), null);
					if (errorCount > 0)
					{
						// This is triggered twice (once trying to enter, and once because it failed and stops again).
						// Hence the counter.
						if (compileErrorCounter == 0)
						{
							PlayClip(errorSound);

							++compileErrorCounter;
						}
						else
						{
							compileErrorCounter = 0;

							nvdaController_cancelSpeech();
							nvdaController_speakText("Cannot enter play mode because there are compile errors.");
						}
					}
					else
					{
						if (!isPlaying)
						{
							//nvdaController_cancelSpeech();
							//nvdaController_speakText("Entering Game Mode.");
							Debug.Log("Entering Game Mode");
							PlayClip(enterPlayModeSound);
							isPlaying = true;
							EditorApplication.playmodeStateChanged += StateChange;
						}
					}
				}
			}


			/*
			//UnityEditorInternal.InternalEditorUtility.get
			//UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded()
			 */
			// In case we are in the Inspector
			/*
			{
				EditorWindow inspectorWindow = EditorWindow.focusedWindow;
				if (inspectorWindow != null && inspectorWindow.titleContent.text.CompareTo("Inspector") == 0)
				{
					ActiveEditorTracker tracker = (ActiveEditorTracker)inspectorWindow.GetType().GetMethod("GetTracker").Invoke(inspectorWindow, null);

					Editor[] editors = tracker.activeEditors;

					for (int i = 0; i < editors.Length; i++)
					{
						// in my case I need to keep one component unfolded (expanded, visible)
						// I use Instance ID or local file ID to identify component instance
						// editors[i].target links to the component, so I can get the id of the component
						// and compare it with the previously saved id of the component I need
						// to keep unfolded
						Debug.Log("Editor " + i + ": " + editors[i].target.GetType() + " " + editors[i].target.name);
						SerializedProperty prop = editors[i].serializedObject.GetIterator();
						prop.Next(true);
						do
						{
							Debug.Log("Property: " + prop.name + " value " + prop.type.ToString());
						}
						while (prop.Next(false));
						//long id = CSObjectTools.GetLocalIdentifierInFileForObject(editors[i].target);
						//tracker.SetVisible(i, id != componentId ? 0 : 1);
					}
				}
			}
			//*/
			//EditorGUIUtility.editingTextField
			//		EditorGUIUtility.GetStateObject()

			/*
					if (IsActive && Event.current.type == EventType.MouseUp)
					{
						if (GUIUtility.hotControl == ID)
							GUIUtility.hotControl = 0;
						Event.current.Use();
						IsActive = false;
					}
*/
		}

		//////////////////////////////////////////////////////////////////////////

		public static void PlayClip(AudioClip clip)
		{
			if (clip == null)
				return;

			var type = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
			var method = type.GetMethod(
					"PlayClip",
					BindingFlags.Static | BindingFlags.Public,
					null,
					new System.Type[] { typeof(AudioClip) }, null);

			method.Invoke(null, new object[] { clip });
		}

		//////////////////////////////////////////////////////////////////////////
	}

}

