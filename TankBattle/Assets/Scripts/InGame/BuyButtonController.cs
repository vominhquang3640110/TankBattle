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
        //����\������(����0��菬�����Ȃ�X�V���Ȃ��Ă���)
        if (amount < 1000)
        {
            amountText.text = amount.ToString();
        }
        
    }

    /// <summary>
    /// �{�^���̏�Ԃ��X�V
    /// </summary>
    public void CheckActiveButton()
    {
        //�R�C��������A�����鐔���c���Ă���ꍇ
        if (inGameManager._ControllerPlayer.currentCoin >= cost && amount > 0)
        {
            //�{�^����L���ɂ��A���̐F��߂�
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = Color.white;
            isActive = true;
        }
        else
        {
            //�{�^���𖳌��ɂ���
            OffActiveButton();
        }
    }

    /// <summary>
    /// �{�^���𖳌��ɂ���
    /// </summary>
    public void OffActiveButton()
    {
        //�{�^���𖳌��ɂ��A�F��ς���
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = Color.red;
        isActive = false;
    }

    /// <summary>
    /// Recycle���N���b�N����ۂ̏���
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
    /// buyButton���N���b�N����ۂ̏���
    /// </summary>
    public void OnClickBuyButton()
    {
        inGameManager.soundManager.PlayClickButton();
        amount--;   //�c��̐���1���炷

        if (!isTrap)
        {
            //myHandButton��amount�𒼐ڂ�1���₷
            for (int i = 0; i < uiManager.myHandButtons.Count; i++)
            {
                if (buttonName == uiManager.myHandButtons[i].buttonName)
                {
                    uiManager.myHandButtons[i].amount++;
                }
            }

            inGameManager._ControllerPlayer.handNumber++;     //�v���C���[�̏��L�^���N+1

            //���L�R�C����cost�����炷
            uiManager.UseCoin(cost);

            //�`���[�g���A���ł���
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
            //P1��slotDatas���X�V
            inGameManager._ControllerPlayer.slotDatas[uiManager.setSlotCounter] = buttonName;

            //SetSlot�̏����X�V
            uiManager.setSlotButtons[uiManager.setSlotCounter].buttonName = buttonName;
            uiManager.setSlotButtons[uiManager.setSlotCounter].SetSprite();

            CheckActiveButton();    //���̃{�^���̏�Ԃ��X�V
            uiManager.setSlotCounter++;     //����SetSlot��

            //SetSlot�����^���ł����myHandButton�S�������ɂ���
            if (uiManager.setSlotCounter > 4)
            {
                uiManager.OffAllBuyButton();
            }
            //�`���[�g���A���ł���
            if (inGameManager.userManager.isTutorialMode && TutorialManager.Instance.stepCounter == 5)
            {
                TutorialManager.Instance.Message_21();
            }
        }
    }
}
