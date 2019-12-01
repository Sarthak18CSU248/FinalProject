using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    
    public GameObject timeup,Game_Over;
    public Text uitext;
    public float maintimer;
    private float timer;
    private bool canCount = true;
    private bool doOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = maintimer;
    }
    public void GameOver()
    {
        Game_Over.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (timer >=0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uitext.text = timer.ToString("F");
        }
        else if (timer <=0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uitext.text = "0.00";
            timer = 0.0f;

        }
        if (uitext.text == "0.00")
        {
           
            StartCoroutine(Game_end());
        }
        IEnumerator Game_end()
        {
            timeup.SetActive(true);
            yield return new WaitForSeconds(3f);
            timeup.SetActive(false);
            GameOver();

        }
    }
}
