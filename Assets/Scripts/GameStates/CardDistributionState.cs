using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class CardDistributionState : IGameState
{
    private const int CardAmount = 4;
    private bool _initialDistributionCompleted;
    
    public void EnterState()
    {
        throw new System.NotImplementedException();
    }
    
    private void DistributeCards()
    {
        if (!_initialDistributionCompleted)
        {
            
            _initialDistributionCompleted = true;
        }
        else
        {
            
        }
    }

    private void DistributeTableCards()
    {
        for (int i = 0; i < CardAmount; i++)
        {
            var config = GameController.Instance.GetRandomConfig();
            var card = GameController.Instance.GetCard();
            card.ConfigureSelf(config, i < CardAmount - 1);
        }
    }

    private void DistributeUserCards()
    {
        for (int i = 0; i < GameController.Instance.UserCount; i++)
        {
            for (int j = 0; j < CardAmount; j++)
            {
                
            }
        }
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }
}
