using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monitor : MonoBehaviour {

	//control for final position guides
	public bool positionGuides = false;

	//Control for collision guides
	public bool collisionGuides = false;

	//Objets to track
	public GameObject[] objects;

	//Final objects to compare to
	public GameObject[] finalObjects;

	//Linear tolerance
	public float linearTolerance;

	//Angular tolerance
	public float angularTolerance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Show final positions if the guide options is enabled
		ShowFinalGuides();

		//Log position differences

		//Log angular differences
	}

	//Show final positions if the guide options is enabled
	private void ShowFinalGuides(){
		for (int i = 0; i < finalObjects.Length; i++) {
			GameObject obj = finalObjects [i];
			obj.transform.GetComponentInChildren<MeshRenderer> ().enabled = positionGuides;
		}
		
	}

	//Calculate overall difference of positions between an object and its desired position
	private float GetOverallDistanceDiff (GameObject obj, GameObject target){
		//Raw difference between the positions
		float difference = Vector3.Distance(obj.transform.position, target.transform.position);
		return difference;
	}

	//Calculate the distance difference for each coordinate between an object and its desired position
	//Returns the differences in an vector 3 object
	private Vector3 GetCoordinateDistanceDiff(GameObject obj, GameObject target){
		//X difference
		float xdiff = Mathf.Abs(obj.transform.position.x - target.transform.position.x);

		//Y difference
		float ydiff = Mathf.Abs(obj.transform.position.y - target.transform.position.y);

		//Z difference
		float zdiff = Mathf.Abs(obj.transform.position.z - target.transform.position.z);

		return new Vector3 (xdiff, ydiff, zdiff);
	}

	//Calculate overall difference of angles between an object and its desired position
	private float GetOverallAngleDiff(GameObject obj, GameObject target){
		//Raw difference between angles of position vectors
		float difference = Vector3.Distance(obj.transform.eulerAngles, target.transform.eulerAngles);
		return difference;
	}

	//Calculate the angular difference for each coordinate between an object and its desired position
	//Returns the differences in an vector 3 object
	private Vector3 GetCoordinateAngleDiff(GameObject obj, GameObject target){
		//X difference
		float xdiff = Mathf.Abs(obj.transform.eulerAngles.x - target.transform.eulerAngles.x);

		//Y difference
		float ydiff = Mathf.Abs(obj.transform.eulerAngles.y - target.transform.eulerAngles.y);

		//Z difference
		float zdiff = Mathf.Abs(obj.transform.eulerAngles.z - target.transform.eulerAngles.z);

		return new Vector3 (xdiff, ydiff, zdiff);
	}

	//change state of the boolean control for the position guides
	public void ChangePositionGuides(bool state){
		positionGuides = state;
	}

	//Change state of the boolean control for the collision guides
	public void ChangeCollisionGuides(bool state){
		collisionGuides = state;
	}
}
