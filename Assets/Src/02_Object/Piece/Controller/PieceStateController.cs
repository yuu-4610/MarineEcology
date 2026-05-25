using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceStateController
{
    /*<責務>パズルピースの状態管理を行い、Stateの変更と参照機能を提供する。
     *
     */
    private PieceState pieceState = PieceState.Idle; //パズルピースの現在状態を保持

    //Stateの変更
    public void ChangeExecutionPieceState(PieceState pieceStateNumber)
    {
        //指定Stateが現在のStateと同じの場合は処理をしない
        if (pieceState == pieceStateNumber)
        {
            return;
        }
        // && pieceStateNumber != PieceState.Follow
        pieceState = pieceStateNumber;
    }
    //現在のパズルピースの状態を返す
    public PieceState GetPieceState()
    {
        return pieceState;
    }
}
