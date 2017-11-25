using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPoo : MonoBehaviour
{
    public static System.Action<GoldPoo> onPooCleared;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearPoo()
    {
        if (onPooCleared != null)
            onPooCleared(this); // used for dereferencing
        Destroy(gameObject);
    }
}
