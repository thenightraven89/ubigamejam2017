using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Character : MonoBehaviour
{
	public AudioClip _dashAudio;
	public AudioClip _eatAudio;

	private float _xaxis;
	private float _yaxis;
	private float _dashMultiplier;
	private Transform _t;
	private Vector2 _baseSpeed;
	private CharacterState _state;
	private float _stunTime;

	private const float DASH_COOLDOWN = 0.5f;
	private const float DASH_MULTIPLIER_MAX = 3f;
	private const float DASH_DECAY = 0.25f; //decay in seconds
	private const float XSPEED = 0.05f;
	private const float YSPEED = 0.05f;
	private const float CHOMP_TIME = 0.5f;

	private XboxController _controller;

	private Sprite _sprite;
	private Sprite _deadSprite;

	public SpriteRenderer _spriteRenderer;

	private AudioSource _source;

	private Collider2D _collider;

	public GameObject _stunEmoji;

    public CharacterState GetCharState
    {
        get { return _state; }
    }

    public void Initialize(XboxController controller, Sprite sprite, Sprite deadSprite)
	{
		_sprite = sprite;
		_deadSprite = deadSprite;
		_spriteRenderer.sprite = _sprite;
		_controller = controller;
		_t = transform;
		_dashMultiplier = 1f;
		_state = CharacterState.Roaming;
		GetComponent<Animator>().Play("Idle");
		_source = FindObjectOfType<AudioSource>();
		_collider = GetComponent<Collider2D>();
	}

	void Update()
	{
		_xaxis = XCI.GetAxis(XboxAxis.LeftStickX, _controller);
		_yaxis = XCI.GetAxis(XboxAxis.LeftStickY, _controller);
		_baseSpeed = new Vector2(_xaxis, _yaxis).normalized;

		_pushedSpeed /= 2f;

		switch (_state)
		{
			case CharacterState.Roaming:

				if (!_collider.enabled)
				{
					_collider.enabled = true;
					_stunEmoji.SetActive(false);
				}

				if ((Mathf.Sign(_baseSpeed.x) != - Mathf.Sign(_t.localScale.x)) && _baseSpeed.x != 0f)
				{
					_t.localScale = new Vector3(-_t.localScale.x, 1f, 1f);
				}

				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier) + _pushedSpeed);

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
					var hit = Physics2D.CircleCastAll(_t.position, 0.5f, Vector2.up, 0.5f, 1 << LayerMask.NameToLayer("Poopoo"));
					if (hit.Length > 0)
					{
						EatDaPoopoo(hit[0]);
					}
				}

				if (XCI.GetButtonDown(XboxButton.X, _controller))
				{
					var hits = Physics2D.CircleCastAll(_t.position, 0.5f, Vector2.up, 0.5f, 1 << LayerMask.NameToLayer("Player"));
					foreach (var h in hits)
					{
						if (h.transform != _t)
						{
							var other = h.transform.GetComponent<Character>();
							other.ReceiveStun(2f);
							other.ReceivePush(other.transform.position - _t.position);
						}
					}
				}
				
				break;

			case CharacterState.Dashing:
				_t.Translate(new Vector2(_baseSpeed.x * XSPEED * _dashMultiplier, _baseSpeed.y * YSPEED * _dashMultiplier));
				break;

			case CharacterState.Deceiving:
				if (XCI.GetButtonUp(XboxButton.B, _controller))
				{
					GetComponent<Animator>().Play("Idle");
					Interrupt();
				}
				break;

			case CharacterState.Mining:

				if (XCI.GetButtonUp(XboxButton.Y, _controller))
				{
					GetComponent<Animator>().Play("Idle");
					Interrupt();
				}
				break;

			case CharacterState.Stunned:
				_stunTime = Mathf.Clamp(_stunTime - Time.deltaTime, 0f, _stunTime);
				if (_stunTime == 0f)
				{
					_state = CharacterState.Roaming;
					_collider.enabled = true;
					_stunEmoji.SetActive(false);
				}

				_t.Translate(_pushedSpeed);

				break;

			default:
				break;
		}

		if (_t.position.x < -9)
		{
			_t.position = new Vector3(-9, _t.position.y, _t.position.z);
		}

		if (_t.position.x > 9)
		{
			_t.position = new Vector3(9, _t.position.y, _t.position.z);
		}

		if (_t.position.y < -4.5)
		{
			_t.position = new Vector3(_t.position.x, -4.5f, _t.position.z);
		}

		if (_t.position.y > 4.5)
		{
			_t.position = new Vector3(_t.position.x, 4.5f, _t.position.z);
		}
	}

	public void Dash(float decay, float cooldown)
	{
		_state = CharacterState.Dashing;
		_source.PlayOneShot(_dashAudio);
		StartCoroutine(DashCoroutine(DASH_DECAY, DASH_COOLDOWN));
	}

	private Vector2 _pushedSpeed;

	public void ReceivePush(Vector3 push)
	{
		_pushedSpeed = push.normalized * 3f;
	}

	public void ReceiveStun(float duration)
	{
		_stunEmoji.SetActive(true);
		_state = CharacterState.Stunned;
		_dashMultiplier = 1f;
		_stunTime = duration;
		_collider.enabled = false;
	}

	public void Deceive()
	{
		_state = CharacterState.Deceiving;
		_spriteRenderer.sprite = _deadSprite;
		GetComponent<Animator>().Play("faint");
		// PLAY DEAD ANIMATION GETS TRIGGERED HERE
	}

	private Coroutine _eatDaPoopooCoroutine;

	public void EatDaPoopoo(RaycastHit2D poopoo)
	{
		GetComponent<Animator>().Play("Eat");
		_t.position = new Vector3(poopoo.transform.position.x, poopoo.transform.position.y, _t.position.z);
		_state = CharacterState.Mining;
		_eatDaPoopooCoroutine = StartCoroutine(EatDaPoopooCoroutine(poopoo.transform.GetComponent<GoldPoo>(), CHOMP_TIME));
	}

	public void Interrupt()
	{
		_state = CharacterState.Roaming;
		_spriteRenderer.sprite = _sprite;
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

	private IEnumerator EatDaPoopooCoroutine(GoldPoo poopoo, float chompTime)
	{
		while (_state == CharacterState.Mining && poopoo != null && poopoo.PooGold > 0)
		{
			_source.PlayOneShot(_eatAudio);

			yield return new WaitForSeconds(chompTime);
			poopoo.PooGold--;
			UIManager.Instance.UpdateScore(_controller, 1);

			if (poopoo.PooGold == 0)
			{
				poopoo.ClearPoo();
			}
			
			// EAT POOPOO ANIMATION GETS TRIGGERED HERE
		}

		GetComponent<Animator>().Play("Idle");

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