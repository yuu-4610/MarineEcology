using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManegement : MonoBehaviour
{
    /*<責務>プレイヤーに追従するパズルピースの監視
     *
     */

    [SerializeField] Transform playerTransform;
    [SerializeField] int generateRangeValue; //生成するパズルピースの値の範囲
    [SerializeField] int leavePosition;

    //生成したオブジェクトの参照をもらうため
    [SerializeField] ObjectFactory objectFactory; //パズルピース生成クラス

    private GameObject surveillancePieceObject;


    private void OnEnable()
    {
        //予測リストの更新後に生成処理処理をする（引数で生成するパズルピースの種類を指定）
        EventManager.Instance.pieceObjectGenerateDecided += SetSurveillanceAndGeneratePieceObject;
        //パズルピースがプレイヤーから離れた＝プレイヤーが落とした時に発火
        EventManager.Instance.onPlayerLeftRange += SurveillanceObjectIsNull;
    }
    private void OnDestroy()
    {
        EventManager.Instance.pieceObjectGenerateDecided -= SetSurveillanceAndGeneratePieceObject;
        EventManager.Instance.onPlayerLeftRange -= SurveillanceObjectIsNull;
    }
    void Start()
    {
        //生成範囲値が設定されていない＝０であれば初期値を以下に設定
        if (generateRangeValue == 0) generateRangeValue = 5;

        //一番最初の生成処理
        var nextGeneratePieceFishType = Random.Range(0, generateRangeValue);

        //パズルピースの生成と監視対象に登録
        SetSurveillanceAndGeneratePieceObject(nextGeneratePieceFishType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //パズルピースがプレイヤーから離れた＝プレイヤーが落とした場合、監視対象の参照をNullとする（＝監視対象から外れる
    private void SurveillanceObjectIsNull()
    {
        surveillancePieceObject = null;
    }

    /*イベント発火時に走る処理
     *プレイヤーに追従するパズルピースの新規生成と監視対象に登録
     */
    private void SetSurveillanceAndGeneratePieceObject(int generateType)
    {
        //新たなパズルピースの生成 + 監視対象に登録
        var generateObject = OnPlayerFollowPieceGenerate(generateType);

        //生成情報の取得
        var followActionClass = generateObject.GetComponent<PieceFollowAction>();
        //生成オブジェクトのクラスに情報を渡す
        followActionClass.SetLeavePiecePosition(leavePosition);
        followActionClass.SetPlayerTransformInfomation(playerTransform);

        //生成したオブジェクトを監視対象とする
        SetSurveillancePieceObject(generateObject);
    }
    private GameObject OnPlayerFollowPieceGenerate(int generateType)
    {
        //監視対象がある場合は生成しない（今あるオブジェクトを返す
        if(surveillancePieceObject != null) return surveillancePieceObject;

        //監視対象が存在しない場合は新しく生成しこれを返す
        return objectFactory.GeneratePieceObject(generateType);
    }
    private void SetSurveillancePieceObject(GameObject surveillancePieceObject)
    {
        //監視対象の参照を代入
        this.surveillancePieceObject = surveillancePieceObject;
    }
}
