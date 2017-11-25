using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Transform upperLimit;
    public Transform lowerLimit;
    public Transform leftLimit;
    public Transform rightLimit;
    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetValidPosForBearPatrol()
    {
        float x = leftLimit.position.x + Random.Range(0f, rightLimit.position.x - leftLimit.position.x);
        float y = lowerLimit.position.y + Random.Range(0f, upperLimit.position.y - lowerLimit.position.y);

        return new Vector3(x, y, 0f);
    }
}
