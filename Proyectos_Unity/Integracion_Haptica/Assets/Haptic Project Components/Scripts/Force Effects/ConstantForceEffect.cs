using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class ConstantForceEffect : MonoBehaviour {

	public static ConstantForceEffect instance;

	public string Type;
	public int effect_index;
	public float gain;
	public float magnitude;
	public float duration;
	public float frequency;
	public float[] positionEffect =  new float[3];
	public float[] directionEffect = new float[3];
	
	
	// Use this for initialization
	void Start () {
		instance = this;
		Type = "constant";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setGain(float newGain){
		this.gain = newGain;
	}
}
