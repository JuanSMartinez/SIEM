using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour {

	public float speed = 2.0f;

	//Initial forward position
	private Vector3 initial;

	// Use this for initialization
	void Start () {
		//Get initial transform of the object
		initial = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left* speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position += Vector3.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			transform.position += Vector3.back* speed * Time.deltaTime;
		}
		checkBounds ();
	}

	//Check bounds and alert
	private void checkBounds(){
		Vector3 actualPosition = gameObject.transform.position;
		float diff = Mathf.Abs (Vector3.Distance (actualPosition, initial));
		GUIText text = GameObject.Find ("Label").GetComponent<GUIText> ();
		text.text = "Initial: " + initial +
		"\n Actual: " + actualPosition +
		"\n Diff: " + diff;
		
	}
}
