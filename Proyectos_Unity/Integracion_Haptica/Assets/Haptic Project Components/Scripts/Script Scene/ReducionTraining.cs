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
		//Set Environmental Haptic Effect
		/***************************************************************/

		// Constant Force Example - We use this environmental force effect to simulate the weight of the cursor
		//myGenericFunctionsClassScript.SetEnvironmentConstantForce();

		/***************************************************************/
		//Setup the Haptic Geometry in the OpenGL context
		/***************************************************************/

		myGenericFunctionsClassScript.SetHapticGeometry();


		//Get the Number of Haptic Object
		//Debug.Log ("Total Number of Haptic Objects: " + PluginImport.GetHapticObjectCount());

		/***************************************************************/
		//Launch the Haptic Event for all different haptic objects
		/***************************************************************/
		PluginImport.LaunchHapticEvent();
	}

	void Update()
	{

		/***************************************************************/
		//Act on the rigid body of the Manipulated object
		// if Mode = Manipulation Mode
		/***************************************************************/
		if (PluginImport.GetMode () == 1) {
			ActivatingGrabbedObjectPropperties();
		}

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

	//Deactivating gravity and enabled kinematics when a object is grabbed
	bool previousButtonState = false;
	string myObjStringName = "null";

	void ActivatingGrabbedObjectPropperties(){

		GameObject grabbedObject;


		if (!previousButtonState && PluginImport.GetButton1State ()) {
			
			myObjStringName = ConverterClass.ConvertIntPtrToByteToString (PluginImport.GetTouchedObjectName ());

			if (!myObjStringName.Equals ("null")) {
				previousButtonState = true;
				startBoundingForces (myObjStringName);
			}
		} else if (previousButtonState && !PluginImport.GetButton1State ()) {
			myObjStringName = "null";
			previousButtonState = false;
			stopBoundingForces ();
		}
	
	}

	private void startBoundingForces(string touchedObject){
		//Get Touched Object initial position
		Vector3 touchedPosition = dict[touchedObject]*1000;
		//Vector3 touchedPosition = new Vector3(-3,0,0);
		if (touchedPosition == null) {
			throw new Exception ("Position not present in dictionary");
		}
		//Get current cursor position
		Vector3 cursorPosition = GameObject.Find("Cursor").transform.position;

		//Get forces direction
		Vector3 forward = touchedPosition - cursorPosition;
		//Vector3 forward = new Vector3(-3,0,0);
		Debug.Log("Posicion: " + touchedPosition);
		Debug.Log ("Direction: " + forward);

		//Get Force Scripts
		SpringEffect spring = transform.GetComponent<SpringEffect>();
		FrictionEffect friction = transform.GetComponent<FrictionEffect> ();

		//Set Spring anchor point and direction effect 
		spring.positionEffect = new float[] {touchedPosition.x, touchedPosition.y, touchedPosition.z};
		spring.directionEffect = new float[]{ forward.x, forward.y, forward.z };

		//Set spring magnitude and gain
		spring.gain = 0.5f;
		spring.magnitude = 0.5f;

		//Set friction anchor point and direction effect 
		friction.positionEffect = new float[] {touchedPosition.x, touchedPosition.y, touchedPosition.z};
		friction.directionEffect = new float[]{ forward.x, forward.y, forward.z };

		//Set friction magnitude and gain
		friction.gain = 0.5f;
		friction.magnitude = 0.5f;

		//Restart effects
		stopBoundingForces();
		myGenericFunctionsClassScript.SetEnvironmentSpring ();
		myGenericFunctionsClassScript.SetEnvironmentFriction();

	}

	private void stopBoundingForces(){
		myGenericFunctionsClassScript.stopEnvironmentSpringForce ();
		myGenericFunctionsClassScript.stopEnvironmentFriction ();
	}


	private void getInitialParameters(){
		GameObject A = GameObject.Find ("A_real_smooth");
		GameObject B = GameObject.Find ("B_real_smooth");
		dict.Add ("A_real_smooth", A.transform.parent.position);
		dict.Add ("B_real_smooth", B.transform.parent.position);
	}
		

	private Vector3 getCurrentForward(string objName){
		
		return GameObject.Find (objName).transform.forward;
	}



	 


}


