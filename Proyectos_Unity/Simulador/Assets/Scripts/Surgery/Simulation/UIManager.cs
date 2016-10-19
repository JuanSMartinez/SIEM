using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject panel;
	public GameObject semaforo;
	public Material red;
	public Material green;

	void OnEnable(){
		Interaction.OnCollision += SignalCollision;
		Interaction.ExitCollision += StoppedCollision;

	}

	void OnDisable(){
		Interaction.OnCollision -= SignalCollision;
		Interaction.ExitCollision -= StoppedCollision;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SignalCollision(){
		semaforo.GetComponent <MeshRenderer> ().material = red;
	}

	void StoppedCollision(){
		semaforo.GetComponent <MeshRenderer> ().material = green;
	}

	void Movement(float[] parameters){
		float distanceDiff_A = parameters [0];
		float angleDiff_A = parameters [1];
		float distanceDiff_B = parameters [2];
		float angleDiff_B = parameters [3];
		GameObject panel_A = transform.FindChild ("Panel_A").gameObject;

		panel_A.GetComponent<UnityEngine.UI.Text> ().text = "A:\nDiferencia Distancia: " + 
			round(distanceDiff_A,3) + "\nDiferencia Angular: " + round(angleDiff_A, 3);

		GameObject panel_B = transform.FindChild ("Panel_B").gameObject;

		panel_B.GetComponent<UnityEngine.UI.Text> ().text = "B:\nDiferencia Distancia: " + 
			round(distanceDiff_B,3) + "\nDiferencia Angular: " + round(angleDiff_B, 3);
	}

	float round(float number, int decimalPlaces){
		int i = (int)number;
		float decimalNumbers = number + i;
		float p = decimalNumbers * Mathf.Pow (10f, decimalPlaces);
		float p2 = (int)p/Mathf.Pow (10f, decimalPlaces);
		return i + p2;
	}



}
