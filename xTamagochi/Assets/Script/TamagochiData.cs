using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TamagochiData
{
    public int hambre;
    public int amor;

    public TamagochiData(Tamagochi tamagochi)
    {
        hambre = tamagochi.hambre;
        amor = tamagochi.amor;
    }
   
}
