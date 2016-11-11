using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	//Label to tell the user the training has finished succesfully
	public Text finalLabel;

	//Reference to the scene monitor
	public Monitor monitor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Tell the user the training has finished and reset
	public void Finished(){
		finalLabel.text = "ENTRENAMIENTO COMPLETADO!";
		Color color = new Color (0f, 208f, 0f, 255f);
		finalLabel.color = color;
	}

	//Reset the scene
	public void Reset(){
		monitor.start = false;
		monitor.SendMessage ("CloseFile");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//Start training
	public void StartTraining(){
		finalLabel.text = "ENTRENAMIENTO INICIADO";
		monitor.start = true;
		monitor.SendMessage ("CreateLog");

	}

	//Stop the training
	public void StopTraining(){
		monitor.start = false;
		monitor.SendMessage ("CloseFile");
		finalLabel.text = "ENTRENAMIENTO DETENIDO";
	}
}
