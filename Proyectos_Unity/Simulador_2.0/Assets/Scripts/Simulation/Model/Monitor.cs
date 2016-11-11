using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Monitor : MonoBehaviour {

	//path to log of position movement
	private static string POSITION_FILE_BASE;

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

	//Log file for movement tracking
	private StreamWriter positions_log;

	//Canvas
	public UIManager canvas;

	//Boolean control to tell if the training has started
	public bool start;

	// Use this for initialization
	void Start () {
		start = false;
		POSITION_FILE_BASE = Application.dataPath;
		positions_log = null;
	}

	void OnDisable()
	{
		
	}
	
	// Update is called once per frame
	void Update () {

		//Show final positions if the guide options is enabled
		ShowFinalGuides();

		//Log position differences
		if(start)
			TrackMovements();
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

	//Log position differences 
	private void TrackMovements(){
		int positionedObjects = 0;
		//Loop through all tracked game objects
		for(int i = 0; i < objects.Length; i++){
			GameObject obj = objects[i];
			GameObject target = finalObjects[i];

			//Get individual coordinate linear differences
			Vector3 vectorDistanceDiff = GetCoordinateDistanceDiff(obj, target);

			//Get overal linear difference
			float overalDistanceDiff = GetOverallDistanceDiff(obj, target);

			//Get individual coordinate linear differences
			Vector3 vectorAngleDiff = GetCoordinateAngleDiff(obj, target);

			//Get overal linear difference
			float overalAngleDiff = GetOverallAngleDiff(obj, target); 

			//Log differences
			if (positions_log == null)
				canvas.SendMessage ("TrainingNotStarted");
			else {
				positions_log.WriteLine (obj.name + "," +
				vectorDistanceDiff.x + "," + vectorDistanceDiff.y + "," + vectorDistanceDiff.z + "," +
				overalDistanceDiff + "," +
				vectorAngleDiff.x + "," + vectorAngleDiff.y + "," + vectorAngleDiff.z + "," +
				overalAngleDiff);
			}

			//Check object if correctly positioned
			if (overalAngleDiff <= angularTolerance && overalDistanceDiff <= linearTolerance)
				positionedObjects += 1;
		}

		if (positionedObjects == objects.Length)
			canvas.SendMessage ("Finished");

	}

	//Create the log file to track movements
	public void CreateLog(){
		if (positions_log == null) {

			if (File.Exists (POSITION_FILE_BASE + SceneManager.GetActiveScene ().name + ".txt")) {
				File.Delete (POSITION_FILE_BASE + SceneManager.GetActiveScene ().name + ".txt");
			}

			positions_log = File.CreateText (POSITION_FILE_BASE + SceneManager.GetActiveScene ().name + ".txt");
			positions_log.WriteLine ("Tracking de posiciones para la escena " + SceneManager.GetActiveScene ().name);
		} else
			canvas.SendMessage ("TrainingRunning");
	}

	//Close stream to the log file
	public void CloseFile(){
		if (positions_log == null)
			canvas.SendMessage ("TrainingNotStarted");
		else {
			positions_log.Close ();
			positions_log = null;
		}
	}
}
