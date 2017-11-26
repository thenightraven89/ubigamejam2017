using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeColliderDetector : MonoBehaviour {

    public System.Action<Collider2D> onPlayerSpotted;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("found player");
            if (collision.gameObject.GetComponent<Character>().GetCharState == Character.CharacterState.Deceiving)
            {
                print("butIsPlaying dead");
                return;
            }
            if (onPlayerSpotted != null)
                onPlayerSpotted(collision);
            //sRend.sprite = RedSprite;
        }
    }
}
