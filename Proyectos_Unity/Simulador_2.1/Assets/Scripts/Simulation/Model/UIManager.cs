using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	//Label to tell the user the training has finished succesfully
	public Text finalLabel;

	//Reference to the scene monitor
	public Monitor monitor;

	//Reference to the slider to choose the reduction type
	public Slider slider;

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
		monitor.SendMessage ("CloseFile");
		monitor.start = false;
	}

	//Reset the scene
	public void Reset(){
		monitor.SendMessage ("CloseFile");
		monitor.start = false;
		ForceManager.ResetIndex ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//Start training
	public void StartTraining(){
		finalLabel.text = "ENTRENAMIENTO INICIADO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
		string reduction = slider.value == 0 ? "Anatomica" : "Funcional";
		monitor.SendMessage ("CreateLog", reduction);
		monitor.start = true;


	}

	//Pause the training
	public void PauseTraining(){
		monitor.start = false;
		finalLabel.text = "ENTRENAMIENTO SUSPENDIDO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
		Time.timeScale = 0;
	}

	//Stop the training
	public void StopTraining(){
		monitor.SendMessage ("CloseFile");
		monitor.start = false;
		finalLabel.text = "ENTRENAMIENTO FINALIZADO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
	}

	//Go back to the main menu
	public void MainMenu(){
		monitor.SendMessage ("CloseFile");
		monitor.start = false;
		ForceManager.ResetIndex ();
		SceneManager.LoadScene("MainMenu");
	}

	//Resume training
	public void ResumeTraining(){
		monitor.start = true;
		finalLabel.text = "ENTRENAMIENTO REANUDADO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
		Time.timeScale = 1;
	}

	public void TrainingNotStarted(){
		finalLabel.text = "ENTRENAMIENTO NO INICIADO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
	}

	public void TrainingRunning(){
		finalLabel.text = "ENTRENAMIENTO YA INICIADO";
		Color color = new Color (0f, 0f, 0f, 255f);
		finalLabel.color = color;
	}

}
