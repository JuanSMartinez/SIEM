using UnityEngine;
using System.Collections;

public class AcnhorMovement : MonoBehaviour {

	public GameObject A;
	public GameObject B;
	public GameObject[] cursorObjects;
	public Material staticMaterial;
	public Material anchorMaterial;
	private bool movement = false;

	void Update(){
		if(PluginImport.GetButton2State())
			movement = !movement;

		SetMaterial ();
		ModifyJoints ();
		

	}

	private void SetMaterial (){
		for (int i = 0; i < cursorObjects.Length; i++) {
			cursorObjects[i].GetComponent <MeshRenderer> ().material = movement ? anchorMaterial: staticMaterial;
		}

	}

	private void ModifyJoints(){
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

}
