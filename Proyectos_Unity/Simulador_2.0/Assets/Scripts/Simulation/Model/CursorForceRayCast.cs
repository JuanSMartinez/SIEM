using UnityEngine;
using System.Collections;

public class CursorForceRayCast : MonoBehaviour {

	//Distance for raycast to hit the object
	public float distance;

	//Friction force gain
	public float gain;

	//Friction force magnitude
	public float magnitude;

	//Force index
	private int forceIndex;

	//Indicator to tell that the force started
	private bool forceStarted;

	// Use this for initialization
	void Start () {
		forceIndex = ForceManager.GetNextIndex ();
		forceStarted = false;
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray ray = new Ray (gameObject.transform.position, gameObject.transform.forward);

		if (enabled) {
			if (Physics.Raycast (ray, out hit, distance)) {
				
				//Set anchor point and direction effect 
				float[] position = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z };
				float[] direction = new float[]{ -gameObject.transform.position.x, -gameObject.transform.position.y, -gameObject.transform.position.z };

				if (!forceStarted && HapticManager.GetGrabbed () == null) {
					//Start the force
					ForceManager.SetEnvironmentForce (ForceManager.FRICTION, forceIndex, position, direction, gain, magnitude, 0, 0);
					forceStarted = true;
				} else if (forceStarted && HapticManager.GetGrabbed () != null) {
					ForceManager.StopEnvironmentForce (forceIndex);
					forceStarted = false;
				}

			} else {
				if (forceStarted) {
					ForceManager.StopEnvironmentForce (forceIndex);
					forceStarted = false;
				}
			}

		}
	
	}
}
