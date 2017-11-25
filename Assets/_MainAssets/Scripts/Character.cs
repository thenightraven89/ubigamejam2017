﻿using System.Collections;
using UnityEngine;
using XboxCtrlrInput;

public class Character : MonoBehaviour
{
	private float _xaxis;
	private float _yaxis;
	private float _dashMultiplier;
	private Transform _t;
	private Vector2 _baseSpeed;
	private CharacterState _state;

	private const float DASH_COOLDOWN = 0.5f;
	private const float DASH_MULTIPLIER_MAX = 3f;
	private const float DASH_DECAY = 0.25f; //decay in seconds
	private const float XSPEED = 0.05f;
	private const float YSPEED = 0.05f;
	private const float CHOMP_TIME = 0.5f;

	private XboxController _controller;

	public void Initialize(XboxController controller, Sprite sprite)
	{
		GetComponent<SpriteRenderer>().sprite = sprite;
		_controller = controller;
		_t = transform;
		_dashMultiplier = 1f;
		_state = CharacterState.Roaming;
	}

	void Update()
	{
		_xaxis = XCI.GetAxis(XboxAxis.LeftStickX, _controller);
		_yaxis = XCI.GetAxis(XboxAxis.LeftStickY, _controller);
		_baseSpeed = new Vector2(_xaxis, _yaxis).normalized;

		switch (_state)
		{
			case CharacterState.Roaming:


				if ((Mathf.Sign(_baseSpeed.x) != - Mathf.Sign(_t.localScale.x)) && _baseSpeed.x != 0f)
				{
					_t.localScale = new Vector3(-_t.localScale.x, 1f, 1f);
				}
				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));

				if (XCI.GetButtonDown(XboxButton.A, _controller))
				{
					Dash(DASH_DECAY, DASH_COOLDOWN);
				}

				if (XCI.GetButtonDown(XboxButton.B, _controller))
				{
					Deceive();
				}

				if (XCI.GetButtonDown(XboxButton.Y, _controller))
				{
					var hit = Physics2D.CircleCastAll(_t.position, 0.5f, Vector2.up);
					if (hit.Length > 0)
					{
						EatDaPoopoo(hit[0]);
					}
				}
				
				break;

			case CharacterState.Dashing:
				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));
				break;

			case CharacterState.Deceiving:
				if (XCI.GetButtonUp(XboxButton.B, _controller))
				{
					Interrupt();
				}
				break;

			case CharacterState.Mining:

				if (XCI.GetButtonUp(XboxButton.Y, _controller))
				{
					Interrupt();
				}
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

	public void Deceive()
	{
		_state = CharacterState.Deceiving;
		GetComponent<SpriteRenderer>().color = Color.red;
		// PLAY DEAD ANIMATION GETS TRIGGERED HERE
	}

	private Coroutine _eatDaPoopooCoroutine;

	public void EatDaPoopoo(RaycastHit2D poopoo)
	{
		_t.position = new Vector3(poopoo.transform.position.x, poopoo.transform.position.y, _t.position.z);
		_state = CharacterState.Mining;
		GetComponent<SpriteRenderer>().color = Color.yellow;
		_eatDaPoopooCoroutine = StartCoroutine(EatDaPoopooCoroutine(poopoo.transform.GetComponent<GoldPoo>(), CHOMP_TIME));
	}

	public void Interrupt()
	{
		_state = CharacterState.Roaming;
		GetComponent<SpriteRenderer>().color = Color.white;
		if (_eatDaPoopooCoroutine != null)
		{
			StopCoroutine(_eatDaPoopooCoroutine);
		}
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

	private IEnumerator EatDaPoopooCoroutine(GoldPoo poopoo, float chompTime)
	{
		while (_state == CharacterState.Mining && poopoo != null && poopoo.PooGold > 0)
		{
			yield return new WaitForSeconds(chompTime);
			poopoo.PooGold--;
			UIManager.Instance.UpdateScore(_controller, 1);

			if (poopoo.PooGold == 0)
			{
				poopoo.ClearPoo();
			}
			
			// EAT POOPOO ANIMATION GETS TRIGGERED HERE
		}

		Interrupt();
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