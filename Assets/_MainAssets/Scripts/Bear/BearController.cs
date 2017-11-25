using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Bear stuff")]
    public float bearSpeed = 2f;
    public float bearChargeSpeed = 3f;

    [Header("Bear stuff but this time it's references")]
    public GoldPoo goldPooPrefab;

    //patrol vars
    private Vector3 patrolDest;
    private Vector3 initPos;
    private float startTime;
    private float journeyLength;
    //we need this to get valid positions for patrolling
    private LevelScript ls;

    private BearStateMachine bsm;
    private bool isBsmInit = false;
    // Use this for initialization
    void Awake()
    {
        bsm = new BearStateMachine();
        ls = FindObjectOfType<LevelScript>();
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

    //returns true if arrived at patrol dest
    public bool Patrol()
    {
        float distCovered = (Time.time - startTime) * bearSpeed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(initPos, patrolDest, fracJourney);
        if (transform.position == patrolDest) return true;
        return false;
    }


    public void PoopGold()
    {
        print("<color=#DFEC00C6>pooping gold</color>");
        Instantiate(goldPooPrefab, transform.position,Quaternion.identity);
    }
    //debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(patrolDest, 0.2f);
    }
}
