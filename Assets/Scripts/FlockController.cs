using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The flock class to be used to store a flock center, as well as other static data for all flock members.
/// </summary>
public class FlockController : MonoBehaviour {

    public Transform m_Sharky;
	
	/// <summary>
	/// The center of the flock, updated with each call to Update().
	/// </summary>
	private Vector3 m_FlockCenter;
    private Vector3 m_FlockVelocity;

    public float m_CohesionWeight = 1f;
    public float m_CohesionRadius = 1f;
    private float m_sqrCohesionRadius;
    public float m_SeparationWeight = 1f;
    public float m_SeparationRadius = 1f;
    private float m_sqrSeparationRadius;
    public float m_AlignmentWeight = 1f;
    public float m_AlignmentRadius = 1f;
    private float m_sqrAlignmentRadius;
    public float m_AvoidanceWeight = 1f;
    public float m_AvoidanceRadius = 1f;
    public float m_AvoidanceRadiusFactor = 1f;
    public float m_WanderWeight = 1f;
    public float m_DestinationWeight = 1f;

    /// <summary>
    /// All the transforms of the objects in the flock.
    /// </summary>
    public List<Transform> m_Flocklings;
    public List<Transform> flocklings
    {
        get
        {
            return m_Flocklings;
        }
    }

    private static FlockController instance;
    public static FlockController Instance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

	/// <summary>
	/// The flock is composed of all of its children.
	/// </summary>
	void Start () {
        m_Flocklings = new List<Transform>();
		//TODO: flocks should be more dynamic, seeing as objects can join and leave flocks at pretty much any time
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Fishy");  //.GetComponentsInChildren<Transform>();
		foreach(GameObject obj in objs)
		{
			m_Flocklings.Add(obj.transform);
		}

        m_sqrCohesionRadius = m_CohesionRadius * m_CohesionRadius;
        m_sqrSeparationRadius = m_SeparationRadius * m_SeparationRadius;
        m_sqrAlignmentRadius = m_AlignmentRadius * m_AlignmentRadius;
	}
	
	/// <summary>
	/// Updates the flock center based on the transform of all of this flock's children.
	/// </summary>
	void Update () {
        m_AvoidanceRadiusFactor = SharkyControl.Instance().speed;
	}

    public Vector3 GetMemberNetVector(Transform inExtantMember)
    {
        Vector3 net = Vector3.zero;
        Vector3 netcohesion = Vector3.zero;
        int cohesioncount = 0;
        Vector3 netalignment = Vector3.zero;
        int alignmentcount = 0;
        foreach (Transform flockling in m_Flocklings)
        {
            if (flockling != null && flockling != inExtantMember)
            {
                Vector3 cohesion = flockling.position - inExtantMember.position;
                float sqrcohesion = cohesion.sqrMagnitude;
                if (sqrcohesion < m_sqrCohesionRadius)
                {
                    cohesioncount++;
                    netcohesion += cohesion.normalized * Mathf.Sqrt(sqrcohesion) * m_CohesionWeight;
                }
                Vector3 separation = inExtantMember.position - flockling.position;
                float sqrseparation = separation.sqrMagnitude;
                if (sqrseparation < m_sqrSeparationRadius)
                {
                    net += separation.normalized * (m_SeparationRadius - Mathf.Sqrt(sqrseparation)) * m_SeparationWeight;
                }
                Vector3 alignment = flockling.position - inExtantMember.position;
                float sqralignment = alignment.sqrMagnitude;
                if (sqralignment < m_sqrAlignmentRadius)
                {
                    alignmentcount++;
                    netalignment += flockling.rigidbody.velocity.normalized * m_AlignmentWeight;
                }
            }
        }
        Vector3 avoidance = inExtantMember.position - m_Sharky.position;
        float sqravoidance = avoidance.sqrMagnitude;
        if (sqravoidance < Mathf.Pow(m_AvoidanceRadius * m_AvoidanceRadiusFactor, 2f))
        {
            inExtantMember.GetComponent<FishyController>().Scare(10f * Time.deltaTime * m_AvoidanceRadiusFactor);
            net += avoidance.normalized * (m_AvoidanceRadius - Mathf.Sqrt(sqravoidance)) * m_AvoidanceWeight;
            //net += Vector3.Cross(m_Sharky.transform.forward, inExtantMember.transform.forward) * m_AvoidanceWeight / 2f;
        }

        if (cohesioncount > 0)
        {
            net += netcohesion / (float)cohesioncount;
        }
        if (alignmentcount > 0)
        {
            net += netalignment / (float)alignmentcount;
        }

        return net;
    }
	
	public void join(Transform inNewMember)
	{
        m_Flocklings.Add(inNewMember);
		
	}
	
	public void leave(Transform inOutMember)
	{
        m_Flocklings.Remove(inOutMember);
	}
	
	
	/// <summary>
	/// Returns the number of objects in the flock
	/// </summary>
	/// <returns>
	/// int number of objects in flock
	/// </returns>
	internal int numObjsInFlock () {
		return m_Flocklings.Count;
	}
}
