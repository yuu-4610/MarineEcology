using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathHelper
{
    /*<Ó–±>Žw’è‚³‚ê‚½’l‚ð‘Î‰ž‚·‚é•¶Žš—ñ‚É•ÏŠ·‚·‚é
     */
    public static string ToName(ResourcePath pathName)
    {
        switch (pathName)
        {
            case ResourcePath.BGM: return "Audio/BGM";
            case ResourcePath.SE: return "Audio/SE";
            default: return "";
        }
    }
}
