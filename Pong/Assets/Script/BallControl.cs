using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    //simpan variable rigidbody2d
    private Rigidbody2D rigidbody2D;
    //variable besar gaya awal pendorong bola
    public float xInitialForce;
    public float yInitialForce;

    //titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

    //ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisonExit2D(Collision2D collison)
    {
        trajectoryOrigin = transform.position;
    }

    public Vector2 TrajectoryPosition
    {
        get { return trajectoryOrigin; }
    }
    
    void ResetBall()
    {
        //reset posisi menjadi (0,0)
        transform.position = Vector2.zero;
        //reset kecepatan menjadi (0,0)
        rigidbody2D.velocity = Vector2.zero;
    }

    void PushBall()
    {
        //agar besar gaya sama, maka saya tidak mencari angka acak diantara yInitialForce
        //tetapi menggunakan yInitialForce itu sendiri sebagai gaya dan memakai yRandomInitialForce
        //Untuk menentukan apakah gaya y bergerak ke atas atau kebawah
        float yRandomInitialForce = Random.Range(0,2);
        if (yRandomInitialForce < 1.0f)
        {
            yRandomInitialForce = yInitialForce;
        }
        else
        {
            yRandomInitialForce = -yInitialForce;
        }
        //isi variable random direction dengan angka random antara 0 dan 2
        float RandomDirection = Random.Range(0, 2);
        //angka random tadi digunakan untuk menggerakan bola ke kiri atau kanan
        //jika kurg dari 1, gerak ke kiri
        if (RandomDirection < 1.0f)
        {
            rigidbody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce));   
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(xInitialForce,yRandomInitialForce));
        }
    }

    void RestartGame()
    {
        //kembalikan posisi bola
        ResetBall();
        //setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    void Start()
    {
        //nilai awal trajectoryOrigin adalah posisi bola
        trajectoryOrigin = transform.position;

        rigidbody2D = GetComponent<Rigidbody2D>();
        RestartGame();
    }
    void Update()
    {
        Debug.Log(rigidbody2D.velocity);
    }
    
}
