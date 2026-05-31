using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PieceSynthesisHandler : IPieceMove
{
    /*<責務>パズルピースの衝突処理命令
     */
    private PieceSynthesisAction pieceSynthesisAction;
    private PieceStateHandler pieceStateHandler;
    private const PieceState pieceStateSynthesisProcess = PieceState.Synthesis;
    public PieceSynthesisHandler(PieceSynthesisAction pieceSynthesisAction, PieceStateHandler pieceStateHandler)
    {
        this.pieceStateHandler = pieceStateHandler;
        this.pieceSynthesisAction = pieceSynthesisAction;
    }
    
    public void Execute()
    {
        //ステートの更新
        pieceStateHandler.Execute(pieceStateSynthesisProcess);

        //衝突時処理
        pieceSynthesisAction.PieceSynthesis();
    }
}
