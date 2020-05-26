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
    #endregion

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
    /// 旋轉角度
    ///</summary>
    private Vector3 angle;

    private Animator ani;
    private Rigidbody rig;

    ///<summary>
    /// 跳躍力道 : 從 0 慢慢增加
    ///</summary>
    private float jump;

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
    private void HitProp()
    {

    }

    #endregion


    #region 事件
    private void Start()
    {
        // GetComponent<泛型> 泛行方法 - 泛型 所有類型 Rigidbody, Transform, Collider...
        // 剛體 = 取得元件
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

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
    #endregion
}
