using UnityEngine;
using System.Collections;

public class DebugGUI : MonoBehaviour {

    //Singleton setup
    private static DebugGUI instance;
    public static DebugGUI Instance()
    {
        return instance;
    }

    //data
    public bool m_Show = false;
    private string m_InGameLog = "";
    private int lines = 0;
    private int fontsize = 15;
    private Rect m_WindowPosition = new Rect(Screen.width - 300, 3f*Screen.height / 4f, 300, Screen.height / 2f);
    private Vector2 m_ScrollPosition = Vector2.zero;

    //internal methods
    void Awake()
    {
        instance = this;
    }

    
    void OnGUI()
    {
        if (m_Show)
        {
            if(lines > 0) GUI.Box(m_WindowPosition, "");
            m_ScrollPosition = GUI.BeginScrollView(m_WindowPosition, m_ScrollPosition, new Rect(0, 0, 280, lines * fontsize + 3));
            GUILayout.Label(m_InGameLog);
            GUI.EndScrollView();
            if (lines > 0)
            {
                if (GUI.Button(new Rect(Screen.width - 130, m_WindowPosition.y - 25, 70, 20), "Reset Game"))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }

                if (GUI.Button(new Rect(Screen.width - 55, m_WindowPosition.y - 25, 50, 20), "Hide"))
                {
                    m_Show = false;
                }
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 55, Screen.height - 25, 50, 20), "Hide"))
                {
                    m_Show = false;
                }
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 25, 90, 20), "Reset Game"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            if (GUI.Button(new Rect(Screen.width - 55, Screen.height - 25, 50, 20), "Debug"))
            {
                m_Show = true;
            }
        }
    }

    //public interface
    public void Print(string aText)
    {
        foreach (char c in aText)
        {
            if (c == '\n')
            {
                lines++;
            }
        }
        m_InGameLog += aText + "\n";
        lines++;
    }

    public void ClearText()
    {
        m_InGameLog = "";
        lines = 0;
    }
}
