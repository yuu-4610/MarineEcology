using System.Collections;
using UnityEngine;

public class PlayerInput
{
    /*<責務>プレイヤーの入力受け付とり値に変換する
     */
    public static int OptionButton()
    {
        //オプションボタンの押下
        var shiftButtonValue = Input.GetKeyDown(KeyCode.LeftShift) ? 1 : Input.GetKeyDown(KeyCode.RightShift) ? 1 : 0;

        return shiftButtonValue;
    }

    //Spaceキー入力受け付け
    public static int PieceObjectDropInput()
    {
        var mouseButtonValue = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

        return mouseButtonValue;
    }

    //マウス操作入力受け付け
    public static float MousePositionValue()
    {
        //カーソル位置（x）を取得
        Vector2 mousePosition = Input.mousePosition;
        Vector2 target = Camera.main.ScreenToWorldPoint(mousePosition);

        return target.x;
    }
}