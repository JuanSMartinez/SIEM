using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	//Force index 
	private int forceIndex;

	//Maximum stiffness
	private float maxStiffness = 0.7f;

	//Haptic Mesh
	public string hapticMeshName;

	// Use this for initialization
	void Start () {
		forceIndex = ForceManager.GetNextIndex ();
	}
	
	//Collision detection
	void OnCollisionEnter(Collision collision){
		
		//Haptic properties of the object we are colliding with
		HapticProperties props = collision.gameObject.transform.FindChild(hapticMeshName).GetComponent<HapticProperties>();

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
