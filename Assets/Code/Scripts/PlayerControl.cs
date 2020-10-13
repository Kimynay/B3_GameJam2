using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Camera mCamera;
    public float mNormalSpeed = 4;
    public float mHorizontalRotationSpeed = 1.5f;
    public float mVerticalRotationSpeed = 1.5f;
    public float mMaxVerticalRotation = 60;
    public float mCrouchMovementHeight = 0.8f;
    public float mCrouchYSpeed = 1.0f;
    public float mRatioCrounchMovementSpeed = 0.5f;
    public LayerMask mPumpkinMask;

    public bool mIsCrouch = false;

    private Vector3 mMovementRight;
    private Vector3 mMovementForward;
    private Vector3 mMovement;
    private bool mIsUp = true;
    private float mSpeed = 0;
    private bool mHaveSeenPumpkin;
    private Pumpkin mLastPumpkinSeen;
    private bool mLastPumpkinReset;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Crouch();
        InteractWithPumpkin();

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
                if (mCamera.transform.localPosition.y <= -mCrouchMovementHeight)
                {
                    mCamera.transform.localPosition = new Vector3(0.0f, -mCrouchMovementHeight, 0.0f);
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
                if (mCamera.transform.localPosition.y >= 0.0f)
                {
                    mCamera.transform.localPosition = Vector3.zero;
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
}
