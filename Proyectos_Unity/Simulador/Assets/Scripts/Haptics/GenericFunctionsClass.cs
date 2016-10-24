﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class GenericFunctionsClass : MonoBehaviour {

	
	/*************************************************************/
	// Variables
	/*************************************************************/

	//Lock
	private static object obj = new object();
	
	//Haptic Properties
	private HapticProperties myHapticPropertiesScript;

	//Access to script SimpleShapeManipulation
	public HapticClassScript myHapticClassScript;

	
	//GetHapticWorkSpace Values
	private float[] myWSPosition = new float[3];
	private float[] myWSSize = new float[3];

	//GetProxyValues - for haptic proxy position and orientation
	private double[] myProxyPosition = new double[3];
	private double[] myProxyRight = new double[3];
	private double[] myProxyDirection = new double[3];
	private double[] myProxyTorque = new double[3];
	private double[] myProxyOrientation = new double[4];

	//Manipulated object variables
	private int clickCount = 0;
	private static bool grabbed = false;
	private GameObject manipObj = null;
	private Transform prevParent;


	
	/*************************************************************/
	


	// Use this for initialization
	void Awake () {

	}

	/******************************************************************************************************************************************************************/

	/*************************************************************/
	// Generic functionnalities
	/*************************************************************/


	/******************************************************************************************************************************************************************/
	//generic function that returns the current mode
	public void IndicateMode()
	{
		if(PluginImport.GetMode () == 0)
			myHapticClassScript.HapticMode = "Simple contact"; 
		else if(PluginImport.GetMode () == 1)
			myHapticClassScript.HapticMode = "Object Manipulation";
		else if(PluginImport.GetMode () == 2)
			myHapticClassScript.HapticMode = "Custom Effect";
		else if(PluginImport.GetMode () == 3)
			myHapticClassScript.HapticMode = "Puncture";
	}
	

	/******************************************************************************************************************************************************************/

	//Haptic workspace generic functions
	public void SetHapticWorkSpace()
	{
		
		//Convert float3Array to IntPtr
		IntPtr dstPosPtr = ConverterClass.ConvertFloat3ToIntPtr(myHapticClassScript.myWorkSpacePosition);
		
		//Convert float3Array to IntPtr
		IntPtr dstSizePtr = ConverterClass.ConvertFloat3ToIntPtr(myHapticClassScript.myWorkSpaceSize);
		
		//Set Haptic Workspace
		PluginImport.SetWorkspace(dstPosPtr,dstSizePtr);
	}
	
	public void GetHapticWorkSpace()
	{
		//Convert IntPtr to float3Array
		myWSPosition = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspacePosition());
		
		//Convert IntPtr to float3Array
		myWSSize = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspaceSize());
		
		//Refine my workspaceSize in the Unity Editor in case it has been changed
		myHapticClassScript.myWorkSpacePosition = ConverterClass.AssignFloat3ToFloat3(myWSPosition);
		
		//Refine my workspaceSize in the Unity Editor in case it has been changed
		myHapticClassScript.myWorkSpaceSize = ConverterClass.AssignFloat3ToFloat3(myWSSize);
	}

	public void UpdateGraphicalWorkspace()
	{
		//Position
		Vector3 pos;
		pos = ConverterClass.ConvertFloat3ToVector3(myWSPosition);
		myHapticClassScript.workSpaceObj.transform.position = pos;
		
		//Orientation
		myHapticClassScript.workSpaceObj.transform.rotation = Quaternion.Euler(0.0f,myHapticClassScript.myHapticCamera.transform.eulerAngles.y, 0.0f);
		
		//Scale
		Vector3 size;
		size = ConverterClass.ConvertFloat3ToVector3(myWSSize);
		myHapticClassScript.workSpaceObj.transform.localScale = size;
	}

	/******************************************************************************************************************************************************************/

	//Get Proxy Position and Orientation generic function
	public 	void GetProxyValues()
	{
		/*Proxy Position*/
		
		//Convert IntPtr to Double3Array
		myProxyPosition = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyPosition());
		
		//Attach the Cursor Node
		Vector3 positionCursor = new Vector3();
		positionCursor = ConverterClass.ConvertDouble3ToVector3(myProxyPosition);
		
		//Assign Haptic Values to Cursor
		myHapticClassScript.hapticCursor.transform.position = positionCursor;
		
		
		//Proxy Right - Not use in that case
		//Convert IntPtr to Double3Array
		myProxyRight =  ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyRight());
		//Attach the Cursor Node
		//Vector3 rightCursor = new Vector3();
		//rightCursor = ConverterClass.ConvertDouble3ToVector3(myProxyRight);

		//Proxy Direction
		//Convert IntPtr to Double3Array
		myProxyDirection =  ConverterClass.ConvertIntPtrToDouble3( PluginImport.GetProxyDirection());
		//Attach the Cursor Node
		Vector3 directionCursor = new Vector3();
		directionCursor = ConverterClass.ConvertDouble3ToVector3(myProxyDirection);

		//Proxy Torque
		myProxyTorque = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyTorque());
		//Attach the Cursor Node
		Vector3 torqueCursor = new Vector3();
		torqueCursor = ConverterClass.ConvertDouble3ToVector3(myProxyTorque);

		//Set Orientation
		myHapticClassScript.hapticCursor.transform.rotation = Quaternion.LookRotation(directionCursor,torqueCursor);
	
	}

	

	
		

	/******************************************************************************************************************************************************************/

	public void SetHapticGeometry()
	{
		//Get array of all object with tag "Touchable"
		GameObject[] myObjects = GameObject.FindGameObjectsWithTag("Touchable") as GameObject[];

        for (int ObjId = 0; ObjId < myObjects.Length; ObjId++)
		{
			/***************************************************************/
			//Set the Transformation Matric of the Object
			/***************************************************************/
			//Get the Transformation matrix from object
			Matrix4x4 m = new Matrix4x4();

			//Build a transform Matrix from the translation/rotation and Scale parameters fo the object - Glabal Matrix
			m  = myObjects[ObjId].transform.localToWorldMatrix;
			
			//Convert Matrix4x4 to double16
			double[] matrix = ConverterClass.ConvertMatrix4x4ToDouble16(m);
			//Convert Double16 To IntPtr
			IntPtr dstDoublePtr = ConverterClass.ConvertDouble16ToIntPtr(matrix);
			
			//Convert String to Byte[] (char* in C++) and Byte[] to IntPtr
			IntPtr dstCharPtr = ConverterClass.ConvertStringToByteToIntPtr(myObjects[ObjId].name);
			
			//Send the transformation Matrix of the object
			PluginImport.SetObjectTransform(ObjId, dstCharPtr, dstDoublePtr);
			
			/***************************************************************/
			
			/***************************************************************/
			//Set the Mesh of the Object
			/***************************************************************/
			//Get Mesh of Object
			Mesh mesh = myObjects[ObjId].GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;

			int[] triangles = mesh.triangles;
			
			//Reorganize the Array
			float[] verticesToSend = ConverterClass.ConvertVector3ArrayToFloatArray(vertices);
			//Allocate Memory according to needed space for float* (3*4)
			IntPtr dstVerticesArrayPtr = Marshal.AllocCoTaskMem(vertices.Length * 3 * Marshal.SizeOf(typeof(float)));
			//Copy to dstPtr
			Marshal.Copy(verticesToSend,0,dstVerticesArrayPtr,vertices.Length * 3);
			
			//Convert Int[] to IntPtr
			IntPtr dstTrianglesArrayPtr = ConverterClass.ConvertIntArrayToIntPtr(triangles);
			
			//Send the Raw Mesh of the object - transformation are not applied on the Mesh vertices
			PluginImport.SetObjectMesh(ObjId,dstVerticesArrayPtr, dstTrianglesArrayPtr,vertices.Length,triangles.Length);
			/***************************************************************/
			
			/***************************************************************/
			//Get the haptic parameter configuration
			/***************************************************************/
			ReadHapticProperties(ObjId, myObjects[ObjId]);
			/***************************************************************/
		}
	}

	//Haptic Properties generic function
	private void ReadHapticProperties(int ObjId, GameObject obj)
	{
		myHapticPropertiesScript = obj.transform.GetComponent<HapticProperties>();
		
		if (myHapticPropertiesScript == null)//Set default Values
		{
			PluginImport.SetStiffness(ObjId, 1.0f);
			PluginImport.SetDamping(ObjId, 0.0f);
			PluginImport.SetStaticFriction(ObjId, 0.0f);
			PluginImport.SetDynamicFriction(ObjId, 0.0f);
			PluginImport.SetTangentialStiffness(ObjId, 0.0f);
			PluginImport.SetTangentialDamping(ObjId, 0.0f);
			PluginImport.SetPopThrough(ObjId, 0.0f);
			PluginImport.SetPuncturedStaticFriction(ObjId, 0.0f);
			PluginImport.SetPuncturedDynamicFriction(ObjId, 0.0f);
			PluginImport.SetMass(ObjId,0.0f);
			PluginImport.SetFixed(ObjId,true);
			Debug.Log ("Haptic Characteristics not set for " + obj.name);
		}
		else
		{
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("stiffness"),myHapticPropertiesScript.stiffness);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("damping"),myHapticPropertiesScript.damping);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("staticFriction"),myHapticPropertiesScript.staticFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("dynamicFriction"),myHapticPropertiesScript.dynamicFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("tangentialStiffness"),myHapticPropertiesScript.tangentialStiffness);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("tangentialDamping"),myHapticPropertiesScript.tangentialDamping);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("popThrough"),myHapticPropertiesScript.popThrough);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("puncturedStaticFriction"),myHapticPropertiesScript.puncturedStaticFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("puncturedDynamicFriction"),myHapticPropertiesScript.puncturedDynamicFriction);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("mass"),myHapticPropertiesScript.mass);
			PluginImport.SetHapticProperty(ObjId,ConverterClass.ConvertStringToByteToIntPtr("fixed"),System.Convert.ToInt32(myHapticPropertiesScript.fixedObj));


		}
	}

	public void UpdateHapticObjectMatrixTransform()
	{
		//Get array of all object with tag "Touchable"
		GameObject[] myObjects = GameObject.FindGameObjectsWithTag("Touchable") as GameObject[];

        for (int ObjId = 0; ObjId < myObjects.Length; ObjId++)
		{
			/***************************************************************/
			//Set the Transformation Matric of the Object
			/***************************************************************/
			//Get the Transformation matrix from object
			Matrix4x4 m = new Matrix4x4();
			//Build a transform Matrix from the translation/rotation and Scale parameters fo the object
			m.SetTRS(myObjects[ObjId].transform.position,myObjects[ObjId].transform.rotation,myObjects[ObjId].transform.localScale);
			
			//Convert Matrix4x4 to double16
			double[] matrix = ConverterClass.ConvertMatrix4x4ToDouble16(m);
			//Convert Double16 To IntPtr
			IntPtr dstDoublePtr = ConverterClass.ConvertDouble16ToIntPtr(matrix);
			
			//Convert String to Byte[] (char* in C++) and Byte[] to IntPtr
			IntPtr dstCharPtr = ConverterClass.ConvertStringToByteToIntPtr(myObjects[ObjId].name);
			
			//Send the transformation Matrix of the object
			PluginImport.SetObjectTransform(ObjId, dstCharPtr, dstDoublePtr);
			
			/***************************************************************/
		}
	}

	/******************************************************************************************************************************************************************/

	//Haptic Effects generic functions


	public void manipulateObject(){
		//Convert Convert IntPtr To byte[] to String
		string myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjectName());

		//If in Manipulation Mode enable the manipulation of the selected object

		if(PluginImport.GetButton1State())
		{
			if(clickCount == 0)
			{
				//Set the manipulated object at first click
				manipObj = GameObject.Find (myObjStringName);

				//Setup Manipulated object Hierarchy as a child of haptic cursor - Only if object is declared as Manipulable object
				if(manipObj != null && !PluginImport.IsFixed(PluginImport.GetManipulatedObjectId()))
				{
					//Store the Previous parent object 
					prevParent = manipObj.transform.parent.parent;


					//Asign New Parent - the tip of the manipulation object device
					manipObj.transform.parent.parent = myHapticClassScript.hapticCursor.transform;



				}

			}
			clickCount++;
			Grab ();
		}
		else 
		{
			//Reset Click counter
			clickCount = 0;

			//Reset Manipulated Object Hierarchy
			if (manipObj != null && !myObjStringName.Equals("Musculo")) {
				manipObj.transform.parent.parent = prevParent;

			}

			//Reset Manipulated Object
			manipObj = null;

			//Reset prevParent
			prevParent = null;

			//Clear all unwanted childs
			for (int i = 0; i < myHapticClassScript.hapticCursor.transform.childCount; i++) {
				if(!myHapticClassScript.hapticCursor.transform.GetChild(i).name.Equals("Sphere")
					&& !myHapticClassScript.hapticCursor.transform.GetChild(i).name.Equals("Capsule"))
					myHapticClassScript.hapticCursor.transform.GetChild(i).transform.parent = null;
			}

			Ungrab ();
		}

		//Only in Manipulation otherwise object are not moving so there is no need to proceed
		UpdateHapticObjectMatrixTransform();

	}

	public static void Grab(){
		lock (obj) {
			grabbed = true;
		}
	}

	public static void Ungrab(){
		lock (obj) {
			grabbed = false;
		}
	}

	public static bool GetGrabbed(){
		lock(obj){
			return grabbed;
		}
	}

	private static void ToggleGrabbed(){
		lock (obj) {
			grabbed = !grabbed;
		}
	}
	/******************************************************************************************************************************************************************/
}
