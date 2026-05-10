using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFollowAction : MonoBehaviour
{
    /*<責務>Pieceオブジェクト（パズルピース）の移動処理を担う
     *１．MonoBehaviour継承クラス → 位置情報の更新で値を使用しているため（設定値）＝Pieceクラスの責務が増える
     *メリット：責務の重複を防ぐ
     *デメリット：Pieceオブジェクトの依存性の向上（このクラスを保持しているため）
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
