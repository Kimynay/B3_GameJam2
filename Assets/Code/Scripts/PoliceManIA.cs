using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManIA : MonoBehaviour
{
    public Transform[] mPatrolPoints;
    private Vector3[] mPatrolPointsPos;

    public float mPoliceManSpeed = 3.0f;
    public float mStayTimeduration = 4.0f;

    private int mIndex = 0;
    private bool mIsWaiting;
    void Start()
    {
        mPatrolPointsPos = new Vector3[mPatrolPoints.Length];
        for (int i = 0; i < mPatrolPoints.Length; i++)
        {
            mPatrolPointsPos[i] = mPatrolPoints[i].position;
        }
    }

    void Update()
    {
        if(!mIsWaiting)
            GoToNextPoint(mIndex);
    }
    void GoToNextPoint(int index)
    {
        Vector3 direction = mPatrolPointsPos[index] - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.position += direction.normalized * Time.deltaTime * mPoliceManSpeed;
        if (direction.magnitude < 0.02f)
        {
            transform.position = mPatrolPointsPos[index];
            transform.rotation = Quaternion.LookRotation(mPatrolPoints[index].forward);
            StayThereSomeTime();
            mIsWaiting = true;
        }
    }
    void StayThereSomeTime()
    {
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
}
