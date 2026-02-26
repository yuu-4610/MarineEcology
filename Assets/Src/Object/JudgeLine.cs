using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer; //画像情報
    private float judgeCount = 0; //判定カウント
    private const int countRimit = 10; //判定上限
    private bool isGameFinish;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        // tag が Piece のオブジェクトに触れたら
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            // tag が Pieceの オブジェクトに触れてる間カウント → 10秒間触れていれば
            judgeCount += Time.deltaTime;
            if (judgeCount >= countRimit && !isGameFinish)
            {
                //ゲーム終了
                Debug.Log("ゲーム終了");
                //イベント発火
                //ObjectEventManager.Instance.TrantitionGameToResultEvent();
                isGameFinish = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagName.Piece.ToString())
        {
            judgeCount = 0;
            Debug.Log($"離れた：{judgeCount}");
        }
    }
}
