using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceStateController : MonoBehaviour
{
    /*<責務>パズルピースの状態を管理するクラス
     *
     */
    private bool hasCollider = false;
    private bool isDropObject = false; //落下処理を行うオブジェクトか（＝プレイヤーに追従するオブジェクトか）

    void Start()
    {

    }

    // Update is called once per frame
    public void SetHasCollier(bool hasCollider)
    {
        this.hasCollider = hasCollider;
    }
    public bool GetHasCollider()
    {
        return this.hasCollider;
    }

    public void SetIsDropObject(bool isDropObject)
    {
        this.isDropObject = isDropObject;
    }
    public bool GetIsDropObject()
    {
        return this.isDropObject;
    }
}
