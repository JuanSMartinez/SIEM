using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

/**
 * Simple manipulation of bones
*/
public class ReducionTraining: HapticClassScript {

	//Generic Haptic Functions
	private GenericFunctionsClass myGenericFunctionsClassScript;

	//Dictionary of elements and initial positions
	Dictionary<string, Vector3> dict = new Dictionary<string, Vector3>();

	//Index of spring force
	private int SPRING_INDEX;

	//Index of friction force
	private int VISCOSITY_INDEX;

	public GameObject canvas;

	/*****************************************************************************/

	void Awake()
	{
		myGenericFunctionsClassScript = transform.GetComponent<GenericFunctionsClass>();
	}

	void Start()
	{


		if (PluginImport.InitHapticDevice())
		{
			Debug.Log("OpenGL Context Launched");
			Debug.Log("Haptic Device Launched");

			getInitialParameters ();
			SPRING_INDEX = ForceManager.GetNextIndex();
			VISCOSITY_INDEX = ForceManager.GetNextIndex();

			myGenericFunctionsClassScript.SetHapticWorkSpace();
			myGenericFunctionsClassScript.GetHapticWorkSpace();

			//Update Workspace as function of camera
			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

			//Set Mode of Interaction
			/*
			 * Mode = 0 Contact
			 * Mode = 1 Manipulation - So objects will have a mass when handling them
			 * Mode = 2 Custom Effect - So the haptic device simulate vibration and tangential forces as power tools
			 * Mode = 3 Puncture - So the haptic device is a needle that puncture inside a geometry
			 */
			PluginImport.SetMode(1);
			//Show a text descrition of the mode
			myGenericFunctionsClassScript.IndicateMode();


			//Set the touchable face(s)
			PluginImport.SetTouchableFace(ConverterClass.ConvertStringToByteToIntPtr(TouchableFace));



		}
		else
			Debug.Log("Haptic Device cannot be launched");

		/***************************************************************/
		//Setup the Haptic Geometry in the OpenGL context
		/***************************************************************/

		myGenericFunctionsClassScript.SetHapticGeometry();

		/***************************************************************/
		//Launch the Haptic Event for all different haptic objects
		/***************************************************************/
		PluginImport.LaunchHapticEvent();
	}

	void Update()
	{

		/***************************************************************/
		//Update forces
		/***************************************************************/
		UpdateForces ();


		/***************************************************************/
		//Update Workspace as function of camera
		/***************************************************************/
		PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

		/***************************************************************/
		//Update cube workspace
		/***************************************************************/
		myGenericFunctionsClassScript.UpdateGraphicalWorkspace();

		/***************************************************************/
		//Haptic Rendering Loop
		/***************************************************************/
		PluginImport.RenderHaptic ();

		myGenericFunctionsClassScript.GetProxyValues();

		//myGenericFunctionsClassScript.GetTouchedObject();
		myGenericFunctionsClassScript.manipulateObject ();


	}

	void OnDisable()
	{
		if (PluginImport.HapticCleanUp())
		{
			Debug.Log("Haptic Context CleanUp");
			Debug.Log("Desactivate Device");
			Debug.Log("OpenGL Context CleanUp");
		}
	}
		

	//Updating spring and friction forces
	bool previousButtonState = false;
	string myObjStringName = "null";

	private void UpdateForces(){
		myObjStringName = ConverterClass.ConvertIntPtrToByteToString (PluginImport.GetTouchedObjectName ());
		if (!previousButtonState && PluginImport.GetButton1State ()) {
			if (!myObjStringName.Equals ("null")) {
				previousButtonState = true;
				StartBoundingForces (myObjStringName);

			}
		} else if (previousButtonState && !PluginImport.GetButton1State ()) {
			myObjStringName = "null";
			previousButtonState = false;
			StopForces ();
		}

		float distanceDiff_A = GetDistanceDiff ("A_real_smooth");
		float angleDiff_A = GetAngleDiff ("A_real_smooth");
		float distanceDiff_B = GetDistanceDiff ("B_real_smooth");
		float angleDiff_B = GetAngleDiff ("B_real_smooth");
		float[] parameters = { distanceDiff_A, angleDiff_A, distanceDiff_B, angleDiff_B};
		canvas.SendMessage ("Movement", parameters);


	
	}

	private void StartBoundingForces(string touchedObject){
		//Restart forces
		StopForces();
		SetSpringForce (touchedObject);
		SetViscousForce (touchedObject);

	}

	private float GetDistanceDiff (string touchedObject){
		//Get Touched Object initial position
		Vector3 initialPosition;
		try{
			initialPosition = dict[touchedObject];
		}
		catch (KeyNotFoundException e){
			throw e;
		}

		//Get current position
		Vector3 actual = GameObject.Find(touchedObject).transform.parent.position;

		//Raw difference between the positions
		float difference = Vector3.Distance(initialPosition, actual);
		return difference;
	}

	private float GetAngleDiff(string touchedObject){
		//Get Touched Object initial position
		Vector3 initialPosition;
		try{
			initialPosition = dict[touchedObject];
		}
		catch (KeyNotFoundException e){
			throw e;
		}

		//Get current position
		Vector3 actual = GameObject.Find(touchedObject).transform.parent.position;

		//Raw difference between angles of position vectors
		float difference = Vector3.Angle(initialPosition, actual);
		return difference;
	}


	private void getInitialParameters(){
		GameObject A = GameObject.Find ("A_real_smooth");
		GameObject B = GameObject.Find ("B_real_smooth");
		dict.Add ("A_real_smooth", A.transform.parent.position);
		dict.Add ("B_real_smooth", B.transform.parent.position);

	}
		

	private float GetSpringMagnitude(Vector3 initialPos, Vector3 actualPos){

		//Raw difference between the positions
		float difference = Vector3.Distance(initialPos, actualPos);

		//First aproximation: Magnitude as sigmoid function of the difference
		return Sigmoid(difference);
		//return 0.2f;
	}
		

	private float GetViscousMagnitude(Vector3 initialPos, Vector3 actualPos){
		
		//Raw difference between angles of position vectors
		float difference = Vector3.Angle(initialPos, actualPos);

		//Degrees to radians
		difference = difference*Mathf.PI/180.0f;

		//First aproximation: Magnitude as sigmoid function of the difference
		return Sigmoid(difference);
	}

	public float Sigmoid(float x){
		return 1 / (1 + Mathf.Exp (-x));
	}

	private void SetSpringForce(string touchedObjectName){
		
		//Get Touched Object initial position
		Vector3 initialPosition;
		try{
			initialPosition = dict[touchedObjectName];

		}
		catch (KeyNotFoundException){
			return;
		}
		//Scale position to workspace relative position and mm dimensions
		float multiplier = (162.56f/4f);
		Vector3 offset = new Vector3 (-0.02f, -0.75f, -0.6f);
		Vector3 touchedPosition = (initialPosition - offset) * multiplier;
	
		//Get current cursor position
		Vector3 cursorPosition = GameObject.Find("Cursor").transform.position;

		//Get forces direction
		Vector3 forward = (initialPosition - cursorPosition);

		//Set spring anchor point and direction effect 
		float[] position = new float[] {touchedPosition.x, touchedPosition.y, touchedPosition.z};
		float[] direction = new float[]{ forward.x, forward.y, forward.z };

		//Constant gain and variable magnitude
		float gain = 0.2f;
		float correction = 0.4f;
		float magnitude = GetSpringMagnitude (initialPosition, cursorPosition) - correction;
		Debug.Log ("Resorte: " + magnitude);

		//Start spring force
		ForceManager.SetEnvironmentForce(ForceManager.SPRING, SPRING_INDEX, position, direction, gain, magnitude, 0, 0);
	}

	private void SetViscousForce(string touchedObjectName){
		
		//Get Touched Object initial position
		Vector3 initialPosition;
		try{
			initialPosition = dict[touchedObjectName];
		}
		catch (KeyNotFoundException){
			return;
		}
		//Get current cursor position
		Vector3 cursorPosition = GameObject.Find("Cursor").transform.position;

		//Get forces direction
		Vector3 forward = initialPosition - cursorPosition;

		//Set friction anchor point and direction effect 
		float[] position = new float[] {initialPosition.x, initialPosition.y, initialPosition.z};
		float[] direction = new float[]{ forward.x, forward.y, forward.z };

		//Constant gain and magnitude
		float gain = 0.4f;
		//float magnitude = GetViscousMagnitude (initialPosition, cursorPosition);


		//Start friction force
		ForceManager.SetEnvironmentForce(ForceManager.VISCOSITY, VISCOSITY_INDEX, position, direction, gain, 0.8f, 0, 0);
	}

	private void StopForces(){
		ForceManager.StopEnvironmentForce (SPRING_INDEX);
		ForceManager.StopEnvironmentForce (VISCOSITY_INDEX);
	}

	 


}


