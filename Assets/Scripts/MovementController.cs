using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovementController : MonoBehaviour {
	
	public float gravityFactor = 1.0f;
	public float forceThreshold = 1.0f;
	public float forceFactor = 3.0f;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}

	void FixedUpdate() {
		Vector3 gravity = new Vector3 (-Input.acceleration.y, Input.acceleration.z, Input.acceleration.x);
		Physics.gravity = (gravityFactor * 9.8f) * gravity.normalized;
		if (Input.gyro.userAcceleration.magnitude >= forceThreshold) {
			Vector3 userAcc = new Vector3 (-Input.gyro.userAcceleration.y, Input.gyro.userAcceleration.z, Input.gyro.userAcceleration.x);
			DiceController.AddForceToDices (forceFactor * userAcc);
		}
	}
}
