using UnityEngine;
using System;
using System.Collections;

//C# event
public delegate void ActionEventHandler(object sender);

public class SharkyControl : MonoBehaviour
{
    public event ActionEventHandler Bite;

    #region data
    //associated in editor
    public Animator m_SharkyAnim;

    //internally calculated variables
    //readonly public access
    private float m_TurnAmt = 0;
    public float turnamt
    {
        get
        {
            return m_TurnAmt;
        }
    }
    private bool m_IsSprinting = false;
    public bool sprinting
    {
        get
        {
            return m_IsSprinting;
        }
    }
    private float m_Speed = 1f;
    public float speed
    {
        get
        {
            return m_Speed;
        }
    }
    private float m_RotationalVelocity = 1.5f;
    public float rotationalvelocity
    {
        get
        {
            return m_RotationalVelocity;
        }
    }
    private float m_VertSpeed = 0f;
    public float vertspeed
    {
        get
        {
            return m_VertSpeed;
        }
    }
    private float m_VertSpeedMult = 1f;
    public float vertspeedmult
    {
        get
        {
            return m_VertSpeedMult;
        }
    }
    private int m_InvertFactor = 1;
    public float invertfactor
    {
        get
        {
            return m_InvertFactor;
        }
    }

    //Inspector accessible parameters
    public bool m_InvertY = false;

    [System.Serializable]
    public class TurnControls
    {
        public float baseRotVel = 1.5f;
        public float maxRotVel = 3f;
        public float acceleration = 0.0125f;
        public float reverseAccel = 0.02f;
        public float oobAccel = 0.025f;
    }
    public TurnControls m_TurnParams;

    [System.Serializable]
    public class VerticalControls
    {
        public float maxVertSpeedMult = 2f;
        public float vertSpeedConst = 0.015f;
        public float floor = 0.3f;
        public float ceiling = 25f;
        public float maxAngle = 30f;
        public float angleAccel = 0.0125f;
        public float reverseangleAccel = 0.02f;
        public float oobangleAccel = 0.025f;
    }
    public VerticalControls m_VertParams;

    [System.Serializable]
    public class SpeedControls
    {
        public float baseSpeed = 1f;
        public float maxSpeed = 8f;
        public float speedConst = 0.025f;
        public float acceleration = 0.025f;
        public float reverseAccel = 0.05f;
    }
    public SpeedControls m_SpeedParams;

    private const float STDFRAMERATE = 60f;

    //Singleton accessor
    private static SharkyControl instance;
    public static SharkyControl Instance()
    {
        return instance;
    }
    #endregion


    #region UnityMethods
    void Awake()
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        HandleInvert();
        HandleSprintInput();
        HandleActionInput();
        HandleDirectionalInput();
        HandleVerticalBounds();
        OutputAnimationParams();
        ResolveTransform();
	}
    #endregion


    #region InternalInputResolution
    private void HandleInvert()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_InvertY = !m_InvertY;
        }
        if (m_InvertY)
        {
            m_InvertFactor = -1;
        }
        else
        {
            m_InvertFactor = 1;
        }
    }

    private void OutputAnimationParams()
    {
        m_SharkyAnim.SetBool("IsSprinting", m_IsSprinting);
        m_SharkyAnim.SetFloat("Speed", Mathf.Clamp(m_Speed / 2f, 1f, 3f));
        m_SharkyAnim.SetFloat("TurnAmt", m_TurnAmt / 2f);
    }

    private void HandleActionInput()
    {
        if (InputManager.Instance.GetAction1ButtonDown() || InputManager.Instance.GetSecondaryFireBool())
        {
            m_SharkyAnim.SetTrigger("Bite");
            if (Bite != null)
            {
                Bite(this);
            }
        }
    }

    private void HandleSprintInput()
    {
        if (InputManager.Instance.GetPrimaryFireBool())
        {
            m_IsSprinting = true;
            m_Speed += m_SpeedParams.acceleration * STDFRAMERATE * Time.deltaTime;
            m_Speed = Mathf.Min(m_Speed, m_SpeedParams.maxSpeed);

            m_RotationalVelocity += m_SpeedParams.acceleration * STDFRAMERATE * Time.deltaTime;
            m_RotationalVelocity = Mathf.Min(m_RotationalVelocity, m_TurnParams.maxRotVel);

            m_VertSpeedMult += m_SpeedParams.acceleration * STDFRAMERATE * Time.deltaTime;
            m_VertSpeedMult = Mathf.Min(m_VertSpeedMult, m_VertParams.maxVertSpeedMult);
        }
        else
        {
            m_IsSprinting = false;
            m_Speed -= m_SpeedParams.reverseAccel * STDFRAMERATE * Time.deltaTime;
            m_Speed = Mathf.Max(m_Speed, m_SpeedParams.baseSpeed);

            m_RotationalVelocity -= m_SpeedParams.reverseAccel * STDFRAMERATE * Time.deltaTime;
            m_RotationalVelocity = Mathf.Max(m_RotationalVelocity, m_TurnParams.baseRotVel);

            m_VertSpeedMult -= m_SpeedParams.reverseAccel * STDFRAMERATE * Time.deltaTime;
            m_VertSpeedMult = Mathf.Max(m_VertSpeedMult, 1f);
        }
    }

    //handle left control stick and wsad inputs
    //modulate rates and caps based on analog input
    //handle snap back to origin position
    private void HandleDirectionalInput()
    {
        float horiz = InputManager.Instance.GetLeftHorizontal();
        float vert = InputManager.Instance.GetLeftVertical();
        if (horiz < 0f)
        {
            if (m_TurnAmt < horiz)//handle case where cap falls below current value, due to analog input
            {
                m_TurnAmt += m_TurnParams.oobAccel * STDFRAMERATE * Time.deltaTime * -horiz;
                m_TurnAmt = Mathf.Min(m_TurnAmt, horiz);
            }
            else                    //standard value ramp with positive input for this axis/direction
            {
                m_TurnAmt -= m_TurnParams.acceleration * STDFRAMERATE * Time.deltaTime * -horiz;
                m_TurnAmt = Mathf.Max(m_TurnAmt, horiz);
            }
        }
        else if (m_TurnAmt < 0)     //snap to origin with no input
        {
            m_TurnAmt += m_TurnParams.reverseAccel * STDFRAMERATE * Time.deltaTime;
            m_TurnAmt = Mathf.Min(m_TurnAmt, 0f);
        }
        if (horiz > 0f)
        {
            if (m_TurnAmt > horiz)  //handle case where cap falls below current value, due to analog input
            {
                m_TurnAmt -= m_TurnParams.oobAccel * STDFRAMERATE * Time.deltaTime;
                m_TurnAmt = Mathf.Max(m_TurnAmt, horiz);
            }
            else                    //standard value ramp with positive input for this axis/direction
            {
                m_TurnAmt += m_TurnParams.acceleration * STDFRAMERATE * Time.deltaTime * horiz;
                m_TurnAmt = Mathf.Min(m_TurnAmt, horiz);
            }
        }
        else if (m_TurnAmt > 0)     //snap to origin with no input
        {
            m_TurnAmt -= m_TurnParams.reverseAccel * STDFRAMERATE * Time.deltaTime;
            m_TurnAmt = Mathf.Max(m_TurnAmt, 0f);
        }

        if (vert > 0f)
        {
            if (m_VertSpeed > vert) //handle case where cap falls below current value, due to analog input
            {
                m_VertSpeed -= m_VertParams.oobangleAccel * STDFRAMERATE * Time.deltaTime;
                m_VertSpeed = Mathf.Max(m_VertSpeed, vert);
            }
            else                    //standard value ramp with positive input for this axis/direction
            {
                m_VertSpeed += m_VertParams.angleAccel * STDFRAMERATE * Time.deltaTime * vert;
                m_VertSpeed = Mathf.Min(m_VertSpeed, vert);
            }
        }
        else if (m_VertSpeed > 0)   //snap to origin with no input
        {
            m_VertSpeed -= m_VertParams.reverseangleAccel * STDFRAMERATE * Time.deltaTime;
            m_VertSpeed = Mathf.Max(m_VertSpeed, 0f);
        }
        if (vert < 0f)
        {
            if (m_VertSpeed < vert) //handle case where cap falls below current value, due to analog input
            {
                m_VertSpeed += m_VertParams.oobangleAccel * STDFRAMERATE * Time.deltaTime;
                m_VertSpeed = Mathf.Min(m_VertSpeed, vert);
            }
            else                    //standard value ramp with positive input for this axis/direction
            {
                m_VertSpeed -= m_VertParams.angleAccel * STDFRAMERATE * Time.deltaTime * -vert;
                m_VertSpeed = Mathf.Max(m_VertSpeed, vert);
            }
        }
        else if (m_VertSpeed < 0)   //snap to origin with no input
        {
            m_VertSpeed += m_VertParams.reverseangleAccel * STDFRAMERATE * Time.deltaTime;
            m_VertSpeed = Mathf.Min(m_VertSpeed, 0f);
        }
    }

    //Buffer vertical angle and speed before hard clamp and floor and ceiling
    private void HandleVerticalBounds()
    {
        if (transform.position.y < m_VertParams.floor + 2f && m_VertSpeed * m_InvertFactor < 0)
        {
            //ugh magic numbers
            m_VertSpeed += 0.025f * STDFRAMERATE * Time.deltaTime * m_InvertFactor * m_VertSpeedMult * Mathf.Max(Mathf.Abs(m_VertSpeed), 0.45f) * (2.5f - (transform.position.y - m_VertParams.floor));
            if (m_InvertFactor > 0)
            {
                m_VertSpeed = Mathf.Min(m_VertSpeed, 0f);
            }
            else if (m_InvertFactor < 0)
            {
                m_VertSpeed = Mathf.Max(m_VertSpeed, 0f);
            }
        }

        if (transform.position.y > m_VertParams.ceiling - 2f && m_VertSpeed * m_InvertFactor > 0)
        {
            //ugh magic numbers
            m_VertSpeed -= 0.025f * STDFRAMERATE * Time.deltaTime * m_InvertFactor * m_VertSpeedMult * Mathf.Max(Mathf.Abs(m_VertSpeed), 0.45f) * (2.5f - (m_VertParams.ceiling - transform.position.y));
            if (m_InvertFactor > 0)
            {
                m_VertSpeed = Mathf.Max(m_VertSpeed, 0f);
            }
            else if (m_InvertFactor < 0)
            {
                m_VertSpeed = Mathf.Min(m_VertSpeed, 0f);
            }
        }
    }

    //final transform value application after input handlers and internal value calculations
    private void ResolveTransform()
    {
        //translate vertically to supplement vertical rotation
        transform.position += Vector3.up * m_VertSpeed * m_VertSpeedMult * m_VertParams.vertSpeedConst * STDFRAMERATE * Time.deltaTime * m_InvertFactor;
        //Apply ceiling and floor
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, m_VertParams.floor, m_VertParams.ceiling), transform.position.z);
        //Forward movement
        transform.position += transform.forward * m_Speed * m_SpeedParams.speedConst * STDFRAMERATE * Time.deltaTime;
        //Lateral rotational turning
        transform.Rotate(Vector3.up, m_TurnAmt * m_TurnParams.baseRotVel * STDFRAMERATE * Time.deltaTime);
        //Apply vertical movement rotation, also enforce zero z-axis roll
        transform.rotation = Quaternion.Euler(m_VertSpeed * m_VertParams.maxAngle * -m_InvertFactor, transform.rotation.eulerAngles.y, 0f);
    }
    #endregion
}
