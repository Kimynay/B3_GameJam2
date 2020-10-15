using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManIA : MonoBehaviour
{
    public Transform[] mPatrolPoints;
    private Vector3[] mPatrolPointsPos;
    private Animator mAnimator;

    public float mPoliceManSpeed = 3.0f;
    public float mPoliceManRunSpeed = 5.0f;
    public float mStayTimeduration = 4.0f;
    public float mDistanceToArrestThePlayer = 0.8f;

    public bool mHaveDetectedThePlayer = false;

    private int mIndex = 0;
    private bool mIsWaiting;
    private float mDistanceToStopAtPoint = 0.02f;
    private bool mGameFinished = false;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mPatrolPointsPos = new Vector3[mPatrolPoints.Length];
        for (int i = 0; i < mPatrolPoints.Length; i++)
        {
            mPatrolPointsPos[i] = mPatrolPoints[i].position;
        }
    }

    void Update()
    {
        if(!mGameFinished)
        {
            if (!mHaveDetectedThePlayer)
            {
                if (!mIsWaiting)
                    GoToNextPoint(mIndex);
            }
            else
            {
                GoToPlayer();
            }
        }

    }
    void GoToNextPoint(int index)
    {
        Vector3 direction = mPatrolPointsPos[index] - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.position += direction.normalized * Time.deltaTime * mPoliceManSpeed;
        mAnimator.SetBool("walk", true);
        if (direction.magnitude < mDistanceToStopAtPoint)
        {
            transform.position = mPatrolPointsPos[index];
            transform.rotation = Quaternion.LookRotation(mPatrolPoints[index].forward);
            StayThereSomeTime();
            mIsWaiting = true;
        }
    }
    void StayThereSomeTime()
    {
        mAnimator.SetBool("walk", false);
        StartCoroutine(WaitThereXSecond(mStayTimeduration));
    }

    IEnumerator WaitThereXSecond(float secondToWait)
    {
        yield return new WaitForSeconds(secondToWait);
        mIndex++;
        if (mIndex > mPatrolPoints.Length - 1)
            mIndex = 0;
        mIsWaiting = false;
    }
    void GoToPlayer()
    {
        Vector3 direction = PlayerControl.sPlayer.transform.position - transform.position;
        transform.position += direction.normalized * Time.deltaTime * mPoliceManRunSpeed;
        mAnimator.SetBool("run", true);
        if (direction.magnitude < mDistanceToArrestThePlayer)
        {
            PlayerControl.sPlayer.mHasBeenArrested = true;
            mGameFinished = true;
        }
    }
}
