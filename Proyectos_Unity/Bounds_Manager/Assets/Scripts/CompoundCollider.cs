using UnityEngine;
using System.Collections;

public class CompoundCollider : MonoBehaviour {

	public int n = 100;

	// Use this for initialization
	void Start () {

		//Bounds
		Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;


	}

	/**
	 * Create and center cutting plane
	 * */
	private GameObject SetCuttingPlane(Bounds bounds){

		//Create the plane
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

		//Sort bounds size indices
		int[] sortedIndices = SortIndices(bounds.size);

		//Set X as the biggest axis of the plane and Z as the smallest
		Vector3 scale = new Vector3(bounds.size[sortedIndices[1]], 1, bounds.size[sortedIndices[2]]);
		plane.transform.lossyScale = scale;

		//Rotate the plane 

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
