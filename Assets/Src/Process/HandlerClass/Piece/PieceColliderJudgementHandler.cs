using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceColliderJudgementHandler : IColliderJudgement
{
    private PieceColliderJudgement pieceColliderJudgement;
    private FishType fishPieceType;

    public PieceColliderJudgementHandler(PieceColliderJudgement pieceColliderJudgement, FishType fishPieceType)
    {
        this.pieceColliderJudgement = pieceColliderJudgement;
        this.fishPieceType = fishPieceType;
    }

    public bool IsHitJudgement(Collision2D collision2D)
    {
        return pieceColliderJudgement.IsOtherPieceCollisionJudgement(collision2D, fishPieceType);
    }
}
