using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        GameObject p = (GameObject)Instantiate(Resources.Load("AttackParticleEffect"), transform.position, Quaternion.identity);
        p.transform.parent = GameObject.FindGameObjectWithTag("ParticleEffectsParent").transform;
        Destroy(gameObject);
    }
}
