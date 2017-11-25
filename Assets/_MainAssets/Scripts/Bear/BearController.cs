﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Bear stuff")]
    public float bearSpeed = 2f;
    public float bearHasToPooSpeed = 2.5f;
    public float bearChargeSpeed = 3f;

    public float minDistanceBetweenPoos = 5f;
    public int maxPooCount = 5;
    public int cooldownBetweenPoos = 6;

    [Header("Bear stuff but this time it's references")]
    public GoldPoo goldPooPrefab;

    //poos
    private List<GoldPoo> pooList; 

    //patrol vars
    private Vector3 patrolDest;
    private Vector3 pooDest;
    private Vector3 initPos;
    private float startTime;
    private float journeyLength;

    //poo occurence
    private int currentPooCd = 0;

    //we need this to get valid positions for patrolling
    private LevelScript ls;

    private BearStateMachine bsm;
    private bool isBsmInit = false;
    // Use this for initialization
    void Awake()
    {
        bsm = new BearStateMachine();
        Reset();
        ls = FindObjectOfType<LevelScript>();
    }

    private void OnEnable()
    {
        GoldPoo.onPooCleared += HandlePooCleaned;
    }

    private void OnDisable()
    {
        GoldPoo.onPooCleared -= HandlePooCleaned;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MakeBearAware();
        }

        if (isBsmInit)
            bsm.UpdateState();
    }

    public void Reset()
    {
        //reset variables
        pooList = new List<GoldPoo>();        
    }

    public void MakeBearAware()
    {
        if (bsm == null)
        {
            Debug.LogError("Bear state machine is null!");
            return;
        }
        bsm.InitStateMachine(this);
        isBsmInit = true;
    }

    public void GetPatrolPoint()
    {
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        patrolDest = ls.GetValidPosForBearPatrol();

        startTime = Time.time;
        journeyLength = Vector3.Distance(initPos, patrolDest);
    }

    public bool ShouldPoo()
    {
        if (pooList.Count > maxPooCount) return false;
        if (cooldownBetweenPoos >= currentPooCd) return false;
        return true;
    }

    public void GetPooPoint()
    {
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float dist = 0f;

        do
        {
            pooDest = ls.GetValidPosForBearPatrol();
            dist = Vector3.Distance(pooDest, initPos);
        } while (dist > minDistanceBetweenPoos);

        startTime = Time.time;
    }

    //returns true if arrived at patrol destination
    public bool Patrol()
    {
        float distCovered = (Time.time - startTime) * bearSpeed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(initPos, patrolDest, fracJourney);
        if (transform.position == patrolDest) return true;
        return false;
    }

    //returns true if arrived at poo destination
    public bool GoToPooDestination()
    {
        float distCovered = (Time.time - startTime) * bearHasToPooSpeed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(initPos, pooDest, fracJourney);
        if (transform.position == pooDest) return true;
        return false;
    }
    public void PoopGold()
    {
        print("<color=#DFEC00C6>pooping gold</color>");
        GoldPoo pooTemp = Instantiate(goldPooPrefab, transform.position,Quaternion.identity);
        pooList.Add(pooTemp);
        currentPooCd = 0;
    }

    public void LookConcerned()
    {
        print("<color=#DFEC00C6>looking concerned</color>");
        currentPooCd++;
    }

    //callbacks
    private void HandlePooCleaned(GoldPoo poo)
    {
        print("found poo. Removing from list");
        pooList.Remove(poo);
    }

    //debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(patrolDest, 0.2f);
    }
}
