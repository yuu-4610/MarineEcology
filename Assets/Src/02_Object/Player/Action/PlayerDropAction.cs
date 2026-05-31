using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropAction
{
    public void PlayerDrop()
    {
        EventManager.Instance.DropPieceEvent();
    }
}
