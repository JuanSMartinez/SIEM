using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monitor : MonoBehaviour {

	//control for final position guides
	public bool positionGuides = false;

	//Control for collision guides
	public bool collisionGuides = false;

	//Objets to track
	public GameObject[] objects;

	//Final objects to compare to
	public GameObject[] finalObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Show final positions if the guide options is enabled
		ShowFinalGuides();
	}

	//Show final positions if the guide options is enabled
	private void ShowFinalGuides(){
		for (int i = 0; i < finalObjects.Length; i++) {
			GameObject obj = finalObjects [i];
			obj.transform.GetComponentInChildren<MeshRenderer> ().enabled = positionGuides;
		}
		
	}
}
