using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    //パラメータ後から調整するためにpublic
    public float speed = 5.0f;
    public float jumpforce = 400.0f;
    //キー入力のためのx方向の力
    float x;
    //アニメーション用
    private Animator anim;
    //レイヤー選択用
    public LayerMask StageLayer;

    //キャラの向き制御
    private SpriteRenderer sprite;
    //ジャンプ防止
    private bool jumpbool ;
    //坂道上るようにフラグ付け
    private bool slop;
    //コインのスコア
    public static int coinScore = 0;
    //コインとったときのスコア表記
    public Text coinTxt;
    //死んだときに操作できなくするようのフラグ
    private bool isDead = false;

    //死んだときの効果音
    public AudioClip sound1;
    AudioSource audioSource;
    //コイン効果音
    public AudioClip sound2;
    AudioSource audioSource2;
    //敵踏んだ時の効果音
    public AudioClip sound3;
    AudioSource audioSource3;
    //敵踏んだ時の効果音
    public AudioClip sound4;
    AudioSource audioSource4;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.sprite = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        audioSource2 = GetComponent<AudioSource>();
        coinScore = 0;
        coinTxt.text = "Score=" + coinScore.ToString();
        audioSource3 = GetComponent<AudioSource>();
        audioSource4 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //速度制御で使う
        float veloX = rb2d.velocity.x;
        //ジャンプモーション判別に使う
        float veloY = rb2d.velocity.y;

        
        if (!isDead)//死んでいないなら操作可能にする
        {
            x = Input.GetAxisRaw("Horizontal");//左入力で-1,右入力で１
                                               //xが負なら反対向き（左向き）にさせる

            if (x < 0)
            {
                sprite.flipX = true;
            }
            else if (x > 0)
            {
                sprite.flipX = false;
            }
            //走行モーション
            //キーを押した方向に力を加える
            rb2d.AddForce(Vector2.right * x * speed);//右が正にしているからright
                                                     //|5|>|0.5|となり、走行モーション再生
            anim.SetFloat("Speed", Mathf.Abs(x * speed));
            //addforceじゃないジャンプ
            float jumppow = 15.0f;
            //接地していてジャンプボタンが押されたら
            if (jumpbool && Input.GetButtonDown("Jump"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumppow);
            }
            //接地していなくて上側の速度があってジャンプボタンが押されている間
            if (!jumpbool&& rb2d.velocity.y > 0.0f && Input.GetButton("Jump"))
            {
                //重力加速度半分にする
                rb2d.gravityScale = 2.0f;
            }
            else
            {
                rb2d.gravityScale = 4.0f;
            }
            
        }
        
        //  斜面は物理演算無視で移動させる
        if (slop)
        {
            this.gameObject.transform.Translate(0.04f * x, 0.0f, 0.0f);
        }
        if (jumpbool)//地面にいるときにはジャンプモーションを切る
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
        }
        //veloYが正ならジャンプモーション、下向きなら落下モーション
        if(veloY > 1.0f)
        {
            anim.SetBool("isJump", true);
            
        }
        if (veloY < -0.8f)
        {
            anim.SetBool("isFall", true);
        }

        //速度は7で頭打ちにする
        if (Mathf.Abs(veloX) > speed)
        {
            if(veloX > speed)
            {
                rb2d.velocity = new Vector2(speed, veloY);
            }

            if (veloX < speed)
            {
                rb2d.velocity = new Vector2(-speed, veloY);
            }
        }   
    }
    
    private void FixedUpdate()
    {
        //当たり判定反転用
        x = Input.GetAxisRaw("Horizontal");
        jumpbool = false;
        //地面のコライダー探す用
        Vector2 StagePos =
            new Vector2(transform.position.x, transform.position.y - 1.7f);
        Vector2 StageArea = new Vector2(0.3f, 0.5f);
        //壁のコライダー探すよう用(area1)
        Vector2 wallArea1 = new Vector2(x * 0.8f, 1.5f);
        Vector2 wallArea2 = new Vector2(x * 0.3f, 1.0f);
        //斜面のコライダー探すよう(area2)
        Vector2 wallArea3 = new Vector2(x * 1.5f, 0.6f);
        Vector2 wallArea4 = new Vector2(x * 1.0f, 0.1f);
        
        //足元でStageのLayerにあるオブジェクトがあるか探索
        jumpbool = Physics2D.OverlapArea(StagePos + StageArea, StagePos - StageArea, StageLayer);

        Debug.Log("Jump:" + jumpbool);

        bool area1 = false;//壁検知
        bool area2 = false;//斜面検知

        Debug.DrawLine(StagePos + StageArea, StagePos - StageArea, Color.red);
        Debug.DrawLine(StagePos + wallArea1, StagePos + wallArea2, Color.blue);
        Debug.DrawLine(StagePos + wallArea3, StagePos + wallArea4, Color.red);

        
        area1 = Physics2D.OverlapArea(StagePos + wallArea1, StagePos + wallArea2, StageLayer);
        area2 = Physics2D.OverlapArea(StagePos + wallArea3, StagePos + wallArea4, StageLayer);

        if( !area1 & area2)//壁でなく斜面だけ当たり判定があったら
        {
            slop = true;
        }
        else
        {
            slop = false;
        }
        Debug.Log(slop);
    }
    

    void OnCollisionEnter2D(Collision2D col)//ぶつかったら
    {
        //敵を踏んだら
        if (col.gameObject.tag == "Enemy" )
        {
            //ジャンプ再生
            anim.SetBool("isJump", true);
            //上に飛ばす
            rb2d.AddForce( Vector2.up * jumpforce);
            audioSource3.PlayOneShot(sound3);
            Debug.Log("Attacked");
        }
        //とげを踏んだら
        if (col.gameObject.tag == "DamageObject")
        {
            //死後移動バグがあったため解消
            x = 0;
            //とげに刺さったら死ぬフラグを立てる
            isDead = true;
            //刺さったら音を鳴らす
            audioSource.PlayOneShot(sound1);
            //とげに刺さったら死ぬ
            StartCoroutine("Dead");
            Debug.Log("Damage");
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        //敵を踏んだら
        if (col.gameObject.tag == "Enemy")
        {
            //ジャンプ再生
            anim.SetBool("isJump", true);
            //上に飛ばす
            rb2d.AddForce(Vector2.up * 50);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)//Enemyの当たり判定が通り抜けたら死ぬ
    {
        if(col.gameObject.tag == "Enemy")
        {
            if (!isDead)
            {
                //あたったら死ぬ
                StartCoroutine("Dead");
                audioSource4.PlayOneShot(sound4);
            }
            //死ぬフラグを立てる
            isDead = true;
        }
        if (col.gameObject.tag == "Coin")
        {
            StartCoroutine("Coin");
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Finish")
        {
            isDead = true;
        }
       

    }

    //コルーチン。死んだときの処理
    IEnumerator Dead()
    {
        //被ダメージのアニメーションをtrueにして遷移
        anim.SetBool("isDamaged", true);

        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);
        //1回飛ぶ。カービィみたいに
        rb2d.AddForce(Vector2.up * 200);
        //死んだら落とす。キャラの下側の円コライダーを消す
        GetComponent<CircleCollider2D>().enabled = false;
    }
    //コインを2回とらないためのコルーチン
    IEnumerator Coin()
    {
        coinScore += 1000;
        coinTxt.text = "Score=" + coinScore.ToString();
        audioSource2.PlayOneShot(sound2);
        yield return new WaitForSeconds(0.1f);
    }
  

}
