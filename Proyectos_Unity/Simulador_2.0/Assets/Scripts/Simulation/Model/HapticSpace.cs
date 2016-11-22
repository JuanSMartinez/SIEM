using UnityEngine;
using System.Collections;
using System;

public class HapticSpace : MonoBehaviour {

	/**
	 * Necessary components for the haptic device
	 * */
	public GameObject myHapticCamera;
	public GameObject workSpaceObj;
	public GameObject hapticCursor;
	public HapticManager hapticManager;

	/**
	 * Distance from the cursor from which an object can be grabbed
	 * */
	public float grabbingDistance = 0.2f;

	void Start(){
		if (PluginImport.InitHapticDevice ()) {
			Debug.Log("Haptic Device Launched");

			//Set workspace
			hapticManager.SetHapticWorkSpace ();
			hapticManager.GetHapticWorkSpace ();

			//Update Workspace as function of camera
			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

			//Set manipulation mode
			PluginImport.SetMode(1);

			//Set the touchable face(s)
			PluginImport.SetTouchableFace(ConverterClass.ConvertStringToByteToIntPtr("front"));

		} else
			Debug.Log ("Haptic device cannot be launched");

		//Set haptic geometry and launch event
		hapticManager.SetHapticGeometry();
		PluginImport.LaunchHapticEvent();
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

	void Update()
	{
		try{
			//Update Workspace as function of camera
			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

			//Update cube workspace
			hapticManager.UpdateGraphicalWorkspace();

			//Haptic Rendering Loop
			PluginImport.RenderHaptic ();

			hapticManager.GetProxyValues();

			//Move object with the cursor;
			//hapticManager.manipulateObject ();
			hapticManager.manipulateObjectRayCast();
		}
		catch(Exception){
			Debug.Log ("Exception in main loop");
			PluginImport.HapticCleanUp ();
		}

	}





}
