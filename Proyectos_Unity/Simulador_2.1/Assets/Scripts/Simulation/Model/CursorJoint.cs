using UnityEngine;
using System.Collections;

public class CursorJoint : MonoBehaviour {

	//Spring joint that holds objects on collision
	SpringJoint joint;

	// Use this for initialization
	void Start () {
		if (enabled) {
			joint = gameObject.AddComponent<SpringJoint> ();
			joint.autoConfigureConnectedAnchor = true;
			joint.spring = 500f;
			joint.damper = 0.2f;
			joint.maxDistance = 0.5f;
			joint.minDistance = 0;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		if (enabled) {
			joint.connectedBody = collision.gameObject.GetComponent<Rigidbody> ();
			joint.anchor = collision.gameObject.transform.position;
		}

	}
	void OnCollisionStay(Collision collisionInfo){
		//manipulateObject ();
	}
	void OnCollisionExit(Collision collision){
		if (enabled) {
			joint.connectedBody = null;
		}
	}
}
