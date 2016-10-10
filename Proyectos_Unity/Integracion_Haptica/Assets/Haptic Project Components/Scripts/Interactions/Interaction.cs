using UnityEngine;
using System.Collections;
using System;

//Dynamic interaction between two rigid objects with haptic feedback as environment forces
public class Interaction : MonoBehaviour {

	public GameObject first;

	public GameObject second;

	// Start
	void Start () {
		if (first == null || second == null) {
			throw Exception ("Objetos de interaccion nulos");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
