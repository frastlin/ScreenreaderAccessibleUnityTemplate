using UnityEngine;
using System.Collections;

//This is an example script that is attached to an object in the MainScene in the unity editor after the Play mode is entered.
// See the function called "Main" near the end of the file for further explanation.
public class ExampleScript : MonoBehaviour
{
	// This is a reference to an audio player to play sound files
	AudioSource audioSource = null; 

	// The following method is called as soon as this component is created, but before the Start function.
	void Awake()
	{
		// Debug.Log is like Console.WriteLine, but will print to the Unity console as there is no regular console by default.
		// Everything that is logged to the console is also stored in the log file, which can be found in the Assets folder.
		Debug.Log("Hello world!");

		// Load the sound file
		var sound = Resources.Load<AudioClip>("Sounds/Death");
		
		// Add an audio player component to this game object and save a reference
		audioSource = gameObject.AddComponent<AudioSource>();

		// Tell the audio player which sound file to play
		audioSource.clip = sound;
	}
	
	// This method is called after Awake, but before the first Update.
	void Start()
	{
		// The following instructions move the camera to a point in the world.
		// This has no purpose for this demo, but serves as an example of how to change the position of any game object in Unity via code.
		if (Camera.main != null)
		{
			GameObject cameraObject = Camera.main.gameObject;
			cameraObject.transform.position = new Vector3(0, 0, -50.0f);
		}
	}

	// The Update method is called every frame.
	void Update()
	{
		// Check whether the space key was pressed this frame
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Since the space key was pressed, tell the audio player to play the sound that was assigned to it
			audioSource.Play();
		}
	}
}


class ProjectMain
{
	// The following method is called automatically as soon as game mode is entered.
	// It will create a new empty game object and attach an instance of the example script component to it
	[RuntimeInitializeOnLoadMethod]
	public static void Main()
	{
		// Create a new game object with the name "Test Object"
		var obj = new GameObject("Test Object");

		// Add the Example Script component to it.
		// This will trigger the function Awake, OnEnable, Start and Update to be called to that script.
		obj.AddComponent<ExampleScript>();
	}
}
