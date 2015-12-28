using UnityEngine;
using System.Collections;

public class MainApp : MonoBehaviour {

	public Camera mainCamera;
	public float wallHeight = 30.0f;
	public float cameraToCeiling = 60.0f;
	public float spawnOffset = 5.0f;
	public Light pointLight;

	// determine a random color
	private string randomColor
	{
		get
		{
			string _color = "blue";
			int c = System.Convert.ToInt32(Random.value * 6);
			switch(c)
			{
			case 0: _color = "red"; break;
			case 1: _color = "green"; break;
			case 2: _color = "blue"; break;
			case 3: _color = "yellow"; break;
			case 4: _color = "white"; break;
			case 5: _color = "black"; break;				
			}
			return _color;
		}
	}

	// Use this for initialization
	void Start () {
		SetUpWorld ();
		SpawnSample ();
	}

	void SetUpWorld() {
		Vector3 cameraPos = mainCamera.transform.position;
		cameraPos.y = wallHeight + cameraToCeiling;
		mainCamera.transform.position = cameraPos;

		Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3 (0, 0, cameraToCeiling));
		Vector3 upperRight = mainCamera.ViewportToWorldPoint(new Vector3 (1, 1, cameraToCeiling));

		Debug.Log (bottomLeft);
		Debug.Log (upperRight);

		float worldWidth = Mathf.Abs (upperRight.x - bottomLeft.x);
		float worldHeight = Mathf.Abs (upperRight.z - bottomLeft.z);

		GameObject.Find ("ground").transform.localScale = new Vector3 (worldWidth, 1, worldHeight);
		MoveAndScale (GameObject.Find ("ceiling"),
			new Vector3(0, wallHeight, 0),
			new Vector3 (worldWidth, 0, worldHeight));

		pointLight.transform.position = new Vector3 (bottomLeft.x, wallHeight, bottomLeft.z);
		pointLight.range = Mathf.Sqrt(wallHeight * wallHeight + worldWidth * worldWidth + worldHeight * worldHeight);

		float wallY = wallHeight / 2;
		MoveAndScale (GameObject.Find ("wall-west"),
			new Vector3 (bottomLeft.x, wallY, 0),
			new Vector3 (0.1f, wallHeight, worldHeight));
		MoveAndScale (GameObject.Find ("wall-east"),
			new Vector3 (upperRight.x, wallY, 0),
			new Vector3 (0.1f, wallHeight, worldHeight));
		MoveAndScale (GameObject.Find ("wall-north"),
			new Vector3 (0, wallY, upperRight.z),
			new Vector3 (worldWidth, wallHeight, 0.1f));
		MoveAndScale (GameObject.Find ("wall-south"),
			new Vector3 (0, wallY, bottomLeft.z),
			new Vector3 (worldWidth, wallHeight, 0.1f));
	}

	void SpawnSample() {
		Vector3 spawnPoint = new Vector3(0, wallHeight - spawnOffset, 0);
		DiceController.SpawnDices ("1d6", "d6-" + randomColor + "-dots", spawnPoint);
	}

	void MoveAndScale(GameObject obj, Vector3 position, Vector3 scale) {
		if (obj != null) {
			obj.transform.localPosition = position;
			obj.transform.localScale = scale;
		}
	}

	void Update() {
		HandleTouch ();
	}

	void HandleTouch() {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				Vector3 spawnPoint = mainCamera.ScreenToWorldPoint(new Vector3 (touch.position.x, touch.position.y, cameraToCeiling - spawnOffset));
				spawnPoint.y = wallHeight - spawnOffset;
				DiceController.SpawnDices ("1d6", "d6-" + randomColor + "-dots", spawnPoint);
			}
		}
	}
}
