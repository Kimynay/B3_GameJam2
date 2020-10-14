﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sMenuManager;
    public GameObject mLoadingScreen;

    public GameObject mMenuCamera;
    public GameObject mMainMenu;
    public GameObject mPauseMenu;
    public GameObject mEndFailMenu;
    public GameObject mEndWinMenu;
    public GameObject mMainMenuFirstSelected;
    public GameObject mLoadGameMenuFirstSelected;
    public GameObject mSaveGameMenuFirstSelected;
    public GameObject mControlsMenuFirstSelected;
    public GameObject mOptionMenuFirstSelected;
    public GameObject mCreditsMenuFirstSelected;
    public GameObject mPauseMenuFirstSelected;
    public GameObject mEndFailMenuFirstSelected;
    public GameObject mEndWinMenuFirstSelected;

    public Slider mVolumeSlider;
    public Slider mBrightnessSlider;

    public static bool sIsPaused = false;
    public static bool sInMainMenu = true;
    public bool mEndFail = false;
    public bool mEndWin = false;

    private void OnEnable()
    {
        if (sMenuManager == null)
            sMenuManager = this;
    }
    private void Start()
    {
        ChangeFirstButton(mMainMenuFirstSelected);
    }
    private void Update()
    {
        if (!sInMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                if (sIsPaused)
                {
                    sIsPaused = false;
                    mPauseMenu.SetActive(false);
                    Time.timeScale = 1.0f;
                }
                else
                {
                    sIsPaused = true;
                    mPauseMenu.SetActive(true);
                    Time.timeScale = 0.0f;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(mPauseMenuFirstSelected);

                }
            }
        }
        if (mEndFail)
        {
            EndFail();
        }
        if (mEndWin)
        {
            EndWin();
        }
        AudioListener.volume = mVolumeSlider.value;
        
    }
    public void GoBackToLastMenu()
    {
        if (sInMainMenu)
        {
            mMainMenu.SetActive(true);
            ChangeFirstButton(mMainMenuFirstSelected);
        }
        else
        {
            mPauseMenu.SetActive(true);
            ChangeFirstButton(mPauseMenuFirstSelected);

        }
    }
    public void GoToPauseMenu()
    {
        ChangeFirstButton(mPauseMenuFirstSelected);
    }
    public void GoToOptions()
    {
        ChangeFirstButton(mOptionMenuFirstSelected);
    }
    public void GoToCredits()
    {
        ChangeFirstButton(mCreditsMenuFirstSelected);
    }
    public void GoToControls()
    {
        ChangeFirstButton(mControlsMenuFirstSelected);
    }
    public void GoToLoadGame()
    {
        ChangeFirstButton(mLoadGameMenuFirstSelected);
    }
    public void GoToSaveGame()
    {
        ChangeFirstButton(mSaveGameMenuFirstSelected);
    }
    public void PlayGame()
    {
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        mMenuCamera.SetActive(false);
        sInMainMenu = false;
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        sIsPaused = false;
        Time.timeScale = 1.0f;
    }
    public void BackToMainMenu()
    {
        sIsPaused = false;
        sInMainMenu = true;
        mMenuCamera.SetActive(true);
        SceneManager.UnloadSceneAsync(1);
        ChangeFirstButton(mMainMenuFirstSelected);
    }

    public void EndFail()
    {
        mEndFail = false;
        mEndFailMenu.SetActive(true);
        Time.timeScale = 0.0f;
        ChangeFirstButton(mEndFailMenuFirstSelected);
    }

    public void EndWin()
    {

        mEndWin = false;
        mEndWinMenu.SetActive(true);
        Time.timeScale = 0.0f;
        ChangeFirstButton(mEndWinMenuFirstSelected);
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    public void ActivatePlayButton()
    {
        ChangeFirstButton(mMainMenuFirstSelected);
    }

    void ChangeFirstButton(GameObject ButtonToActivate)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ButtonToActivate);
    }
}