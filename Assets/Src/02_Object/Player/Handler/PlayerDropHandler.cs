using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropHandler
{
    /*<責務>プレイヤーによるパズルピースの投下処理命令を行う。
     */
    private PlayerDropAction playerDropAction;
    public PlayerDropHandler(PlayerDropAction playerDropAction)
    {
        this.playerDropAction = playerDropAction;
    }

    public void Execute()
    {
        playerDropAction.PlayerDrop();
    }
}
