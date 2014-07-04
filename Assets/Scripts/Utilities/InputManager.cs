using UnityEngine;
using System.Collections;

public class InputManager
{

    static InputManager instance = null;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }
#if UNITY_STANDALONE_OSX

    // because for some reason, the OSX Xbox Controller starts at 0.0f and then
    // is -1 at unpressed and 1 at pressed. >:(
    static private bool leftTriggerPressed = false;
    static private bool rightTriggerPressed = false;
    // prevent garbage collection, since not going to relinquish from memory
    static private float leftTriggerValue = 0.0f;
    static private float rightTriggerValue = 0.0f;

	public float GetLeftHorizontal()
	{
		return Input.GetAxis("Horizontal");
	}

	public float GetLeftVertical()
	{
		return Input.GetAxis("Vertical");
	}

	public float GetRightHorizontal()
	{
		return Input.GetAxis("HorizontalRight_Mac");
	}
	
	public float GetRightVertical()
	{
		return Input.GetAxis("VerticalRight_Mac");
	}

	public float GetDPadHorizontal()
	{
		return Input.GetAxis ("HorizontalDPad");
	}
	
	public float GetLeftTrigger()
	{
        leftTriggerValue = Input.GetAxis("LeftTrigger_Mac");
        if (leftTriggerPressed)
        {
            return (leftTriggerValue + 1.0f) / 2.0f;
        } 
        else
        {
            if(leftTriggerValue == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                leftTriggerPressed = true;
                return (leftTriggerValue + 1.0f) / 2.0f;
            }
        }
	}
	
	public float GetRightTrigger()
	{
        rightTriggerValue = Input.GetAxis("RightTrigger_Mac");
        if (rightTriggerPressed)
        {
            return (rightTriggerValue + 1.0f) / 2.0f;
        } 
        else
        {
            if(rightTriggerValue == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                rightTriggerPressed = true;
                return (rightTriggerValue + 1.0f) / 2.0f;
            }
        }
	}
	
	public bool GetStartButton()
	{
		return Input.GetButton("Start_Mac");
	}
	
	public bool GetFire1Button()
	{
		return Input.GetButton("Fire1_Mac");
	}
	
	public bool GetFire1ButtonDown() //this is used by the menu, but I'm unsure of its accuracy
	{
		return Input.GetButtonDown("Fire1_Mac");
	}
	public bool GetFire1ButtonUp() //this is used by the menu, but I'm unsure of its accuracy
	{
		return Input.GetButtonUp("Fire1_Mac");
	}
#endif

#if UNITY_STANDALONE_WIN
    public float GetLeftHorizontal()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            return -1f;
        }
        return Input.GetAxis("Horizontal");
    }

    public float GetLeftVertical()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            return -1f;
        }
        return Input.GetAxis("Vertical");
    }

    public float GetRightHorizontal()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            return -1f;
        }
        return Input.GetAxis("HorizontalRight");
    }

    public float GetRightVertical()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            return -1f;
        }
        return Input.GetAxis("VerticalRight");
    }

    public float GetSecondaryFire()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            return 1f;
        }
        return Input.GetAxis("LeftTrigger");
    }

    public bool GetSecondaryFireBool()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetAxis("LeftTrigger") > 0.1f;
    }

    public float GetPrimaryFire()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return 1f;
        }
        return Input.GetAxis("RightTrigger");
    }

    public bool GetPrimaryFireBool()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetAxis("RightTrigger") > 0.1f;
    }

    public float GetDPadHorizontal()
    {
        return Input.GetAxis("HorizontalDPad");
    }

    public bool GetStartButton()
    {
        return Input.GetButton("Start") || Input.GetKey(KeyCode.Escape);
    }

    public bool GetAction1Button()
    {
        return Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space);
    }

    public bool GetAction1ButtonDown()
    {
        return Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetAction1ButtonUp()
    {
        return Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Space);
    }

#endif
}