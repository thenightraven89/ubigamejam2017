using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager Instance { get; private set; }

	private int[]_scores;
	public Text[] _scoreTexts;

	private void Awake()
	{
		Instance = this;
		_scores = new int[4];
	}

	public void UpdateScore(XboxController controller, int diff)
	{
		if (_isFinished) return;

		_scores[(int)controller - 1] += diff;
		_scoreTexts[(int)controller - 1].text = _scores[(int)controller - 1].ToString();
		_scoreTexts[(int)controller - 1].rectTransform.localScale = Vector3.one;
		_scoreTexts[(int)controller - 1].rectTransform.DOPunchScale(new Vector2(1f, 1f), 0.25f).OnComplete(CheckForWinner);
	}

	private void CheckForWinner()
	{
		int[] sortedScores = new int[4];
		int[] sortedWinners = new int[4];

		for (int i = 0; i < _scores.Length; i++)
		{
			sortedWinners[i] = i;
			sortedScores[i] = _scores[i];
		}

		for (int i = 0; i < sortedScores.Length - 1; i++)
		{
			for (int j = i + 1; j < sortedScores.Length; j++)
			{
				if (sortedScores[i] < sortedScores[j])
				{
					int aux = _scores[i];
					sortedScores[i] = sortedScores[j];
					sortedScores[j] = aux;
					aux = sortedWinners[i];
					sortedWinners[i] = sortedWinners[j];
					sortedWinners[j] = aux;
				}
			}
		}
		
		if (sortedScores[0] >= 3)
		{
			_isFinished = true;
			ShowGameOverScreen(sortedWinners[0]);
		}
	}

	public GameObject _gameOverScreen;
	public GameObject _winner;
	public GameObject _winnerText;

	public Sprite[] _players;

	private bool _isFinished = false;

	public void ShowGameOverScreen(int winnerIndex)
	{
		Debug.Log("winner index is " + winnerIndex);
		_gameOverScreen.SetActive(true);
		_winner.GetComponent<Image>().sprite = _players[winnerIndex];
		_winner.transform.DOPunchScale(Vector3.one * 1.5f, 0.5f);
		_winnerText.transform.DOPunchScale(Vector3.one * 1.5f, 0.5f);
		StartCoroutine(Reload());
	}

	private IEnumerator Reload()
	{
		yield return new WaitForSeconds(4f);
		SceneManager.LoadScene(0);
	}
}