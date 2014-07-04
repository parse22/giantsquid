using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
	public float timeUntilSelfDestruct;
	public VoidDelegate Cleanup;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(DoSelfDestruct());
	}
	
	private IEnumerator DoSelfDestruct()
	{
		yield return new WaitForSeconds(timeUntilSelfDestruct);
		if(Cleanup != null)
		{
			Cleanup();
		}
		Destroy(this.gameObject);
	}
}
