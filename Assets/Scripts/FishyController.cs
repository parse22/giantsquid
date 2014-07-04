using UnityEngine;
using System.Collections;

public class FishyController : MonoBehaviour {

    public Transform m_KelpRegion;

    protected Vector3 m_NewHeading;
    protected float m_MaxHeadingChange = 45f;

    private float m_Fear = 0f;
    private float m_FearBreak = 15f;

    public enum FishyState { None, Wander, Migrate };
    public FishyState m_FishyState = FishyState.None;

    private const float m_KelpThreshold = 4f;

    void Start()
    {
        DefineHeading();
        InvokeRepeating("CheckSurroundings", Random.Range(2f, 6f), 5f);
    }

    void Update()
    {
        m_Fear -= Time.deltaTime;
        m_Fear = Mathf.Clamp(m_Fear, 0f, 30f);

        switch (m_FishyState)
        {
            case FishyState.None:
                if (SpacialUtils.ArrivalCheck(transform, m_KelpRegion.position, m_KelpThreshold))
                {
                    KelpsReached();
                }
                else
                {
                    FindKelps();
                }
                break;
            case FishyState.Wander:
                DoFlockWander();
                break;
            case FishyState.Migrate:
                DoFlockToKelps();
                break;
            default:
                Debug.Log("m_FishyState is bad...");
                break;
        }
    }

    private void DoFlockToKelps()
    {
        if (SpacialUtils.ArrivalCheck(transform, m_KelpRegion.position, m_KelpThreshold))
        {
            KelpsReached();
        }
        else
        {
            DefineFlockToPosition(m_KelpRegion.position);
        }
    }

    private void DoFlockWander()
    {
        Vector3 flocknet = FlockController.Instance().GetMemberNetVector(transform);
        flocknet += m_NewHeading.normalized * FlockController.Instance().m_WanderWeight;
        
        Vector3 flockDest = flocknet + transform.position;
        flockDest = new Vector3(flockDest.x, Mathf.Clamp(flockDest.y, 1f, 20f), flockDest.z);

        Debug.DrawLine(transform.position, m_NewHeading.normalized + transform.position, Color.green);
        Debug.DrawLine(transform.position, flockDest, Color.red);

        DefineMove(flockDest, (Afraid()) ? 2.5f : 1f);
    }

    private void DefineFlockToPosition(Vector3 myDestination)
    {
        Vector3 flocknet = FlockController.Instance().GetMemberNetVector(transform);
        flocknet += (myDestination - transform.position).normalized * FlockController.Instance().m_DestinationWeight;
        flocknet += m_NewHeading.normalized * FlockController.Instance().m_WanderWeight;
        
        Vector3 flockDest = flocknet + transform.position;
        flockDest = new Vector3(flockDest.x, Mathf.Clamp(flockDest.y, 1f, 20f), flockDest.z);

        Debug.DrawLine(transform.position, m_NewHeading.normalized + transform.position, Color.green);
        Debug.DrawLine(transform.position, flockDest, Color.red);
        Debug.DrawLine(transform.position, m_KelpRegion.position);

        DefineMove(flockDest, (Afraid()) ? 2.5f : 1f);
    }

    private void CheckSurroundings()
    {
        if (SpacialUtils.ArrivalCheck(transform, m_KelpRegion.position, m_KelpThreshold))
        {
            KelpsReached();
        }
        else
        {
            FindKelps();
        }
    }

    private void KelpsReached()
    {
        m_FishyState = FishyState.Wander;
    }

    private void FindKelps()
    {
        m_FishyState = FishyState.Migrate;
    }

    private void DoRotate(Vector3 myDestination)
    {
        Vector3 direction = myDestination - transform.position;
        Quaternion starting = transform.rotation;
        Quaternion ending = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(starting, ending, Time.deltaTime * 80f);
    }

    private void DefineMove(Vector3 myDestination, float inSpeedModifier = 1f)
    {
        DoRotate(myDestination);

        rigidbody.AddForce(transform.forward * 50f * 60f * Time.deltaTime * inSpeedModifier);
        rigidbody.AddForce(-10f * rigidbody.velocity * 60f * Time.deltaTime);
        rigidbody.AddForce(-25f * rigidbody.velocity.normalized * 60f * Time.deltaTime);
    }

    public void Scare(float amount)
    {
        m_Fear += amount;
    }

    public bool Afraid()
    {
        return m_Fear >= m_FearBreak;
    }

    private void DefineHeading()
    {
        m_NewHeading = transform.forward;
        StartCoroutine(DefineHeadingRoutine(UnityEngine.Random.Range(0.5f, 2f)));
    }

    private IEnumerator DefineHeadingRoutine(float durationScale)
    {
        m_NewHeading = (Quaternion.AngleAxis(UnityEngine.Random.Range(-m_MaxHeadingChange, m_MaxHeadingChange), transform.up) * m_NewHeading);
        m_NewHeading = (Quaternion.AngleAxis(UnityEngine.Random.Range(-m_MaxHeadingChange, m_MaxHeadingChange), transform.right) * m_NewHeading);

        yield return new WaitForSeconds(durationScale * 1f);
        DefineHeading();
    }
}
