using UnityEngine;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour
{
    private InGameManager inGameManager;
    private UIManager uiManager;

    void Start()
    {
        inGameManager = InGameManager.instance;
        uiManager = inGameManager.uiManager;

        OffActiveButton();    
    }

    /// <summary>
    /// ボタンを有効にする
    /// </summary>
    public void OnActiveButton()
    {
        //ボタンを有効にし、元の色を戻す
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// ボタンを無効にする
    /// </summary>
    public void OffActiveButton()
    {
        //アニメーションを無効にし、ボタンを無効にし、色を変える
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = Color.red;
    }

    /// <summary>
    /// Decisionボタンをクリックする際の処理
    /// </summary>
    public void OnClickDecisionButton()
    {
        inGameManager.soundManager.PlayClickButton();

        //チュートリアルである
        if (inGameManager.userManager.isTutorialMode)
        {
            TutorialManager tutorial = TutorialManager.Instance;
            if (tutorial.stepCounter == 3)
            {
                tutorial.Message_17Off();
            }
            else if (tutorial.stepCounter == 5)
            {
                tutorial.Message_21Off();
            }
            else if (tutorial.stepCounter == 11)
            {
                tutorial.Message_31Off();
            }
        }

        if (inGameManager._ControllerPlayer.stepName == "Defend" || inGameManager._ControllerPlayer.stepName == "Attack")
        {
            //相手を待つ文字のアニメショーンを中止
            uiManager.PlayOPLoadingText();
            //battle1に移る
            inGameManager._ControllerPlayer.stepName = "Battle1";
            //SetSlotを初期化
            uiManager.ReSetSlotButton();
            
        }
        else if (inGameManager._ControllerPlayer.stepName == "Battle1")
        {
            inGameManager._ControllerPlayer.stepName = "Battle2";
        }
        OffActiveButton();
    }
}
