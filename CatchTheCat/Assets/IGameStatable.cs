using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStatable 
{
    public void HandleGameState()
    {
        switch (GameManager.instance.GetGameState())
        {
        
        }
            
    }
}
