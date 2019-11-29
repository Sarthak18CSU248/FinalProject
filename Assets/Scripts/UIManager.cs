using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject player, ground_enemy, platforms, playerinfo, startmenu;
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

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
