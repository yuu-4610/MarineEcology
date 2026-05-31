using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFallHandler : IPieceMove
{
    /*<責務>パズルピースが落とされた時に必要な命令クラス・または処理クラスを呼ぶ
     */
    private PieceFallAction pieceFallAction;
    private PieceStateHandler pieceStateHandler;
    private PieceViewHandler pieceViewHandler;
    private const PieceState piecestateFall = PieceState.Fall;

    public PieceFallHandler(PieceFallAction pieceFallAction, PieceStateHandler pieceStateHandler, PieceViewHandler pieceViewHandler)
    {
        this.pieceFallAction = pieceFallAction;
        this.pieceStateHandler = pieceStateHandler;
        this.pieceViewHandler = pieceViewHandler;
    }
    public void Execute()
    {
        //状態変化処理の命令クラス
        pieceStateHandler.Execute(piecestateFall);

        //落下処理命令クラス
        pieceFallAction.PieceFall();

        //見た目、コンポーネント変化処理の命令クラス
        pieceViewHandler.ChangeView((int)piecestateFall);

        //落下と同時におこすイベント
        //->予測リストの更新と先頭要素番号の譲渡、パズルピースの生成
        //１．監視クラスへの通知
        //２．先頭要素の譲渡（イベント関数の引数で渡す）
        //３．パズルピースの生成
        //イベント発火、プレイヤーから離れた
        EventManager.Instance.FallPieceEvent();
    }
}

