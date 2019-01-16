using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public AudioClip select;
	AudioSource audio;
	public float volume = 1f;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		Cursor.lockState = CursorLockMode.None;
	}
	public void Load()
	{
		audio.PlayOneShot (select, volume);
		SceneManager.LoadScene ("Level01");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
