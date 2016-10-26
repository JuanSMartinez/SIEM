using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	//Force index 
	private int forceIndex;

	//Maximum stiffness
	private float maxStiffness = 0.7f;

	//Haptic Mesh
	public string hapticMeshName;

	//Events
	public delegate void CollisionAction();
	public static event CollisionAction OnCollision;

	public delegate void ExitCollisionAction ();
	public static event ExitCollisionAction ExitCollision;

	// Use this for initialization
	void Start () {
		forceIndex = ForceManager.GetNextIndex ();
	}

	void Update(){
		/*
		if (!GenericFunctionsClass.GetGrabbed ())
			ForceManager.StopEnvironmentForce (forceIndex);*/
	}
	
	//Collision detection
	void OnCollisionEnter(Collision collision){
		
		if (collision != null) {
			/*
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
			Vector3 cursorPosition = GameObject.Find ("Cursor").transform.position;

			//Set friction anchor point and direction effect 
			float[] position = new float[] { cursorPosition.x, cursorPosition.y, cursorPosition.z };
			float[] direction = new float[]{ -cursorPosition.x, -cursorPosition.y, -cursorPosition.z };

			//Constant gain and magnitude
			float gain = 0.2f;
			if (GenericFunctionsClass.GetGrabbed ()) {
				ForceManager.SetEnvironmentForce (type, forceIndex, position, direction, gain, stiffness, 0, 0);

			} else
				ForceManager.StopEnvironmentForce (forceIndex);
			*/
			if (OnCollision != null)
				OnCollision ();
		}

	}

	//Collision leaving
	void OnCollisionExit(Collision collision){
		/*
		ForceManager.StopEnvironmentForce (forceIndex);
		*/
		if (ExitCollision != null)
			ExitCollision ();

	}
}
