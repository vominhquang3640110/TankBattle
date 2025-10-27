using ExitGames.Demos.DemoAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private InGameManager ingameManager;
    private UIManager uiManager;

    public List<Button> buttons = new List<Button>();
    public List<GameObject> messageImages = new List<GameObject>();

    public int stepCounter;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        ingameManager = InGameManager.instance;
        uiManager = ingameManager.GetComponent<UIManager>();

        stepCounter = 1;
    }
    /// <summary>
    /// 特定のボタンだけクリック可能にする。他のボタンを無効にする。
    /// </summary>
    /// <param name="buttonNumber">有効にするボタンのindex</param>
    public void OnButton(int buttonNumber)
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
        buttons[buttonNumber].interactable = true;
    }
    /// <summary>
    /// 全てのボタンを無効にする。
    /// </summary>
    void OffButton()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
    public void Message_01()
    {
        OnButton(4);
        messageImages[0].SetActive(true);
    }
    public int TutorialProcess_01()
    {
        return 5;
    }
    public void Message_02()
    {
        messageImages[0].SetActive(false);
        messageImages[1].SetActive(true);
    }
    public void Message_03()
    {
        messageImages[1].SetActive(false);
        messageImages[2].SetActive(true);
    }
    public void Message_03Off()
    {
        messageImages[2].SetActive(false);
        stepCounter++;
    }
    public void Message_04()
    {
        OffButton();
        messageImages[3].SetActive(true);
    }
    public void Message_05()
    {
        messageImages[3].SetActive(false);
        messageImages[4].SetActive(true);
        TutorialProcess_02();
    }
    void TutorialProcess_02()
    {
        ingameManager._ControllerPlayer = ingameManager._P2;
        uiManager.GetCoin();
    }
    public int TutorialProcess_03()
    {
        return 6;
    }
    public void Message_06()
    {
        messageImages[4].SetActive(false);
        messageImages[5].SetActive(true);
    }
    public void Message_07()
    {
        messageImages[5].SetActive(false);
        messageImages[6].SetActive(true);
    }
    public void Message_08()
    {
        messageImages[6].SetActive(false);
        messageImages[7].SetActive(true);
    }
    public void Message_09()
    {
        messageImages[7].SetActive(false);
        messageImages[8].SetActive(true);
    }
    public void Message_10()
    {
        messageImages[8].SetActive(false);
        messageImages[9].SetActive(true);
    }
    public void Message_11()
    {
        stepCounter++;
        messageImages[9].SetActive(false);
        messageImages[10].SetActive(true);
    }
    public void Message_12()
    {
        ingameManager._ControllerPlayer = ingameManager._P1;
        OnButton(5);
        messageImages[10].SetActive(false);
        messageImages[11].SetActive(true);
    }
    public void Message_13()
    {
        OffButton();
        OnButton(12);
        messageImages[11].SetActive(false);
        messageImages[12].SetActive(true);
    }
    public void Message_14()
    {
        OffButton();
        messageImages[12].SetActive(false);
        messageImages[13].SetActive(true);
    }
    public void Message_15()
    {
        messageImages[13].SetActive(false);
        messageImages[14].SetActive(true);
    }
    public void Message_16()
    {
        messageImages[14].SetActive(false);
        messageImages[15].SetActive(true);
        TutorialProcess_04();
        StartCoroutine(TutorialProcess_05());
    }
    void TutorialProcess_04()
    {
        ingameManager._P2.handNumber += 3;
        ingameManager._P2.currentCoin -= 4;
    }
    IEnumerator TutorialProcess_05()
    {
        ingameManager._P2.handNumber--;
        ingameManager._P2.slotDatas[0] = "Tank2";
        yield return new WaitForSeconds(0.2f);
        ingameManager._P2.handNumber--;
        ingameManager._P2.slotDatas[1] = "Tank1";
        yield return new WaitForSeconds(0.2f);
        ingameManager._P2.handNumber--;
        ingameManager._P2.slotDatas[2] = "Tank1";
    }
    public void Message_17()
    {
        OnButton(1);
        messageImages[15].SetActive(false);
        messageImages[16].SetActive(true);
    }
    public void Message_17Off()
    {
        messageImages[16].SetActive(false);
    }
    public void TutorialProcess_06()
    {
        ingameManager._P2.stepName = "Battle1";
    }
    public void Message_18()
    {
        messageImages[17].SetActive(true);
    }
    public void Message_18Off()
    {
        stepCounter++;
        messageImages[17].SetActive(false);
    }
    public void Message_19()
    {
        OnButton(9);
        messageImages[18].SetActive(true);
    }
    public void Message_20()
    {
        stepCounter++;
        messageImages[18].SetActive(false);
        messageImages[19].SetActive(true);
    }
    public void Message_21()
    {
        OnButton(1);
        messageImages[19].SetActive(false);
        messageImages[20].SetActive(true);
    }
    public void Message_21Off()
    {
        messageImages[20].SetActive(false);
    }
    public void Message_22()
    {
        OffButton();
        messageImages[21].SetActive(true);
    }
    public void Message_22Off()
    {
        stepCounter++;
        messageImages[21].SetActive(false);
    }
    public void Message_23()
    {
        OnButton(4);
        messageImages[22].SetActive(true);
    }
    public void Message_24()
    {
        OffButton();
        messageImages[22].SetActive(false);
        messageImages[23].SetActive(true);
    }
    //stepCounter=6
    public void Message_25()
    {
        OnButton(7);
        messageImages[23].SetActive(false);
        messageImages[24].SetActive(true);
    }
    //stepCounter=7
    public void Message_26()
    {
        stepCounter++;
        OnButton(6);
        messageImages[24].SetActive(false);
        messageImages[25].SetActive(true);
    }
    //stepCounter=8
    public void Message_27()
    {
        stepCounter++;
        OnButton(5);
        messageImages[25].SetActive(false);
        messageImages[26].SetActive(true);
    }
    //stepCounter=9
    public void Message_28()
    {
        stepCounter++;
        ingameManager._P2.handNumber++;
        ingameManager._P2.currentCoin--;
        ingameManager._P2.handNumber--;
        ingameManager._P2.slotDatas[0] = "Tank1";
        OnButton(14);
        messageImages[26].SetActive(false);
        messageImages[27].SetActive(true);
    }
    //stepCounter=10
    public void Message_29()
    {
        stepCounter++;
        OnButton(13);
        messageImages[27].SetActive(false);
        messageImages[28].SetActive(true);
    }
    //stepCounter=11
    public void Message_30()
    {
        stepCounter++;
        OnButton(12);
        messageImages[28].SetActive(false);
        messageImages[29].SetActive(true);
    }
    public void Message_31()
    {
        OnButton(1);
        messageImages[29].SetActive(false);
        messageImages[30].SetActive(true);
    }
    public void Message_31Off()
    {
        messageImages[30].SetActive(false);
        StartCoroutine(TutorialProcess_07());
    }
    IEnumerator TutorialProcess_07()
    {
        yield return new WaitForSeconds(1);
        ingameManager._P2.stepName = "Battle1";
    }
    public void Message_32()
    {
        messageImages[31].SetActive(true);
    }
    //stepCounter=12
    public void Message_32Off()
    {
        stepCounter++;
        messageImages[31].SetActive(false);
        for (int i = 0; i < ingameManager._P2.slotDatas.Length; i++)
        {
            ingameManager._P2.slotDatas[i] = "";
        }
    }
    public void TutorialProcess_08()
    {
        ingameManager._P2.stepName = "Battle2";
    }
    public void Message_33()
    {
        messageImages[32].SetActive(true);
    }
    public void Message_34()
    {
        messageImages[32].SetActive(false);
        messageImages[33].SetActive(true);
    }
    public void Message_35()
    {
        messageImages[33].SetActive(false);
        messageImages[34].SetActive(true);
    }
    //stepCounter=13
    public void Message_35Off()
    {
        stepCounter++;
        messageImages[34].SetActive(false);
    }
    public void Message_36()
    {
        messageImages[35].SetActive(true);
    }
    //stepCounter=14
    public void Message_36Off()
    {
        stepCounter++;
        messageImages[35].SetActive(false);
        StartCoroutine(TutorialProcess_09());
    }
    IEnumerator TutorialProcess_09()
    {
        yield return new WaitForSeconds(1);
        ingameManager._P2.isSurrender = true;
    }
}
