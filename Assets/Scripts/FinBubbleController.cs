using UnityEngine;
using System.Collections;

public class FinBubbleController : MonoBehaviour {
    bool triggered = false;
    ParticleSystem bubbles;
	// Use this for initialization
	void Start () {
        bubbles = particleSystem;
	}
	
	// Update is called once per frame
	void Update () {
        if (!triggered)
        {
            if (SharkyControl.Instance().speed > 4f)
            {
                triggered = true;
                bubbles.Play();
            }
        }
        else if(SharkyControl.Instance().speed < 1.1f)
        {
            triggered = false;
            bubbles.Stop();
        }
	}
}
