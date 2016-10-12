using UnityEngine;
using System.Collections;

public class CompoundCollider : MonoBehaviour {

	public int n = 100;

	// Use this for initialization
	void Start () {

		//Bounds
		Bounds bounds = gameObject.GetComponentInChildren<MeshRenderer>().bounds;
		int[] sortedIndices = SortIndices (bounds.size);
		TestCapsule (gameObject.transform, bounds, sortedIndices);
	}

	/**
	 * Slice mesh between two planes A and B
	 * */
	private Mesh SliceMesh(Vector3[] verticesA, Vector3[] verticesB, Vector3[] vertices, int[] triangles, int biggestIndex){

		//New Mesh
		Mesh sliced = new Mesh();
		Vector3[] newVertices;
		Vector2[] newUV;
		int[] newTriangles;

		for (int i = 0; i < vertices.Length; i++) {
			//If the vertex is inside the planes or in the volume between them, add it to the new mesh
			if (SuperiorVertex (vertices [i], verticesA, biggestIndex) && InferiorVertex (vertices [i], verticesB, biggestIndex)) {

			}
		}

		return sliced;
	}

	/**
	 * Check if vertex is greater to a plane or inside in a certain direction
	 * */
	private bool SuperiorVertex(Vector3 vertex, Vector3[] planeVertices, int biggestIndex){
		bool response = false;
		for(int i = 0; i < planeVertices.Length && !response; i++){
			if (vertex [biggestIndex] >= planeVertices [i] [biggestIndex])
				response = true;
		}
		return response;
	}

	/**
	 * Check if vertex is lower to a plane or inside in a certain direction
	 * */
	private bool InferiorVertex(Vector3 vertex, Vector3[] planeVertices, int biggestIndex){
		bool response = false;
		for(int i = 0; i < planeVertices.Length && !response; i++){
			if (vertex [biggestIndex] <= planeVertices [i] [biggestIndex])
				response = true;
		}
		return response;
	}
		
	private void TestCapsule(Transform transform, Bounds bounds, int[] sortedIndices){
		//Create the capsule
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		//Rotate
		Vector3 baseRotation;
		switch (sortedIndices [0]) {
		case 0:
			//El eje X es el mayor 
			baseRotation = new Vector3(0, 0, 90);				
			break;
		case 1:
			baseRotation = new Vector3(0, 0, 0);
			break;
		case 2:
			baseRotation = new Vector3(90, 0, 0);
			break;
		default:
			baseRotation = new Vector3(0, 0, 0);
			break;
		}
		capsule.transform.Rotate (transform.eulerAngles + baseRotation);

		//Position capsule in the center of the object
		capsule.transform.position = bounds.center;
	}
		
	/**
	 * Create child capsule positioned around a mesh bounds
	 * */
	private void CreateChildCapsule(Bounds bounds, int[] sortedIndices){
		//Create the capsule
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);

		//Scale to the second biggest axis and the third biggest axis
		Vector3 scale = new Vector3(bounds.size[sortedIndices[1]], bounds.extents[sortedIndices[0]], bounds.size[sortedIndices[1]]);
		capsule.transform.localScale = scale;

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
		capsule.transform.Rotate (angles);

		//Position capsule in the center of the object
		capsule.transform.position = bounds.center;

	}

	/**
	 * Create and center cutting plane
	 * */
	private GameObject CreateCuttingPlane(Bounds bounds, int[] sortedIndices){

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

		//Position plane in the center of the object
		plane.transform.position = bounds.center;

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
