using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject _character;

	// Use this for initialization
	void Start()
	{
		var newChar = Instantiate(_character);
		newChar.GetComponent<Character>().Initialize(1);

		var newChar2 = Instantiate(_character);
		newChar2.GetComponent<Character>().Initialize(2);
	}
}