using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PieceSynthesisHandler : IPieceMove
{
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
        pieceStateHandler.Execute((int)pieceStateSynthesisProcess);

        pieceSynthesisAction.PieceSynthesis();
    }
}
