using UnityEngine;
using System.Collections;

public class TrailController : MonoBehaviour {
    TrailRenderer trail;
	// Use this for initialization
	void Start () {
        trail = transform.GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        trail.time = Mathf.Clamp(SharkyControl.Instance().speed/4f - 1f, 0f, 1f);
	}
}
