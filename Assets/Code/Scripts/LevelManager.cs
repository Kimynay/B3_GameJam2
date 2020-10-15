using System.Collections;
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
            MenuManager.sMenuManager.EndWin();
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
}
