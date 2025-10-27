using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonController : MonoBehaviour
{
    private InGameManager inGameManager;
    private UIManager uiManager;

    [Header("Button Data")]
    public string buttonName;
    public int amount;
    public int cost;
    public bool isTrap;
    public bool isActive = false;

    [Header("Text Data")]
    public TextMeshProUGUI amountText;

    void Start()
    {
        inGameManager = InGameManager.instance;
        uiManager = inGameManager.uiManager;

        OffActiveButton();
    }

    void Update()
    {
        //数を表示する(数は0より小さいなら更新しなくていい)
        if (amount < 1000)
        {
            amountText.text = amount.ToString();
        }
        
    }

    /// <summary>
    /// ボタンの状態を更新
    /// </summary>
    public void CheckActiveButton()
    {
        //コインがある、買える数が残っている場合
        if (inGameManager._ControllerPlayer.currentCoin >= cost && amount > 0)
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

    /// <summary>
    /// Recycleをクリックする際の処理
    /// </summary>
    public void OnClickRecycle()
    {
        inGameManager.soundManager.PlayClickButton();
        foreach (var buyButton_inGame in inGameManager.buyButtonDatas)
        {
            foreach (var buyButtonUI in uiManager.buyButtons)
            {
                if (buyButton_inGame.Key == buyButtonUI.buttonName)
                {
                    buyButtonUI.amount = buyButton_inGame.Value;
                }
            }
        }
        uiManager.UseCoin(cost);
    }

    /// <summary>
    /// buyButtonをクリックする際の処理
    /// </summary>
    public void OnClickBuyButton()
    {
        inGameManager.soundManager.PlayClickButton();
        amount--;   //残りの数を1減らす

        if (!isTrap)
        {
            //myHandButtonのamountを直接に1増やす
            for (int i = 0; i < uiManager.myHandButtons.Count; i++)
            {
                if (buttonName == uiManager.myHandButtons[i].buttonName)
                {
                    uiManager.myHandButtons[i].amount++;
                }
            }

            inGameManager._ControllerPlayer.handNumber++;     //プレイヤーの所有タンク+1

            //所有コインをcost分減らす
            uiManager.UseCoin(cost);

            //チュートリアルである
            if (inGameManager.userManager.isTutorialMode)
            {
                TutorialManager tutorial = TutorialManager.Instance;
                if (tutorial.stepCounter == 3)
                {
                    tutorial.Message_13();
                }
                else if (tutorial.stepCounter == 6)
                {
                    tutorial.Message_26();
                }
                else if (tutorial.stepCounter == 7)
                {
                    tutorial.Message_27();
                }
                else if (tutorial.stepCounter == 8)
                {
                    tutorial.Message_28();
                }
            }
        }
        else
        {
            //P1のslotDatasを更新
            inGameManager._ControllerPlayer.slotDatas[uiManager.setSlotCounter] = buttonName;

            //SetSlotの情報を更新
            uiManager.setSlotButtons[uiManager.setSlotCounter].buttonName = buttonName;
            uiManager.setSlotButtons[uiManager.setSlotCounter].SetSprite();

            CheckActiveButton();    //このボタンの状態を更新
            uiManager.setSlotCounter++;     //次のSetSlotへ

            //SetSlotが満タンであればmyHandButton全部無効にする
            if (uiManager.setSlotCounter > 4)
            {
                uiManager.OffAllBuyButton();
            }
            //チュートリアルである
            if (inGameManager.userManager.isTutorialMode && TutorialManager.Instance.stepCounter == 5)
            {
                TutorialManager.Instance.Message_21();
            }
        }
    }
}
