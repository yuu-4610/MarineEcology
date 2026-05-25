using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceViewController : MonoBehaviour 
{
    /*<責務>パズルピースのコンポーネントの変更処理を行う
     */
    private CircleCollider2D circleCollider2D;

    void Awake()
    {
        //なければ追加
        if (!gameObject.TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            this.circleCollider2D = circleCollider2D.AddComponent<CircleCollider2D>();
        }
        //あればコンポーネントの取得
        else
        {
            this.circleCollider2D = GetComponent<CircleCollider2D>();
        }
    }
    public void OnDisable()
    {
        
    }

    //State:Follow時のコンポーネント変化
    public void FollowView()
    {
        circleCollider2D.enabled = false;
    }

    //State:Fall時のコンポーネント変化
    public void FallView()
    {
        circleCollider2D.enabled = true;
    }
}
