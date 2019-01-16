using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voltswitch : Interactable {
	public Material on, off;
	public AudioSource source;
	public AudioClip beep;
	public override string Description(){
		return "Right click: seal door";
	}
	public override void Interact(){
		GameStateManager.electrified = !GameStateManager.electrified;
		source.PlayOneShot (beep);
		if (GameStateManager.electrified) {
			GameStateManager.instance.usage += 5;
		} else {
			GameStateManager.instance.usage -= 5;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Renderer> ().material = GameStateManager.electrified ? on : off;
	}
}
