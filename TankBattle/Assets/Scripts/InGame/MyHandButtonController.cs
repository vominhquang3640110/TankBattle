using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyHandButtonController : MonoBehaviour
{
    private InGameManager inGameManager;
    private UIManager uiManager;
    private PrefabManager prefabManager;

    [Header("Button Data")]
    public string buttonName;
    public int amount;
    public bool isATK;
    public bool isDEF;
    public bool isActive = false;

    [Header("Text Data")]
    public TextMeshProUGUI amountText;

    void Start()
    {
        inGameManager = InGameManager.instance;
        uiManager = inGameManager.uiManager;
        prefabManager = inGameManager.prefabManager;
    }

    void Update()
    {
        //攻撃ステップだが、攻撃タンクではない場合、ボタンを無効にする
        if (inGameManager._ControllerPlayer.stepName == "Attack" && !isATK)
        {
            OffActiveButton();
        }
        //守備ステップだが、守備タンクではない場合、ボタンを無効にする
        if (inGameManager._ControllerPlayer.stepName == "Defend" && !isDEF)
        {
            OffActiveButton();
        }

        //数を表示する
        amountText.text = amount.ToString();
    }

    /// <summary>
    /// ボタンの状態を更新
    /// </summary>
    public void CheckActiveButton()
    {
        //コインがある、買える数が残っている場合
        if (amount > 0)
        {
            //ボタンを有効にし、元の色を戻す
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = Color.white;
            isActive = true;
        }
        else
        {
            //ボタンを無効にする
            OffActiveButton();
        }
    }

    /// <summary>
    /// ボタンを無効にする
    /// </summary>
    public void OffActiveButton()
    {
        //ボタンを無効にし、色を変える
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = Color.red;
        isActive = false;
    }

    public void OnClickMyHandButton()
    {
        inGameManager.soundManager.PlayClickButton();
        //P1のslotDatasと所有タンクの数を更新
        inGameManager._ControllerPlayer.handNumber--;
        inGameManager._ControllerPlayer.slotDatas[uiManager.setSlotCounter] = buttonName;

        amount--;   //残りの数を-1

        //SetSlotの情報を更新
        uiManager.setSlotButtons[uiManager.setSlotCounter].buttonName = buttonName;
        uiManager.setSlotButtons[uiManager.setSlotCounter].SetSprite();

        CheckActiveButton();    //このボタンの状態を更新
        uiManager.setSlotCounter++;     //次のSetSlotへ

        //SetSlotが満タンであればmyHandButton全部無効にする
        if (uiManager.setSlotCounter > 4)
        {
            uiManager.OffAllMyHandButton();
        }

        //チュートリアルである
        if (inGameManager.userManager.isTutorialMode)
        {
            TutorialManager tutorial = TutorialManager.Instance;
            if (tutorial.stepCounter == 3)
            {
                tutorial.Message_14();
            }
            else if (tutorial.stepCounter == 9)
            {
                tutorial.Message_29();
            }
            else if (tutorial.stepCounter == 10)
            {
                tutorial.Message_30();
            }
            else if (tutorial.stepCounter == 11)
            {
                tutorial.Message_31();
            }
        }
    }
}
