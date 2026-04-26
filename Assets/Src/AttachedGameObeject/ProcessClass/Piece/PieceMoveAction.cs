using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveAction : MonoBehaviour
{
    /*<責務>Pieceオブジェクト（パズルピース）の移動処理を担う
     *１．MonoBehaviour継承クラス → 位置情報の更新で値を使用しているため（設定値）＝Pieceクラスの責務が増える
     *メリット：責務の重複を防ぐ
     *デメリット：Pieceオブジェクトの依存性の向上（このクラスを保持しているため）
     */
    [Header("プレイヤーの位置情報(Transform)")]
    [SerializeField] Transform playerTransform;
    [Header("プレイヤーから離れている距離")]
    [SerializeField] float removePiecePosition;

    private float initializePositionY;

    private void Start()
    {
        initializePositionY = transform.position.y;
    }

    public void TargetPlayerFollow()
    {
        var piecePoint = new Vector2(playerTransform.position.x, initializePositionY- removePiecePosition);
        this.transform.position = piecePoint;
        this.transform.position = playerTransform.position;
    }
}
