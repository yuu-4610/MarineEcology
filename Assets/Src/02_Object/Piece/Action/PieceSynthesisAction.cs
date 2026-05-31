using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSynthesisAction
{
    /*<責務>パズルピース同士の衝突時処理を行う。
     */
    private Transform pieceTransform;
    private Transform otherPieceTransform;
    private int addPoint; //加算する得点
    private int currentFishPieceTypeNumber; //パズルピースの種類番号
    public PieceSynthesisAction(int addPoint, int currentFishPieceTypeNumber, Transform pieceTransform, Transform otherPieceTransform)
    {
        this.addPoint = addPoint;
        this.currentFishPieceTypeNumber = currentFishPieceTypeNumber;

        this.pieceTransform = pieceTransform;
        this.otherPieceTransform = otherPieceTransform;
    }
    public void PieceSynthesis()
    {
        //SEの再生
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.onoma));
        //合成時に次の番号のパズルピースを生成するイベント
        EventManager.Instance.SynthesisPieceObjectGenerateEvent(pieceTransform, otherPieceTransform, currentFishPieceTypeNumber);
        UIManager.Instance.SetPoint(addPoint);
    }
}
