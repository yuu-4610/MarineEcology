using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PieceFallAction : MonoBehaviour
{
    /*<責務>パズルピースの落下処理を実行する
     *
     */
    private Rigidbody2D rigidbody2D;
    public bool hasVelocity { get; private set; } = false; //落下スピードを持つか

    private void Start()
    {
        //なければ追加
        if (!gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            this.rigidbody2D = rigidbody2D.AddComponent<Rigidbody2D>();
        }
        //あればコンポーネントの取得
        else
        {
            this.rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        //下方向に力を加える
        if (hasVelocity)
        {
            Vector2 v = rigidbody2D.velocity;
            v.y = -4f;
            rigidbody2D.velocity = v;
        }
    }

    public void PieceFall()
    {
        HasVelocity();
    }
    //外部からアクセスるため、別メソッドで実装
    public void HasVelocity()
    {
        hasVelocity = true;
    }
}
