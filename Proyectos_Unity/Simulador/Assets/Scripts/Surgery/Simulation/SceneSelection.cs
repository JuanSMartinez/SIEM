using UnityEngine;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public GameObject esternon;
	public GameObject acromion;

	// Scene 2
	public void Scene2(){
		Debug.Log ("Second");
		esternon.transform.Rotate(new Vector3(0f, 5f,0f));
		acromion.transform.Rotate(new Vector3(0f, 5f,0f));
	}
}
