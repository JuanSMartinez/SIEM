using UnityEngine;
using System.Collections;

public class NameReplacement : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string name_child = gameObject.name;
        gameObject.GetComponentInChildren<MeshFilter>().mesh.name = name_child;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
