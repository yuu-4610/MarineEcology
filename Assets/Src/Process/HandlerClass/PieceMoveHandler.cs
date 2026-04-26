using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveHandler : IPieceMove
{
    private PieceMoveAction pieceMoveAction;
    // Start is called before the first frame update
    
    public PieceMoveHandler(PieceMoveAction pieceMoveAction)
    {
        this.pieceMoveAction = pieceMoveAction;
    }

    // Update is called once per frame
    public void Execute()
    {
        pieceMoveAction.TargetPlayerFollow();
    }
}
