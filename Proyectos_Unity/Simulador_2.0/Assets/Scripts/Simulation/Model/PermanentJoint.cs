using UnityEngine;
using System.Collections;

public class PermanentJoint : MonoBehaviour {

	//Calibartion offset
	public Vector3 offset = new Vector3 (-0.02f, -0.75f, -0.6f);

	//Bounded object that holds the anchor point
	public GameObject boundedObject;

	//Type of force, defined as a constant of ForceManager
	public string forceType = ForceManager.SPRING;

	//Name of haptic element as a child or the game object itself
	public string touchableName;

	//Force gain
	public float gain = 0.2f;

	//Force magnitude
	public float magnitude = 0.7f;

	//Force index, obtained as a sequential index from ForceManager
	private int forceIndex;

	//Anchor point
	private Transform anchor;

	//Cursor that feels the force of collisions
	public GameObject cursor;

	//Limits in unity units and in degrees
	public float maxX;
	public float maxY;
	public float maxZ;
	public float maxRotationX;
	public float maxRotationY;
	public float maxRotationZ;

	//Indicator to tell that the force started
	private bool forceStarted;


	// Use this for initialization
	void Start () {
		anchor = boundedObject.transform;
		forceIndex = ForceManager.GetNextIndex ();
		//Debug.Log ("Force index " + forceIndex + " given to permanent joint for "+forceType+ " in " + gameObject.name + " game object");
		forceStarted = false;
	}


	// Update is called once per frame
	void Update () {
		anchor = boundedObject.transform;
		if (enabled) {
			//Check transaltion
			CheckTranslation (0, transform.position.x, anchor.position.x, maxX);
			CheckTranslation (1, transform.position.y, anchor.position.y, maxY);
			CheckTranslation (2, transform.position.z, anchor.position.z, maxZ);

			//Check rotation
			CheckRelativeRotation ();

			if (HapticManager.GetGrabbed () != null && HapticManager.GetGrabbed ().name.Equals (touchableName) && !forceStarted) {

				//Scale position to workspace relative position and mm dimensions for spring forces
				float multiplier = (162.56f/4f);
				 
				Vector3 springPosition = (anchor.position - offset) * multiplier;

				Vector3 pos = forceType.Equals (ForceManager.SPRING) ? springPosition : anchor.position;

				//Set anchor point and direction effect 
				float[] position = new float[] { pos.x, pos.y, pos.z };
				float[] direction = new float[]{ -cursor.transform.position.x, -cursor.transform.position.y, -cursor.transform.position.z };

				//Start the force
				ForceManager.SetEnvironmentForce (forceType, forceIndex, position, direction, gain, magnitude, 0, 0);
				forceStarted = true;
			}

			if (HapticManager.GetGrabbed () == null) {
				ForceManager.StopEnvironmentForce (forceIndex);
				forceStarted = false;
			}

		}
			
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


	private void CheckRelativeRotation(){
		float rotX;
		float rotY;
		float rotZ;

		float diffX = transform.eulerAngles.x - anchor.eulerAngles.x;
		float diffY = transform.eulerAngles.y - anchor.eulerAngles.y;
		float diffZ = transform.eulerAngles.z - anchor.eulerAngles.z;
		//Debug.Log ("(" + transform.eulerAngles.y + "," + anchor.eulerAngles.y + ")");
		if (diffX >= maxRotationX && diffX <= (360 - maxRotationX))
			rotX = roundExtremeValue (diffX, maxRotationX, 360 - maxRotationX) + anchor.eulerAngles.x;
		else
			rotX = transform.eulerAngles.x;

		if (diffY >= maxRotationY && diffY <= (360 - maxRotationY))
			rotY = roundExtremeValue (diffY, maxRotationY, 360 - maxRotationY) + anchor.eulerAngles.y;
		else
			rotY = transform.eulerAngles.y;

		if (diffZ >= maxRotationZ && diffZ <= (360 - maxRotationZ))
			rotZ = roundExtremeValue (diffZ, maxRotationZ, 360 - maxRotationZ) + anchor.eulerAngles.z;
		else
			rotZ = transform.eulerAngles.z;
		
		Vector3 rotation = new Vector3 (rotX, rotY, rotZ);
		transform.eulerAngles = rotation;

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
