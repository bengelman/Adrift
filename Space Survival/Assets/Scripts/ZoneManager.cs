using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour {
	public int index;
	public string name;
	float attackTimer;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter(Collider c){
		
		GameStateManager.playerZone = index;
		GameStateManager.instance.location.text = name;
		if (index == 1 && GameStateManager.gameIndex == 1) {
			GameStateManager.instance.AddMessage (new string[]{"You: Commander? Are you in the lab?"}, 2f);
			GameStateManager.gameIndex++;
		}
		if (index == 0 && GameStateManager.gameIndex == 2) {
			GameStateManager.instance.AddMessage (new string[]{"You: This is blood... Where's Sheffield? Did that thing... get him?"}, 2f);
			GameStateManager.gameIndex++;
			attackTimer = 2;
		}
		if (index == 3 && GameStateManager.gameIndex == 4) {
			GameStateManager.instance.backgroundSource.Stop ();
			GameStateManager.instance.backgroundSource.clip = GameStateManager.instance.normalAudio;
			GameStateManager.instance.backgroundSource.Play ();
			GameStateManager.instance.AddMessage (new string[]{"You: I guess my only option is to try and survive. All that's left to do to repair the Soyuz module is to configure the computer.", "Until then... If I use the electromagnetic seal on the door, I should be able to keep the thing out. It uses a lot of power, though.", "You: I'm going to have to pay attention to my power usage. There's barely enough left to power the computer enough to get the ship running."}, 7);
			GameStateManager.gameIndex++;
		}
	}
	// Update is called once per frame
	void Update () {
		if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0) {
				GameStateManager.Jumpscare (new Vector3(1f, 0.7f, 1f));
			}
		}
	}
}
