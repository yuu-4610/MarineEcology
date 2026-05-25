using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceStateHandler
{
    /*<責務>パズルピースのStateの変更と取得の命令クラス
     */
    private PieceStateController pieceStateController;
    
    public PieceStateHandler(PieceStateController pieceStateController)
    {
        this.pieceStateController = pieceStateController;
    }
    //Stateの変更
    public void Execute(PieceState pieceState)
    {
        pieceStateController.ChangeExecutionPieceState(pieceState);
    }
    //現在のStateを取得
    public int IsDesignationState()
    {
        return (int)pieceStateController.GetPieceState();
    }
}
