using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


public class JudgeLine : MonoBehaviour
{
    /*<責務>ゲームオーバーの判定処理を行う
     *枠（水槽）の一番上に設置
     */
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer; //画像情報
    private GameObject firstObject; //判定ラインに最初に乗ったオブジェクト
    private bool isCountProcess = false; //タイムカウント処理開始フラグ
    private int judgementObjectCount = 0; //判定ラインに乗ったオブジェクト数

    public ObjectReference objectReference; //
    // Start is called before the first frame update

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //当たり判定を透明化に → isTrigger = true
        boxCollider2D.isTrigger = true;
        //透明にする
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //tag名がPieceに設定しているオブジェクトが触れれば
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            //Pieceオブジェクトが一度でも当たっていれば加算
            judgementObjectCount++;
        }
        //一番最初に判定したオブジェクトを代入
        //二つ目以降が当たっても処理を繰り返さないための対策
        if (firstObject == null)
        {
            firstObject = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //tag名がPieceに設定しているオブジェクトが触れれば
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            if(firstObject == collision.gameObject && !isCountProcess)
            {
                //テキストオブジェクトの処理を呼ぶ
                EventManager.Instance.JudgeTimeCountStartEvent();
                isCountProcess = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //tag名がPieceに設定しているオブジェクトが触れれば
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            //Pieceオブジェクトが離れたら減算
            judgementObjectCount--;
            firstObject = null;
            //judgeLine に当たっているオブジェクトが一つでもあれば処理を行わない
            //->OnTriggerStay2D 時に以下処理が走ることを防ぐため
            if (judgementObjectCount <= 0)
            {
                EventManager.Instance.JudgeTimeCountResetEvent();
                isCountProcess = false;
            }
        }
    }
}
