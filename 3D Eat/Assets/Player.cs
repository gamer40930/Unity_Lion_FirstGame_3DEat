using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 屬性與欄位
    [Header ("移動速度"),Range (1,1000)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float height;
    [Header("漢堡音效")]
    public AudioClip soundBurger;
    [Header ("酒音效")]
    public AudioClip SouondWine;

    

    ///<summary>
    /// 旋轉角度
    ///</summary>
    private Vector3 angle;

    private Animator ani;     // 動畫
    private Rigidbody rig;    // 剛體
    private AudioSource aud;  // 喇叭
    private GameManager gm;   // 遊戲管理器
    
    ///<summary>
    /// 跳躍力道 : 從 0 慢慢增加
    ///</summary>
    private float jump;
    #endregion




    #region 方法
    ///<summary>
    /// 移動 : 透過鍵盤
    ///</summary>
    private void Move()
    {
        // 浮點數 前後值 = 輸入類別.取得軸向值("垂直") - WS 上下
        // GetAxisRaw - int整數 ;  GetAxis - float浮點數
        #region 移動
        float v =Input.GetAxisRaw("Vertical");
        float h =Input.GetAxisRaw("Horizontal");
       

        // 剛體物件.添加推力(X,Y,Z) - 世界座標
        //rig.AddForce(0 , 0, speed * v);
        // 剛體物件.添加推力(三維向量) -
        // 前方 transform.forward - Z // 右方 transform.right -X  //上方 transform.up - Y
        rig.AddForce(transform.forward * speed * Mathf.Abs(v));
        rig.AddForce(transform.forward * speed * Mathf.Abs(h));



        // 動畫.設定布林值("跑步動畫",布林值) - 當 前後取絕對值  大於 0 時勾選   (跑步動畫->動畫混合樹中的動作設定)
        ani.SetBool("跑步動畫", Mathf.Abs(v) > 0 || Mathf .Abs (h) > 0);

        //ani.Setbool("跑步動畫", v==1 || V==-1); //使用邏輯運算子
        #endregion

        #region 轉向
        
        if (v == 1) angle = new Vector3(0, 0, 0);            //前
        else if (v == -1) angle = new Vector3(0, 180, 0);    //後  
        else if (h == 1) angle = new Vector3(0, 90, 0);      //右  
        else if (h == -1) angle = new Vector3(0, 270, 0);    //左  

        // 只要在類別後面有 : MonoBehavior
        // 就可以直接使用關鍵字 transform 取得此物件的 Transform元件
        // eulerAngles 歐拉角度 0 ~ 360
        transform.eulerAngles = angle;

        #endregion
    }

    ///<summary>
    /// 是否在地上
    ///</summary>
    private  bool isGround
    {
        get
        {
            
            if (transform.position.y <0.056f ) return true; // 如果 Y 軸 小於 0.051 傳回 true           
            else return false; // 否則傳回 false
        }
    }

    ///<summary>
    /// 跳躍 : 判斷在地板上並按下空白鍵跳躍
    ///</summary>
    private void Jump()
    {
        // 如果 在地板上 為 勾選 並且 按下空白鍵
        if(isGround && Input .GetButtonDown("Jump"))
        {

            // 每次跳躍 值都從0開始
            jump = 0;
            // 剛體.推力(0,跳躍高度,0)
            rig.AddForce(0, height, 0);          
        }

        // 如果不再地板上 (在空中)
        if(!isGround)
        {
            //跳躍 遞增 時間.一禎時間
            jump += Time.deltaTime *2;
        }
        // 動畫.設定福點數("跳躍參數".跳躍時間)
        ani.SetFloat("跳躍力道", jump);
    }

    ///<summary>
    /// 碰到道具 : 碰到帶有標籤 "Burger" 的物件
    ///</summary>
    private void HitProp(GameObject prop)
    {
        // print("碰撞到的物件標籤為" + prop.name);

        if (prop.tag == "burger") 
        {
            aud.PlayOneShot(soundBurger, 1.2f); // 喇叭.撥放一次音效 (音效片段,音量)
            Destroy(prop);
        }
        else if (prop.tag == "WINE") 
        {
            aud.PlayOneShot(SouondWine, 1.2f); 
            Destroy(prop);
        }

        gm.GetProp(prop.tag);     //告知 GM 取得道具(將道具標籤傳過去)
        
    }

    #endregion


    #region 事件
    private void Start()
    {
        // GetComponent<泛型> 泛行方法 - 泛型 所有類型 Rigidbody, Transform, Collider...
        // 剛體 = 取得元件
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        //FOOT 僅限於場景上只有一個類別存在時使用 (例如GM)
        //例如 : 場景上只有一個 GameManger 類別時可以使用他來取得
        gm = FindObjectOfType<GameManager>();
    }


    // 固定更新頻率事件 : 1 秒 50 禎，使用物理必須在此事件內
    private void FixedUpdate()
    {
        Move();
    }

    // 事件更新 : 1 秒約60禎
    private void Update()
    {
        Jump();         // 跳躍建議放在update中 
    }


    #region 碰撞事件:沒有勾選 is Trigger (不可穿透物件)
    // 碰撞事件 : 當物件碰撞時執行一次 (沒有勾選 is Trigger)
    // collision: 碰到物件的碰撞資訊
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    // 碰撞事件 : 當物件碰撞離開時執行一次 (沒有勾選 is Trigger)
    private void OnCollisionExit(Collision collision)
    {
        
    }

    // 碰撞事件 : 當物件碰撞開始時持續執行執行一次 (沒有勾選 is Trigger)
    private void OnCollisionStay(Collision collision)
    {
        
    }
    #endregion

    #region 觸發事件:有勾選 is Trigger (可穿透物件)
    //觸發事件
    // 碰撞事件 : 當物件碰撞時執行一次 (有勾選 is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        //碰到道具(碰撞資訊.標籤)
        HitProp(other.gameObject);   
    }
    // 碰撞事件 : 當物件碰撞離開時執行一次 (有勾選 is Trigger)
    private void OnTriggerExit(Collider other)
    {
        
    }
    // 碰撞事件 : 當物件碰撞開始時持續執行執行一次 (有勾選 is Trigger)
    private void OnTriggerStay(Collider other)
    {
        
    }
    #endregion

    #endregion
}
