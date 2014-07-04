using UnityEngine;
using System.Collections;
/*
 * How to use:
 * Place script on player.
 * 
 * */

public class ObstructionHandler : MonoBehaviour {
	
	public Transform mainCamera;
	private RaycastHit hit;
	
	Vector3 dirToCamera;
	Vector3 magToCamera;
	int layer;
	int mask;
	bool hittingSomething = false;

	
	void Start(){
		//mainCamera = GameObject.FindWithTag("MainCamera").transform;
		layer = LayerMask.NameToLayer("EdgeDetect");
		Debug.Log (layer);
		mask =  1 << layer;
		Debug.Log (mask);
		//mask = ~mask;
	}
	
	public void MyLateUpdate () {
		//Debug.DrawRay(transform.position, mainCamera.position - transform.position, Color.yellow);

		Vector3 toPlayer = mainCamera.position - transform.position;
		/*
		if(Physics.SphereCast(transform.position, 0.5f, toPlayer.normalized, out hit, Vector3.Magnitude(toPlayer), mask))
		{
			Vector3 hitToCam = mainCamera.position - hit.point;

			Vector3 hitToJelly = transform.position - hit.point;
			float angle = Vector3.Angle(hitToCam, hitToJelly);

				//not going to work because it's not a right angle
			Vector3 moveForward = mainCamera.forward;
			//moveForward.Normalize();
			float forwardDist = hit.distance;
			if(hit.distance < 1.2f)
			{
				forwardDist = 1.2f;
			}
			if(hit.distance > 1.5f)
			{
				forwardDist = 1.5f;
			}
			moveForward *= forwardDist;
			Debug.Log (hit.distance);
			//mainCamera.position += Mathf.Abs (1.5f - hitToCam.magnitude) / 5 * hitToCam;
			mainCamera.position +=  moveForward;
			transform.GetComponent<JellyControllerTwoStick>().SetJellyToCam(mainCamera.position);	
		}
*/
		//Debug.Log ("toplayer" + toPlayer);
		if (Physics.Raycast (transform.position, toPlayer.normalized, out hit, Vector3.Magnitude(toPlayer))) {
			//Vector3 hitToCam = mainCamera.position - hit.point;
			mainCamera.position = hit.point;
		
			//mainCamera.position += hitToCam;
			
			//Debug.Log("I am being called");
			hittingSomething = true;
			/*
			mainCamera.position -= (toPlayer.normalized * 0.15f);

			Vector3 moveAway = mainCamera.position - hit.point;
			moveAway.Normalize();
			mainCamera.position += (moveAway * 1.0f);
			*/
			//Debug.DrawLine(transform.position, mainCamera.position, Color.yellow);
		}
		else
		{
			hittingSomething = false;
		}
	}

	public bool GetHittingSomething()
	{
		return hittingSomething;
	}
}
