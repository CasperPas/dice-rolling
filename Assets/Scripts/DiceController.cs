using UnityEngine;
using System.Collections;

public class DiceController : MonoBehaviour {

	// rolling = true when there are dice still rolling, rolling is checked using rigidBody.velocity and rigidBody.angularVelocity
	public static bool rolling = false;

	// reference to all dice
	private static ArrayList allDice = new ArrayList();

	// Update is called once per frame
	void Update () {
		if (rolling) {
			rolling = IsRolling();
		}
	}

	public static void AddForceToDices(Vector3 force) {
		for (int d = 0; d < allDice.Count; d++) {
			GameObject die = ((DiceInfo) allDice [d]).gameObject;
			die.GetComponent<Rigidbody>().AddForce((Vector3) force, ForceMode.Impulse);
		}
		rolling = true;
	}

	/// <summary>
	/// Roll one or more dice with a specific material from a spawnPoint and give it a specific force.
	/// format dice 			: 	({count}){die type}	, exmpl.  d6, 4d4, 12d8 , 1d20
	/// possible die types 	:	d4, d6, d8 , d10, d12, d20
	/// </summary>
	public static void SpawnDices(string dice, string mat, Vector3 spawnPoint) {
		rolling = true;
		// sotring dice to lowercase for comparing purposes
		dice = dice.ToLower();				
		int count = 1;
		string dieType = "d6";
		
		// 'd' must be present for a valid 'dice' specification
		int p = dice.IndexOf("d");
		if (p >= 0) {
			// check if dice starts with d, if true a single die is rolled.
			// dice must have a count because dice does not start with 'd'
			if (p > 0) {
				// extract count
				string[] a = dice.Split('d');
				count = System.Convert.ToInt32(a[0]);
				// get die type
				if (a.Length > 1)
					dieType = "d"+a[1];
				else
					dieType = "d6";
			} else
				dieType = dice;
			
			// instantiate the dice
			for (int d = 0; d < count; d++) {
				// create the die prefab/gameObject
				GameObject die = Dice.prefab(dieType, spawnPoint, Vector3.zero, Vector3.one, mat);
				die.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				// give it a random rotation
				die.transform.Rotate(new Vector3(Random.value * 360, Random.value * 360, Random.value * 360));
				// inactivate this gameObject because activating it will be handeled using the rollQueue and at the apropriate time
				die.SetActive(true);
				DiceInfo diceInfo = new DiceInfo(die, dieType, mat);
				// add allDices list
				allDice.Add(diceInfo);
			}
		}
	}

	/// <summary>
	/// Clears all currently rolling dice
	/// </summary>
	public static void Clear()
	{
		for (int d = 0; d < allDice.Count; d++) {
			GameObject.Destroy (((DiceInfo) allDice [d]).gameObject);
		}

		allDice.Clear();
		
		rolling = false;
	}

	/// <summary>
	/// Get value of all ( dieType = "" ) dice or dieType specific dice.
	/// </summary>
	public static int Value(string dieType)
	{
		int v = 0;
		// loop all dice
		for (int d = 0; d < allDice.Count; d++)
		{
			DiceInfo die = (DiceInfo) allDice[d];
			v += die.value;
		}
		return v;
	}

	/// <summary>
	/// Check if there all dice have stopped rolling
	/// </summary>
	private bool IsRolling()
	{
		for (int d = 0; d < allDice.Count; d++) {
			DiceInfo die = (DiceInfo) allDice[d];
			if (die.rolling) {
				return true;
			}
		}
		return false;
	}
}

/// <summary>
/// Supporting rolling die class to keep die information
/// </summary>
class DiceInfo
{

	public GameObject gameObject;		// associated gameObject
	public Die die;								// associated Die (value calculation) script

	public string name = "";				// dieType
	public string mat;						// die material (asString)

	// rolling attribute specifies if this die is still rolling
	public bool rolling
	{
		get
		{
			return die.rolling;
		}
	}

	public int value
	{
		get
		{
			return die.value;
		}
	}

	// constructor
	public DiceInfo(GameObject gameObject, string name, string mat)
	{
		this.gameObject = gameObject;
		this.name = name;
		this.mat = mat;
		// get Die script of current gameObject
		die = (Die)gameObject.GetComponent(typeof(Die));
	}
}