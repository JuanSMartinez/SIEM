using UnityEngine;
using System.Collections;

public class Joint : MonoBehaviour {

	public GameObject boundedObject;

	//Anchor point
	private Transform anchor;

	//Limits in unity units and in degrees
	public float maxX;
	public float maxY;
	public float maxZ;
	public float maxRotationX;
	public float maxRotationY;
	public float maxRotationZ;

	// Use this for initialization
	void Start () {
		anchor = boundedObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

		//Check transaltion
		CheckTranslation (0, transform.position.x, anchor.position.x, maxX);
		CheckTranslation (1, transform.position.y, anchor.position.y, maxY);
		CheckTranslation (2, transform.position.z, anchor.position.z, maxZ);

		//Check rotation
		CheckRotation();

	
	}

	private void CheckTranslation(int index, float coordinate, float initialCoordinate, float maxDiff){
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

	private void CheckRotation(){
		float rotX;
		float rotY;
		float rotZ;

		if (transform.eulerAngles.x >= maxRotationX && transform.eulerAngles.x <= (360 - maxRotationX))
			rotX = roundExtremeValue (transform.eulerAngles.x, maxRotationX, 360 - maxRotationX);
		else
			rotX = transform.eulerAngles.x;

		if (transform.eulerAngles.y >= maxRotationY && transform.eulerAngles.y <= (360 - maxRotationY))
			rotY = roundExtremeValue (transform.eulerAngles.y, maxRotationY, 360 - maxRotationY);
		else
			rotY = transform.eulerAngles.y;

		if (transform.eulerAngles.z >= maxRotationZ && transform.eulerAngles.z <= (360 - maxRotationZ))
			rotZ = roundExtremeValue (transform.eulerAngles.z, maxRotationZ, 360 - maxRotationZ);
		else
			rotZ = transform.eulerAngles.z;
	
		Vector3 rotation = new Vector3 (rotX, rotY, rotZ);
		transform.eulerAngles = rotation;
	}

	private float roundExtremeValue(float angle, float min, float max){
		float diffMin = Mathf.Abs (min - angle);
		float diffMax = Mathf.Abs (max - angle);
		return diffMin < diffMax ? min : max;
	}


}
