using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
	private int _playerIndex;
	private string _xaxisName;
	private string _yaxisName;
	private float _xaxis;
	private float _yaxis;
	private float _xspeed = 0.1f;
	private float _yspeed = 0.1f;
	private Transform _t;
	private bool _isStunned;

	public void Initialize(int playerIndex)
	{
		_playerIndex = playerIndex;
		_xaxisName = string.Format("xaxis_{0}", playerIndex);
		_yaxisName = string.Format("yaxis_{0}", playerIndex);
		_t = transform;
		_isStunned = false;
	}

	// Update is called once per frame
	void Update()
	{
		_xaxis = Input.GetAxis(_xaxisName);
		_yaxis = Input.GetAxis(_yaxisName);

		if (!_isStunned)
		{
			_t.Translate(new Vector2(_xaxis * _xspeed, _yaxis * _yspeed));
		}
	}

	public void Stun(float duration)
	{
		StopAllCoroutines();
		StartCoroutine(StunCoroutine(duration));
	}

	private IEnumerator StunCoroutine(float duration)
	{
		_isStunned = true;
		yield return new WaitForSeconds(duration);
		_isStunned = false;
	}
}