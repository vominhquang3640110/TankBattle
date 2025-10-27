using ExitGames.Demos.DemoAnimator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InGameManager inGameManager;
    private PrefabManager prefabManager;
    private FlowManager flowManager;
    private TutorialManager tutorialManager;

    [Header("UI")]
    public DecisionButton decisionButton;
    public GameObject background;
    public GameObject result;
    public GameObject backButton;

    [Header("SetSlot")]
    public GameObject setBox;
    public GameObject setBoxP1;
    public GameObject setBoxP2;
    public List<SetSlotButton> setSlotButtons;
    public int setSlotCounter = 0;

    [Header("Button")]
    public List<BuyButtonController> buyButtons = new List<BuyButtonController>();
    public List<MyHandButtonController> myHandButtons = new List<MyHandButtonController>();

    [Header("Turn Text")]
    [SerializeField] private Animator firstText;
    [SerializeField] private Animator secondText;
    [SerializeField] private Animator myTurnText;
    [SerializeField] private Animator opTurnText;
    [SerializeField] private Animator attackIText;
    [SerializeField] private Animator defendIText;
    [SerializeField] private Animator attackIIText;
    [SerializeField] private Animator defendIIText;
    [SerializeField] private Animator endMYText;
    [SerializeField] private Animator endOPText;
    [SerializeField] private Animator winText;
    [SerializeField] private Animator lostText;
    [SerializeField] private TextMeshProUGUI opLoadingText;

    [Header("Coin")]
    [SerializeField] private Animator bagCoinIcon;
    [SerializeField] private TextMeshProUGUI coinText;
    public GameObject getCoinButton;
    public const int GET_COIN_MIN = 1;
    public const int GET_COIN_MAX = 6;

    [Header("P2 Infor")]
    public TextMeshProUGUI p2CoinText;
    public TextMeshProUGUI p2HandText;

    void Start()
    {
        inGameManager = InGameManager.instance;
        prefabManager = inGameManager.prefabManager;
        flowManager = inGameManager.flowManager;

        //チュートリアルである
        if (inGameManager.userManager.isTutorialMode)
        {
            tutorialManager = TutorialManager.Instance;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        //相手のコインと手札を表示する
        p2CoinText.text = inGameManager._P2.currentCoin.ToString();
        p2HandText.text = inGameManager._P2.handNumber.ToString();
    }

    /// <summary>
    /// コインを取得する処理
    /// </summary>
    public void GetCoin()
    {
        //チュートリアルである
        if (tutorialManager != null)
        {
            if (tutorialManager.stepCounter == 1)
            {
                tutorialManager.Message_02();
            }
            else if (tutorialManager.stepCounter == 6)
            {
                tutorialManager.Message_24();
            }
        }

        inGameManager.soundManager.PlayClickButton();
        getCoinButton.GetComponent<Button>().interactable = false;
        StartCoroutine(GetCoinIE());
    }
    IEnumerator GetCoinIE()
    {
        int coinValue = Random.Range(GET_COIN_MIN, GET_COIN_MAX + 1);

        //チュートリアルである
        if (tutorialManager != null && tutorialManager.stepCounter == 1)
        {
            coinValue = tutorialManager.TutorialProcess_01();
        }
        else if (tutorialManager != null && flowManager.turnCounter == 2)
        {
            coinValue = tutorialManager.TutorialProcess_03();
        }

        for (int i = 0; i < coinValue; i++)
        {
            //プレイヤーの所有コインを増やす
            inGameManager._ControllerPlayer.currentCoin++;

            if (inGameManager._ControllerPlayer == inGameManager._P1)
            {
                //バッグコインのアニメーションを再生
                bagCoinIcon.Play("CoinIcon_GetMove");
                inGameManager.soundManager.PlayGetCoin();
                yield return new WaitForSeconds(1 / 3f);
                //表示するコイン数を更新
                coinText.text = inGameManager._P1.currentCoin.ToString();
            }

            yield return new WaitForSeconds(0.1f);
        }

        //Attackステップに移す
        inGameManager._ControllerPlayer.stepName = "Attack";

        //コインを取得するボタンを無効にする
        getCoinButton.SetActive(false);
    }

    /// <summary>
    /// 所有コインを変化する処理
    /// </summary>
    /// <param name="value">Valueは+であれば、その分減る。-であれば、その分増える</param>
    public void UseCoin(int value)
    {
        inGameManager._ControllerPlayer.currentCoin -= value;
        coinText.text = inGameManager._ControllerPlayer.currentCoin.ToString();
        CheckAllBuyButton();  //全てのBuyButtonの状態を更新する
        CheckAllMyHandButton();   //全てのMyHandButtonの状態を更新
    }

    /// <summary>
    /// TrapButtonの状態だけ更新する
    /// </summary>
    public void CheckAllTrapButton()
    {
        foreach (var buyButton in buyButtons)
        {
            if (buyButton.isTrap)
            {
                buyButton.CheckActiveButton();
            }
        }
    }

    /// <summary>
    /// TrapButton以外全てのBuyButtonの状態を更新する
    /// </summary>
    public void CheckAllBuyButton()
    {
        foreach (var buyButton in buyButtons)
        {
            if (!buyButton.isTrap)
            {
                buyButton.CheckActiveButton();
            }
        }
    }

    /// <summary>
    /// 全てのMyHandButtonの状態を更新する
    /// </summary>
    public void CheckAllMyHandButton()
    {
        foreach (var myHandButton in myHandButtons)
        {
            myHandButton.CheckActiveButton();
        }
    }

    /// <summary>
    /// 全てのBuyButtonの状態をオフにする
    /// </summary>
    public void OffAllBuyButton()
    {
        foreach (var buyButton in buyButtons)
        {
            buyButton.OffActiveButton();
        }
    }

    /// <summary>
    /// 全てのMyHandButtonの状態をオフにする
    /// </summary>
    public void OffAllMyHandButton()
    {
        foreach (var myHandButton in myHandButtons)
        {
            myHandButton.OffActiveButton();
        }
    }

    /// <summary>
    /// 出陣するタンクをリセット処理
    /// </summary>
    public void OnClickReSetSlot()
    {
        inGameManager.soundManager.PlayClickButton();
        //Trapで守備するステップ
        if (inGameManager._P1.stepName == "Battle1")
        {
            for (int i = 0; i < setSlotButtons.Count; i++)
            {
                if (setSlotButtons[i].buttonName != "")
                {
                    for (int j = 0; j < buyButtons.Count; j++)
                    {
                        if (setSlotButtons[i].buttonName == buyButtons[j].buttonName && buyButtons[j].isTrap)
                        {
                            buyButtons[j].amount++;
                        }
                    }
                }
                setSlotButtons[i].buttonName = "";
                setSlotButtons[i].SetSprite();
                inGameManager._P1.slotDatas[i] = "";
            }
            setSlotCounter = 0;
            CheckAllTrapButton();
        }
        //出陣するタンクを決めるステップ
        else
        {
            for (int i = 0; i < setSlotButtons.Count; i++)
            {
                if (setSlotButtons[i].buttonName != "")
                {
                    for (int j = 0; j < myHandButtons.Count; j++)
                    {
                        if (setSlotButtons[i].buttonName == myHandButtons[j].buttonName)
                        {
                            myHandButtons[j].amount++;
                            inGameManager._P1.handNumber++;
                        }
                    }
                }
                setSlotButtons[i].buttonName = "";
                setSlotButtons[i].SetSprite();
                inGameManager._P1.slotDatas[i] = "";
            }
            setSlotCounter = 0;
            CheckAllMyHandButton();
        }
    }

    /// <summary>
    /// 出陣が決まった後、SetSlotButtonを初期化
    /// </summary>
    public void ReSetSlotButton()
    {
        for (int i = 0; i < setSlotButtons.Count; i++)
        {
            setSlotButtons[i].buttonName = "";
            setSlotButtons[i].SetSprite();
        }
        setSlotCounter = 0;
    }

    /// <summary>
    /// 先攻文字のアニメーションを再生
    /// </summary>
    public void PlayFirstText()
    {
        StartCoroutine(PlayFirstTextIE());
    }
    IEnumerator PlayFirstTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        firstText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        firstText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 後攻文字のアニメーションを再生
    /// </summary>
    public void PlaySecondText()
    {
        StartCoroutine(PlaySecondTextIE());
    }
    IEnumerator PlaySecondTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        secondText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        secondText.SetBool("isPlay", false);
    }

    /// <summary>
    /// あなたのターン文字のアニメーションを再生
    /// </summary>
    public void PlayMyTurnText()
    {
        StartCoroutine(PlayMyTurnTextIE());
    }
    IEnumerator PlayMyTurnTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        myTurnText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        myTurnText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 相手のターン文字のアニメーションを再生
    /// </summary>
    public void PlayOPTurnText()
    {
        StartCoroutine(PlayOPTurnTextIE());
    }
    IEnumerator PlayOPTurnTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        opTurnText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        opTurnText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 攻撃I文字のアニメーションを再生
    /// </summary>
    public void PlayAttackIText()
    {
        StartCoroutine(PlayAttackITextIE());
    }
    IEnumerator PlayAttackITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        attackIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        attackIText.SetBool("isPlay", false);
    }
    /// <summary>
    /// 攻撃II文字のアニメーションを再生
    /// </summary>
    public void PlayAttackIIText()
    {
        StartCoroutine(PlayAttackIITextIE());
    }
    IEnumerator PlayAttackIITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        attackIIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        attackIIText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 守備I文字のアニメーションを再生
    /// </summary>
    public void PlayDefendIText()
    {
        StartCoroutine(PlayDefendITextIE());
    }
    IEnumerator PlayDefendITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        defendIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        defendIText.SetBool("isPlay", false);
    }
    /// <summary>
    /// 守備II文字のアニメーションを再生
    /// </summary>
    public void PlayDefendIIText()
    {
        StartCoroutine(PlayDefendIITextIE());
    }
    IEnumerator PlayDefendIITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        defendIIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        defendIIText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 自分の終了文字のアニメーションを再生
    /// </summary>
    public void PlayMyEndText()
    {
        StartCoroutine(PlayMyEndTextIE());
    }
    IEnumerator PlayMyEndTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        endMYText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        endMYText.SetBool("isPlay", false);
    }
    /// <summary>
    /// 相手の終了文字のアニメーションを再生
    /// </summary>
    public void PlayOPEndText()
    {
        StartCoroutine(PlayOPEndTextIE());
    }
    IEnumerator PlayOPEndTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //アニメーションを再生した後、初期化する
        endOPText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        endOPText.SetBool("isPlay", false);
    }

    /// <summary>
    /// 勝利文字のアニメショーンを再生
    /// </summary>
    public void PlayWinText()
    {
        inGameManager.soundManager.PlayWin();
        result.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
        winText.Play("Result");
        StartCoroutine(ActiveBackButton());
    }
    /// <summary>
    /// 敗北文字のアニメショーンを再生
    /// </summary>
    public void PlayLostText()
    {
        inGameManager.soundManager.PlayLose();
        result.gameObject.SetActive(true);
        lostText.gameObject.SetActive(true);
        lostText.Play("Result");
        StartCoroutine(ActiveBackButton());
    }
    IEnumerator ActiveBackButton()
    {
        yield return new WaitForSeconds(1);
        backButton.SetActive(true);
    }

    /// <summary>
    /// 相手を待つ文字のアニメショーンを再生
    /// </summary>
    public void PlayOPLoadingText()
    {
        opLoadingText.gameObject.SetActive(true);
        StartCoroutine(PlayOPLoadingTextIE());
    }
    IEnumerator PlayOPLoadingTextIE()
    {
        opLoadingText.text = "相手を待つ";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "相手を待つ.";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "相手を待つ..";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "相手を待つ...";
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(PlayOPLoadingTextIE());
    }
    /// <summary>
    /// 相手を待つ文字のアニメショーンを中止
    /// </summary>
    public void StopOPLoadingText()
    {
        StopCoroutine(PlayOPLoadingTextIE());
        opLoadingText.gameObject.SetActive(false);
    }
}
