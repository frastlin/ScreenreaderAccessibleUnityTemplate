using UnityEngine;
using System.Collections.Generic;
//This is an example script that is attached to an object in the MainScene in the unity editor.

public class ExampleScript : MonoBehaviour
{

	AudioSource audioSource;

    //Called as soon as this component is created, but before Start.
    void Awake()
    {
        //Debug.Log is like Console.WriteLine, but will print to the Unity console as there is no regular console by default.
        Debug.Log("Hello world!");

		//Load the sounds
	var sound = Resources.Load<AudioClip>("Sounds/Death");
//	var audioSource = gameObject.AddComponent<AudioSource>();
	audioSource = gameObject.AddComponent<AudioSource>();

	audioSource.clip = sound;
    }
    //Called after Awake, but before the first Update.
    void Start()
    {
Camera.main.transform.position = new Vector3(0, 0, -100);
        }

    //Called every frame.
    void Update()
    {
if(Input.GetKeyDown(KeyCode.Space))
{
	audioSource.Play();
}
}
}

class ProjectMain
{
[RuntimeInitializeOnLoadMethod]
public static void Main()
{
var obj = new GameObject("TestObj");
obj.AddComponent<ExampleScript>();
}
}