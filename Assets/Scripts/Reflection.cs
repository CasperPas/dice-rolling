using UnityEngine;
using System.Collections;

public class Reflection : MonoBehaviour {

	private ReflectionProbe probe;

	// Use this for initialization
	void Awake () {
		probe = GetComponent<ReflectionProbe> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (probe != null) {
			probe.transform.position = new Vector3 (
				Camera.main.transform.position.x, 
				Camera.main.transform.position.y * -1, 
				Camera.main.transform.position.z
			);

			probe.RenderProbe ();
		}
	}
}
