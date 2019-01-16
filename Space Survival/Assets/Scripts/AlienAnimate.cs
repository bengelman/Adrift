using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimate : MonoBehaviour {
	public Texture tex1, tex2, tex3;
	float totalTime;
	public AudioSource source;
	public AudioClip vents;
	public float idletimer = 0;
	public float speed = 2f;
	float nextUpdate = 20;
	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3 (0.772f, 0.013f, 1.912f);
	}
	public bool isDoorOpen(string doorName){
		return GameObject.Find (doorName).GetComponent<OpenDoor> ().open;
	}
	// Update is called once per frame
	void Update () {
		if (!GameStateManager.instance.ventsUp) {
			GameStateManager.instance.inVents = false;
		}
		totalTime += Time.deltaTime;
		if (GameStateManager.gameIndex >= 4)
			idletimer += Time.deltaTime;
		int index = ((int)totalTime * 4) % 3;
		switch (index) {
		case 0:
			GetComponent<Renderer> ().material.mainTexture = tex1;
			break;
		case 1:
			GetComponent<Renderer> ().material.mainTexture = tex2;
			break;
		case 2:
			GetComponent<Renderer> ().material.mainTexture = tex3;
			break;
		}
		if (GameStateManager.gameIndex <= 4 && GameStateManager.alienRoom == 0) {
			if (!isDoorOpen ("LabWindow") && GameStateManager.gameIndex == 4) {
				GameStateManager.gameIndex++;
			}
			else if (idletimer > 2) {
				idletimer = 0;
				GameStateManager.alienRoom = 1;
				this.transform.position = new Vector3 (-0.862f, 0.315f, -0.299f);
			}
		}
		else if (GameStateManager.playerZone == GameStateManager.alienRoom && !GameStateManager.instance.inVents) {
			
			Vector3 direction = (transform.position - GameStateManager.instance.player.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			transform.Rotate (new Vector3 (0, lookRotation.eulerAngles.y + 90 - transform.rotation.eulerAngles.y, 0));
			idletimer = 0;
			Vector3 prevpos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			bool xsame = false;
			bool zsame = false;
			if (GameStateManager.instance.player.transform.position.x == transform.position.x) {
				xsame = true;
			} else if (GameStateManager.instance.player.transform.position.x > transform.position.x) {
				transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
				if (GameStateManager.instance.player.transform.position.x <= transform.position.x) {
					transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
					xsame = true;
				}
			} else if (GameStateManager.instance.player.transform.position.x < transform.position.x) {
				transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
				if (GameStateManager.instance.player.transform.position.x >= transform.position.x) {
					transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
					xsame = true;
				}
			}

			if (GameStateManager.instance.player.transform.position.z == transform.position.z) {
				zsame = true;
			} else if (GameStateManager.instance.player.transform.position.z > transform.position.z) {
				transform.Translate (Vector3.forward * speed * Time.deltaTime, Space.World);
				if (GameStateManager.instance.player.transform.position.z <= transform.position.z) {
					transform.Translate (Vector3.back * speed * Time.deltaTime, Space.World);
					zsame = true;
				}
			} else if (GameStateManager.instance.player.transform.position.z < transform.position.z) {
				transform.Translate (Vector3.back * speed * Time.deltaTime, Space.World);
				if (GameStateManager.instance.player.transform.position.z >= transform.position.z) {
					transform.Translate (Vector3.forward * speed * Time.deltaTime, Space.World);
					zsame = true;
				}
			}
			if (xsame && zsame || GameStateManager.playerZone != 1) {
				GameStateManager.Jumpscare (new Vector3 (prevpos.x, GameStateManager.instance.cam.transform.position.y, prevpos.z));
			}
		} else {
			if (idletimer >= nextUpdate) {
				idletimer = 0;
				if (GameStateManager.instance.inVents) {
					nextUpdate = Random.Range (5f, 10f);
					if (!GameStateManager.instance.ventsUp) {
						GameStateManager.instance.inVents = false;
					} else {
						if (GameStateManager.alienRoom == 3) {
							GameStateManager.instance.inVents = false;
							this.transform.position = new Vector3 (1.648f, -0.287f, -6.34f);
							GetComponent<Renderer> ().enabled = true;
						} else if (GameStateManager.alienRoom == 2){
							GameStateManager.alienRoom = 3;
							source.PlayOneShot (vents, 1f);
						} else {
							GameStateManager.alienRoom = 2;
							source.PlayOneShot (vents, 1f);
						}
					}

				} else {
					nextUpdate = Random.Range (15f, 30f);
					switch (GameStateManager.alienRoom) {
					case 0:
						if (isDoorOpen ("LabWindow")) {
							if (GameStateManager.instance.ventsUp && Random.Range (1, 3) == 2) {
								
								GameStateManager.instance.inVents = true;
								source.PlayOneShot (vents, 1f);
								GetComponent<Renderer> ().enabled = false;

							} else {
								GameStateManager.alienRoom = 1;
								this.transform.position = new Vector3 (-0.862f, 0.315f, -0.299f);
								GetComponent<Renderer> ().enabled = true;
							}
						} else if (GameStateManager.instance.ventsUp) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else {
							nextUpdate = 5f;
						}
						break;
					case 1:
						if (isDoorOpen ("LabWindow")) {
							if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
								GameStateManager.instance.inVents = true;
								source.PlayOneShot (vents, 1f);
								GetComponent<Renderer> ().enabled = false;

							} else if (isDoorOpen ("PowerDoor") && Random.Range (-9, 10) > -5) {
								GameStateManager.alienRoom = 2;
								this.transform.position = new Vector3 (-1.062f, 0.364f, -7.44f);
								GetComponent<Renderer> ().enabled = true;
							} else {
								GameStateManager.alienRoom = 0;
								this.transform.position = new Vector3 (0.772f, 0.013f, 1.912f);
								GetComponent<Renderer> ().enabled = true;
							}
						} else if (isDoorOpen ("PowerDoor")) {
							if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
								GameStateManager.instance.inVents = true;
								source.PlayOneShot (vents, 1f);
								GetComponent<Renderer> ().enabled = false;

							} else {
								GameStateManager.alienRoom = 2;
								this.transform.position = new Vector3 (-1.062f, 0.364f, -7.44f);
								GetComponent<Renderer> ().enabled = true;
							}
						} else if (GameStateManager.instance.ventsUp) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else {
							nextUpdate = 5f;
						}
						break;
					case 2:
						if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else if (isDoorOpen ("CommandDoor") && GameStateManager.playerZone == 3) {
							GameStateManager.alienRoom = 3;
							this.transform.position = new Vector3 (1.648f, -0.287f, -6.34f);
							GetComponent<Renderer> ().enabled = true;
						} else if (!GameStateManager.electrified && GameStateManager.playerZone == 3) {
							GameObject.Find ("CommandDoor").GetComponent<OpenDoor> ().Interact();
							GameObject.Find ("CommandDoor").GetComponent<OpenDoor> ().doorSource.PlayOneShot (GameObject.Find ("CommandDoor").GetComponent<OpenDoor> ().bang);
							nextUpdate = 3f;
						} else if (isDoorOpen ("StorageDoor") && Random.Range (0, 30) >= 20) {
							GameStateManager.alienRoom = 6;
							GetComponent<Renderer> ().enabled = false;
						} else if (isDoorOpen ("PowerDoor") && Random.Range (0, 40) >= 20) {
							GameStateManager.alienRoom = 1;
							this.transform.position = new Vector3 (-1.062f, 0.341f, -5.279f);
							GetComponent<Renderer> ().enabled = true;
						} else {
							GameStateManager.alienRoom = 5;
							GetComponent<Renderer> ().enabled = false;
						}
						break;
					case 3:
						if (isDoorOpen ("CommandDoor")) {
							if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
								GameStateManager.instance.inVents = true;
								source.PlayOneShot (vents, 1f);
								GetComponent<Renderer> ().enabled = false;

							} else if (GameStateManager.playerZone == 4) {
								GameStateManager.alienRoom = 4;
								GetComponent<Renderer> ().enabled = false;
							} else {
								GameStateManager.alienRoom = 2;
								this.transform.position = new Vector3 (-1.062f, 0.364f, -7.44f);
								GetComponent<Renderer> ().enabled = true;
							}
						} else {
							if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
								GameStateManager.instance.inVents = true;
								source.PlayOneShot (vents, 1f);
								GetComponent<Renderer> ().enabled = false;

							} else {
								GameStateManager.alienRoom = 4;
								GetComponent<Renderer> ().enabled = false;
							}
						}
						break;
					case 4:
						if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else {
							GameStateManager.alienRoom = 3;
							this.transform.position = new Vector3 (1.648f, -0.287f, -6.34f);
							GetComponent<Renderer> ().enabled = true;
						} 
						break;
					case 5:
						if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else {
							GameStateManager.alienRoom = 2;
							this.transform.position = new Vector3 (-1.062f, 0.364f, -7.44f);
							GetComponent<Renderer> ().enabled = true;
						} 
						break;
					case 6:
						if (GameStateManager.instance.ventsUp && Random.Range (0, 10) == 2) {
							GameStateManager.instance.inVents = true;
							source.PlayOneShot (vents, 1f);
							GetComponent<Renderer> ().enabled = false;

						} else {
							GameStateManager.alienRoom = 2;
							this.transform.position = new Vector3 (-1.062f, 0.364f, -7.44f);
							GetComponent<Renderer> ().enabled = true;
						} 
						break;
					}
				}
			}
		}
	}

}
