using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveHandler : IPieceMove
{
    /*<責務>プレイヤーを追従するために必要な命令クラス・または処理クラスを呼ぶ
     */
    private PieceMoveAction pieceMoveAction;
    private PieceStateHandler pieceStateHandler;
    private PieceViewHandler pieceViewHandler;
    private const PieceState pieceStateFollow = PieceState.Follow;
    // Start is called before the first frame update
    
    public PieceMoveHandler(PieceMoveAction pieceMoveAction, PieceStateHandler pieceStateHandler, PieceViewHandler pieceViewHandler)
    {
        this.pieceMoveAction = pieceMoveAction;
        this.pieceStateHandler = pieceStateHandler;
        this.pieceViewHandler = pieceViewHandler;
    }

    // Update is called once per frame
    public void Execute()
    {
        pieceStateHandler.Execute((int)pieceStateFollow);
        pieceViewHandler.ChangeView((int)pieceStateFollow);
        pieceMoveAction.TargetPlayerFollow();
    }
}
