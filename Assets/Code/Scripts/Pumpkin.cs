using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    public Light mLight;
    public Light mLight2;
    public ParticleSystem mParticleSystem;
    public MeshRenderer mMeshRenderer;
    public bool mSeen = false;
    public bool mInfected = false;
    private ParticleSystem.MainModule mainModule;
    void OnEnable()
    {
        mainModule = mParticleSystem.main;
    }

    void Update()
    {
        
    }
    public void SetSeen()
    {
        mMeshRenderer.material.SetFloat("_Seen", 1.0f);
    }
    public void SetInfected()
    {
        mInfected = true;
        mLight.color = Color.green;
        mLight2.color = Color.green;
        mainModule.startColor = Color.green;
        mMeshRenderer.material.SetFloat("_Seen", 0.0f);
    }
    public void SetUnSeen()
    {
        mMeshRenderer.material.SetFloat("_Seen", 0.0f);
    }
}
