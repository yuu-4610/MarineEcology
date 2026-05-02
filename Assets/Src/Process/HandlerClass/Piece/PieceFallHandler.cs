using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFallHandler : IPieceMove
{
    /*<責務>パズルピースが落とされた時に必要な命令クラス・または処理クラスを呼ぶ
     */
    private PieceStateHandler pieceStateHandler;
    private PieceViewHandler pieceViewHandler;
    private const PieceState piecestateDrop = PieceState.Drop;

    public PieceFallHandler(PieceStateHandler pieceStateHandler, PieceViewHandler pieceViewHandler)
    {
        this.pieceStateHandler = pieceStateHandler;
        this.pieceViewHandler = pieceViewHandler;
    }
    public void Execute()
    {
        //状態変化クラス
        pieceStateHandler.Execute((int)piecestateDrop);

        pieceViewHandler.ChangeView((int)piecestateDrop);
        //落下処理クラス
        //状態変化クラスで当たり判定をアクティブ化かつPieceクラスでアクティブなら追従しないとしているため、特段落下処理がない・・・
    }
}

