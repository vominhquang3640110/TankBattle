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

        //�`���[�g���A���ł���
        if (inGameManager.userManager.isTutorialMode)
        {
            tutorialManager = TutorialManager.Instance;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        //����̃R�C���Ǝ�D��\������
        p2CoinText.text = inGameManager._P2.currentCoin.ToString();
        p2HandText.text = inGameManager._P2.handNumber.ToString();
    }

    /// <summary>
    /// �R�C�����擾���鏈��
    /// </summary>
    public void GetCoin()
    {
        //�`���[�g���A���ł���
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

        //�`���[�g���A���ł���
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
            //�v���C���[�̏��L�R�C���𑝂₷
            inGameManager._ControllerPlayer.currentCoin++;

            if (inGameManager._ControllerPlayer == inGameManager._P1)
            {
                //�o�b�O�R�C���̃A�j���[�V�������Đ�
                bagCoinIcon.Play("CoinIcon_GetMove");
                inGameManager.soundManager.PlayGetCoin();
                yield return new WaitForSeconds(1 / 3f);
                //�\������R�C�������X�V
                coinText.text = inGameManager._P1.currentCoin.ToString();
            }

            yield return new WaitForSeconds(0.1f);
        }

        //Attack�X�e�b�v�Ɉڂ�
        inGameManager._ControllerPlayer.stepName = "Attack";

        //�R�C�����擾����{�^���𖳌��ɂ���
        getCoinButton.SetActive(false);
    }

    /// <summary>
    /// ���L�R�C����ω����鏈��
    /// </summary>
    /// <param name="value">Value��+�ł���΁A���̕�����B-�ł���΁A���̕�������</param>
    public void UseCoin(int value)
    {
        inGameManager._ControllerPlayer.currentCoin -= value;
        coinText.text = inGameManager._ControllerPlayer.currentCoin.ToString();
        CheckAllBuyButton();  //�S�Ă�BuyButton�̏�Ԃ��X�V����
        CheckAllMyHandButton();   //�S�Ă�MyHandButton�̏�Ԃ��X�V
    }

    /// <summary>
    /// TrapButton�̏�Ԃ����X�V����
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
    /// TrapButton�ȊO�S�Ă�BuyButton�̏�Ԃ��X�V����
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
    /// �S�Ă�MyHandButton�̏�Ԃ��X�V����
    /// </summary>
    public void CheckAllMyHandButton()
    {
        foreach (var myHandButton in myHandButtons)
        {
            myHandButton.CheckActiveButton();
        }
    }

    /// <summary>
    /// �S�Ă�BuyButton�̏�Ԃ��I�t�ɂ���
    /// </summary>
    public void OffAllBuyButton()
    {
        foreach (var buyButton in buyButtons)
        {
            buyButton.OffActiveButton();
        }
    }

    /// <summary>
    /// �S�Ă�MyHandButton�̏�Ԃ��I�t�ɂ���
    /// </summary>
    public void OffAllMyHandButton()
    {
        foreach (var myHandButton in myHandButtons)
        {
            myHandButton.OffActiveButton();
        }
    }

    /// <summary>
    /// �o�w����^���N�����Z�b�g����
    /// </summary>
    public void OnClickReSetSlot()
    {
        inGameManager.soundManager.PlayClickButton();
        //Trap�Ŏ������X�e�b�v
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
        //�o�w����^���N�����߂�X�e�b�v
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
    /// �o�w�����܂�����ASetSlotButton��������
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
    /// ��U�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayFirstText()
    {
        StartCoroutine(PlayFirstTextIE());
    }
    IEnumerator PlayFirstTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        firstText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        firstText.SetBool("isPlay", false);
    }

    /// <summary>
    /// ��U�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlaySecondText()
    {
        StartCoroutine(PlaySecondTextIE());
    }
    IEnumerator PlaySecondTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        secondText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        secondText.SetBool("isPlay", false);
    }

    /// <summary>
    /// ���Ȃ��̃^�[�������̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayMyTurnText()
    {
        StartCoroutine(PlayMyTurnTextIE());
    }
    IEnumerator PlayMyTurnTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        myTurnText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        myTurnText.SetBool("isPlay", false);
    }

    /// <summary>
    /// ����̃^�[�������̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayOPTurnText()
    {
        StartCoroutine(PlayOPTurnTextIE());
    }
    IEnumerator PlayOPTurnTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        opTurnText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        opTurnText.SetBool("isPlay", false);
    }

    /// <summary>
    /// �U��I�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayAttackIText()
    {
        StartCoroutine(PlayAttackITextIE());
    }
    IEnumerator PlayAttackITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        attackIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        attackIText.SetBool("isPlay", false);
    }
    /// <summary>
    /// �U��II�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayAttackIIText()
    {
        StartCoroutine(PlayAttackIITextIE());
    }
    IEnumerator PlayAttackIITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        attackIIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        attackIIText.SetBool("isPlay", false);
    }

    /// <summary>
    /// ���I�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayDefendIText()
    {
        StartCoroutine(PlayDefendITextIE());
    }
    IEnumerator PlayDefendITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        defendIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        defendIText.SetBool("isPlay", false);
    }
    /// <summary>
    /// ���II�����̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayDefendIIText()
    {
        StartCoroutine(PlayDefendIITextIE());
    }
    IEnumerator PlayDefendIITextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        defendIIText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        defendIIText.SetBool("isPlay", false);
    }

    /// <summary>
    /// �����̏I�������̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayMyEndText()
    {
        StartCoroutine(PlayMyEndTextIE());
    }
    IEnumerator PlayMyEndTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        endMYText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        endMYText.SetBool("isPlay", false);
    }
    /// <summary>
    /// ����̏I�������̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayOPEndText()
    {
        StartCoroutine(PlayOPEndTextIE());
    }
    IEnumerator PlayOPEndTextIE()
    {
        inGameManager.soundManager.PlayStepChange();
        //�A�j���[�V�������Đ�������A����������
        endOPText.SetBool("isPlay", true);
        yield return new WaitForSeconds(1);
        endOPText.SetBool("isPlay", false);
    }

    /// <summary>
    /// ���������̃A�j���V���[�����Đ�
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
    /// �s�k�����̃A�j���V���[�����Đ�
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
    /// �����҂����̃A�j���V���[�����Đ�
    /// </summary>
    public void PlayOPLoadingText()
    {
        opLoadingText.gameObject.SetActive(true);
        StartCoroutine(PlayOPLoadingTextIE());
    }
    IEnumerator PlayOPLoadingTextIE()
    {
        opLoadingText.text = "�����҂�";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "�����҂�.";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "�����҂�..";
        yield return new WaitForSeconds(0.3f);
        opLoadingText.text = "�����҂�...";
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(PlayOPLoadingTextIE());
    }
    /// <summary>
    /// �����҂����̃A�j���V���[���𒆎~
    /// </summary>
    public void StopOPLoadingText()
    {
        StopCoroutine(PlayOPLoadingTextIE());
        opLoadingText.gameObject.SetActive(false);
    }
}
