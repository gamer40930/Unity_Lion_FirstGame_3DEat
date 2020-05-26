using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 欄位與屬性
    [Header ("道具")]
    public GameObject[] props;
    [Header ("文字介面:道具總數")]    
    public Text textCount;
    [Header("文字介面:時間")]
    public Text textTime;
    [Header ("文字介面:結束畫面標題")]
    public Text textTitle;
    [Header("結束畫面")]
    public CanvasGroup final;
    
    ///<summary>
    /// 道具數量
    ///</summary>
    private int countTotal;
    
    ///<summary>
    /// 取得道具數量
    ///</summary>
    private int countProp;

    /// <summary>
    /// 遊戲時間
    /// </summary>
    private float gameTime = 30;

    #endregion

    #region 方法
    #endregion

    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="count"></param>
    /// <returns>傳回生成幾顆</returns>
    private int CreatProp(GameObject prop,int count)
    {
        int total = count + Random.Range(-5,5);

        for(int i =0; i <total;i++)
        {
            //座標 = (隨機，1.5，隨機)
            Vector3 pos = new Vector3(Random.Range(-9, 9), 0.8f, Random.Range(-9, 9));
            // 生成 (物件，座標，角度)
            Instantiate(prop, pos, Quaternion.identity);
        }

        return total;
    }

    /// <summary>
    /// 時間倒數
    /// </summary>
    private void CountTime()
    {
        // 遊戲時間 遞減 一禎的時間
        gameTime -= Time.deltaTime;
        // 更新倒數時間介面 ToString("f小數點位數")
        textTime.text = "倒數時間:" + gameTime.ToString("f2");
    }

    #region 事件

    private void Start()
    {
        countTotal = CreatProp(props[0], 20);  //道具總數 = 生成道具 (道具1號，指定數量) 
        textCount.text = "道具數量: 0/" + countTotal;

        CreatProp(props[1], 10);  // 生成道具(道具2號，指定數量)

    }

    private void Update()
    {
        CountTime();
    }
    #endregion




}
