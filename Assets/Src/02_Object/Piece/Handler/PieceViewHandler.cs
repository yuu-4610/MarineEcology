using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceViewHandler
{
    /*<責務>パズルピースのコンポーネントの変更処理命令
     */
    private PieceViewController pieceViewController;
    public PieceViewHandler(PieceViewController pieceViewController)
    {
        this.pieceViewController = pieceViewController;
    }

    //State別処理の呼び出し
    public void ChangeView(int stateNumber)
    {
        if(stateNumber == (int)PieceState.Follow)pieceViewController.FollowView();
        else if (stateNumber == (int)PieceState.Fall)pieceViewController.FallView();
    }
}
