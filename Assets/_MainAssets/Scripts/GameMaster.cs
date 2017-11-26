using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using DG.Tweening;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public GameObject _character;

	public Sprite[] _characterSprites;

	public Sprite[] _deadCharacterSprites;

	private List<Character> _characters;

	public BearController _bear;

	public Canvas _canvas;

	// Use this for initialization
	void Awake()
	{
		_characters = new List<Character>();
		_playerReady = new bool[4];
	}

	private bool[] _playerReady;

	public SpriteRenderer[] _playerImages;

	private void Update()
	{
		if (XCI.GetButtonDown(XboxButton.Start, XboxController.Any) && _characters.Count >= 2)
		{
			StartGame();
		}

		if (XCI.GetButtonDown(XboxButton.A, XboxController.First))
		{
			_playerReady[0] = true;
			_playerImages[0].gameObject.SetActive(true);
			_playerImages[0].transform.DOKill();
			_playerImages[0].transform.localScale = Vector3.one;
			_playerImages[0].transform.DOPunchScale(Vector3.one * 1.2f, 0.25f);
		}

		if (XCI.GetButtonDown(XboxButton.A, XboxController.Second))
		{
			_playerReady[1] = true;
			_playerImages[1].gameObject.SetActive(true);
			_playerImages[1].transform.DOKill();
			_playerImages[1].transform.localScale = Vector3.one;
			_playerImages[1].transform.DOPunchScale(Vector3.one * 1.2f, 0.25f);
		}

		if (XCI.GetButtonDown(XboxButton.A, XboxController.Third))
		{
			_playerReady[2] = true;
			_playerImages[2].gameObject.SetActive(true);
			_playerImages[2].transform.DOKill();
			_playerImages[2].transform.localScale = Vector3.one;
			_playerImages[2].transform.DOPunchScale(Vector3.one * 1.2f, 0.25f);
		}

		if (XCI.GetButtonDown(XboxButton.A, XboxController.Fourth))
		{
			_playerReady[3] = true;
			_playerImages[3].gameObject.SetActive(true);
			_playerImages[3].transform.DOKill();
			_playerImages[3].transform.localScale = Vector3.one;
			_playerImages[3].transform.DOPunchScale(Vector3.one * 1.2f, 0.25f);
		}
	}

	private void StartGame()
	{
		transform.DOMove(new Vector3(0f, 0f, -10f), 3f).OnComplete(ActivatePlayers);
	}

	private void ActivatePlayers()
	{
		if (_playerReady[0])
		{
			var newChar0 = Instantiate(_character);
			_characters.Add(newChar0.GetComponent<Character>());
			newChar0.GetComponent<Character>().Initialize(XboxController.First, _characterSprites[0], _deadCharacterSprites[0]);
		}

		if (_playerReady[1])
		{
			var newChar1 = Instantiate(_character);
			_characters.Add(newChar1.GetComponent<Character>());
			newChar1.GetComponent<Character>().Initialize(XboxController.Second, _characterSprites[1], _deadCharacterSprites[1]);
		}

		if (_playerReady[2])
		{
			var newChar2 = Instantiate(_character);
			_characters.Add(newChar2.GetComponent<Character>());
			newChar2.GetComponent<Character>().Initialize(XboxController.Third, _characterSprites[2], _deadCharacterSprites[2]);
		}

		if (_playerReady[3])
		{
			var newChar3 = Instantiate(_character);
			_characters.Add(newChar3.GetComponent<Character>());
			newChar3.GetComponent<Character>().Initialize(XboxController.Fourth, _characterSprites[3], _deadCharacterSprites[3]);
		}

		_bear.MakeBearAware();

		_canvas.gameObject.SetActive(true);
	}
}