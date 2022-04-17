using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool isRunning;

    [SerializeField] private Image countDownImg;
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private int min;
    [SerializeField] private int sec;
    public int timeUsed;

    private IEnumerator co;

    // Start is called before the first frame update
    void Start()
    {
        timeUsed = -1;
        timeTMP.text = min.ToString() + ":" + sec.ToString();
    }

    public void Activate()
    {
        if (!isRunning && co == null)
        {
            co = Co_StartCounting();
            StartCoroutine(co);
            isRunning = true;
        }
    }

    public void Stop()
    {
        if (isRunning && co != null)
        {
            StopCoroutine(co);
            co = null;
            isRunning = false;
        }
    }

    IEnumerator Co_StartCounting()
    {
        var newCol = Color.green;
        newCol.a = 0.8f;
        countDownImg.color = newCol;
        int totalSec = min * 60 + sec;
        while (min > 0 || sec > 0)
        {
            // count sec
            if (sec > 0)
            {
                sec -= 1;
                timeUsed += 1;
                yield return new WaitForSecondsRealtime(1f);
                if(sec < 10)
                {
                    timeTMP.text = min.ToString() + ":0" + sec.ToString();
                }
                else
                {
                    timeTMP.text = min.ToString() + ":" + sec.ToString();
                }
            }
            // count min
            else
            {
                if (min > 0)
                {
                    min -= 1;
                    sec = 59;
                    timeTMP.text = min.ToString() + ":" + sec.ToString();
                }
            }
            // update fill image
            countDownImg.fillAmount = (float)(min * 60 + sec) / (float)totalSec;
        }
        yield return null;
    }
}
