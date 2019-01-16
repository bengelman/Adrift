﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	public float xoffset = 0f, yoffset = 0f;
	GameObject character;
	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameStateManager.jumpscareTimer > 0) {
			return;
		}
		var md = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
		smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -90.0f, 90.0f);
		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y + yoffset, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x + xoffset, character.transform.up);
	}
}
