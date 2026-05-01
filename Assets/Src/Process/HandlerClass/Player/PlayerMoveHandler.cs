using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHandler : IPlayerMove
{
    private PlayerMoveAction playerMoveAction;
    public PlayerMoveHandler(PlayerMoveAction playerMoveAction)
    {
        this.playerMoveAction = playerMoveAction;
    }

    public void Execute(float inputMoveValue)
    {
        playerMoveAction.PlayerMove(inputMoveValue);
    }
}
