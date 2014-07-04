using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public float smooth = 3f;		// a public variable to adjust smoothing of camera motion
    public Transform m_TargetPos;			// the usual position for the camera, specified by a transform in the game
    public Transform m_Pivot;
    
    public float m_CamAccel = 0.5f;
    public float m_CamAngle = 45f;

    private Vector2 m_xyTilt;

    void Update()
    {
        float vert = InputManager.Instance.GetRightVertical();
        float horiz = InputManager.Instance.GetRightHorizontal();
        if (vert < 0f)
        {
            if (m_xyTilt.y < vert)
            {
                m_xyTilt.y -= 2f * m_CamAccel * 60f * Time.deltaTime;
                m_xyTilt.y = Mathf.Max(m_xyTilt.y, m_CamAngle * vert);
            }
            else
            {
                m_xyTilt.y += m_CamAccel * 60f * Time.deltaTime * -vert;
                m_xyTilt.y = Mathf.Min(m_xyTilt.y, m_CamAngle);
            }
        }
        else if(m_xyTilt.y > 0)
        {
            m_xyTilt.y -= 2f * m_CamAccel * 60f * Time.deltaTime;
            m_xyTilt.y = Mathf.Max(m_xyTilt.y, 0f);
        }
        if (vert > 0f)
        {
            if (m_xyTilt.y > vert)
            {
                m_xyTilt.y += 2f * m_CamAccel * 60f * Time.deltaTime;
                m_xyTilt.y = Mathf.Min(m_xyTilt.y, m_CamAngle * vert);
            }
            else
            {
                m_xyTilt.y -= m_CamAccel * 60f * Time.deltaTime * vert;
                m_xyTilt.y = Mathf.Max(m_xyTilt.y, -m_CamAngle);
            }
        }
        else if (m_xyTilt.y < 0)
        {
            m_xyTilt.y += 2f * m_CamAccel * 60f * Time.deltaTime;
            m_xyTilt.y = Mathf.Min(m_xyTilt.y, 0f);
        }

        if (horiz > 0f)
        {
            if (m_xyTilt.x > horiz)
            {
                m_xyTilt.x -= 2f * m_CamAccel * 60f * Time.deltaTime;
                m_xyTilt.x = Mathf.Max(m_xyTilt.x, m_CamAngle * horiz);
            }
            else
            {
                m_xyTilt.x += m_CamAccel * 60f * Time.deltaTime * horiz;
                m_xyTilt.x = Mathf.Min(m_xyTilt.x, m_CamAngle);
            }
        }
        else if (m_xyTilt.x > 0)
        {
            m_xyTilt.x -= 2f*m_CamAccel * 60f * Time.deltaTime;
            m_xyTilt.x = Mathf.Max(m_xyTilt.x, 0f);
        }
        if (horiz < 0f)
        {
            if (m_xyTilt.x < horiz)
            {
                m_xyTilt.x += 2f * m_CamAccel * 60f * Time.deltaTime;
                m_xyTilt.x = Mathf.Min(m_xyTilt.x, horiz * m_CamAngle);
            }
            else
            {
                m_xyTilt.x -= m_CamAccel * 60f * Time.deltaTime * -InputManager.Instance.GetRightHorizontal();
                m_xyTilt.x = Mathf.Max(m_xyTilt.x, -m_CamAngle);
            }
        }
        else if (m_xyTilt.x < 0)
        {
            m_xyTilt.x += 2f * m_CamAccel * 60f * Time.deltaTime;
            m_xyTilt.x = Mathf.Min(m_xyTilt.x, 0f);
        }

        // return the camera to standard position and direction
        // also apply roll based on player turn radius via pivot
        m_Pivot.localRotation = Quaternion.Euler(m_xyTilt.y * SharkyControl.Instance().invertfactor, m_xyTilt.x, 0f);
        transform.position = Vector3.Lerp(transform.position, m_TargetPos.position, Time.deltaTime * smooth);
        transform.forward = Vector3.Lerp(transform.forward, m_TargetPos.forward, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, SharkyControl.Instance().turnamt * (SharkyControl.Instance().rotationalvelocity - 1.25f) * -10f);
    }
}
