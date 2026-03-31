using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathHelper
{
    
    public static string ToName(ResourcePath pathName)
    {
        switch (pathName)
        {
            case ResourcePath.BGM: return "Audio/BGM";
            case ResourcePath.SE: return "Audio/BGM";
            default: return "";
        }
    }
}
