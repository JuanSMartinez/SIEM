using UnityEngine;
using System.Collections;

public class MeshName : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Mesh mesh = gameObject.GetComponentInChildren<MeshFilter>().mesh;
        Debug.Log("Mesh name: " + mesh.name);
        mesh.name = "Pancrasio";
        Debug.Log("Mesh name pancrazised: " + mesh.name);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
