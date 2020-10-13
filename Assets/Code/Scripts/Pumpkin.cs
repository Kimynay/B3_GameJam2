using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    private Color mPumpkinBaseColor;
    private MeshRenderer mMeshRenderer;
    public bool mSeen = false;
    public bool mInfected = false;
    void OnEnable()
    {
        mMeshRenderer = transform.GetComponent<MeshRenderer>();
        mPumpkinBaseColor = mMeshRenderer.material.color;
    }

    void Update()
    {
        
    }
    public void SetSeen()
    {
        mMeshRenderer.material.color = Color.yellow;
    }
    public void SetInfected()
    {
        mInfected = true;
        mMeshRenderer.material.color = Color.green;
    }
    public void SetUnSeen()
    {
        mMeshRenderer.material.color = mPumpkinBaseColor;
    }
}
