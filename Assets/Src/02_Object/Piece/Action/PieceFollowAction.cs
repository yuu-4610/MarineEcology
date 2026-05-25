using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFollowAction : MonoBehaviour
{
    /*<責務>プレイヤーを追従するパズルピースの移動を制御する
     *
     */

    private float leavePiecePosition;
    private Transform playerTransform;

    private float initializePositionY;

    private void Start()
    {
        
    }

    public void TargetPlayerFollow()
    {
        var piecePoint = new Vector2(playerTransform.position.x, initializePositionY - leavePiecePosition);
        this.transform.position = piecePoint;
        //this.transform.position = playerTransform.position;
    }

    public void SetLeavePiecePosition(float leavePiecePosition)
    {
        this.leavePiecePosition = leavePiecePosition;
    }
    public void SetPlayerTransformInfomation(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        initializePositionY = this.playerTransform.position.y;
    }
}
