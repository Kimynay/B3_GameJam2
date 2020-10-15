using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionVideo : MonoBehaviour
{
    public Material mPostProcessMaterial;
    private Slider mBrightness;

    private void Start()
    {
        mBrightness = MenuManager.sMenuManager.mBrightnessSlider;
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mPostProcessMaterial);
    }

    public void Update()
    {
        Shader.SetGlobalFloat("Brightness", (mBrightness.value - 0.5f) * 2.0f);
    }
}
