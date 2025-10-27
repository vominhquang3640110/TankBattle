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
        //�U���X�e�b�v�����A�U���^���N�ł͂Ȃ��ꍇ�A�{�^���𖳌��ɂ���
        if (inGameManager._ControllerPlayer.stepName == "Attack" && !isATK)
        {
            OffActiveButton();
        }
        //����X�e�b�v�����A����^���N�ł͂Ȃ��ꍇ�A�{�^���𖳌��ɂ���
        if (inGameManager._ControllerPlayer.stepName == "Defend" && !isDEF)
        {
            OffActiveButton();
        }

        //����\������
        amountText.text = amount.ToString();
    }

    /// <summary>
    /// �{�^���̏�Ԃ��X�V
    /// </summary>
    public void CheckActiveButton()
    {
        //�R�C��������A�����鐔���c���Ă���ꍇ
        if (amount > 0)
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

    public void OnClickMyHandButton()
    {
        inGameManager.soundManager.PlayClickButton();
        //P1��slotDatas�Ə��L�^���N�̐����X�V
        inGameManager._ControllerPlayer.handNumber--;
        inGameManager._ControllerPlayer.slotDatas[uiManager.setSlotCounter] = buttonName;

        amount--;   //�c��̐���-1

        //SetSlot�̏����X�V
        uiManager.setSlotButtons[uiManager.setSlotCounter].buttonName = buttonName;
        uiManager.setSlotButtons[uiManager.setSlotCounter].SetSprite();

        CheckActiveButton();    //���̃{�^���̏�Ԃ��X�V
        uiManager.setSlotCounter++;     //����SetSlot��

        //SetSlot�����^���ł����myHandButton�S�������ɂ���
        if (uiManager.setSlotCounter > 4)
        {
            uiManager.OffAllMyHandButton();
        }

        //�`���[�g���A���ł���
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
