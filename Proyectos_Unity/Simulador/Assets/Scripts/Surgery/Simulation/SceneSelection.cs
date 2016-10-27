using UnityEngine;
using System.Collections;

public class SceneSelection : MonoBehaviour {

	public GameObject esternon;
	public GameObject acromion;

	public string[] objects;
	private Transform[] initialTransforms;

	void Start(){
		initialTransforms = new Transform[objects.Length];
		for (int i = 0; i < objects.Length; i++) {
			initialTransforms [i] = GameObject.Find(objects[i]).transform;
		}
	}


	public void Scene1(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 2.5f,0f));
		}
		EnableJoints ();
	}

	public void Scene2(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 5f,0f));
		}
		EnableJoints ();
	}

	public void Scene3(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 7.5f,0f));
		}
		EnableJoints ();
	}

	public void Scene4(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 10f,0f));
		}
		EnableJoints ();
	}

	public void Scene5(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 0, 1f));
		}
		EnableJoints ();
	}

	public void Scene6(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 0, 2f));
		}
		EnableJoints ();
	}
	public void Scene7(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 0, 3f));
		}
		EnableJoints ();
	}
	public void Scene8(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
			GameObject.Find(objects[i]).transform.Rotate(new Vector3(0f, 0, 4f));
		}
		EnableJoints ();
	}

	public void Reset(){
		DisableJoints ();
		for (int i = 0; i < objects.Length; i++) {
			GameObject.Find(objects[i]).transform.position = initialTransforms [i].position;
			GameObject.Find(objects[i]).transform.eulerAngles = initialTransforms [i].eulerAngles;
		}
		EnableJoints ();

	}

	private void DisableJoints(){
		for (int i = 0; i < objects.Length; i++) {
			DisablePermanentJoints (GameObject.Find(objects[i]));
		}
	}

	private void EnableJoints(){
		for (int i = 0; i < objects.Length; i++) {

			EnablePermanentJoints (GameObject.Find(objects[i]));

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
