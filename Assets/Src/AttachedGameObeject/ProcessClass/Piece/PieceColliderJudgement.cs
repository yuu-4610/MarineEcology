using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PieceColliderJudgement
{
    public bool isDestroyed { get; private set; } = false;
    // Start is called before the first frame update

    public bool IsOtherPieceCollisionJudgement(Collision2D otherCollision2D, FishType thisObjectFishPieceType)
    {
        //指定オブジェクトの確認
        if (!otherCollision2D.gameObject.TryGetComponent(out Piece otherPiece)) return false;
        //Enumの確認
        if (otherPiece.fishPieceType != thisObjectFishPieceType) return false;
        //相互のオブジェクトを徹底調査
        //if (this == null || otherPiece == null || this.transform == null || otherPiece.transform == null) return false;

        if(otherPiece.isDestroyed) return false;

        return true;
    }
}
