using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PoliceCarIA : MonoBehaviour
{
    public float mCarSpeed = 2.0f;
    public float mOnCurveRatioSpeed = 0.2f;
    public GameObject[] mBezierCurves;
    public GameObject[] mGiroPhares;
    public float mGiroRotationSpeed = 2.0f;

    public bool mHaveDetectedThePlayer = false;

    private BezierCurve[] mCurvesScripts;
    private float mRatioOnTheCurve = 0;

    private int mIndex = 0;
    private bool mIsOnTheCurve = false;
    private bool mGiroTurnedOn = false;
    void Start()
    {
        mCurvesScripts = new BezierCurve[mBezierCurves.Length];
        for(int i = 0; i < mBezierCurves.Length; i++)
        {
            mCurvesScripts[i] = mBezierCurves[i].GetComponent<BezierCurve>();
        }
    }

    void Update()
    {
        if (!mHaveDetectedThePlayer)
        {
            if (!mIsOnTheCurve)
                GoToNextCurve(mIndex);

            if (mIsOnTheCurve)
                SlideOnTheCurve(mIndex);
        }
        else
        {
            if (!mGiroTurnedOn)
            {
                mGiroTurnedOn = true;

                //PlaySound !!!
                for (int i = 0; i < mGiroPhares.Length; i++)
                {
                    mGiroPhares[i].GetComponent<Light>().intensity = 5.0f;
                }
            }
            for (int i = 0; i < mGiroPhares.Length; i++)
            {
                mGiroPhares[i].transform.localEulerAngles += new Vector3(0.0f, Time.deltaTime * 360 * mGiroRotationSpeed, 0.0f);
            }
        }
    }
    void GoToNextCurve(int index)
    {
        Vector3 direction = mCurvesScripts[index].startingPoint.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.position += direction.normalized * Time.deltaTime * mCarSpeed;
        if(direction.magnitude < 0.02f)
        {
            transform.position = mCurvesScripts[index].startingPoint.position;
            mIsOnTheCurve = true;
        }
    }
    void SlideOnTheCurve(int index)
    {
        mRatioOnTheCurve += Time.deltaTime * mCarSpeed * mOnCurveRatioSpeed;
        transform.position = mCurvesScripts[index].GetPosition(mRatioOnTheCurve);
        transform.rotation = Quaternion.LookRotation(mCurvesScripts[index].GetVelocity(mRatioOnTheCurve).normalized);
        if(mRatioOnTheCurve >= 1.0f)
        {
            mRatioOnTheCurve = 0;
            mIsOnTheCurve = false;
            mIndex++;
            if (mIndex > mBezierCurves.Length - 1)
                mIndex = 0;
        }
    }
}
