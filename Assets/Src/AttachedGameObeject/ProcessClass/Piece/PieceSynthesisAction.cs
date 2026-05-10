using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSynthesisAction
{
    private Transform pieceTransform;
    private Transform otherPieceTransform;
    private int addPoint;
    private int currentFishPieceTypeNumber;
    public PieceSynthesisAction(int addPoint, int currentFishPieceTypeNumber, Transform pieceTransform, Transform otherPieceTransform)
    {
        this.addPoint = addPoint;
        this.currentFishPieceTypeNumber = currentFishPieceTypeNumber;

        this.pieceTransform = pieceTransform;
        this.otherPieceTransform = otherPieceTransform;
    }
    public void PieceSynthesis()
    {
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.onoma));
        //合成時に次の番号のパズルピースを生成するイベント
        EventManager.Instance.SynthesisPieceObjectGenerateEvent(pieceTransform, otherPieceTransform, currentFishPieceTypeNumber);
        UIManager.Instance.SetPoint(addPoint);
    }
}
