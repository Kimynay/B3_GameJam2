using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public GameObject mThisPoliceCar;
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
            if(mThisPoliceCar.GetComponent<PoliceCarIA>() != null)
            {
                mThisPoliceCar.GetComponent<PoliceCarIA>().mHaveDetectedThePlayer = true;
            }
            
        }
    }
}
