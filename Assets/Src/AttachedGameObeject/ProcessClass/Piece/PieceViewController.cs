using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceViewController : MonoBehaviour 
{
    /*パズルピースのコンポーネントや見た目を管理しているクラス
     */
    private CircleCollider2D circleCollider2D;

    void Start()
    {
        if(TryGetComponent<CircleCollider2D>(out circleCollider2D)) circleCollider2D = GetComponent<CircleCollider2D>();
        else circleCollider2D.AddComponent<CircleCollider2D>();
    }

    public void FollowView()
    {
        circleCollider2D.enabled = false;
    }

    public void DropView()
    {
        circleCollider2D.enabled = true;
    }
}
