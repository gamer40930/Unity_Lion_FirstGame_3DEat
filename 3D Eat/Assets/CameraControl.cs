using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// 玩家變形元件
    /// </summary>
    #region 欄位與屬性
    private Transform Player;

    [Header ("追蹤速度"),Range (0.1f,50.5f)]
    public float speed = 1.5f;

    #endregion

    #region 方法
    /// <summary>
    /// 追蹤玩家
    /// </summary>

    private void Track()
    {
        // 攝影機與小明 Y 軸距離  3 - 1.5 = 1.5
        // 攝影機與小明 Z 軸距離 -3 - 0 = -3

        Vector3 posTrack = Player.position;
        posTrack.y += 1.5f;
        posTrack.z += -3f;

        // 攝影機座標 = 變形.座標 
        Vector3 posCam = transform.position;

        //Lerp 插值 Mathf.Lerp (A點,B點, 百分比);  插值 - 按照百分比取得從一個值過度到另外一個值的中間值
        posCam = Vector3.Lerp(posCam, posTrack, 0.5f * Time.deltaTime * speed);

        //變形.座標 = 攝影機座標
        transform.position = posCam;
    }

    #endregion

    #region 事件
    /* Lerp 插值練習
    public float A = 0;
    public float B = 1000;


    public Vector2 V2A = Vector2.zero;
    public Vector2 V2B = Vector2.one * 1000;

    private void Update()
    {
        // Mathf.Lerp (A點,B點,float t);  插值 - 按照百分比取得從一個值過度到另外一個值的中間值

        A = Mathf.Lerp(A, B, 0.5f);
        V2A = Vector2.Lerp(V2A, V2B, 0.5f);


    }
    */

    private void Start()
    {
        // 小明物件 = 遊戲物件.尋找("物件名稱").變形
        Player = GameObject.Find("小明").transform;
    }

    // 延遲更新 : 會在 Update 執行後執行
    // 建議 : 需要追蹤座標要在此事件內
    private void LateUpdate()
    {
        Track();
    }
    #endregion
}
