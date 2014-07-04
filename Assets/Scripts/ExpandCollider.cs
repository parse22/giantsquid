using UnityEngine;
using System.Collections;

public class ExpandCollider : MonoBehaviour {
    Object bubbles;
    Transform particleParent;
	// Use this for initialization
	void Start () {
	    bubbles = Resources.Load("BubbleSpout");
        particleParent = GameObject.Find("ParticleEffects").transform;
	}
	
	// Update is called once per frame
	void Update () {
        float s = Mathf.Clamp(SharkyControl.Instance().speed, 1f, 6f);
        transform.localScale = new Vector3(s, s, s);
	}

    void OnCollisionEnter(Collision other)
    {
        GameObject go = (GameObject)Instantiate(bubbles, other.contacts[0].point, Quaternion.identity);
        go.transform.parent = particleParent;
    }
}
