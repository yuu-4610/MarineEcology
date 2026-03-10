using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer; //画像情報
    private float judgeCount = 0; //判定カウント
    private const float countRimit = 6.0f; //判定上限 5 → リミット　0.5 → 落下処理に対応するため
    private bool isGameFinish;
    private int triggerObjectCount = 0;

    public ObjectReference objectReference;
    // Start is called before the first frame update
    
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //当たり判定を透明化に → isTrigger = true
        boxCollider2D.isTrigger = true;
        //アルファ値の設定
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            //Pieceオブジェクトが一度でも当たっていれば加算
            triggerObjectCount++;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // tag が Piece のオブジェクトに触れたら
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            //時間の測定
            judgeCount += Time.deltaTime;
            //テキストオブジェクトの処理を呼ぶ
            ObjectManager.Instance.Get<GameSceneUI>(ReferenceObjectName.Canvas_GameScene.ToString(), objectReference.gameSceneUI).IsNearGameOver(judgeCount);

            //測定時間が countRimit(６秒)以上であれば
            if (judgeCount >= countRimit && !isGameFinish)
            {
                //ゲーム終了
                Debug.Log("ゲーム終了");
                //ゲーム終了時のイベントを発火
                ObjectEventManager.Instance.TrantitionGameToResultEvent();
                isGameFinish = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            //Pieceオブジェクトが離れたら減算
            triggerObjectCount--;
            //judgeLine に当たっているオブジェクトが一つでもあれば処理を行わない
            // →OnTriggerStay2D 時に以下処理が走ることを防ぐため
            if (triggerObjectCount <= 0)
            {
                judgeCount = 0;
                ObjectManager.Instance.Get<GameSceneUI>(ReferenceObjectName.Canvas_GameScene.ToString(), objectReference.gameSceneUI).IsNearGameOver(judgeCount);
            }
        }
    }
}
