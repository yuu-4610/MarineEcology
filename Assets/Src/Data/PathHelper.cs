using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathHelper
{
    
    public static string ToName(ResourcePath pathName)
    {
        switch (pathName)
        {
            case ResourcePath.BGM: return $"Audio/{ResourcePath.BGM.ToString()}";
            case ResourcePath.SE: return $"Audio/{ResourcePath.SE.ToString()}";
            default: return "";
        }
    }
}
