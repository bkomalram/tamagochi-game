using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using System.IO;


public class Tamagochi : MonoBehaviour
{
    public int hambre = 50;
    public int amor = 20;        
    private float time = 5;
    /*Salidas*/
    public Text textHambre;
    public Text textAmor;

    public void SaveGame()
    {
        SaveSystem.SavedPlayer(this);
    }

    public void LoadGame()
    {
        TamagochiData data = SaveSystem.LoadPlayer();
        if (data != null) {
            hambre = data.hambre;
            amor = data.amor;
            textHambre.text = hambre.ToString();
            textAmor.text = amor.ToString();
        }  
        else {
            textAmor.text = "No encontro el archivo.";
        }      
    }

    public void Alimentar()
    {
        hambre++;
        SaveGame();
    }
    public void Amar()
    {
        amor++;
        SaveGame();
    }

    private void Start()
    {
        LoadGame();
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            var rand = Random.Range(0, 100);
            if (rand > 50)
            {
                amor=amor-5;
            }
            if (rand < 50)
            {
                hambre=hambre-5;                
            }            

            time = 5;
            
        }
        textHambre.text = hambre.ToString();
        textAmor.text = amor.ToString();
    }

    

}
