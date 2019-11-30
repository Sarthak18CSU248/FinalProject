using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    
    public GameObject player, ground_enemy, platforms, playerinfo, startmenu,ground_enemy2,ground_enemy3,airenemy;
    public void restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void StartGame()
    {
        startmenu.SetActive(false);
        player.SetActive(true);
        ground_enemy.SetActive(true);
        platforms.SetActive(true);
        playerinfo.SetActive(true);
        ground_enemy.SetActive(true);
        ground_enemy2.SetActive(true);
        ground_enemy3.SetActive(true);

    }
    
    

   
}
