using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingController : MonoBehaviour
{
    //プレイヤーの座標とるために
    private GameObject player;
    private Animator anim;
    private Rigidbody2D rb2d;
    //当たり判定確認フラグ
    private bool isStart = false;
    //死んだときのフラグ
    private bool isDead = false;
    //反転用
    private SpriteRenderer sprite;


    //パーティクル用
    public GameObject fxhit;
    //スピード設定
    public float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //出現したら動くようにする
        if (isStart)
        {
            Vector2 offset = (player.transform.position - this.transform.position).normalized;
            rb2d.velocity = offset * speed;
            if (offset.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
        //死んだら点滅して消える
        if (isDead)
        {
            //点滅して徐々に消えていく
            float level = Mathf.Abs(Mathf.Sin(Time.time * 20) / Time.time);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, level);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //カメラ外側の当たり判定が当たってきたら
        if (col.gameObject.tag == "MainCamera")
        {
            //妖精出現
            isStart = true;
            anim.SetBool("isStart", true);
        }
        if (col.gameObject.tag == "Destroy")
        {
            Destroy(this.gameObject);
        }
    }
    //踏まれたら死ぬ
    private void OnCollisionEnter2D(Collision2D col)
    {
        //プレイヤーが当たったら死ぬ
        if (col.gameObject.tag == "Player")
        {
            speed = 0;
            //コライダーを消す
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;

            //コルーチン使う
            StartCoroutine("Dead");
        }
    }

    //カメラから消えたら敵も消える
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            isStart = false;
            anim.SetBool("isStart", false);
        }
    }

    //コルーチン設定
    IEnumerator Dead()
    {
        isDead = true;
        //パーティクルを生成
        Instantiate(fxhit, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
