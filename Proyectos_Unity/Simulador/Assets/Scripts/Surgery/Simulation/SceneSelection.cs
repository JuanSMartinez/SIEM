using UnityEngine;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public GameObject esternon;
	public GameObject acromion;

	public GameObject[] objects;
	private Transform[] initialTransforms;

	void Start(){
		initialTransforms = new Transform[objects.Length];
		for (int i = 0; i < objects.Length; i++) {
			initialTransforms [i] = objects [i].transform;
		}
	}


	public void Scene1(){
		
		for (int i = 0; i < objects.Length; i++) {
			DisablePermanentJoints (objects [i]);
		}

		for (int i = 0; i < objects.Length; i++) {
			
			objects [i].transform.position = initialTransforms [i].position;
			objects [i].transform.eulerAngles = initialTransforms [i].eulerAngles;
			objects [i].transform.Rotate(new Vector3(0f, 2.5f,0f));
		

		}

		for (int i = 0; i < objects.Length; i++) {

			EnablePermanentJoints (objects [i]);

		}
		/**
		esternon.transform.eulerAngles = new Vector3 (0, 0, 0);
		acromion.transform.eulerAngles = new Vector3 (0, 0, 0);
		esternon.transform.Rotate(new Vector3(0f, 2.5f,0f));
		acromion.transform.Rotate(new Vector3(0f, 2.5f,0f));*/

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

	private void ResetObjects(){
		for (int i = 0; i < objects.Length; i++) {
			objects [i].transform.position = initialTransforms [i].position;
			objects [i].transform.eulerAngles = initialTransforms [i].eulerAngles;
		}
	}

	private void DisablePermanentJoints(GameObject obj){
		PermanentJoint[] joints = obj.GetComponents<PermanentJoint> ();
		for (int i = 0; i < joints.Length; i++) {
			joints [i].enabled = false;
		}
	}

	private void EnablePermanentJoints(GameObject obj){
		PermanentJoint[] joints = obj.GetComponents<PermanentJoint> ();
		for (int i = 0; i < joints.Length; i++) {
			joints [i].enabled = true;
		}
	}



}
