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

	//Initial positions of fragments
	Transform initA;
	Transform initB;
	Bounds boundsA;
	Bounds boundsB;

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
	string grabbedObjectName = "";

	void ActivatingGrabbedObjectPropperties(){

		GameObject grabbedObject;
		string myObjStringName;

		if (!previousButtonState && PluginImport.GetButton1State ()) {
			
			myObjStringName = ConverterClass.ConvertIntPtrToByteToString (PluginImport.GetTouchedObjectName ());
			//startBoundingForces ();
			if(!myObjStringName.Equals("null") )
				previousButtonState = true;
		} else if (previousButtonState && !PluginImport.GetButton1State ()) {
			
			grabbedObjectName = "";
			stopBoundingForces ();
			previousButtonState = false;
		} else if (previousButtonState){
			stopBoundingForces ();
			startBoundingForces ();
		}
	
	}


	private void startBoundingForces(){
		
		GameObject cursor = GameObject.Find ("Cursor");
		Vector3 pos = cursor.transform.forward;
		float diff = Math.Abs(1 - Vector3.Distance (pos, initA.forward));
		ConstantForceEffect myContantForceScript = transform.GetComponent<ConstantForceEffect> ();
		if (PluginImport.GetButton1State ()) {
			myContantForceScript.directionEffect = new float[]{pos.x, pos.y, pos.z};
			myContantForceScript.gain = diff;
			myContantForceScript.magnitude = diff;

		} 
		else {
			myContantForceScript.directionEffect = new float[]{0, -1, 0};
			myContantForceScript.gain = 0.1f;
			myContantForceScript.magnitude = 0.1f;
		}
			myGenericFunctionsClassScript.SetEnvironmentConstantForce();

	}

	private void stopBoundingForces(){
		myGenericFunctionsClassScript.StopEnvironmentConstantForce ();

	}

	private void getInitialParameters(){
		GameObject A = GameObject.Find ("A_real_smooth");
		GameObject B = GameObject.Find ("B_real_smooth");
		initA = A.transform;
		initB =B.transform;

		boundsA = A.GetComponentInChildren<MeshFilter> ().mesh.bounds;
		boundsB = B.GetComponentInChildren<MeshFilter> ().mesh.bounds;
	}



	 


}


