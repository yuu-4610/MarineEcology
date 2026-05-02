using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDieHandler : IPieceMove
{
    private PieceStateHandler pieceStateHandler;
    private PieceDieAction pieceDieAction;
    private const PieceState piecestate = PieceState.Die;
    public PieceDieHandler(PieceStateHandler pieceStateHandler, PieceDieAction pieceDieAction)
    {
        this.pieceStateHandler = pieceStateHandler;
        this.pieceDieAction = pieceDieAction;
    }
    public void Execute()
    {
        /*オブジェクトの状態を「破壊された」状態にする
         *ー＞第３のオブジェクトと衝突したときに処理を走らせないようにする
        */
        pieceStateHandler.Execute((int)piecestate);
        pieceDieAction.PieceSyntghesisConditionsLaterDie();
    }
}
