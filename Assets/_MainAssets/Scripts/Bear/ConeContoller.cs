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

    public Transform copilulubogdan;

    public Transform target;
    bool isFollowTarget = false;
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

        if(isFollowTarget)
        {
            if(Vector3.Angle(transform.position, target.position) < 0)
                transform.Rotate(0f, 0f, 1.5f);
            else
                transform.Rotate(0f, 0f, -1.5f);
        }
	}

    public void DisableCone()
    {
        isFollowTarget = false;
        colDet.gameObject.SetActive(false);
        DOTween.Kill(transform);
        CancelInvoke();
    }

    public void EnableCone()
    {
        colDet.gameObject.SetActive(true);
        sRend.sprite = NormalSprite;
        RotateCone();
    }

    void RotateCone()
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, UnityEngine.Random.Range(-180f, 180f)), 1f);
        Invoke("RotateCone", 1.5f);
    }



    private void HandlePlayerSpotted(Collider2D col)
    {
        target = col.transform;
        isFollowTarget = true;
        sRend.sprite = RedSprite;
        DOTween.Kill(transform);
        CancelInvoke();
        //transform.LookAt(col.transform, Vector3.back);
        //copilulubogdan.LookAt(col.transform, Vector3.forward);
        //float zAngle = Mathf.Acos(new Vector3(transform.position.x - col.transform.position.x, transform.position.y - col.transform.position.y, transform.position.z - col.transform.position.z).y);
        //transform.Rotate(new Vector3(0f, 0f, zAngle));
    }
}
