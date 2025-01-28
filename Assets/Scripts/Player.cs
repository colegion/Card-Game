using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : User
{
    public override void OnTurnStart()
    {
        foreach (var card in Cards)
        {
            card.ToggleInteractable(true);
        }
    }

    public override void OnCardPlayed(Card card)
    {
        throw new NotImplementedException();
    }
}
