using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using UnityEngine;

public class Trader : MonoBehaviour
{
     public void StartTrading(GameObject character,GameObject go)
     {
          TradingCanvas.Instance.Open(character);
     }
}
