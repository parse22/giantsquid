using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Current : MonoBehaviour {

	protected Vector3 ForceDirectionInit;
	protected List<Transform> ContainedActors;
	public List<Transform> BlockingObstacles;
	public bool DrawDebugCubes = false;
	public int xsample = 10;
	public int ysample = 10;
	public bool PlayerInCurrent = false;
	int sampleTot;
	public Vector3 CustomForceVector;
	protected Vector3 DebugForceVector;
	public float Magnitude = 5f;
	public float[] zbuf;
	protected double lastUpdate = 0;
	Vector3 force;
	public Vector3 jellyInVector;
	public Vector3 jellyInPos;

	protected void Start () {
		sampleTot = xsample * ysample;
		if(CustomForceVector.magnitude == 0)
		{
			ForceDirectionInit = transform.forward;
		}
		else
		{
			ForceDirectionInit = transform.TransformPoint(CustomForceVector) - transform.position;
			DebugForceVector = transform.TransformPoint(CustomForceVector);
			ForceDirectionInit.Normalize();
		}
		force = ForceDirectionInit * Magnitude *  30f;
		zbuf = new float[sampleTot];
	}

	protected virtual void Awake()
	{
		ContainedActors = new List<Transform>();
		BlockingObstacles = new List<Transform>();
	}

	protected virtual void Update () {
		PlayerInCurrent = false;
		if(Time.time > lastUpdate + 1)
		{
			//Debug.Log (lastUpdate);			
        	UpdateZbuf();
			lastUpdate = Time.time;
		}
		UpdateCurrentVector();
        ApplyForces();
	}
	
	protected void OnTriggerEnter(Collider col)
    {
//        Debug.LogError("Current : OnTriggerEnter : " + col.name + " : " + col.tag);
        if (col.tag != "obstacle" && (!ContainedActors.Contains(col.transform)))
        {
            ContainedActors.Add(col.transform);

			//if (col.CompareTag("Player") && transform.GetComponent<FlowCurrent>() == null ) { //&& PlayerInCurrent){
            if(col.gameObject.tag.ToString() == "Player")// && transform.GetComponent<FlowCurrent>() == null && PlayerInCurrent)
            {
				jellyInVector = col.transform.rigidbody.velocity;
				jellyInPos = col.transform.position;
			}

        }
	}

	protected void OnTriggerStay(Collider col) //only for blocking current
	{
		if(col.tag == "obstacle")
		{
			if(!BlockingObstacles.Contains(col.transform))
			{	
				BlockingObstacles.Add(col.transform);
			}
		}
//		else if (col.CompareTag("Player")){
//			//if(!PlayerInCurrent){
//				FMODManager.Get ().Stop("Immortal_CurrentSound");
//			//}

//		}

		                       
	}


	protected int GetIndex(float x, float y)
	{
		float nx, ny;
		nx = x + 0.5f;
		ny = y + 0.5f;
		int i = (int) (nx*10);
		int j = (int) (ny*10);
		return i * ysample + j;
		
	}
	protected virtual void UpdateZbuf()
	{
		for(int i = 0; i < (sampleTot); i++)
		{
			zbuf[i] = 0.5f;
		}		
		
		
		if(BlockingObstacles.Count > 0)
		{
			float mult = 1f/xsample;
			float zMult = (1f/(50-1));
			//Debug.Log ("DDDD");
			Vector3 localPos = new Vector3(-0.5f, -0.5f, -0.5f);
			for(int x = 0; x < xsample; x++)
			{
				for(int y = 0; y < ysample; y++)
				{
					bool blocked = false;
					for(int z = 0; z < 50; z++)
					{
						if(blocked)
							break;
							
						localPos = new Vector3(-0.5f + x*mult, -0.5f + y*mult, -0.5f + z*zMult);
						Vector3 wpos = transform.TransformPoint(localPos);
						foreach(Transform t in BlockingObstacles)
						{
							//Debug.Log(localPos);
							//DrawHelper.DrawCube(wpos, Vector3.one/2, Color.red);
							if(t.collider.bounds.Contains(wpos))
							{	
								zbuf[x * ysample + y] = localPos.z;
								blocked = true;
							}
							
							
						}
					}
				
				}
			}
		}
	}
	
	
	protected void OnTriggerExit(Collider col)
	{
//		if (col.CompareTag("Player") && transform.GetComponent<FlowCurrent>() == null && PlayerInCurrent){
//			playerAudio.playerInCurrent --; //supa - hacky
//			if (playerAudio.playerInCurrent == 0)
//				FMODManager.Get().setParam("Immortal_CurrentSound", "Current Status", 1, 1);
//		}
		ContainedActors.Remove(col.transform);
	}

    protected virtual void UpdateCurrentVector()
    {
        //Not implemented in basic current, this version exhibits constant force
    }

    protected virtual void ApplyForces()
    {
		if(DrawDebugCubes && CustomForceVector.magnitude != 0)
		{
			Debug.DrawLine(transform.position, DebugForceVector, Color.yellow);
		}
        for (int i = 0; i < ContainedActors.Count; i++)
        {
			
            Transform obj = ContainedActors[i];
            if (obj)
            {
                Vector3 localPos = transform.InverseTransformPoint(obj.position);
                //float localZ = Mathf.Max(0f, 0.5f - localPos.z);
				if (localPos.x > 0.5f)
					localPos.x = 0.5f;
				else if (localPos.x < -0.5f)
					localPos.x = -0.5f;
				if (localPos.y > 0.5f)
					localPos.y = 0.5f;
				else if (localPos.y < -0.5f)
					localPos.y = -0.5f;
				if (localPos.z > 0.5f)
					localPos.z = 0.5f;
				else if (localPos.z < -0.5f)
					localPos.z = -0.5f;




				int index = GetIndex(localPos.x, localPos.y);
				//Debug.Log (index + " " + localPos.x + " " + localPos.y);
                
				if(localPos.z < zbuf[index])
				{
                    //Debug.LogError("Current : ApplyForces() : obj.tag = " + obj.tag);
						if(obj.tag == "Player")
						{
							PlayerInCurrent = true;
						}
						obj.rigidbody.AddForce(force * Time.deltaTime);

						Quaternion ending = Quaternion.LookRotation(transform.up*-1, transform.forward);                
						obj.rotation = Quaternion.RotateTowards(obj.rotation, ending, 60f*Time.deltaTime);
				}
			}
            else
            {
                ContainedActors.RemoveAt(i);
            }
        }
    }
}
