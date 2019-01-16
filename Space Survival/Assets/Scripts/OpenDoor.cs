using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : Interactable {
	public bool open = false;
	public float speed = 1f;
	float moveLeft;
	public AudioClip sound;
	public AudioClip alienSound;
	public AudioClip safeSound;
	public AudioSource doorSource;
	public int adjacent1, adjacent2;
	public float x;
	public float y;
	public float z;
	Vector3 rotationPoint;
	public AudioClip close;
	public AudioClip bang;
	public AudioSource audioso;
	public float volume = 1f;
	public float totalMove;
	public bool useMin = true;
	float bangDelay;
	int bangsLeft;
	int unlockTime = 0;
	public override string Description(){
		return "Right click: open/close door | Left click: listen through door";
	}
	public override void Interact(){
		if (moveLeft != 0 || GameStateManager.gameIndex == 0) {
			return;
		}

		//audioso.PlayOneShot (sound, volume);
		if (open) {
			moveLeft = totalMove;
		} else {
			moveLeft = -totalMove;
		}
		open = !open;

	}
	// Use this for initialization
	void Start () {
		
		rotationPoint = useMin ? GetComponent<Renderer> ().bounds.min : GetComponent<Renderer> ().bounds.max;
		//audioso = GetComponent<AudioSource> ();
		//doorSource = gameObject.AddComponent<AudioSource> ();
		//doorSource.playOnAwake = false;

	}
	public void Listen(){
		if (open)
			return;
		AudioClip sound = safeSound;
		if ((GameStateManager.alienRoom == adjacent1 || GameStateManager.alienRoom == adjacent2) && !GameStateManager.instance.inVents) {
			sound = alienSound;
		}
		if (sound != doorSource.clip) {
			if (doorSource.isPlaying) {
				doorSource.Stop ();
			}
			doorSource.clip = sound;
			doorSource.loop = true;
			doorSource.Play ();
		}


		
	}
	public void UnListen(){
		doorSource.Stop ();
		doorSource.clip = null;
	}
	// Update is called once per frame
	void Update () {
		if (unlockTime > 0) {
			if (!open) {
				Interact ();
			}
			unlockTime = Random.Range (30, 60);
		}
		if (bangDelay > 0) {
			bangDelay -= Time.deltaTime;
			if (bangDelay <= 0) {
				audioso.PlayOneShot (bang, volume);
				if (bangsLeft > 0) {
					bangsLeft--;
					bangDelay = 0.2f;
				}
			}
		}
		if (moveLeft > 0) {
			float moveAmount = Mathf.Min (moveLeft, speed * Time.deltaTime);
			moveLeft -= moveAmount;
			transform.RotateAround (rotationPoint, Vector3.up, moveAmount);

			if (moveLeft == 0 && !open) {
				audioso.PlayOneShot (close, volume);
				if ((GameStateManager.alienRoom == adjacent1 || GameStateManager.alienRoom == adjacent2) && !GameStateManager.instance.inVents) {
					bangDelay = 1;
					bangsLeft = 2;
				}
			}
		} else if (moveLeft < 0) {
			float moveAmount = Mathf.Max (moveLeft, -speed * Time.deltaTime);
			moveLeft -= moveAmount;
			transform.RotateAround (rotationPoint, Vector3.up, moveAmount);

			if (moveLeft == 0 && !open) {
				audioso.PlayOneShot (close, volume);
				if ((GameStateManager.alienRoom == adjacent1 || GameStateManager.alienRoom == adjacent2) && GameStateManager.alienRoom != GameStateManager.playerZone) {
					bangDelay = 1;
					bangsLeft = 2;
				}
			}
		}
	}
}
