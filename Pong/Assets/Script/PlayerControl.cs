using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //variable input
    public KeyCode upButton = KeyCode.UpArrow;
    public KeyCode downButton = KeyCode.DownArrow;
    //variable gerak player
    public float playerSpeed = 10.0f;
    //variable batas gerak
    public float yBoundary = 9.0f;
    private int score;
    private Rigidbody2D rigidbody2D;

    //titik tumbukan terakhir
    private ContactPoint2D lastContactPoint;
    //untuk mengakses informasi titik kontak dari kelas lain
    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    void OnCollisionEnter2D(Collision2D collison)
    {
        if (collison.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collison.GetContact(0);
        }
    }
    
    public void IncreaseScore()
    {
        score++;
    }
    public void ResetScore()
    {
        score = 0;
    }
    public int Score()
    {
        return score;
    }
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {

        //dapatkan kecepatan raket
        Vector2 velocity = rigidbody2D.velocity;
        //jika menekan tombol atas, maka beri kecepatan positif ke komponen y
        if (Input.GetKey(upButton))
            velocity.y = playerSpeed;
        //jika tombol bawah, beri kecepatan negatif ke y
        else if (Input.GetKey(downButton))
            velocity.y = -playerSpeed;
        //jika tidak ada input, maka kecepatan 0
        else
            velocity.y = 0f;
        rigidbody2D.velocity = velocity;

        //Jika posisi lebih boundry, maka posisi kembalikan ke batas boundry
        Vector3 posisi = transform.position;
        if (posisi.y > yBoundary)
            posisi.y = yBoundary;
        //hal yang sama tetapi untuk batas bawah
        else if (posisi.y < -yBoundary)
            posisi.y = -yBoundary;
        transform.position = posisi;
    }

}
