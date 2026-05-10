using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceViewHandler
{
    /*パズルピースのコンポーネントや見た目の変更を命令するクラス
     */
    private PieceViewController pieceViewController;
    public PieceViewHandler(PieceViewController pieceViewController)
    {
        this.pieceViewController = pieceViewController;
    }
    public void ChangeView(int stateNumber)
    {
        if(stateNumber == (int)PieceState.Follow)pieceViewController.FollowView();
        else if (stateNumber == (int)PieceState.Fall)pieceViewController.DropView();
    }
}
