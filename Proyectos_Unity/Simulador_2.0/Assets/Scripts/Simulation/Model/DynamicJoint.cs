using UnityEngine;
using System.Collections;

public class DynamicJoint : MonoBehaviour {

	//Type of force, defined as a constant of ForceManager
	public string forceType = ForceManager.FRICTION;

	//Force gain
	public float gain = 0.2f;

	//Force magnitude
	public float magnitude = 0.7f;

	//Cursor that feels the force of collisions
	public GameObject cursor;

	//Object bounded by the joint that will cause the collision
	public GameObject boundedObject;

	//Force index, obtained as a sequential index from ForceManager
	private int forceIndex;

	//Indicator to tell that the force started
	private bool forceStarted;

	//Grabbed object, should be the child mesh with the haptic properties
	public GameObject grabbedObject;

	//Collision material in case of enabling the collision guides
	public Material collisionMaterial;

	//Reference to the simulation monitor to query for collision guides
	public Monitor monitor;

	//Material of collided object before collision
	private Material prevMaterial;

	//Mesh Renderer of object we collide with
	public MeshRenderer collisionRenderer;

	// Use this for initialization
	void Start () {
		forceIndex = ForceManager.GetNextIndex ();
		//Debug.Log ("Force index " + forceIndex + " given to dynamic joint for "+forceType+ " in " + gameObject.name + " game object");
		forceStarted = false;
	}

	void OnCollisionEnter(Collision collision){
		if (enabled) {
			if (boundedObject.name.Equals (collision.gameObject.name) &&
			   HapticManager.GetGrabbed () != null &&
				HapticManager.GetGrabbed ().name.Equals (grabbedObject.name)&&
				!forceStarted) {

				//Change material to indicate collision if the monitor says so
				if (monitor.collisionGuides) {
					prevMaterial = collisionRenderer.material;
					collisionRenderer.material = collisionMaterial;
				}

				//Get current cursor position
				Vector3 cursorPosition = cursor.transform.position;

				//Set anchor point and direction effect 
				float[] position = new float[] { cursorPosition.x, cursorPosition.y, cursorPosition.z };
				float[] direction = new float[]{ -cursorPosition.x, -cursorPosition.y, -cursorPosition.z };

				//Start the force
				ForceManager.SetEnvironmentForce (forceType, forceIndex, position, direction, gain, magnitude, 0, 0);
				forceStarted = true;
			}
		}
	}

	void OnCollisionStay(Collision collisionInfo){
		if (enabled) {
			if (HapticManager.GetGrabbed () == null && forceStarted) {
				ForceManager.StopEnvironmentForce (forceIndex);
				forceStarted = false;
			}
		}
	}

	void OnCollisionExit(Collision collision){
		if (enabled) {
			if (prevMaterial != null) {
				collisionRenderer.material = prevMaterial;
				prevMaterial = null;
			}
			if (forceStarted) {
				ForceManager.StopEnvironmentForce (forceIndex);
				forceStarted = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
