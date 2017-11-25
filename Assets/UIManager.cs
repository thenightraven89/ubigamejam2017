using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using DG.Tweening;

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
		_scores[(int)controller - 1] += diff;
		_scoreTexts[(int)controller - 1].text = _scores[(int)controller - 1].ToString();
		_scoreTexts[(int)controller - 1].rectTransform.DOPunchScale(new Vector2(1f, 1f), 0.25f);
	}

}