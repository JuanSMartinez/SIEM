using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour {

	public float speed = 2.0f;
	public float maxDiff = 1.0f;

	//Initial position
	private Vector3 init;

	// Use this for initialization
	void Start () {
		//Get initial transform of the object
		init = transform.position;
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
		
		setCoordinate (0, transform.position.x, init.x);
		setCoordinate (1, transform.position.y, init.y);
		setCoordinate (2, transform.position.z, init.z);

	}

	private void setCoordinate(int index, float coordinate, float initialCoordinate){
		float diff = Mathf.Abs (coordinate - initialCoordinate);
		int multiplier = coordinate >= initialCoordinate ? 1 : -1;
		float value = diff >= maxDiff ? initialCoordinate + multiplier*maxDiff : coordinate;
		Vector3 newPos;
		switch (index) {
		case 0:
			newPos = new Vector3 (value, transform.position.y, transform.position.z);
			transform.position = newPos;
			break;
		case 1:
			newPos = new Vector3 (transform.position.x, value, transform.position.z);
			transform.position = newPos;
			break;
		case 2:
			newPos = new Vector3 (transform.position.x, transform.position.y, value);
			transform.position = newPos;
			break;
		default:
			break;
		}
	}
}
