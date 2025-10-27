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
    /// �{�^����L���ɂ���
    /// </summary>
    public void OnActiveButton()
    {
        //�{�^����L���ɂ��A���̐F��߂�
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// �{�^���𖳌��ɂ���
    /// </summary>
    public void OffActiveButton()
    {
        //�A�j���[�V�����𖳌��ɂ��A�{�^���𖳌��ɂ��A�F��ς���
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = Color.red;
    }

    /// <summary>
    /// Decision�{�^�����N���b�N����ۂ̏���
    /// </summary>
    public void OnClickDecisionButton()
    {
        inGameManager.soundManager.PlayClickButton();

        //�`���[�g���A���ł���
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
            //�����҂����̃A�j���V���[���𒆎~
            uiManager.PlayOPLoadingText();
            //battle1�Ɉڂ�
            inGameManager._ControllerPlayer.stepName = "Battle1";
            //SetSlot��������
            uiManager.ReSetSlotButton();
            
        }
        else if (inGameManager._ControllerPlayer.stepName == "Battle1")
        {
            inGameManager._ControllerPlayer.stepName = "Battle2";
        }
        OffActiveButton();
    }
}
