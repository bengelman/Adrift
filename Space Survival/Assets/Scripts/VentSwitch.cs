using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSwitch : Interactable {
	public Material green, red;
	public AudioSource sauce;
	public AudioClip beep;
	public override string Description(){
		return "Right click: activate/deactive ventillation system";
	}
	public override void Interact(){
		GameStateManager.instance.ventsUp = !GameStateManager.instance.ventsUp;

		GetComponent<Renderer> ().material =  GameStateManager.instance.ventsUp ? green : red;
		sauce.PlayOneShot (beep, 1f);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
