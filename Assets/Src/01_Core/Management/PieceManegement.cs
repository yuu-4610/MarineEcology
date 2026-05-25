using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManegement : MonoBehaviour
{
    /*<責務>プレイヤーを追従するパズルピースの監視および生成情報の管理を行う。
     *
     */

    [SerializeField] Transform playerTransform; //プレイヤーの位置情報
    [SerializeField] int generateRangeValue; //生成するパズルピースの値の範囲
    [SerializeField] int leavePosition; //プレイヤーからの距離

    //生成したオブジェクトの参照をもらうため
    [SerializeField] ObjectFactory objectFactory; //パズルピース生成クラス

    private GameObject surveillancePieceObject;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        //予測リストの更新後に生成処理処理をする（引数で生成するパズルピースの種類を指定）
        EventManager.Instance.pieceObjectGenerateDecided += SurveillanceePieceObjectUpdate;
        //パズルピースが落下したときに発火
        EventManager.Instance.fallPiece += SurveillanceePieceObjectRemove;
    }
    private void OnDisable()
    {
        EventManager.Instance.pieceObjectGenerateDecided -= SurveillanceePieceObjectUpdate;
        EventManager.Instance.fallPiece -= SurveillanceePieceObjectRemove;
    }
    void Start()
    {
        //初期値が設定されていなければ（＝０であれば）、値を設定
        if (generateRangeValue == 0) generateRangeValue = 5;
        if (leavePosition == 0) generateRangeValue = 1;

        //一番最初の生成処理
        var nextGeneratePieceFishType = Random.Range(0, generateRangeValue);

        //パズルピースの生成と監視対象に登録
        SurveillanceePieceObjectUpdate(nextGeneratePieceFishType);

        Initialize();
    }

    void Update()
    {
        
    }
    private void Initialize()
    {
        //別オブジェクトのクラス参照の保障
        if (objectFactory == null)
        {
            objectFactory = GameObject.Find(AcquisitionObjectName.ObjectFactory.ToString()).GetComponent<ObjectFactory>();
        }
    }

    //現在の監視対象を外す
    private void SurveillanceePieceObjectRemove()
    {
        surveillancePieceObject = null;
    }

    /*イベント発火時に走る処理
     *プレイヤーに追従するパズルピースの新規生成と監視対象に登録
     */
    private void SurveillanceePieceObjectUpdate(int generateType)
    {
        //監視対象が存在してなければ
        if(surveillancePieceObject == null)
        {
            StartCoroutine(SetSurveillanceAndGeneratePieceObject(generateType));
        }
    }

    //プレイヤーに追従するパズルピースの生成命令と参照取得
    private GameObject OnPlayerFollowPieceAcQuisition(int generateType)
    {
        //監視対象がある場合は生成しない（今あるオブジェクトを返す
        //if(surveillancePieceObject != null) return surveillancePieceObject;

        //監視対象が存在しない場合は新しく生成しこれを返す
        var leavePositoinY = playerTransform.position.y - leavePosition;
        var generatePosition = new Vector3(playerTransform.position.x, leavePositoinY, playerTransform.position.z);
        return objectFactory.GeneratePieceObject(generateType, generatePosition);
    }

    private void SetSurveillancePieceObject(GameObject surveillancePieceObject)
    {
        //監視対象の参照を代入
        this.surveillancePieceObject = surveillancePieceObject;
    }

    //パズルピースの生成と情報の提供
    private IEnumerator SetSurveillanceAndGeneratePieceObject(int generateType)
    {
        yield return new WaitForSeconds(0.8f);
        //新たなパズルピースの生成 + 監視対象に登録
        var generateObject = OnPlayerFollowPieceAcQuisition(generateType);

        //生成情報の取得
        var followActionClass = generateObject.GetComponent<PieceFollowAction>();
        //生成オブジェクトのクラスに情報を渡す
        followActionClass.SetLeavePiecePosition(leavePosition);
        followActionClass.SetPlayerTransformInfomation(playerTransform);

        //生成したオブジェクトを監視対象とする
        SetSurveillancePieceObject(generateObject);
    }
}
