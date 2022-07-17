using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Span")]
    public Transform[] spanPoint;
    public GameObject Enemy;
    public float Timevalue;
    public float Timedelay;

    [Header("Targer")]
    public Transform player;
    public PlayerController playerScript;

    private int value;

    private void Update()
    {
        Span();
        Timer();
    }

    private void Timer()
    {
        Timevalue += Time.deltaTime;
    }

    private void Span()
    {
        
        if (Timevalue >= Timedelay)
        {
            GameObject instantEnemy = Instantiate(Enemy, spanPoint[Random.Range(0, 9)].position, spanPoint[Random.Range(0, 9)].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.targer = player;
            enemy.player = playerScript;
            Timevalue = 0;
        }
    }

    //public float criticalStr(float AttackStr)
    //{
    //    value = Random.Range(0, 10);
        
    //    //È®·ü 20%
    //    if (value == 0)
    //    {
    //        return AttackStr * Random.Range(1.5f,2.3f);
    //    }
    //    else
    //    {
    //        return AttackStr;
    //    }
    //}
}
