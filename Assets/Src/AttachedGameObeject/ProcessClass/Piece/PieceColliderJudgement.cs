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

    //public bool IsPieceObjectDestroyed(Piece otherPiece)
    //{
    //    if (!otherPiece.isDestroyed)
    //    {
    //        // my_serial の値が大きいオブジェクトに処理をさせる
    //        if (mySerial < otherPiece.mySerial)
    //        {
    //            //第３のオブジェクトと衝突したときに処理を走らせないようにする
    //            isDestroyed = true;
    //            otherPiece.isDestroyed = true;
    //            //効果音を出す
    //            AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.onoma));

    //            //次のオブジェクトがあれば実行
    //            if (objectReference.pieceObjects.Length > (int)fishPieceType + 1)
    //            {
    //                //次番号のオブジェクトを生成
    //                var nextObject = ObjectFactory.Instance.SynthesisPieceEvent(objectReference.pieceObjects[(int)fishPieceType + 1], this.gameObject, otherPiece, GenerateParentObjectName.Pieces.ToString());
    //                if (nextObject != null)
    //                {
    //                    nextObject.GetComponent<Piece>().hasCollider = true;
    //                }
    //                processOrder = true;
    //            }
    //            UIManager.Instance.SetPoint(otherPiece.point);
    //        }
    //        // my_serial の値の値が小さいほうの処理
    //        else
    //        {
    //            processOrder = true;
    //        }
    //        //削除可能（processOrder）になるまで繰り返す
    //        while (!processOrder) yield return null;
    //        Destroy(this.gameObject);
    //    }

    //}
}
