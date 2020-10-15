using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public GameObject mThisPoliceIA;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!other.GetComponent<PlayerControl>().mIsHide)
            {
                other.GetComponent<PlayerControl>().mHaveBeenSeen = true;
                if(mThisPoliceIA.GetComponent<PoliceCarIA>() != null)
                {
                    mThisPoliceIA.GetComponent<PoliceCarIA>().mHaveDetectedThePlayer = true;
                }
                if (mThisPoliceIA.GetComponent<PoliceManIA>() != null)
                {
                    mThisPoliceIA.GetComponent<PoliceManIA>().mHaveDetectedThePlayer = true;
                }
            }
        }
    }
}
