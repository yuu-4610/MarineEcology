using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceIdleHandler : IPieceMove
{
    /*<Ó–±>State:Idleó‘Ô‚Ìˆ—ƒNƒ‰ƒX‚ğŒÄ‚Ô
     */
    private PieceStateHandler pieceStateHandler;
    private const PieceState pieceStateIdle = PieceState.Idle;
    public PieceIdleHandler(PieceStateHandler pieceStateHandler) 
    {
        this.pieceStateHandler = pieceStateHandler;
    }
    
    public void Execute()
    {
        pieceStateHandler.Execute((int)pieceStateIdle);
    }
}
