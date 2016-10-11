using UnityEngine;
using System.Collections;
using System;

//Dynamic interaction between two rigid objects with haptic feedback as environment forces
public class Interaction : MonoBehaviour {

	//Maximum stiffness
	private float maxStiffness = 0.7f;

	//Force index
	private int forceIndex;

	// Start
	void Start () {
		forceIndex = ForceManager.GetNextIndex ();
	}
	
	// Update is called once per frames
	void Update () {
	
	}

	//Collision detection
	void OnCollisionEnter(Collision collision){

		//Haptic properties of the object we are colliding with
		HapticProperties props = collision.gameObject.GetComponentInChildren<HapticProperties>();

		//Get Stiffness
		float stiffness = props.stiffness;

		//Set friction or viscosity effect
		string type;
		if (stiffness >= maxStiffness) {
			type = ForceManager.FRICTION;
		} else {
			type = ForceManager.VISCOSITY;
		}
			
		//Get current cursor position
		Vector3 cursorPosition = GameObject.Find("Cursor").transform.position;

		//Set friction anchor point and direction effect 
		float[] position = new float[] {cursorPosition.x, cursorPosition.y, cursorPosition.z};
		float[] direction = new float[]{ -cursorPosition.x, -cursorPosition.y, -cursorPosition.z };

		//Constant gain and magnitude
		float gain = 0.2f;
		ForceManager.SetEnvironmentForce(type, forceIndex, position, direction, gain, stiffness, 0, 0);
		Debug.Log ("Force started with index " + forceIndex);
	}

	//Collision leaving
	void OnCollisionExit(Collision collision){
		ForceManager.StopEnvironmentForce (forceIndex);
		Debug.Log ("Force stoped with index " + forceIndex);
	}
}
