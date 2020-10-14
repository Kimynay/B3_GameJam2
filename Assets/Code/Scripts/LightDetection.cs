using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public PoliceCarIA mThisPoliceCar;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().mHaveBeenSeen = true;
            mThisPoliceCar.mHaveDetectedThePlayer = true;
        }
    }
}
