using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
	public Camera cam;
	float cooldown = 0f;
	public float clickInterval = 1f;
	OpenDoor listening = null;
	public Text descText;
	bool prevSafe;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldown > 0) {
			cooldown = Time.deltaTime > cooldown ? 0 : cooldown - Time.deltaTime;
			return;
		}
		bool continueListening = false;
		if (listening != null) {

		}
		descText.text = "";
		if (Input.GetButton ("Fire2")) {
			RaycastHit hitInfo;

			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hitInfo, 2f)) {
				GameObject hit = hitInfo.transform.gameObject;
				Interactable script;
				if (script = (Interactable)hit.GetComponent (typeof(Interactable))) {
					script.Interact ();
					cooldown = clickInterval;
				}
			}
		} else if (Input.GetButton ("Fire1")) {
			RaycastHit hitInfo;

			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hitInfo, 2f)) {
				GameObject hit = hitInfo.transform.gameObject;
				Terminal terminal;
				if (terminal = (Terminal)hit.GetComponent (typeof(Terminal))) {
					terminal.OnOff ();
					cooldown = clickInterval;
				}
				OpenDoor script;
				if (script = (OpenDoor)hit.GetComponent (typeof(OpenDoor))) {
					script.Listen ();
					cooldown = clickInterval;
					continueListening = true;
					listening = script;
				}
			}
		} else {
			RaycastHit hitInfo;

			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hitInfo, 2f)) {
				GameObject hit = hitInfo.transform.gameObject;
				Interactable script;
				if (script = (Interactable)hit.GetComponent (typeof(Interactable))) {
					descText.text = script.Description ();
				}
			}
		}
		if (listening != null && !continueListening) {
			listening.UnListen ();
			listening = null;
		}
	}
}
