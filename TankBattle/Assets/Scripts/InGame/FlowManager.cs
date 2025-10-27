using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour
{
    private InGameManager inGameManager;
    private UIManager uiManager;
    private TankManager tankManager;
    private TutorialManager tutorialManager;
    private Coroutine flowCoroutine;

    public int turnCounter = 0;

    IEnumerator Start()
    {
        inGameManager = InGameManager.instance;
        uiManager = inGameManager.uiManager;
        tankManager = inGameManager.tankManager;
        if (inGameManager.userManager.isTutorialMode)
        {
            //����̓`���[�g���A���ł���
            tutorialManager = TutorialManager.Instance;
        }

        //�v���C���[��ϐ��Ɋi�[���A��U�E��U�����߂�܂ő҂�
        while (!inGameManager.isGetPlayer)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        //���buyButton�����ׂĖ�����
        uiManager.OffAllBuyButton();

        flowCoroutine = StartCoroutine(Flow());
    }

    private void Update()
    {
        if (inGameManager.isSetGame)
        {
            if (flowCoroutine != null)
            {
                StopCoroutine(flowCoroutine);
                flowCoroutine = null;
            }
        }
    }

    /// <summary>
    /// �Q�[���̗���𐧌䂷��R���[�`��
    /// </summary>
    public IEnumerator Flow()
    {
        turnCounter++;  //�^�[��+1

        if (inGameManager._P1.isTurn)
        {
            //���Ȃ��̃^�[��������\��
            uiManager.PlayMyTurnText();
            yield return new WaitForSeconds(2);

            //GetCoin�X�e�b�v�Ɉڂ�
            inGameManager._P1.stepName = "GetCoin";
            //�Q�b�g�R�C���{�^����\��������
            uiManager.background.SetActive(true);
            uiManager.getCoinButton.SetActive (true);
            uiManager.getCoinButton.GetComponent<Button>().interactable = true;

            //�`���[�g���A���ł���
            if (tutorialManager != null)
            {
                if (tutorialManager.stepCounter == 1)
                {
                    tutorialManager.Message_01();
                    while (tutorialManager.stepCounter == 1)
                    {
                        yield return null;
                    }
                }
                else if (tutorialManager.stepCounter == 6)
                {
                    tutorialManager.Message_23();
                }
            }

            //Attack�X�e�b�v�Ɉڂ��܂ő҂�
            while (inGameManager._P1.stepName != "Attack")
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);

            if (turnCounter > 1)
            {
                //�o�w����^���N�̃{�b�N�X�ƌ���{�^����\��������
                uiManager.setBox.SetActive(true);
                //���buyButton�����ׂĖ�����
                uiManager.OffAllBuyButton();
                //MyHandButton�̏�Ԃ��X�V
                uiManager.CheckAllMyHandButton();
                //TrapButton�ȊOBuyButton�̏�Ԃ��X�V
                uiManager.CheckAllBuyButton();
                //����{�^����L����
                uiManager.decisionButton.OnActiveButton();

                //Battle1�X�e�b�v�܂ő҂�
                while (inGameManager._P1.stepName != "Battle1")
                {
                    yield return null;
                }
                //buyButton��myHandButton�����ׂĖ�����
                uiManager.OffAllBuyButton();
                uiManager.OffAllMyHandButton();
                //background��setBox���\����
                uiManager.background.SetActive (false);
                uiManager.setBox.SetActive (false);
                //����̃X�e�b�v��Battle1�X�e�b�v�ɂȂ�܂ő҂�
                while (inGameManager._P2.stepName != "Battle1")
                {
                    yield return null;
                }
                //�����҂����̃A�j���V���[���𒆎~
                uiManager.StopOPLoadingText();
                //�U��I������\��
                uiManager.PlayAttackIText();
                yield return new WaitForSeconds(2);
                //�o�g��1���
                yield return StartCoroutine(tankManager.Battle1Flow());

                //background��setBox��\����(setBoxP1���\��)
                uiManager.background.SetActive(true);
                uiManager.setBoxP1.SetActive(false);
                uiManager.setBox.SetActive(true);
                //�����҂����̃A�j���V���[�����Đ�
                uiManager.PlayOPLoadingText();

                //�`���[�g���A���ł���
                if (tutorialManager != null && tutorialManager.stepCounter == 12)
                {
                    yield return new WaitForSeconds(2);
                    tutorialManager.TutorialProcess_08();
                }

                while (inGameManager._P2.stepName != "Battle2")
                {
                    yield return null;
                }
                //�����҂����̃A�j���V���[���𒆎~
                uiManager.StopOPLoadingText();
                //background��setBox���\����(setBoxP1��\��)
                uiManager.background.SetActive(false);
                uiManager.setBoxP1.SetActive(true);
                uiManager.setBox.SetActive(false);
                //�U��II������\��
                uiManager.PlayAttackIIText();
                yield return new WaitForSeconds(2);
                //�o�g��2���
                yield return StartCoroutine(tankManager.Battle2Flow());

                //�܂��U�����Ă��Ȃ��^���N�̐��𐔂���
                List<Tank> canAttackTank = new List<Tank>();
                foreach (var tank in tankManager.battleTankListP1)
                {
                    if (tank.GetComponent<Tank>().attackNumber > 0)
                    {
                        canAttackTank.Add(tank.GetComponent<Tank>());
                    }
                }
                if (canAttackTank.Count > 0)
                {
                    //P2�֍U��
                    yield return StartCoroutine(tankManager.PlayerAttack(canAttackTank, tankManager.P2Transform));
                }

                //�`���[�g���A���ł���
                if (tutorialManager != null)
                {
                    if (tutorialManager.stepCounter == 13)
                    {
                        tutorialManager.Message_36();
                        while (tutorialManager.stepCounter == 13)
                        {
                            yield return null;
                        }
                    }
                }
            }
            else
            {
                uiManager.background.SetActive(false);
                inGameManager._P1.stepName = "End";
            }
        }
        else
        {
            //����̃^�[��������\��
            uiManager.PlayOPTurnText();
            yield return new WaitForSeconds(2);

            if (turnCounter > 1)
            {
                //Defend�X�e�b�v�Ɉڂ�
                inGameManager._P1.stepName = "Defend";

                //���buyButton�����ׂĖ�����
                uiManager.OffAllBuyButton();
                //TrapButton�ȊOBuyButton�̏�Ԃ��X�V
                uiManager.CheckAllBuyButton();
                //MyHandButton�̏�Ԃ��X�V
                uiManager.CheckAllMyHandButton();
                //����{�^����L����
                uiManager.decisionButton.OnActiveButton();

                //�`���[�g���A���ł���
                if (tutorialManager != null && tutorialManager.stepCounter == 2)
                {
                    tutorialManager.Message_04();
                    while (tutorialManager.stepCounter == 2)
                    {
                        yield return null;
                    }
                }

                //�o�w����^���N�̃{�b�N�X�ƌ���{�^����\��������
                uiManager.background.SetActive(true);
                uiManager.setBox.SetActive(true);

                //Battle1�X�e�b�v�܂ő҂�
                while (inGameManager._P1.stepName != "Battle1")
                {
                    yield return null;
                }
                //buyButton��myHandButton�����ׂĖ�����
                uiManager.OffAllBuyButton();
                uiManager.OffAllMyHandButton();
                //background��setBox���\����
                uiManager.background.SetActive(false);
                uiManager.setBox.SetActive(false);

                //�`���[�g���A���ł���
                if (tutorialManager != null && tutorialManager.stepCounter == 3)
                {
                    tutorialManager.TutorialProcess_06();
                }

                //����̃X�e�b�v��Battle1�X�e�b�v�ɂȂ�܂ő҂�
                while (inGameManager._P2.stepName != "Battle1")
                {
                    yield return null;
                }
                //�����҂����̃A�j���V���[���𒆎~
                uiManager.StopOPLoadingText();
                //���I������\��
                uiManager.PlayDefendIText();

                yield return new WaitForSeconds(2);
                //�o�g������
                yield return StartCoroutine(tankManager.Battle1Flow());

                //����I�������AslotDatas���N���A
                for (int i = 0; i < inGameManager._P1.slotDatas.Length; i++)
                {
                    inGameManager._P1.slotDatas[i] = "";
                }
                //����{�^����L����
                uiManager.decisionButton.OnActiveButton();
                //TrapButton�̏�Ԃ����X�V
                uiManager.CheckAllTrapButton();
                
                //�`���[�g���A���ł���
                if (tutorialManager != null && tutorialManager.stepCounter == 4)
                {
                    tutorialManager.Message_19();
                    while (tutorialManager.stepCounter == 4)
                    {
                        yield return null;
                    }
                }

                //background��setBox��\����(setBoxP2���\��)
                uiManager.background.SetActive(true);
                uiManager.setBoxP2.SetActive(false);
                uiManager.setBox.SetActive(true);
                while (inGameManager._P1.stepName != "Battle2")
                {
                    yield return null;
                }

                //background��setBox���\����(setBoxP1��\��)
                uiManager.background.SetActive(false);
                uiManager.setBoxP2.SetActive(true);
                uiManager.setBox.SetActive(false);
                //BuyButton�����ׂĖ�����
                uiManager.OffAllBuyButton();
                //���II������\��
                uiManager.PlayDefendIIText();
                yield return new WaitForSeconds(2);
                //�o�g��2���
                yield return StartCoroutine(tankManager.Battle2Flow());

                //�܂��U�����Ă��Ȃ��^���N�̐��𐔂���
                List<Tank> canAttackTank = new List<Tank>();
                foreach (var tank in tankManager.battleTankListP2)
                {
                    if (tank.GetComponent<Tank>().attackNumber > 0)
                    {
                        canAttackTank.Add(tank.GetComponent<Tank>());
                    }
                }
                if (canAttackTank.Count > 0)
                {
                    //P2�֍U��
                    yield return StartCoroutine(tankManager.PlayerAttack(canAttackTank, tankManager.P1Transform));
                }

                //�`���[�g���A���ł���
                if (tutorialManager != null && tutorialManager.stepCounter == 5)
                {
                    tutorialManager.Message_22();
                    while (tutorialManager.stepCounter == 5)
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                //�����҂����̃A�j���V���[�����Đ�
                uiManager.PlayOPLoadingText();
                //����̃X�e�b�v��End�X�e�b�v�Ɉڂ��܂ő҂�
                while (inGameManager._P2.stepName != "End")
                {
                    yield return null;
                }
                //�����҂����̃A�j���V���[���𒆎~
                uiManager.StopOPLoadingText();
            }
        }

        if (turnCounter > 1)
        {
            yield return new WaitForSeconds(3);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

        //�^�[���̏I���ɂ��������p�����[�^������������
        if (turnCounter > 1)
        {
            for (int i = 0; i < inGameManager._P1.slotDatas.Length; i++)
            {
                inGameManager._P1.slotDatas[i] = "";
            }
            uiManager.ReSetSlotButton();
            foreach (var tank in tankManager.battleTankListP1)
            {
                if (tank != null)
                {
                    tank.GetComponent<Tank>().TankDestroy();
                }
            }
            foreach (var tank in tankManager.battleTankListP2)
            {
                if (tank != null)
                {
                    tank.GetComponent<Tank>().TankDestroy();
                }
            }
            tankManager.battleTankListP1.Clear();
            tankManager.battleTankListP2.Clear();
            //�`���[�g���A���ł���
            if (tutorialManager != null)
            {
                for (int i = 0; i < inGameManager._P2.slotDatas.Length; i++)
                {
                    inGameManager._P2.slotDatas[i] = "";
                }
                inGameManager._P2.stepName = "End";
            }
        }

        //�I�������̃A�j���V���[�����Đ�
        if (inGameManager._P1.isTurn)
        {
            uiManager.PlayMyEndText();
        }
        else
        {
            
            uiManager.PlayOPEndText();
        }
        yield return new WaitForSeconds(2);
        //�^�[������
        inGameManager._P1.isTurn = !inGameManager._P1.isTurn;
        inGameManager._P2.isTurn = !inGameManager._P2.isTurn;

        if (!inGameManager.isSetGame)
        {
            //���[�v����
            StartCoroutine(Flow());
        }
    }
}
