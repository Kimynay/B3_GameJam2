using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl sPlayer;

    public Camera mCamera;
    public float mNormalSpeed = 4;
    public float mHorizontalRotationSpeed = 1.5f;
    public float mVerticalRotationSpeed = 1.5f;
    public float mMaxVerticalRotation = 60;
    public float mCrouchHeight = 0.9f;
    public float mCrouchYSpeed = 1.0f;
    public float mRatioCrounchMovementSpeed = 0.5f;
    public LayerMask mPumpkinMask;
    public float mCameraBaseHeight = 1.6f;

    public bool mIsCrouch = false;

    private Vector3 mMovementRight;
    private Vector3 mMovementForward;
    private Vector3 mMovement;
    private bool mIsUp = true;
    private float mSpeed = 0;
    private bool mHaveSeenPumpkin;
    private Pumpkin mLastPumpkinSeen;
    private bool mLastPumpkinReset;
    private List<GameObject> mBushList;
    private Rigidbody mRigidbody;

    public bool mHaveDetectedThePlayer = false;

    public bool mIsHide = false;
    public bool mHaveBeenSeen = false;
    public bool mHasBeenArrested = false;

    public bool mThisLevelFinishSetup;


    void Start()
    {
        
        mBushList = new List<GameObject>();
        if (sPlayer == null)
        {
            sPlayer = this;
            mRigidbody = sPlayer.GetComponent<Rigidbody>();


        }

    }

    void Update()
    {
        if(MenuManager.sInMainMenu)
        {
            mRigidbody.useGravity = false;
        }
        else
        {
            if(!mThisLevelFinishSetup)
            {
                mThisLevelFinishSetup = true;

                mRigidbody.useGravity = true;
            }
            if (!mHasBeenArrested)
            {
                Move();
                Crouch();
                InteractWithPumpkin();
            }
            else
            {
                Debug.Log("GAME OVER !");
            }
            if (mBushList.Count > 0 && mIsCrouch)
            {
                mIsHide = true;
            }
            else
            {
                mIsHide = false;
            }
        }
    }

    void Move()
    {
        if (mIsCrouch)
        {
            mSpeed = mNormalSpeed * mRatioCrounchMovementSpeed;
        }
        else
        {
            mSpeed = mNormalSpeed;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            mMovementRight = Input.GetAxis("Horizontal") * mCamera.transform.right;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            mMovementForward = Input.GetAxis("Vertical") * new Vector3(mCamera.transform.forward.x, 0.0f, mCamera.transform.forward.z).normalized;
        }
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            mMovement = mMovementRight + mMovementForward;
            transform.position += mMovement * Time.deltaTime * mSpeed;
        }
    
        if (Input.GetAxis("RotationH") != 0)
        {
            transform.rotation *= Quaternion.Euler(0.0f, Input.GetAxis("RotationH") * mHorizontalRotationSpeed, 0.0f);
        }
        if (Input.GetAxis("RotationV") != 0)
        {
            mCamera.transform.rotation *= Quaternion.Euler(Input.GetAxis("RotationV") * -mVerticalRotationSpeed, 0.0f, 0.0f);
    
        }
        if (mCamera.transform.localEulerAngles.x < 360 - mMaxVerticalRotation && mCamera.transform.localEulerAngles.x > 180)
        {
            mCamera.transform.localEulerAngles = new Vector3(-mMaxVerticalRotation, 0.0f, 0.0f);
        }
        else if (mCamera.transform.localEulerAngles.x > mMaxVerticalRotation && mCamera.transform.localEulerAngles.x < 180)
        {
            mCamera.transform.localEulerAngles = new Vector3(mMaxVerticalRotation, 0.0f, 0.0f);
        }
    }

    void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            if (!mIsCrouch)
            {
                mIsUp = false;
                mCamera.transform.localPosition += Vector3.down * Time.deltaTime * mCrouchYSpeed;
                if (mCamera.transform.localPosition.y <= mCameraBaseHeight - mCrouchHeight)
                {
                    mCamera.transform.localPosition = new Vector3(0.0f, mCameraBaseHeight - mCrouchHeight, 0.0f);
                    mIsCrouch = true;
                }
            }
        }
        else
        {
            if (!mIsUp)
            {
                mIsCrouch = false;
                mCamera.transform.localPosition += Vector3.up * Time.deltaTime * mCrouchYSpeed;
                if (mCamera.transform.localPosition.y >= mCameraBaseHeight)
                {
                    mCamera.transform.localPosition = new Vector3(0.0f, mCameraBaseHeight, 0.0f);
                    mIsUp = true;
                }
            }
        }
    }
    void InteractWithPumpkin()
    {
        RaycastHit hit;
        if(Physics.Raycast(mCamera.transform.position, mCamera.transform.forward, out hit, 5.0f, mPumpkinMask))
        {
            mLastPumpkinSeen = hit.transform.GetComponent<Pumpkin>();
            if (!mHaveSeenPumpkin)
            {
                mLastPumpkinReset = false;
                mHaveSeenPumpkin = true;
                mLastPumpkinSeen.SetSeen();

            }
            if (Input.GetButtonDown("Action") && !mLastPumpkinSeen.mInfected)
            {
                mLastPumpkinSeen.SetInfected();
            }
        }
        else if(!mLastPumpkinReset && mLastPumpkinSeen != null && !mLastPumpkinSeen.mInfected)
        {
            mLastPumpkinReset = true;
            mHaveSeenPumpkin = false;
            mLastPumpkinSeen.SetUnSeen();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bush"))
        {
            mBushList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bush"))
        {
            mBushList.Remove(other.gameObject);
        }
    }
}
