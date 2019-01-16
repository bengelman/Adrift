using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public float speed = 10.0f;
	public Rigidbody body;
	public float jumpForce = 100.0f;
	private CharacterController controller;
	private bool collided = true;
	public Camera cam;
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		float translation = Input.GetAxis ("Vertical") * speed;
		float straffle = Input.GetAxis ("Horizontal") * speed;
		float ascend = Input.GetAxis ("Ascend/Descend") * speed;
		if (GameStateManager.adrenalineTimer > 0) {
			translation *= 2;
			straffle *= 2;
			ascend *= 2;
		}
		translation *= Time.deltaTime;
		straffle *= Time.deltaTime;
		ascend *= Time.deltaTime;



		transform.Translate (straffle, ascend, translation);
		if (Input.GetKeyDown ("escape")) {
			SceneManager.LoadScene ("Menu");
			Cursor.lockState = CursorLockMode.None;
		}
			//Cursor.lockState = CursorLockMode.None;
		
	}


}
