using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Holes : MonoBehaviour
{
  
    private TextMeshProUGUI Player1Score, Player2Score;
    public GameObject WhiteToken, BlackToken;
    int player1scorecount=0, player2scoreCount = 0;
    private bool fouled = false;
    private void Start()
    {
        Player1Score = GameObject.FindGameObjectWithTag("Player1Score").GetComponent<TextMeshProUGUI>();
        Player2Score =GameObject.FindGameObjectWithTag("Player2Score").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("White"))
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 2.5f)
            {
                Destroy(collision.gameObject);
                IncreaseWhiteScore();
                if (FindObjectOfType<PlayerController>().Player1Turn == true)
                {
                    FindObjectOfType<PlayerController>().TookToken = true;
                }
              
            }
        }
        if (collision.CompareTag("Black"))
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 2.5f)
            {
                Destroy(collision.gameObject);
                IncreaseBlackScore();
                if (FindObjectOfType<PlayerController>().Player1Turn == false)
                {
                    FindObjectOfType<PlayerController>().TookToken = true;
                }

               
            }
        }


        if (collision.CompareTag("Pink"))
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 1.9f)
            {
                if (FindObjectOfType<PlayerController>().Player1Turn == true)
                {
                    FindObjectOfType<PlayerController>().TookToken = true;

                    BlackTookQueen();
                    Destroy(collision.gameObject);
                }
                else
                {
                    FindObjectOfType<PlayerController>().TookToken = true;
                    Destroy(collision.gameObject);
                    WhiteTookQueen();
                }
            }
        }


        if (collision.CompareTag("Striker"))
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 8f)
            {
                FindObjectOfType<PlayerController>().TookToken = true;
                if (FindObjectOfType<PlayerController>().Player1Turn == true)
                {
                    if (!fouled)
                    {

                        FindObjectOfType<PlayerController>().SwitchTurns();
                        GameObject.FindGameObjectWithTag("Striker").GetComponent<SpriteRenderer>().enabled = false;
                        Invoke("GetBackStriker", 3.5f);
                    }
                }
                else
                {
                    if (!fouled)
                    {
                        FindObjectOfType<PlayerController>().SwitchTurns();
                        GameObject.FindGameObjectWithTag("Striker").GetComponent<SpriteRenderer>().enabled = false;
                        Invoke("GetBackStriker",3.5f);
                    }
                }
            }
        }
    }




   

    void IncreaseWhiteScore()
    {
        player1scorecount += 10;
        Player1Score.text = player1scorecount.ToString();
    }
    void IncreaseBlackScore()
    {
        player2scoreCount += 10;
        Player2Score.text = player2scoreCount.ToString();

    }


    void BlackTookQueen()
    {
        player2scoreCount += 50;
        Player2Score.text = player2scoreCount.ToString();
    }
    void WhiteTookQueen()
    {
        player2scoreCount += 50;
        Player2Score.text = player2scoreCount.ToString();
    }

    void GetBackStriker()
    {
        GameObject.FindGameObjectWithTag("Striker").GetComponent<SpriteRenderer>().enabled =true;
        fouled = false;
    }


  
}
