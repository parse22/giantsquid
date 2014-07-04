using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kelp : MonoBehaviour {

    public bool m_Attached = true;
    private List<Kelp> m_DownChainKelps;
    private int m_ChainValue;
    private const int m_ChainLength = 10;
    private Color m_InitColor;
    private Color m_CurColor;
    public Color m_BrownColor;
    private Transform m_FruitSpawnSpot;
    private Transform m_MyFruit;
    private bool m_OnCD = false;
    private Transform m_Cam;

	// Use this for initialization
	void Start () {
        m_ChainValue = int.Parse(name);
        rigidbody.drag = m_ChainValue/4f * Random.Range(0.8f, 1.2f);
        rigidbody.mass *= Random.Range(0.6f, 1.5f);

        m_DownChainKelps = new List<Kelp>();

        for(int i = m_ChainLength; i >= m_ChainValue; i--)
        {
            m_DownChainKelps.Add(transform.parent.FindChild((i).ToString()).GetComponent<Kelp>());
        }
        m_InitColor = renderer.material.color;
        m_CurColor = m_InitColor;
        m_FruitSpawnSpot = transform.FindChild("FruitSpawn");
        FruitRespawn();
        m_Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Attached)
        {
            transform.rigidbody.AddForce(Vector3.up * 60f * Time.fixedDeltaTime * 20f);
            if (m_MyFruit == null && !m_OnCD)
            {
                StartCoroutine(TimerUtils.Timer(FruitRespawn, 10f));
                m_OnCD = true;
            }
        }
        float camsqrdist = (m_Cam.position - transform.position).sqrMagnitude;
        if (camsqrdist < 6f)
        {
            renderer.material.color = new Color(m_CurColor.r, m_CurColor.g, m_CurColor.b, m_CurColor.a - (1 - camsqrdist / 6f));
        }
        else
        {
            renderer.material.color = m_CurColor;
        }
	}

    void FruitRespawn()
    {
        GameObject f = (GameObject)Instantiate(Resources.Load("Fruit"));
        m_MyFruit = f.transform;
        m_MyFruit.position = m_FruitSpawnSpot.position;
        m_MyFruit.parent = transform;
        m_MyFruit.localRotation = Quaternion.identity;
        m_OnCD = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Destroy(transform.GetComponent<ConfigurableJoint>());
            foreach (Kelp k in m_DownChainKelps)
            {
                k.m_Attached = false;
                k.rigidbody.drag = 2f;
                k.StartBrowning();
            }
        }
    }

    public void StartBrowning()
    {
        StartCoroutine(BrowningSequence(Time.time));
    }

    IEnumerator BrowningSequence(float inStartTime)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 30f;
            m_CurColor = new Color(Mathf.Lerp(m_InitColor.r, m_BrownColor.r, t), Mathf.Lerp(m_InitColor.g, m_BrownColor.g, t), Mathf.Lerp(m_InitColor.b, m_BrownColor.b, t), Mathf.Lerp(m_InitColor.a, m_BrownColor.a, t));
            yield return 0;
        }
    }
}
