using UnityEngine;
using System.Collections;

public class AttackHitboxController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SharkyControl.Instance().Bite += new ActionEventHandler(HandleBite);
        collider.enabled = false;
	}

    private void HandleBite(object sender)
    {
        StartCoroutine(TimerUtils.Timer(StartBite, 0.5f));
    }

    private void StartBite()
    {
        collider.enabled = true;
        StartCoroutine(TimerUtils.Timer(FinishBite, 0.25f));
    }

    private void FinishBite()
    {
        collider.enabled = false;
    }
}
