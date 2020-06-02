using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    /// 
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
        if (countProp == countTotal) return; // 跳出

        // 遊戲時間 遞減 一禎的時間
        gameTime -= Time.deltaTime;

        // 遊戲時間 = 數學.夾住(遊戲時間. 最小值，最大值)
        gameTime=Mathf.Clamp(gameTime, 0, 100);

        // 更新倒數時間介面 ToString("f小數點位數")
        textTime.text = "倒數時間:" + gameTime.ToString("f2");

        Lose();
    }

    /// <summary>
    /// 取得道具 : 漢堡 - 更新於數量與介面， 酒 - 扣兩秒時間，並更新在介面
    /// </summary>
    /// <param name="prop"></param>
    public void GetProp(string prop)
    {
        if (prop == "burger")
        {
            countProp++;
            textCount.text = "道具數量" + countProp + "/" + countTotal;

            Win();  // 呼叫勝利方法
        }


       else if (prop == "WINE")
        {
            gameTime -=2;
            textTime.text = "倒數時間" + gameTime.ToString("f2");

        }
                
    }
    /// <summary>
    /// 勝利 : 吃光所有漢堡
    /// </summary>
   private void Win()
    {
        if(countProp == countTotal)                 // 如果漢堡數量 = 漢堡總數
        {                   
            final.alpha = 1;                         // 顯示結束畫面    => Add Component 中 Canvas Group 的設定
            final.interactable = true;               // 啟動互動
            final.blocksRaycasts = true;             // 啟動遮罩
            textTitle.text = "恭喜你吃完漢堡了!!";    // 更新結束畫面標題   
        }
    }

    /// <summary>
    /// 失敗 : 時間為零
    /// </summary>
    private void Lose()
    {
        if(gameTime ==0)
        {
            final.alpha = 1;                         // 顯示結束畫面    => Add Component 中 Canvas Group 的設定
            final.interactable = true;               // 啟動互動
            final.blocksRaycasts = true;             // 啟動遮罩
            textTitle.text = "挑戰失敗!!!";           // 更新結束畫面標題   

            FindObjectOfType<Player>().enabled = false; // 取得玩家.啟動 = false  // 限制小明在遊戲結束後停止移動
        }
    }


    public void replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    public void Quit()
    {
        Application.Quit();
    }






    #region 事件

    private void Start()
    {
        countTotal = CreatProp(props[0], 5);  //道具總數 = 生成道具 (道具1號，指定數量) 
        textCount.text = "道具數量: 0/" + countTotal;

        CreatProp(props[1], 5);  // 生成道具(道具2號，指定數量)

    }

    private void Update()
    {
        CountTime();
    }
    #endregion




}
