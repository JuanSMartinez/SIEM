using UnityEngine;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public GameObject esternon;
	public GameObject acromion;


	public void Scene1(){
		
		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 2.5f,0f));
		acromion.transform.Rotate(new Vector3(0f, 2.5f,0f));
	}

	public void Scene2(){
		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 5f,0f));
		acromion.transform.Rotate(new Vector3(0f, 5f,0f));
	}

	public void Scene3(){
		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 7.5f,0f));
		acromion.transform.Rotate(new Vector3(0f, 7.5f,0f));
	}

	public void Scene4(){
		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 10f,0f));
		acromion.transform.Rotate(new Vector3(0f, 10f,0f));
	}

	public void Scene5(){

		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 0, 1f));
		acromion.transform.Rotate(new Vector3(0f, 0, 1f));
	}

	public void Scene6(){

		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 0, 2f));
		acromion.transform.Rotate(new Vector3(0f, 0, 2f));
	}
	public void Scene7(){

		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 0, 3f));
		acromion.transform.Rotate(new Vector3(0f, 0, 3f));
	}
	public void Scene8(){

		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 0, 4f));
		acromion.transform.Rotate(new Vector3(0f, 0, 4f));
	}

	public void Reset(){

		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);

	}
}
