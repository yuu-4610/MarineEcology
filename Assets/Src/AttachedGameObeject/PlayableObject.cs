using System.Collections;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayableObject : MonoBehaviour
{
    private Vector2 mousePoint; //マウスの位置を取得
    private GameObject pieceObject; //落とすオブジェクト
    private float initializeXPosition; //プレイアブルオブジェクトのX座標取得
    private float initializeYPosition; //プレイアブルオブジェクトのy座標取得
    public ObjectReference objectReferece; //プレハブ生成用
    public int randomValues; //ランダム値
    [SerializeField] int rangeValues; //範囲値
    [SerializeField] int randomRangeMaxValues; //ランダム範囲のマックス値

    private float removePiecePosition = 0.3f; //プレイアブルオブジェクトの距離

    // Use this for initialization
     void Start()
     {
        //初期値の設定
        if (rangeValues == 0) rangeValues = 4;
        if (randomRangeMaxValues == 0) randomRangeMaxValues = 4;
        //このオブジェクトのX,y座標の初期値を取得
        initializeXPosition = this.gameObject.transform.position.x;
        initializeYPosition = this.gameObject.transform.position.y;

        randomValues = Random.Range(0, randomRangeMaxValues);
        StartCoroutine(GenerateObject());
        
        //ObjectManagerに登録
        ObjectManager.Instance.Register("PlayableObject", this.gameObject);
     }

    void Update()
    {
        //プレイヤーの移動処理をピースオブジェクトにも反映
        PieceObjectMove();
    }
    //移動処理

    public void ObjectMove(float inputMove)
    {
        //左右の行動処理
        mousePoint = new Vector2(inputMove, this.gameObject.transform.position.y);
        //オブジェクトの行動範囲（ intializeXPosition +-rangeValues ）を制限
        mousePoint.x = Mathf.Clamp(mousePoint.x, initializeXPosition - rangeValues, initializeXPosition + rangeValues);
        transform.position = mousePoint;
    }
    //落下処理
    public void PieceObjectDrop()
    {
        if (pieceObject == null) return;
        //発火
        //EventManager.Instance.PieceObjectDropEvent();
        //EventManager.Instance.PieceObjectGenerateEvent();
        //参照を破棄
        pieceObject = null;

        //オブジェクトを生成
        StartCoroutine(GenerateObject());
    }
    public void PieceObjectMove()
    {
        //ピースオブジェクトが存在しているならば
        if (pieceObject != null)
        {
            //プレイアブルオブジェクトより removePiecePosition だけ下
            var piecePoint = new Vector2(this.transform.position.x, initializeYPosition - removePiecePosition);
            pieceObject.transform.position = piecePoint;
        }
    }


    private IEnumerator GenerateObject()
    {
        yield return new WaitForSeconds(0.8f);
        //次のオブジェクトを生成 → NextPiece.cs から
        var generateObjectIndex = randomValues;

        //次のオブジェクトの生成・変数への代入
        //var generateObject = ObjectFactory.Instance.GeneratePieceObject(objectReferece.pieceObjects[generateObjectIndex], gameObject.transform, GenerateParentObjectName.Pieces.ToString());
        //pieceObject = generateObject;
    }
}