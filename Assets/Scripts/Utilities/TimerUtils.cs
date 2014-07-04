using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerUtils
{
    public static IEnumerator WaitOneFrame(VoidDelegate callback)
    {
        yield return 0;
        callback();
    }

    public static IEnumerator Timer(VoidDelegate callback, float duration)
    {      
        yield return new WaitForSeconds(duration);
        callback();
    }
}
