using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public abstract string Description();
	// Update is called once per frame
	void Update () {
		
	}
	public abstract void Interact ();
}
