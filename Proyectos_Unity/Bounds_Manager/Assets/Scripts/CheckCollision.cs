using UnityEngine;
using System.Collections;

public class CheckCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log ("Collided with " + collision.gameObject.name);
	}

	void OnCollisionExit(Collision collisionInfo) {
		Debug.Log ("Stop colliding with " + collisionInfo.gameObject.name);
	}
}
