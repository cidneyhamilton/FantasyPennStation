using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
		public static void Log(string message)
    {
        #if UNITY_EDITOR
        Debug.Log(message);
        #endif
    }
    public static void LogWarning(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }
    public static void LogError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }
}
