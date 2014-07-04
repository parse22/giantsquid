using UnityEngine;
using System.Collections;

public class SpacialUtils
{
    /// <summary>
    /// A helper function that calculates whether or not the target is in range of its destination.
    /// </summary>
    /// <param name='myTransform'>
    /// Local transform of the caller.
    /// </param>
    /// <param name='target'>
    /// Target transform.
    /// </param>
    /// <param name='arrivalThreshold'>
    /// Threshold distance fromVector3 target toVector3 trigger arrival.
    /// </param>
    public static bool ArrivalCheck(Transform myTransform, Transform target, float arrivalThreshold)
    {
        if ((myTransform.position - target.position).sqrMagnitude < arrivalThreshold)
        {
			DebugUtils.Print("Arrival at target.");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Calculates the approx distance from your target
    /// </summary>
    /// <param name="maxSpeed">Top speed of gameobject</param>
    /// <param name="approxDistance">current running approxdistance</param>
    /// <returns>new approxdistance</returns>
    public static float ApproxCheck(float maxSpeed, float approxDistance)
    {
        approxDistance = approxDistance - maxSpeed * Time.deltaTime;
        return approxDistance;
    }
    /// <summary>
    /// Arrival Check that returns a float used for approxDistance
    /// </summary>
    /// <param name="myTransform">Local Transform of the caller</param>
    /// <param name="approxDistance">Approximate distance from target</param>
    /// <param name="target">Your target Transform</param>
    /// <returns>The new distance using sqr.magnitude</returns>
    public static float ArrivalCheck(Transform myTransform, float approxDistance, Transform target)
    {
        approxDistance = (myTransform.position - target.position).sqrMagnitude;
        return approxDistance;
    }
    /// <summary>
    /// Arrival Check that returns a float used for approxDistance 
    /// </summary>
    /// <param name="myTransform">Local Transform of the caller</param>
    /// <param name="approxDistance">Approximate distance from target</param>
    /// <param name="destination">Your target Transform</param>
    /// <returns>The new distance using sqr.magnitude</returns>
    public static float ArrivalCheck(Transform myTransform, float approxDistance, Vector3 destination)
    {
        approxDistance = (myTransform.position - destination).sqrMagnitude;
        return approxDistance;
    }

    /// <summary>
    /// A helper function that calculates whether or not the target is in range of its destination.
    /// </summary>
    /// <param name='myTransform'>
    /// Local transform of the caller.
    /// </param>
    /// <param name='destination'>
    /// Destination coordinate.
    /// </param>
    /// <param name='arrivalThreshold'>
    /// Threshold distance fromVector3 destination toVector3 trigger arrival.
    /// </param>
    public static bool ArrivalCheck(Transform myTransform, Vector3 destination, float arrivalThreshold)
    {
        if ((myTransform.position - destination).sqrMagnitude < arrivalThreshold * arrivalThreshold)
        {
			DebugUtils.Print("Arrival at destination.");
            return true;
        }
        return false;
    }

    /// <summary>
    /// A helper function that calculates the rectilinear distance between two points. Faster and significantly less accurate than euclidean distance.
    /// </summary>
    /// <param name="toVector3"></param>
    /// End point.
    /// <param name="fromVector3"></param>
    /// Start point.
    /// <returns></returns>
    public static float ManhattanDistance(Vector3 toVector3, Vector3 fromVector3)
    {
        return Mathf.Abs(toVector3.x - fromVector3.x + toVector3.y - fromVector3.y + toVector3.z - fromVector3.z);
    }

    /// <summary>
    /// A helper function that calculates the rectilinear distance between two points. Faster and significantly less accurate than euclidean distance.
    /// </summary>
    /// <param name="toTransform"></param>
    /// End point.
    /// <param name="fromTransform"></param>
    /// Start point.
    /// <returns></returns>
    public static float ManhattanDistance(Transform toTransform, Transform fromTransform)
    {
        Vector3 to = toTransform.position, from = fromTransform.position;
        return Mathf.Abs(to.x - from.x + to.y - from.y + to.z - from.z);
    }
}
