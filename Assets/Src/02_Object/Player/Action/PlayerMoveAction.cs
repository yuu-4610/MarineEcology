using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveAction
{
    /*<責務>プレイヤーの移動処理
     */
    private Transform playerTransform;
    private float moveRangeValues; //行動範囲値
    private float initialPositionX; //初期位置

    public PlayerMoveAction(Transform playerTransform, float initialPositionX, float moveRangeValues)
    {
        this.playerTransform = playerTransform;
        this.initialPositionX = initialPositionX;
        this.moveRangeValues = moveRangeValues;
    }

    //プレイヤーの移動処理
    public void PlayerMove(float inputMousePositionXValue)
    {
        //左右の行動処理
        var mousePoint = new Vector3(inputMousePositionXValue, playerTransform.position.y, Mathf.Abs(Camera.main.transform.position.z));
        //オブジェクトの行動範囲（ intializeXPosition +-moveRangeValues ）を制限
        mousePoint.x = Mathf.Clamp(mousePoint.x, this.initialPositionX - moveRangeValues, this.initialPositionX + moveRangeValues);
        playerTransform.position = mousePoint;
    }
}
