using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    //variale data terkait Ball
    public BallControl ball;
    CircleCollider2D ballCollider;
    Rigidbody2D ballRB;
    //bola bayangan yang akan ditampilkan di titik tumbukan 
    public GameObject ballAtCollision;

    //inisialisasi ballCollider dan ballRB
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    
    bool drawBallAtCollision = false;
        //status lintasan ball, hanya akan tampil jika lintasan bertumbukan dengan objek

        //titik tumbukan digeser untuk menggambar ballAtCollision
        Vector2 offsetHitPoint = new Vector2();
    void Update()
    {
        bool drawBallAtCollision = false;
        //status lintasan ball, hanya akan tampil jika lintasan bertumbukan dengan objek

        //titik tumbukan digeser untuk menggambar ballAtCollision
        Vector2 offsetHitPoint = new Vector2();

        //tentukan titik tumbukan dengan deteksi pergerakan lingkaran
        RaycastHit2D[] circleCastHit2DArray = Physics2D.CircleCastAll(ballRB.position, ballCollider.radius, ballRB.velocity.normalized);

        //untuk setiap titik tumbukan..
        foreach (RaycastHit2D circleCastHit2D in circleCastHit2DArray)
        {
            //Jika terjadi tumbukan dan tumbukan tersebut tidak dengan bola (Karena garis lintasan digambar dari titik tengah bola

            if(circleCastHit2D.collider != null && circleCastHit2D.collider.GetComponent<BallControl>() == null)
            {
                //garis lintasan akan digambar pada titik tengah bola saat ini ke titik tengah bola pada saat tumbukan
                //yaitu titik yang di offset dari titik tumbukan berdasar vektor normal titik tersebut sebesar jari2 bola

                //simpan titik tumbukan
                Vector2 hitPoint = circleCastHit2D.point;
                //tentukan normal di titik tumbukan
                Vector2 hitNormal = circleCastHit2D.normal;

                //tentukan offset hitpoint dengan menggeser titik tumbukan sepanjang vektor hitNormal sesuai radius bola
                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

                //gambar garis lintasan dari titik tengah bola saat ini ke titik tengah bola pada saat bertumbukan
                DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);


                //kalau bukan sidewall, gambar pantulannya
                if (circleCastHit2D.collider.GetComponent<SideWall>() == null)
                {
                    //hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryPosition).normalized;

                    //hitung vektor keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    //hitung dot product dari outVector dan inVector. Digunakan supaya garis lintasan ketika terjadi tumbukan tidak digambar
                    float outDot = Vector2.Dot(outVector, hitNormal);

                    if(outDot > -1.0f && outDot < 1.0)
                    {
                        //gambar lintasan pantulnya
                        DottedLine.DottedLine.Instance.DrawDottedLine(offsetHitPoint, offsetHitPoint + outVector * 10.0f);

                        //untuk menggambar bola bayangan di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }
                //hanya gambar lintasan untuk satu titik tumbukan, jadi keluar dari loop
                break;
                
            }
        }
        //jika true
        if (drawBallAtCollision)
        {
            //gambar bola bayangan di prediksi titik tumbukan
            ballAtCollision.transform.position = offsetHitPoint;
            ballAtCollision.SetActive(true);

        }
        else
        {
            //sembunyikan bola bayangan
            ballAtCollision.SetActive(false);

        }
    }
}
