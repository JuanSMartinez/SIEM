using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class HapticManager: MonoBehaviour{

	/*************************************************************/
	// Variables for haptic management
	/*************************************************************/

	/**
	* Haptic space object
	*/
	public HapticSpace hapticSpace;

	/**
	 * Haptic workspace size
	 */
	public float[] myWSSize = new float[3];

	/**
	 * Haptic workspace position
	 */
	public float[] myWSPosition = new float[3];

	/**
	 * Proxy values
	 */
	private double[] myProxyPosition = new double[3];
	private double[] myProxyDirection = new double[3];
	private double[] myProxyTorque = new double[3];

	/**
	 * Manipulated object variables
	 */
	private int clickCount = 0;
	private static GameObject manipObj = null;
	private Transform prevParent;

	/**************************************************************/
	// Haptic workspace generic functions
	/*************************************************************/
	public void SetHapticWorkSpace()
	{

		//Convert float3Array to IntPtr
		IntPtr dstPosPtr = ConverterClass.ConvertFloat3ToIntPtr(myWSPosition);

		//Convert float3Array to IntPtr
		IntPtr dstSizePtr = ConverterClass.ConvertFloat3ToIntPtr(myWSSize);

		//Set Haptic Workspace
		PluginImport.SetWorkspace(dstPosPtr,dstSizePtr);
	}

	public void GetHapticWorkSpace()
	{
		//Convert IntPtr to float3Array
		myWSPosition = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspacePosition());

		//Convert IntPtr to float3Array
		myWSSize = ConverterClass.ConvertIntPtrToFloat3(PluginImport.GetWorkspaceSize());
	}

	public void UpdateGraphicalWorkspace()
	{
		//Position
		Vector3 pos;
		pos = ConverterClass.ConvertFloat3ToVector3(myWSPosition);
		hapticSpace.workSpaceObj.transform.position = pos;

		//Orientation
		hapticSpace.workSpaceObj.transform.rotation = Quaternion.Euler(0.0f,hapticSpace.myHapticCamera.transform.eulerAngles.y, 0.0f);

		//Scale
		Vector3 size;
		size = ConverterClass.ConvertFloat3ToVector3(myWSSize);
		hapticSpace.workSpaceObj.transform.localScale = size;
	}

	/**************************************************************/
	// Get Proxy Position and Orientation 
	/*************************************************************/

	public void GetProxyValues()
	{
		/*Proxy Position*/

		//Convert IntPtr to Double3Array
		myProxyPosition = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyPosition());

		//Attach the Cursor Node
		Vector3 positionCursor = new Vector3();
		positionCursor = ConverterClass.ConvertDouble3ToVector3(myProxyPosition);

		//Assign Haptic Values to Cursor
		hapticSpace.hapticCursor.transform.position = positionCursor;

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
		hapticSpace.hapticCursor.transform.rotation = Quaternion.LookRotation(directionCursor,torqueCursor);
	}

	/**************************************************************/
	// Haptic geometry configuration
	/*************************************************************/
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

	/**************************************************************/
	// Read haptic properties of a haptic element
	/*************************************************************/
	private void ReadHapticProperties(int ObjId, GameObject obj)
	{
		HapticProperties myHapticPropertiesScript = obj.transform.GetComponent<HapticProperties>();

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

	/**************************************************************/
	// Manipulate an object with the haptic cursor
	/*************************************************************/
	public void manipulateObject(){
		//Convert Convert IntPtr To byte[] to String
		string myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjectName());

		//If in Manipulation Mode enable the manipulation of the selected object

		if(PluginImport.GetButton2State())
		{
			if(clickCount == 0)
			{
				//Set the manipulated object at first click
				manipObj = GameObject.Find (myObjStringName);

				//Setup Manipulated object Hierarchy as a child of haptic cursor - Only if object is declared as Manipulable object
				if(manipObj != null && !PluginImport.IsFixed(PluginImport.GetManipulatedObjectId()))
				{
					//Store the Previous parent object that is higher in the hierarchy
					prevParent = manipObj.transform.parent.parent;
			
					//Asign New Parent - the tip of the manipulation object device
					manipObj.transform.parent.parent = hapticSpace.hapticCursor.transform;

				}

			}
			clickCount++;

		}
		else 
		{
			//Reset Click counter
			clickCount = 0;

			//Reset Manipulated Object Hierarchy
			if (manipObj != null && manipObj.transform.parent != null) {
				manipObj.transform.parent.parent = prevParent;

			}

			//Reset Manipulated Object
			manipObj = null;

			//Reset prevParent
			prevParent = null;

		}

		//Only in Manipulation otherwise object are not moving so there is no need to proceed
		UpdateHapticObjectMatrixTransform();

	}

	//Returns if there is any object grabbed by the cursor
	public static GameObject GetGrabbed(){
		return manipObj;
	}


}
