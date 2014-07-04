//#define PRINT
//#define WARNING
#define ERROR

using System;
using UnityEngine;
using System.Diagnostics;

public class DebugUtils
{
	public static void Assert(bool condition, string text = "")
	{
		if (!condition) throw new Exception("Assert failure: " + text);
	}

    [Conditional("PRINT")]
    public static void Print(string text)
    {
        UnityEngine.Debug.Log(text);
    }

    [Conditional("WARNING")]
    public static void Warning(string text)
    {
        UnityEngine.Debug.LogWarning(text);
    }

    [Conditional("ERROR")]
    public static void Error(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("ERROR")]
    public static void Error(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }
}