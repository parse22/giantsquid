using UnityEngine;
using System.Collections;

public class PlayerDebugWidget : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        DebugGUI.Instance().ClearText();
        DebugGUI.Instance().Print("Speed = " + SharkyControl.Instance().speed);
        DebugGUI.Instance().Print("IsSprinting = " + SharkyControl.Instance().sprinting.ToString());
        DebugGUI.Instance().Print("TurnAmt = " + SharkyControl.Instance().turnamt);
        DebugGUI.Instance().Print("VertSpeed = " + SharkyControl.Instance().vertspeed);
	}
}
