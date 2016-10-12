using UnityEngine;
using System.Collections;


public class StaticVariables {

	public static int index = - 1; 

	private static Object obj = new Object();

	public static int GetNextIndex(){
		lock (obj) {
			index += 1;
			return index;
		}
	}

}
