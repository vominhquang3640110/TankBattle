using ExitGames.Demos.DemoAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    [HideInInspector] public UserManager userManager;
    [HideInInspector] public PrefabManager prefabManager;
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public TankManager tankManager;
    [HideInInspector] public FlowManager flowManager;
    [HideInInspector] public SoundManager soundManager;
    private TutorialManager tutorialManager;

    [Header("ButtonPosition")]
    public List<Transform> buySlotDatas;
    public List<Transform> trapSlotDatas;
    public BuyButtonController recycleButton;

    [Header("ButtonDatas")]
    public Dictionary<string, int> buyButtonDatas;
    public Dictionary<string, int> trapButtonDatas;

    [Header("MyHandDatas")]
    public Transform myHandContent;
    public Dictionary<string, int> myHandDatas;

    [Header("PhotonDatas")]
    public bool isGetPlayer;  //プレイヤー全員ルームに入ったかどうか
    public PlayerInfor _P1;   //プレイヤー1(自分)
    public PlayerInfor _P2;   //プレイヤー2(相手)
    public PlayerInfor _ControllerPlayer;   //ゲットコインなどの対象とするプレイヤー。チュートリアルでは常に変更される

    public bool isSetGame = false;

    //InGameManagerをシングルトンクラスにする
    void Awake()
    {
        _ControllerPlayer = _P1;

        //BGMを切り替え
        soundManager = SoundManager.instance;
        soundManager.StopBGM();
        soundManager.PlayBGMInGame();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        //チュートリアルでない
        if (!UserManager.instance.isTutorialMode)
        {
            //プレイヤーを生成
            PhotonNetwork.Instantiate("PlayerInfor", Vector3.zero, Quaternion.identity, 0);
        }
    }

    void Start()
    {
        //マネージャーを格納
        userManager = UserManager.instance;
        prefabManager = GetComponent<PrefabManager>();
        uiManager = GetComponent<UIManager>();
        tankManager = GetComponent<TankManager>();
        flowManager = GetComponent<FlowManager>();
        //チュートリアルである
        if (userManager.isTutorialMode)
        {
            tutorialManager = TutorialManager.Instance;
        }

        //UserManagerからデータを参照
        buyButtonDatas = userManager.buyButtonDatas;
        trapButtonDatas = userManager.trapButtonDatas;

        //buyButtonDatasからmyHandデータをセットする
        myHandDatas = new Dictionary<string, int>();
        foreach (var buyButtonData in buyButtonDatas)
        {
            myHandDatas.Add(buyButtonData.Key + "_myHand", 0);
        }
        //各ボタンをセットする
        SetSlotButton(buyButtonDatas, buySlotDatas);
        SetSlotButton(trapButtonDatas, trapSlotDatas);
        SetMyHandButton();
    }

    void Update()
    {
        SetPlayerInfor();

        if (isGetPlayer)
        {
            //降参処理
            if (_P1.isSurrender && !_P2.isWinGame)
            {
                _P2.isWinGame = true;
            }
            else if (_P2.isSurrender && !_P1.isWinGame)
            {
                _P1.isWinGame = true;
            }

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (_P1.isWinGame && !isSetGame)
            {
                //自分が勝った時
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayWinText();
            }
            else if (_P2.isWinGame && !isSetGame)
            {
                //相手が勝った時
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayLostText();
            }
            else if (players.Length < 2 && !isSetGame)
            {
                //相手がリタイアした時
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayWinText();
            }
        }
    }

    /// <summary>
    /// BuyButtonを生成してセットする
    /// </summary>
    void SetSlotButton(Dictionary<string, int> slotButtonDatas, List<Transform> slotDatas)
    {
        int index = 0;
        foreach (var slotButtonData in slotButtonDatas)
        {
            GameObject slotButton = Instantiate(prefabManager.GetButtonPrefab(slotButtonData.Key), 
                                    slotDatas[index].position, Quaternion.identity, slotDatas[index]);
            slotButton.GetComponent<BuyButtonController>().amount = slotButtonData.Value;
            slotButton.GetComponent<BuyButtonController>().OffActiveButton();
            index++;

            //UIManagerにも格納する
            uiManager.buyButtons.Add(slotButton.GetComponent<BuyButtonController>());
            //チュートリアルである
            if (tutorialManager != null)
            {
                tutorialManager.buttons.Add(slotButton.GetComponent<Button>());
            }
        }
        uiManager.buyButtons.Add(recycleButton);
    }

    /// <summary>
    /// MyHandButtonを生成してセットする
    /// </summary>
    void SetMyHandButton()
    {
        foreach (var myHandData in myHandDatas)
        {
            GameObject myHandButton = Instantiate(prefabManager.GetButtonPrefab(myHandData.Key),
                                        myHandContent.position, Quaternion.identity, myHandContent);
            myHandButton.GetComponent<MyHandButtonController>().CheckActiveButton();
            myHandButton.GetComponent<MyHandButtonController>().amount = myHandData.Value;
            //UIManagerにも格納する
            uiManager.myHandButtons.Add(myHandButton.GetComponent<MyHandButtonController>());
            //チュートリアルである
            if (tutorialManager != null)
            {
                tutorialManager.buttons.Add(myHandButton.GetComponent<Button>());
            }
        }
    }

    /// <summary>
    /// プレイヤー情報を変数に格納
    /// </summary>
    void SetPlayerInfor()
    {
        if (!isGetPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length >= 2)
            {
                isGetPlayer = true;

                //チュートリアルでない
                if (!userManager.isTutorialMode)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].GetComponent<PlayerInfor>()._photonView.isMine)
                        {
                            _P1 = players[i].GetComponent<PlayerInfor>();
                            _ControllerPlayer = _P1;
                        }
                        else
                        {
                            _P2 = players[i].GetComponent<PlayerInfor>();
                        }
                    }

                    //先攻・後攻を決める
                    StartCoroutine(IsFirstTurn());
                }
                else
                {
                    //チュートリアルの場合、自分が先攻になる
                    _P1.isTurn = true;
                    uiManager.PlayFirstText();
                }
            }
        }
    }

    /// <summary>
    /// 先攻・後攻を決めるコルーチン
    /// </summary>
    IEnumerator IsFirstTurn()
    {
        //各プレイヤーが乱数を生成するまで待つ
        while (_P1.randomNumberTurn == 0 && _P2.randomNumberTurn == 0)
        {
            yield return null;
        }

        //プレイヤー1と2が生成した乱数によって先攻・後攻が決まる
        if (_P1.randomNumberTurn > _P2.randomNumberTurn)
        {
            _P1.isTurn = true;
            uiManager.PlayFirstText();
        }
        else if (_P1.randomNumberTurn < _P2.randomNumberTurn)
        {
            _P2.isTurn = true;
            uiManager.PlaySecondText();
        }
        else
        {
            //生成した乱数が同じの場合は、ルームのホストが先攻になる
            if (PhotonNetwork.isMasterClient)
            {
                _P1.isTurn = true;
                uiManager.PlayFirstText();
            }
            else
            {
                _P2.isTurn = true;
                uiManager.PlaySecondText();
            }
        }
    }
    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void OnClickBackTitle()
    {
        soundManager.PlayClickButton();
        soundManager.StopBGMInGame();
        SceneManager.LoadScene("Title");
    }
    /// <summary>
    /// 降参する
    /// </summary>
    public void OnClickSurrender()
    {
        soundManager.PlayClickButton();
        _P1.isSurrender = true;
    }
}
