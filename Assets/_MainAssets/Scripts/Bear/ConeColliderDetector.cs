using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeColliderDetector : MonoBehaviour {

    public System.Action<Collider2D> onPlayerSpotted;
    public bool isBeingDeceived = false;
    public bool yodo = false;
    private Character targetChar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        isBeingDeceived = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(targetChar == null)
        {
            targetChar = targetChar = collision.gameObject.GetComponent<Character>();
        }
        if(targetChar.GetCharState != Character.CharacterState.Deceiving)
        {
            isBeingDeceived = false; // we are no longer being deceived
        }
        if(isBeingDeceived == false)
        {
            if (yodo) return;
            yodo = true;
            if (onPlayerSpotted != null)
                onPlayerSpotted(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            yodo = true;
            targetChar = collision.gameObject.GetComponent<Character>();
            print("found player");
            if (targetChar.GetCharState == Character.CharacterState.Deceiving)
            {
                print("butIsPlaying dead");
                isBeingDeceived = true;
                return;
            }
            if (onPlayerSpotted != null)
                onPlayerSpotted(collision);
            //sRend.sprite = RedSprite;
        }
    }
}
