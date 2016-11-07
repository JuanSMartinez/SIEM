using UnityEngine;
using System.Collections;

/**
 * Haptic properties of an haptic element
 * All values are defined between 0 and 1
 * */
public class HapticProperties : MonoBehaviour {

	public float stiffness;
	public float damping;
	public float staticFriction;
	public float dynamicFriction;
	public float tangentialStiffness;
	public float tangentialDamping;
	public float popThrough;
	public float puncturedStaticFriction;
	public float puncturedDynamicFriction;
	public float mass;
	public bool fixedObj;
}
