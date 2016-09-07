using UnityEngine;
using System.Collections;

public class CreateCompoundcollider : MonoBehaviour {

    public float nugget;

    // Use this for initialization
    void Start() {

        MeshRenderer clavicle_renderer = gameObject.GetComponentInChildren<MeshRenderer>();
		//MeshRenderer clavicle_renderer = gameObject.GetComponent<MeshRenderer>();
        Bounds bounds = clavicle_renderer.bounds;
		Transform clavicleTransform = gameObject.GetComponentsInChildren<Transform>()[0];
          
        GameObject hugeCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		scaleCylinder(hugeCylinder, bounds.size);
        positionCylinder(hugeCylinder, bounds.size);
		translateCylinder (hugeCylinder, gameObject.transform);
		hugeCylinder.transform.SetParent(gameObject.transform);
    }

   

    // Update is called once per frame
    void Update () {
	
	}

	void translateCylinder(GameObject cylinder, Transform parentPosition){
		Debug.Log (parentPosition.position);
		Debug.Log (cylinder.transform.position);
		cylinder.transform.Translate (parentPosition.position, Space.World);
		//cylinder.transform.Translate (parentPosition.position, Space.Self);
		Debug.Log (cylinder.transform.position);

	}

    void scaleCylinder(GameObject cylinder, Vector3 parentSize)
    {
        float parentX = parentSize.x;
        float parentY = parentSize.y;
        float parentZ = parentSize.z;
        float[] coords = { parentX, parentY, parentZ };
        for(int i = 0; i < coords.Length-1; i++)
        {
            float max = coords[i];
            int index = i;
            for(int j = i+1 ; j <coords.Length; j++)
            {
                if(coords[j] > max)
                {
                    max = coords[j];
                    index = j;
                }
            }
            float temp = coords[i];
            coords[i] = max;
            coords[index] = temp;
        }
        float height = (float)coords[0]/2 + nugget;
        float radius = coords[1] + nugget;
        Vector3 scale = new Vector3(radius, height, radius);
        cylinder.transform.localScale = scale;

    }

    void positionCylinder(GameObject cylinder, Vector3 parentSize)
    {
        int parentRotation = getBiggestAxis(parentSize);
        Vector3 angles;
        switch (parentRotation)
        {
            case 0:
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
		cylinder.transform.Rotate (angles);
    }

    int getBiggestAxis(Vector3 size)
    {
        float x = size.x;
        float y = size.y;
        float z = size.z;

        if (x >= y && x >= z)
            return 0;
        else if (y >= x && y >= z)
            return 1;
        else
            return 2;

    }

	GameObject getCuttingPlane(){
		
		//Get the object's bounds
		Bounds bounds = gameObject.GetComponentInChildren<MeshRenderer>().bounds;

		//Create the cut plane
		GameObject cutPlane = GameObject.CreatePrimitive (PrimitiveType.Plane);

		//Rotate the plane
		rotatePlane(cutPlane, bounds);
	
		//Scale the plane
		scaleToBounds(cutPlane, bounds);

		//Position the plane

		return null;

	}

	void scaleToBounds(GameObject plane, Bounds bounds){
		//Size of the bounding box
		Vector3 size = bounds.size;

		//Get the sorted indices
		int[] sortedIndices = sortAxis(size);
		Vector3 scale = new Vector3();

		//Get lowest dimensions
		int middleIndex = sortedIndices [1];
		int lowestIndex = sortedIndices [2];

		//Scale vector
		float[] sizes = new float[]{ size.x, size.y, size.z };
		scale = new Vector3 (sizes [middleIndex], 0, sizes [lowestIndex]);

		//Scale the plane where  x >= z 
		plane.transform.localScale = scale;
	}

	void rotatePlane(GameObject plane, Bounds bounds){

		//Biggest axis
		int biggestAxis = getBiggestAxis(bounds.size);

		//If Y is not the biggest axis, rotate around X or Z
		if (biggestAxis != 1) {
			Vector3 rotation = biggestAxis == 0 ? new Vector3 (0, 0, 90) : new Vector3(90,0,0);
			plane.transform.Rotate (rotation);
		}
	}

	int[] sortAxis(Vector3 size){

		float parentX = size.x;
		float parentY = size.y;
		float parentZ = size.z;
		float[] coords = { parentX, parentY, parentZ };
		int[] indices = new int[3];
		for(int i = 0; i < coords.Length-1; i++)
		{
			float max = coords[i];
			int index = i;
			for(int j = i+1 ; j <coords.Length; j++)
			{
				if(coords[j] > max)
				{
					max = coords[j];
					index = j;
				}
			}
			float temp = coords[i];
			coords[i] = max;
			coords[index] = temp;
			indices [i] = index;
		}
		return indices;
	}
}
