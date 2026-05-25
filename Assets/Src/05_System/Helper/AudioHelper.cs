using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    /*<責務>指定された値を対応する文字列に変換する
     */
    public static string ToName(AudioFileName audioFileName)
    {
        switch (audioFileName)
        {
            case AudioFileName.kaityusekai: return "海中世界_backGround";
            case AudioFileName.tokonatunoumi: return "常夏の海_backGround";
            case AudioFileName.onoma: return "Onoma";
            case AudioFileName.whistle: return "警官のホイッスル2";
            case AudioFileName.moveCursor: return "カーソル移動12";
            case AudioFileName.buttonClick: return "決定ボタンを押す29";
            case AudioFileName.turnThePage: return "ページをめくる1";

            default: return "登録されていません";
        }
    }
}
