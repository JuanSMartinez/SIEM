﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

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

	//Start a force
	public static void SetEnvironmentForce(string nType, int index, float[] positionEffect, float[] directionEffect, float gain, float magnitude, float duration, float frequency)
	{
		
		//convert String to IntPtr
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr(nType);
		//Convert float[3] to intptr
		IntPtr position = ConverterClass.ConvertFloat3ToIntPtr(positionEffect);
		//Convert float[3] to intptr
		IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(directionEffect);

		//Set the effect
		PluginImport.SetEffect(type, index, gain, magnitude, duration, frequency, position, direction);
		PluginImport.StartEffect(index);
	}

	//Stop a force
	public static void StopEnvironmentForce(int index){
		PluginImport.StopEffect (index);
	}



}
