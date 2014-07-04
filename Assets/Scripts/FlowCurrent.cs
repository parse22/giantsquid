using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowCurrent : Current {
	
	public float CurrentPower;
	public float Wavelength = 8f;
	private List<Transform> RemovalList;
	
	public float secondaryVectorMagnitude = 6f;
	public float secondaryVectorWavelength = 20f;
	public float secondaryPower;
	
	public float tertVectorMagnitude = 6f;
	public float tertVectorWavelength = 20f;
	public float tertPower;
	public float radMult;
	public float secondaryRadMult;
	public float tertRadMult;

	protected override void Awake()
	{
		radMult = Mathf.PI/(Wavelength / 2);
		secondaryRadMult = Mathf.PI/(secondaryVectorWavelength / 2);
		tertRadMult =  Mathf.PI/(tertVectorWavelength / 2);
		RemovalList = new List<Transform>();
		base.Awake();
	}
    protected override void UpdateCurrentVector()
    {
        float CycleProgress = Time.time % Wavelength;
        float rad = (Wavelength - CycleProgress) * radMult;
        CurrentPower = Magnitude * Mathf.Sin(rad);
		
		CycleProgress = Time.time % secondaryVectorWavelength;
		rad = (secondaryVectorWavelength - CycleProgress) * secondaryRadMult;
		secondaryPower = secondaryVectorMagnitude * Mathf.Sin(rad);
		
		CycleProgress = Time.time % tertVectorWavelength;
		rad = (tertVectorWavelength - CycleProgress) * tertRadMult;
		tertPower = tertVectorMagnitude * Mathf.Sin(rad);
    }

    protected override void ApplyForces()
    {
		Vector3 flowForce = ((ForceDirectionInit * CurrentPower) + (transform.right * secondaryPower) + (ForceDirectionInit * tertPower)) * 60f * Time.deltaTime;
        foreach (Transform ContainedActor in ContainedActors)
        {
            if (ContainedActor)
            {
				if(ContainedActor.CompareTag("Player")){
					ContainedActor.rigidbody.AddForce(flowForce*.25f);
				}
				else{
                	ContainedActor.rigidbody.AddForce(flowForce);
				}
            }
            else
            {
				RemovalList.Add(ContainedActor);
            }
        }
		foreach(Transform ContainedActor in RemovalList)
		{
			ContainedActors.Remove(ContainedActor);
		}
		RemovalList.Clear();
    }
}
