using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : Interactable {
	public Light[] lights;
	AudioSource source;
	public AudioClip click;
	public float volume = 0.5f;
	public override string Description(){
		return "Right click: activate/deactivate light";
	}
	public override void Interact(){
		if (lights [0].enabled) {
			GameStateManager.instance.usage-=1;
		} else {
			GameStateManager.instance.usage+=1;
		}
		foreach (Light light in lights){
			light.enabled = !light.enabled;

		}
		source.PlayOneShot(click, volume);
	}
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
