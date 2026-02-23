using System.Collections;
using UnityEngine;

public class PlayerInput
{
    public int OptionButton()
    {
        var shiftButtonValue = Input.GetKeyDown(KeyCode.LeftShift) ? 1 : Input.GetKeyDown(KeyCode.RightShift) ? 1 : 0;

        return shiftButtonValue;
    }
    public int ObjectDropInput()
    {
        var mouseButtonValue = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

        return mouseButtonValue;
    }
    public float MousePositionValue()
    {
        //カーソル位置（x）を取得
        Vector2 mousePosition = Input.mousePosition;
        Vector2 target = Camera.main.ScreenToWorldPoint(mousePosition);

        return target.x;
    }
}