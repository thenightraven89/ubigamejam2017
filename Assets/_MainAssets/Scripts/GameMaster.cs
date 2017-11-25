using UnityEngine;
using XboxCtrlrInput;

public class GameMaster : MonoBehaviour
{
	public GameObject _character;

	public Sprite[] _characterSprites;

	// Use this for initialization
	void Start()
	{
		var newChar = Instantiate(_character);
		newChar.GetComponent<Character>().Initialize(XboxController.First, _characterSprites[0]);

		var newChar2 = Instantiate(_character);
		newChar2.GetComponent<Character>().Initialize(XboxController.Second, _characterSprites[1]);
	}
}