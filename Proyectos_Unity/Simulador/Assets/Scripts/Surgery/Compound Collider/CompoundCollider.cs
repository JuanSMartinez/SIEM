using UnityEngine;
using System.Collections;
using System;
public class CompoundCollider : MonoBehaviour {

	public int n = 5;
	public Material capMaterial;
	public string childName = "Cylinder";
	public string childCutName = "Cylinder";
	// Use this for initialization
	void Start(){

		//Childs
		GameObject child = transform.Find (childName).gameObject;
		GameObject childCut = transform.Find (childCutName).gameObject;

		//Bounds
		Bounds bounds = childCut.GetComponent<MeshRenderer>().bounds;

		//Vertices and triangles
		Vector3[] vertices = child.GetComponent<MeshFilter>().mesh.vertices;
		int[] triangles = child.GetComponent<MeshFilter>().mesh.triangles;

		Vector2[] uvs = new Vector2[childCut.GetComponent<MeshFilter>().mesh.vertices.Length];
		childCut.GetComponent<MeshFilter> ().mesh.uv = uvs;

		//Sorted indices from bounds
		int[] sortedIndices = SortIndices (bounds.size);


		float step = (bounds.size [sortedIndices [0]] / n);
		GameObject nextPiece = null;
		for (int i = 1; i < n+1; i++) {
			GameObject plane = CreateCuttingPlane(bounds, sortedIndices, i*step);
			GameObject copy = nextPiece == null ? GameObject.Instantiate (childCut) : nextPiece;
			copy.transform.position = childCut.transform.position;
			copy.transform.rotation = childCut.transform.rotation;
			GameObject[] pieces = myMeshCut.Cut (copy,
				plane.transform.position,
				//new Vector3(0,1,0),
				plane.transform.TransformDirection (plane.GetComponent<MeshFilter> ().mesh.normals [sortedIndices[0]]),
				capMaterial);
			GameObject capsule = CreateChildSphere (pieces[0], pieces[0].GetComponent<MeshRenderer>().bounds, sortedIndices);
			capsule.transform.parent = transform;
			Destroy (pieces [0]);
			nextPiece = pieces [1];
			Destroy (pieces [1]);
			Destroy (plane);
			Destroy (copy);
		}
			
	}
		
	/**
	 * Create child capsule positioned around a mesh bounds
	 * */
	private GameObject CreateChildSphere(GameObject around, Bounds bounds, int[] sortedIndices){
		//Reference object in the world space
		//GameObject reference = new GameObject();
		//reference.name = "Ref";

		//Create the capsule
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		capsule.GetComponent<MeshRenderer> ().enabled = false;

		//Scale to the second biggest axis and the third biggest axis
		Vector3 scale = new Vector3(bounds.extents[sortedIndices[0]], bounds.extents[sortedIndices[0]], bounds.extents[sortedIndices[0]]);
		capsule.transform.localScale = scale;

		//Rotate and scale the plane 
		Vector3 angles;
		//float shift = Vector3.Angle (reference.transform.position, around.transform.position);
		switch (sortedIndices [0]) {
		case 0:
			//El eje X es el mayor 
			angles = new Vector3(0, 0, 90);				
			break;
		case 1:
			angles = new Vector3(0, 0, 0);
			break;
		case 2:
			angles = new Vector3(90, 0, 0);
			break;
		default:
			angles = new Vector3(0, 0, 0);
			break;
		}
		capsule.transform.Rotate (angles);

		//Position capsule in the center of the object
		capsule.transform.position = bounds.center;

		return capsule;

	}

	/**
	 * Create and center a cutting plane at a specified distance from the center of the object, along
	 * the biggest axis
	 * */
	private GameObject CreateCuttingPlane(Bounds bounds, int[] sortedIndices, float shift){

		//Create the plane

		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

		//Rotate and scale the plane 
		Vector3 angles;
		switch (sortedIndices [0]) {
		case 0:
			//El eje X es el mayor 
			angles = new Vector3(0, 0, 90);				
			break;
		case 1:
			angles = new Vector3(0, 0, 0);
			break;
		case 2:
			angles = new Vector3(90, 0, 0);
			break;
		default:
			angles = new Vector3(0, 0, 0);
			break;
		}
		plane.transform.Rotate (angles);

		//Scale to the second biggest axis
		Vector3 scale = new Vector3(bounds.extents[sortedIndices[1]], 1, bounds.extents[sortedIndices[1]]);
		plane.transform.localScale = scale;

		//Position plane in the center of the object and shift
		plane.transform.position = bounds.center - bounds.extents;
		Vector3 translation;

		switch (sortedIndices [0]) {
		case 0:
			translation = new Vector3(shift, 0, 0);				
			break;
		case 1:
			translation = new Vector3(0, shift, 0);	
			break;
		case 2:
			translation = new Vector3(0, 0, shift);	
			break;
		default:
			translation = new Vector3(0, 0, 0);	
			break;
		}
		plane.transform.position = plane.transform.position + translation;

		return plane;

	}

	/**
	 * Sort indices for axis in bounding box
	 * Returns:
	 * index 0 -> biggest axis
	 * index 1 -> middle axis
	 * index 2 -> smallest axis
	 * */
	private int[] SortIndices(Vector3 size){
		float parentX = size.x;
		float parentY = size.y;
		float parentZ = size.z;
		float[] coords = { parentX, parentY, parentZ };
		int[] indices = { 0, 1, 2 };
		for (int i = 0; i < coords.Length - 1; i++)
		{
			float max = coords[i];
			int maxIndex = indices[i];
			int index = i;
			for (int j = i + 1; j < coords.Length; j++)
			{
				if (coords[j] > max)
				{
					max = coords[j];
					maxIndex = indices[j];
					index = j;
				}
			}
			float temp = coords[i];
			coords[i] = max;
			coords[index] = temp;

			int tempI = indices[i];
			indices[i] = maxIndex;
			indices[index] = tempI;
		}
		return indices;

	}
	

		


}
