using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayableObject : MonoBehaviour
{
    private Vector2 mousePoint; //マウスの位置を取得
    private GameObject pieceObject; //落とすオブジェクト
    private float initializeXPosition; //プレイアブルオブジェクトのX座標取得
    private float initializeYPosition; //プレイアブルオブジェクトのy座標取得
    public ObjectRefarence objectRefarece; //プレハブ生成用
    public int randomValues; //ランダム値
    [SerializeField] int rangeValues; //範囲値
    [SerializeField] int randomRangeMaxValues; //ランダム範囲のマックス値

    private float removePiecePoint = 0.3f;
    private int count = 0;
    private int count1 = 0;

    // Use this for initialization
     void Start()
     {
        if (rangeValues == 0) rangeValues = 4;
        if (randomRangeMaxValues == 0) randomRangeMaxValues = 4;
        //このオブジェクトのX,y座標の初期値を取得
        initializeXPosition = this.gameObject.transform.position.x;
        initializeYPosition = this.gameObject.transform.position.y;

        randomValues = Random.Range(0, randomRangeMaxValues);
        StartCoroutine(GenerateObject());
        
        //ObjectManagerに登録
        ObjectManager.Instance.Register("PlayableObject", this.gameObject);

        //イベント発火
        Invoke(nameof(EventRegistration), 0.03f);
     }

    void Update()
    {
        
    }

    public void ObjectMove(float inputMove)
    {
        //左右の行動処理
        mousePoint = new Vector2(inputMove, this.gameObject.transform.position.y);
        //オブジェクトの行動範囲（ intializeXPosition +-rangeValues ）を制限
        mousePoint.x = Mathf.Clamp(mousePoint.x, initializeXPosition - rangeValues, initializeXPosition + rangeValues);
        transform.position = mousePoint;

        //プレイヤーの移動処理を駒にも反映
        if (pieceObject != null)
        {
            var piecePoint = new Vector2(this.transform.position.x, initializeYPosition - removePiecePoint);
            pieceObject.transform.position = piecePoint;
        }
    }
    public void ObjectDrop()
    {
        if (pieceObject = null) return;
        //発火
        ObjectEventManager.Instance.ObjectDropEvent();
        ObjectEventManager.Instance.ObjectGenerateEvent();
        //参照を破棄
        pieceObject = null;

        //１秒後にオブジェクトを生成
        StartCoroutine(GenerateObject());
    }


    private IEnumerator GenerateObject()
    {
        yield return new WaitForSeconds(0.8f);
        //次のオブジェクトを生成
        var generateObjectIndex = randomValues;

        var generateObject = ObjectProcess.Instance.GeneratePieceObject(objectRefarece.pieceObjects[generateObjectIndex], gameObject.transform, GenerateParentObjectName.Pieces.ToString());
        pieceObject = generateObject;
    }
    private void EventRegistration()
    {

    }
}