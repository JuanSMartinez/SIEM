using UnityEngine;
using System.Collections;

public class CursorJointRaycast : MonoBehaviour {

	//Spring joint that holds objects on collision
	SpringJoint joint;

	//Distance for raycast to hit the object
	public float distance;

	// Use this for initialization
	void Start () {
		if (enabled) {
			joint = gameObject.AddComponent<SpringJoint> ();
			joint.autoConfigureConnectedAnchor = true;
			joint.enableCollision = true;
			joint.spring = 500f;
			joint.damper = 1f;
			joint.maxDistance = 0.5f;
			joint.minDistance = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray ray = new Ray (gameObject.transform.position, gameObject.transform.forward);
		if (enabled) {
			if (Physics.Raycast (ray, out hit, distance)) {
				GameObject objHit = hit.collider.gameObject;
				joint.connectedBody = objHit.GetComponentInParent<Rigidbody>();
				joint.anchor = gameObject.transform.position;
			} else {
				joint.connectedBody = null;
				joint.anchor = new Vector3 ();
			}

		}
	}
}
