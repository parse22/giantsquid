using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        FlockController.Instance().leave(transform);
        RenderSettings.fogColor = new Color(Mathf.Min(RenderSettings.fogColor.r + 0.05f, 0.5f), RenderSettings.fogColor.g - 0.05f, RenderSettings.fogColor.b - 0.05f);
        RenderSettings.fogDensity *= 1.1f;
        RenderSettings.fogDensity = Mathf.Min(RenderSettings.fogDensity, 0.15f);
        Camera c = GameObject.FindGameObjectWithTag("MainCamera").camera;
        c.backgroundColor = new Color(Mathf.Min(c.backgroundColor.r + 0.05f, 0.5f), c.backgroundColor.g - 0.05f, c.backgroundColor.b - 0.05f);
        Transform p = GameObject.FindGameObjectWithTag("ParticleEffectsParent").transform;
        GameObject blood = (GameObject)Instantiate(Resources.Load("BloodSpout"), transform.position, Quaternion.identity);
        GameObject bits = (GameObject)Instantiate(Resources.Load("MegaAttackParticleEffect"), transform.position, Quaternion.identity);
        blood.transform.parent = p;
        bits.transform.parent = p;
        Destroy(transform.parent.gameObject);
    }
}
