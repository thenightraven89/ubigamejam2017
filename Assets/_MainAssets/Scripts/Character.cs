using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
	private int _playerIndex;
	private string _xaxisName;
	private string _yaxisName;
	private string _dashName;
	private float _xaxis;
	private float _yaxis;
	private float _dashMultiplier;
	private Transform _t;
	private Vector2 _baseSpeed;
	private CharacterState _state;

	private const float DASH_COOLDOWN = 3f;
	private const float DASH_MULTIPLIER_MAX = 3f;
	private const float DASH_DECAY = 0.25f; //decay in seconds
	private const float XSPEED = 0.05f;
	private const float YSPEED = 0.05f;

	public void Initialize(int playerIndex)
	{
		_playerIndex = playerIndex;
		_xaxisName = string.Format("xaxis_{0}", playerIndex);
		_yaxisName = string.Format("yaxis_{0}", playerIndex);
		_dashName = string.Format("dash_{0}", playerIndex);
		_t = transform;
		_dashMultiplier = 1f;
		_state = CharacterState.Roaming;
	}

	void Update()
	{
		_xaxis = Input.GetAxis(_xaxisName);
		_yaxis = Input.GetAxis(_yaxisName);
		_baseSpeed = new Vector2(_xaxis, _yaxis).normalized;

		switch (_state)
		{
			case CharacterState.Roaming:
				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));

				if (Input.GetButtonDown(_dashName))
				{
					Dash(DASH_DECAY, DASH_COOLDOWN);
				}
				break;

			case CharacterState.Dashing:
				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));
				break;

			default:
				break;
		}
	}

	public void Dash(float decay, float cooldown)
	{
		_state = CharacterState.Dashing;
		StartCoroutine(DashCoroutine(DASH_DECAY, DASH_COOLDOWN));
	}

	public void Stun(float duration)
	{
		_state = CharacterState.Stunned;
		_dashMultiplier = 1f;
		StartCoroutine(StunCoroutine(duration));
	}

	private IEnumerator DashCoroutine(float decay, float cooldown)
	{
		_dashMultiplier = DASH_MULTIPLIER_MAX;
		float t = 0f;
		while (t < decay)
		{
			_dashMultiplier = Mathf.Lerp(1f, DASH_MULTIPLIER_MAX, 1 - t / decay);
			yield return null;
			t += Time.deltaTime;
		}
		yield return new WaitForSeconds(cooldown);
		_state = CharacterState.Roaming;
	}

	private IEnumerator StunCoroutine(float duration)
	{
		yield return new WaitForSeconds(duration);
		_state = CharacterState.Roaming;
	}

	public enum CharacterState
	{
		Nada,
		Roaming,
		Dashing,
		Deceiving,
		Mining,
		Stunned,
		Dead
	}
}