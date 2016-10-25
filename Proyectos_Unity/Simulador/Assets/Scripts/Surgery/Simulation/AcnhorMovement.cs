using UnityEngine;
using System.Collections;

public class AcnhorMovement : MonoBehaviour {

	public GameObject A;
	public GameObject B;
	private bool movement = false;

	void Update(){
		if (movement) {
			ConfigurableJoint[] joints = A.GetComponents<ConfigurableJoint> ();
			for (int i = 0; i < joints.Length; i++) {
				joints [i].anchor = A.transform.position;
			}
			joints = B.GetComponents<ConfigurableJoint> ();
			for (int i = 0; i < joints.Length; i++) {
				joints [i].anchor = A.transform.position;
			}
		}
	}

	public void MoveAnchors(){
		movement = !movement;
	}

}
