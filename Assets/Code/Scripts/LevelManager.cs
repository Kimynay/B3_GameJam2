﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Pumpkin[] mPumpkins;
    public PoliceCarIA[] mPoliceCars;
    public PoliceManIA[] mPoliceMens;
    public Childs[] mChilds;

    private bool mAllPumpkinPoisonned;
    private bool mFinishLevelSet = false;
    private float mTimeToWaitAtEnd = 2.0f;
    void Start()
    {
        mFinishLevelSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mFinishLevelSet && VerifyPumpkins())
        {
            mFinishLevelSet = true;
            StartCoroutine(WaitTwosecond(mTimeToWaitAtEnd));
        }
    }
    public bool VerifyPumpkins()
    {
        for (int i = 0; i < mPumpkins.Length; i++)
        {
            if (mPumpkins[i].mInfected)
            {
                mAllPumpkinPoisonned = true;
            }
            else
            {
                mAllPumpkinPoisonned = false;
                return false;   
            }
        }
        if (mAllPumpkinPoisonned)
            return true;
        else
            return false;
    }
    IEnumerator WaitTwosecond(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        MenuManager.sMenuManager.EndWin();
    }
}
