using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //player1
    public PlayerControl player1;
    private Rigidbody2D player1RigidBody;

    //player2
    public PlayerControl player2;
    private Rigidbody2D player2RigidBody;

    //Bola
    public BallControl ball;
    private Rigidbody2D ballRigidBody;
    private CircleCollider2D ballCollider;

    //skor max
    public int maxScore;

    //Variable debug window open atau tidak
    private bool isDebugWindowShown = false;

    //objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;

    private void Start()
    {
        player1RigidBody = player1.GetComponent<Rigidbody2D>();
        player2RigidBody = player2.GetComponent<Rigidbody2D>();
        ballRigidBody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();

    }

    void OnGUI()
    {
        //Tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score());
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score());


        //Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width/2 - 60,35,120,53), "RESTART"))
        {
            //Ketika tombol restart ditekan, reset skor kedua pemain dan reset game
            player1.ResetScore();
            player2.ResetScore();
            ball.SendMessage("RestartGame", 2, SendMessageOptions.RequireReceiver);
        }

        //jika pemain 1 mencapai skor max
        if(player1.Score() == maxScore)
        {
            //tampilkan teks "PLAYER ONE WIN" di bagian kiri
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER 1 WIN");
            //reset bola
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        //jika pemain 2 mencapai skor max
        else if(player2.Score() == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER 2 WIN");
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        //jika isDebugWindowShown == true, tampilkan teks area untuk debug window
        if (isDebugWindowShown)
        {
            //simpan warna lama agar tidak hilang
            Color oldcolor = GUI.backgroundColor;
            //beri GUI warna baru
            GUI.backgroundColor = Color.red;
            //Simpan variable2 fisika yang akan ditampilkan
            float ballMass = ballRigidBody.mass;
            Vector2 ballVelocity = ballRigidBody.velocity;
            float ballSpeed = ballRigidBody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            //text debug
            string debugText =
                "Ball Mass = " + ballMass + "\n" +
                "Ball Velocity = " + ballVelocity + "\n" +
                "Ball Speed = " + ballSpeed + "\n" +
                "Ball Momentum = " + ballMomentum + "\n" +
                "Ball Friction = " + ballFriction + "\n" +
                "Last Impulse from player1 = (" + impulsePlayer1X + " , " + impulsePlayer1Y + ")\n" +
                "Last Impulse from player2 = (" + impulsePlayer2X + " , " + impulsePlayer2Y + ")\n";

            //tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            //kembalikan warna lama
            GUI.backgroundColor = oldcolor;


        }

        //toggle nilai debug window ketika tombol ini ditekan
        if(GUI.Button(new Rect(Screen.width /2 -60, Screen.height - 73,120,53),"TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;

        }

    }
}
