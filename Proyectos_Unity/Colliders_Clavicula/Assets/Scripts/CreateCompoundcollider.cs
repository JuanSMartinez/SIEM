using UnityEngine;
using System.Collections;

public class CreateCompoundcollider : MonoBehaviour {

    public float nugget;

    // Use this for initialization
    void Start() {

        MeshRenderer clavicle_renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        Bounds bounds = clavicle_renderer.bounds;
          
        GameObject hugeCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        positionCylinder(hugeCylinder, bounds.size);
        scaleCylinder(hugeCylinder, bounds.size);
       
        hugeCylinder.transform.SetParent(gameObject.transform);
        

    }

   

    // Update is called once per frame
    void Update () {
	
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
        cylinder.transform.eulerAngles = angles;
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
}
