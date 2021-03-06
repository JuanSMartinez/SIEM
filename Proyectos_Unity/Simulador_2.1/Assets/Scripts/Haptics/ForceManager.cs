﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Runtime.CompilerServices;

/**
 * Manage Environmental forces
 * */
public class ForceManager : MonoBehaviour {

	//Types of forces
	public static string CONSTANT = "constant"; 

	public static string FRICTION = "friction";

	public static string SPRING = "spring";

	public static string TANGENTIAL_FORCE = "tangentialForce";

	public static string VIBRATION_CONTACT = "vibrationContact";

	public static string VIBRATION_MOTOR = "vibrationMotor";

	public static string VISCOSITY = "viscous";

	//Universal index counter, All indices must be generated from this value
	private static int global_index = -1;

	//Lock object for concurrent access
	private static object obj = new object();

	//Start a force
	public static void SetEnvironmentForce(string nType, int index, float[] positionEffect, float[] directionEffect, float gain, float magnitude, float duration, float frequency)
	{
		lock (obj) {
			//convert String to IntPtr
			IntPtr type = ConverterClass.ConvertStringToByteToIntPtr (nType);
			//Convert float[3] to intptr
			IntPtr position = ConverterClass.ConvertFloat3ToIntPtr (positionEffect);
			//Convert float[3] to intptr
			IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr (directionEffect);

			//Set the effect
			try {
				for(int i = 0 ; i <= index; i++)
					PluginImport.SetEffect (type, index, gain, magnitude, duration, frequency, position, direction);
				PluginImport.StartEffect (index);
			} catch (Exception) {
				Debug.Log ("Crashed setting " + nType + " force with index " + index);
			}
		}
	}

	//Stop a force
	public static void StopEnvironmentForce(int index){
		lock (obj) {
			try {
				PluginImport.StopEffect (index);
			} catch (Exception) {
				Debug.Log ("Crashed stopping force with index " + index);
			}
		}
	}

	//Return next free index for a force
	public static int GetNextIndex(){
		lock (obj) {
			global_index += 1;
			return global_index;
		}
	}

	//Reset global index
	public static void ResetIndex(){
		lock (obj) {
			global_index = -1;
		}
	}




}
