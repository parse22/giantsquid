using UnityEngine;
using System.Collections;

public class MoveInCircle : MonoBehaviour
{

    // Move target around circle with tangential speed 
    public float tangentialSpeed;	// m/s
    public float circumference; 	// m
    public float targetRadius; 		// m
    public float period; 			// s
    public float angularSpeed; 		// rad/s
    public float currentAngle; 		// rad/s

    // Use this for initialization
    void Start()
    {
        tangentialSpeed = 6f;
        circumference = 600f;
        targetRadius = circumference / (2 * Mathf.PI);
        period = circumference / tangentialSpeed;
        angularSpeed = 2 * Mathf.PI / period;
        currentAngle = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        transform.localPosition = targetRadius * (
            new Vector3(Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle)));

        //		rigidbody.velocity = new Vector3(Mathf.Cos(currentDeg), 0, -Mathf.Sin(currentDeg));

        currentAngle += angularSpeed * Time.deltaTime;

        if (currentAngle > 2 * Mathf.PI)
        {
            currentAngle = currentAngle - 2 * Mathf.PI;
        }
    }
}