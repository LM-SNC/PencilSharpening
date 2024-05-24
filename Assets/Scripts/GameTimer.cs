using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    private DateTime _startTime;
    
    public void StartTimer()
    {
        _startTime = DateTime.Now;
        StartCoroutine(Timing());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    private IEnumerator Timing()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            var timeSpan = DateTime.Now - _startTime;
            _timerText.text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Minutes, timeSpan.Seconds,
                timeSpan.Milliseconds / 10);
        }
    }
}