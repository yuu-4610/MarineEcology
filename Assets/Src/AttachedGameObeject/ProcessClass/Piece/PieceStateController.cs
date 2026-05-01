using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceStateController
{
    /*<責務>パズルピースの状態を管理するクラス（取得・返す）
     *M：単一責務として成り立っている（PieceStateの状態管理のみ
     *D：
     */
    private PieceState pieceState = PieceState.Idle; //パズルピースの現在状態を保持
    //private bool isDropObject = false; //落下処理を行うオブジェクトか（＝プレイヤーに追従するオブジェクトか）

    public void ChangeExecutionPieceState(PieceState pieceStateNumber)
    {
        //状態変化に伴い、状態ごとの処理も行う
        if (pieceState == pieceStateNumber)
        {
            Debug.Log("同じ状態になろうとしている");
            return;
        }
        pieceState = pieceStateNumber;
        //PieceStateProcess(pieceState);
    }
    //現在のパズルピースの状態を返す
    public PieceState GetPieceState()
    {
        return pieceState;
    }
}
