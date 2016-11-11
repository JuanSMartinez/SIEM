using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Load beginner scene
	public void Beginner(){
		SceneManager.LoadScene ("Principiante");
	}

	//Load intermediate scene
	public void Intermediate(){
		SceneManager.LoadScene ("Intermedio");
	}

	//Load advanced scene
	public void Advanced(){
		SceneManager.LoadScene ("Avanzado");
	}
}
