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
    public bool isGetPlayer;  //�v���C���[�S�����[���ɓ��������ǂ���
    public PlayerInfor _P1;   //�v���C���[1(����)
    public PlayerInfor _P2;   //�v���C���[2(����)
    public PlayerInfor _ControllerPlayer;   //�Q�b�g�R�C���Ȃǂ̑ΏۂƂ���v���C���[�B�`���[�g���A���ł͏�ɕύX�����

    public bool isSetGame = false;

    //InGameManager���V���O���g���N���X�ɂ���
    void Awake()
    {
        _ControllerPlayer = _P1;

        //BGM��؂�ւ�
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

        //�`���[�g���A���łȂ�
        if (!UserManager.instance.isTutorialMode)
        {
            //�v���C���[�𐶐�
            PhotonNetwork.Instantiate("PlayerInfor", Vector3.zero, Quaternion.identity, 0);
        }
    }

    void Start()
    {
        //�}�l�[�W���[���i�[
        userManager = UserManager.instance;
        prefabManager = GetComponent<PrefabManager>();
        uiManager = GetComponent<UIManager>();
        tankManager = GetComponent<TankManager>();
        flowManager = GetComponent<FlowManager>();
        //�`���[�g���A���ł���
        if (userManager.isTutorialMode)
        {
            tutorialManager = TutorialManager.Instance;
        }

        //UserManager����f�[�^���Q��
        buyButtonDatas = userManager.buyButtonDatas;
        trapButtonDatas = userManager.trapButtonDatas;

        //buyButtonDatas����myHand�f�[�^���Z�b�g����
        myHandDatas = new Dictionary<string, int>();
        foreach (var buyButtonData in buyButtonDatas)
        {
            myHandDatas.Add(buyButtonData.Key + "_myHand", 0);
        }
        //�e�{�^�����Z�b�g����
        SetSlotButton(buyButtonDatas, buySlotDatas);
        SetSlotButton(trapButtonDatas, trapSlotDatas);
        SetMyHandButton();
    }

    void Update()
    {
        SetPlayerInfor();

        if (isGetPlayer)
        {
            //�~�Q����
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
                //��������������
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayWinText();
            }
            else if (_P2.isWinGame && !isSetGame)
            {
                //���肪��������
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayLostText();
            }
            else if (players.Length < 2 && !isSetGame)
            {
                //���肪���^�C�A������
                isSetGame = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                uiManager.PlayWinText();
            }
        }
    }

    /// <summary>
    /// BuyButton�𐶐����ăZ�b�g����
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

            //UIManager�ɂ��i�[����
            uiManager.buyButtons.Add(slotButton.GetComponent<BuyButtonController>());
            //�`���[�g���A���ł���
            if (tutorialManager != null)
            {
                tutorialManager.buttons.Add(slotButton.GetComponent<Button>());
            }
        }
        uiManager.buyButtons.Add(recycleButton);
    }

    /// <summary>
    /// MyHandButton�𐶐����ăZ�b�g����
    /// </summary>
    void SetMyHandButton()
    {
        foreach (var myHandData in myHandDatas)
        {
            GameObject myHandButton = Instantiate(prefabManager.GetButtonPrefab(myHandData.Key),
                                        myHandContent.position, Quaternion.identity, myHandContent);
            myHandButton.GetComponent<MyHandButtonController>().CheckActiveButton();
            myHandButton.GetComponent<MyHandButtonController>().amount = myHandData.Value;
            //UIManager�ɂ��i�[����
            uiManager.myHandButtons.Add(myHandButton.GetComponent<MyHandButtonController>());
            //�`���[�g���A���ł���
            if (tutorialManager != null)
            {
                tutorialManager.buttons.Add(myHandButton.GetComponent<Button>());
            }
        }
    }

    /// <summary>
    /// �v���C���[����ϐ��Ɋi�[
    /// </summary>
    void SetPlayerInfor()
    {
        if (!isGetPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length >= 2)
            {
                isGetPlayer = true;

                //�`���[�g���A���łȂ�
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

                    //��U�E��U�����߂�
                    StartCoroutine(IsFirstTurn());
                }
                else
                {
                    //�`���[�g���A���̏ꍇ�A��������U�ɂȂ�
                    _P1.isTurn = true;
                    uiManager.PlayFirstText();
                }
            }
        }
    }

    /// <summary>
    /// ��U�E��U�����߂�R���[�`��
    /// </summary>
    IEnumerator IsFirstTurn()
    {
        //�e�v���C���[�������𐶐�����܂ő҂�
        while (_P1.randomNumberTurn == 0 && _P2.randomNumberTurn == 0)
        {
            yield return null;
        }

        //�v���C���[1��2���������������ɂ���Đ�U�E��U�����܂�
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
            //�������������������̏ꍇ�́A���[���̃z�X�g����U�ɂȂ�
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
    /// �^�C�g����ʂɖ߂�
    /// </summary>
    public void OnClickBackTitle()
    {
        soundManager.PlayClickButton();
        soundManager.StopBGMInGame();
        SceneManager.LoadScene("Title");
    }
    /// <summary>
    /// �~�Q����
    /// </summary>
    public void OnClickSurrender()
    {
        soundManager.PlayClickButton();
        _P1.isSurrender = true;
    }
}
