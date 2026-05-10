using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFallAction : MonoBehaviour
{
    /*<責務>パズルピースの落下処理を実行する
     *
     */
    private Rigidbody2D rigidbody2D;

    //[SerializeField] CircleCollider2D circleCollider2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 v = rigidbody2D.velocity;
        v.y = -4f;
        rigidbody2D.velocity = v;
    }

    public void PieceFall()
    {
        //落下と同時におこすイベントー＞予測リストの更新と先頭要素番号の譲渡、パズルピースの生成
        //１．監視クラスへの通知
        //２．先頭要素の譲渡（イベント関数の引数で渡す）
        //３．パズルピースの生成
        //イベント発火、プレイヤーから離れた
        EventManager.Instance.OnPlayerLeftRangeEvent();
        //もう一つイベント発火を記載（予測リストの更新
    }
}
