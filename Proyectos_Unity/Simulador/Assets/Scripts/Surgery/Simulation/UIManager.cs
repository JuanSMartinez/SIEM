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
		float distanceDiff = parameters [0];
		float angleDiff = parameters [1];
		GameObject panel = transform.FindChild ("Panel").gameObject;

		panel.GetComponent<UnityEngine.UI.Text> ().text = "Diferencia Distancia: " + distanceDiff + "\nDiferencia Angular: " + angleDiff;
	}


}
