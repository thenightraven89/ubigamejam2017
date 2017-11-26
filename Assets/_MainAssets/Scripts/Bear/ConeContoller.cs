using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConeContoller : MonoBehaviour {

    public Sprite NormalSprite;
    public Sprite RedSprite;

    public SpriteRenderer sRend;
    public ConeColliderDetector colDet;

    Tweener rotator;
    // Use this for initialization
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        colDet.onPlayerSpotted += HandlePlayerSpotted;
    }

    private void OnDisable()
    {
        colDet.onPlayerSpotted -= HandlePlayerSpotted;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
        {
            RotateCone();
        }
	}

    void RotateCone()
    {
        transform.DORotate(new Vector3(UnityEngine.Random.Range(-360f, 360f), 0f, 0f), 1f);
    }

    private void HandlePlayerSpotted(Collider2D col)
    {
        sRend.sprite = RedSprite;
        DOTween.Kill(transform);
        //transform.LookAt(col.transform, Vector3.back);

        //float zAngle = Mathf.Acos(new Vector3(transform.position.x - col.transform.position.x, transform.position.y - col.transform.position.y, transform.position.z - col.transform.position.z).y);
        //transform.DORotate(new Vector3(0f, 0f, zAngle), 1f);
    }
}
