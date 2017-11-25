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
	private bool _isStunned;
	private Vector2 _baseSpeed;
	private bool _dashAvailable;

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
		_isStunned = false;
		_dashMultiplier = 1f;
		_dashAvailable = true;
	}

	void Update()
	{
		_xaxis = Input.GetAxis(_xaxisName);
		_yaxis = Input.GetAxis(_yaxisName);
		_baseSpeed = new Vector2(_xaxis, _yaxis).normalized;

		if (!_isStunned)
		{
			_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));

			if (Input.GetButtonDown(_dashName) && _dashAvailable)
			{
				StartCoroutine(DashCoroutine(DASH_DECAY, DASH_COOLDOWN));
			}
		}
	}

	public void Stun(float duration)
	{
		StopAllCoroutines();
		_dashMultiplier = 1f;
		_dashAvailable = true;
		StartCoroutine(StunCoroutine(duration));
	}

	private IEnumerator DashCoroutine(float decay, float cooldown)
	{
		_dashAvailable = false;
		_dashMultiplier = DASH_MULTIPLIER_MAX;
		float t = 0f;
		while (t < decay)
		{
			_dashMultiplier = Mathf.Lerp(1f, DASH_MULTIPLIER_MAX, 1 - t / decay);
			yield return null;
			t += Time.deltaTime;
		}
		yield return new WaitForSeconds(cooldown);
		_dashAvailable = true;
	}

	private IEnumerator StunCoroutine(float duration)
	{
		_isStunned = true;
		yield return new WaitForSeconds(duration);
		_isStunned = false;
	}
}