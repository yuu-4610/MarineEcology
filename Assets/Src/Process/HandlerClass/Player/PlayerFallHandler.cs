using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallHandler : IPlayerMove
{
    public void Execute(float inputValue)
    {
        //入力が検知できればイベントの呼び出し
        if(inputValue > 0) EventManager.Instance.PieceObjectDropEvent();
    }
}
