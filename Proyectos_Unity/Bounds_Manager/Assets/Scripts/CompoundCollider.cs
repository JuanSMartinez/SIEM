﻿using UnityEngine;
using System.Collections;
using System;
public class CompoundCollider : MonoBehaviour {

	public int n = 5;
	public Material capMaterial;
	public string childName = "Cylinder";
	// Use this for initialization
	void Start(){

		//Bounds
		Bounds bounds = gameObject.GetComponentInChildren<MeshRenderer>().bounds;

		//Vertices and triangles
		Vector3[] vertices = gameObject.GetComponentInChildren<MeshFilter>().mesh.vertices;
		int[] triangles = gameObject.GetComponentInChildren<MeshFilter>().mesh.triangles;

		//Sorted indices from bounds
		int[] sortedIndices = SortIndices (bounds.size);

		GameObject plane = CreateCuttingPlane(bounds, sortedIndices, 0);
		GameObject hijo = transform.Find (childName).gameObject;
	
		GameObject[] pieces = myMeshCut.Cut (hijo,
			plane.transform.position,
			plane.transform.TransformDirection (plane.GetComponent<MeshFilter> ().mesh.normals [0]),
			capMaterial);

		CreateChildCapsule (pieces[0].GetComponent<MeshRenderer>().bounds, sortedIndices);
		CreateChildCapsule (pieces[1].GetComponent<MeshRenderer>().bounds, sortedIndices);

	}
	/**void Start () {

		//Bounds
		Bounds bounds = gameObject.GetComponentInChildren<MeshRenderer>().bounds;

		//Vertices and triangles
		Vector3[] vertices = gameObject.GetComponentInChildren<MeshFilter>().mesh.vertices;
		int[] triangles = gameObject.GetComponentInChildren<MeshFilter>().mesh.triangles;
		Debug.Log ("Initial vertices: " + vertices.Length);
		Debug.Log ("Initial triangles: " + triangles.Length);
		//Sorted indices from bounds
		int[] sortedIndices = SortIndices (bounds.size);

		float step = (bounds.size [sortedIndices [0]] / n);

		for (int i = 0; i < n-1; i++) {
			//Get planes A and B with A < B along the biggest axis
			GameObject planeA = CreateCuttingPlane(bounds, sortedIndices, i * step);
			GameObject planeB = CreateCuttingPlane(bounds, sortedIndices, (i+1) * step);
			Vector3[] verticesA = planeA.GetComponent<MeshFilter> ().mesh.vertices;
			Vector3[] verticesB = planeB.GetComponent<MeshFilter> ().mesh.vertices;
			Mesh cut = SliceMesh (verticesA, verticesB, vertices, triangles, sortedIndices [0]);
			GameObject iteration = new GameObject ();
			iteration.AddComponent<MeshFilter> ();
			iteration.AddComponent<MeshRenderer> ();
			iteration.GetComponent<MeshFilter> ().mesh = cut;


			int[] meshSortedIndices = SortIndices (cut.bounds.size);
			CreateChildCapsule (cut.bounds, meshSortedIndices);
		}


	}
	*/

	/**
	 * Slice mesh between two planes A and B
	 * */
	private Mesh SliceMesh(Vector3[] verticesA, Vector3[] verticesB, Vector3[] vertices, int[] triangles, int biggestIndex){

		//New Mesh
		Mesh sliced = new Mesh();
		ArrayList newVertices = new ArrayList();
		ArrayList newTriangles = new ArrayList();
		for (int i = 0; i < triangles.Length - 3; i+=3) {
			int first = triangles [i];
			int second = triangles [i + 1];
			int third = triangles [i + 2];

			if (InsideRegion (vertices [first], verticesA, verticesB, biggestIndex)
			   && InsideRegion (vertices [second], verticesA, verticesB, biggestIndex)
			   && InsideRegion (vertices [third], verticesA, verticesB, biggestIndex)) {
				int indexFirst = SearchForVertex (vertices [first], newVertices);
				int indexSecond = SearchForVertex (vertices [second], newVertices);
				int indexThird = SearchForVertex (vertices [third], newVertices);

				if (indexFirst == -1) {
					newVertices.Add (vertices [first]);
					newTriangles.Add (newVertices.Count-1);
				}
				else
					newTriangles.Add (indexFirst);
				
				if (indexSecond == -1) {
					newVertices.Add (vertices [second]);
					newTriangles.Add (newVertices.Count-1);
				}
				else
					newTriangles.Add (indexSecond);

				if (indexThird == -1) {
					newVertices.Add (vertices [third]);
					newTriangles.Add (newVertices.Count-1);
				}
				else
					newTriangles.Add (indexThird);
			}

		}

		int sizeVertices = newVertices.Count;
		int sizeTriangles = newTriangles.Count;
		Debug.Log ("Final vertices: " + sizeVertices);
		Debug.Log ("Final triangles: " + sizeTriangles);
		Vector3[] nVertices = new Vector3[sizeVertices];
		int[] nTriangles = new int[sizeTriangles];
		for (int i = 0; i < sizeVertices; i++) {
			nVertices [i] = (Vector3)newVertices [i];
		}
		for (int i = 0; i < sizeTriangles; i++) {
			nTriangles [i] = (int)newTriangles [i];
		}

		sliced.vertices = nVertices;
		sliced.triangles = nTriangles;
		return sliced;
	}

	/**
	 * Check search for a vertex inside an array and return its index
	 * */
	private int SearchForVertex(Vector3 vertex, ArrayList array){

		int index = -1;
		bool end = false;
		for (int i = 0; i < array.Count && !end; i++) {
			if (vertex.Equals( array [i])) {
				end = true;
				index = i;
			}
		}
		return index;
	}

	/**
	 * Check if vertex is inside a region between two planes
	 * */
	private bool InsideRegion(Vector3 vertex, Vector3[] planeVerticesA, Vector3[] planeVerticesB, int biggestIndex){
		return SuperiorVertex (vertex, planeVerticesA, biggestIndex) && InferiorVertex (vertex, planeVerticesB, biggestIndex);
	}

	/**
	 * Check if vertex is greater to a plane or inside in a certain direction
	 * */
	private bool SuperiorVertex(Vector3 vertex, Vector3[] planeVertices, int biggestIndex){
		bool response = true;
		for(int i = 0; i < planeVertices.Length && !response; i++){
			if (vertex [biggestIndex] < planeVertices [i] [biggestIndex])
				response = false;
		}
		return response;
	}

	/**
	 * Check if vertex is lower to a plane or inside in a certain direction
	 * */
	private bool InferiorVertex(Vector3 vertex, Vector3[] planeVertices, int biggestIndex){
		bool response = true;
		for(int i = 0; i < planeVertices.Length && !response; i++){
			if (vertex [biggestIndex] > planeVertices [i] [biggestIndex])
				response = false;
		}
		return response;
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
		plane.transform.position = bounds.center;
		Vector3 translation;
		float shiftPos = bounds.center [sortedIndices [0]] - shift;
		switch (sortedIndices [0]) {
		case 0:
			translation = new Vector3(shiftPos, plane.transform.position.y, plane.transform.position.z);				
			break;
		case 1:
			translation = new Vector3(plane.transform.position.x, shiftPos, plane.transform.position.z);
			break;
		case 2:
			translation = new Vector3(plane.transform.position.x, plane.transform.position.y, shiftPos);
			break;
		default:
			translation = new Vector3(plane.transform.position.x, plane.transform.position.y, plane.transform.position.z);
			break;
		}
		plane.transform.position = translation;

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
