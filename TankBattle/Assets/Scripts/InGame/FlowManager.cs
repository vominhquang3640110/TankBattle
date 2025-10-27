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
            //これはチュートリアルである
            tutorialManager = TutorialManager.Instance;
        }

        //プレイヤーを変数に格納し、先攻・後攻が決めるまで待つ
        while (!inGameManager.isGetPlayer)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        //一回buyButtonをすべて無効に
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
    /// ゲームの流れを制御するコルーチン
    /// </summary>
    public IEnumerator Flow()
    {
        turnCounter++;  //ターン+1

        if (inGameManager._P1.isTurn)
        {
            //あなたのターン文字を表示
            uiManager.PlayMyTurnText();
            yield return new WaitForSeconds(2);

            //GetCoinステップに移す
            inGameManager._P1.stepName = "GetCoin";
            //ゲットコインボタンを表示させる
            uiManager.background.SetActive(true);
            uiManager.getCoinButton.SetActive (true);
            uiManager.getCoinButton.GetComponent<Button>().interactable = true;

            //チュートリアルである
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

            //Attackステップに移すまで待つ
            while (inGameManager._P1.stepName != "Attack")
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);

            if (turnCounter > 1)
            {
                //出陣するタンクのボックスと決定ボタンを表示させる
                uiManager.setBox.SetActive(true);
                //一回buyButtonをすべて無効に
                uiManager.OffAllBuyButton();
                //MyHandButtonの状態を更新
                uiManager.CheckAllMyHandButton();
                //TrapButton以外BuyButtonの状態を更新
                uiManager.CheckAllBuyButton();
                //決定ボタンを有効に
                uiManager.decisionButton.OnActiveButton();

                //Battle1ステップまで待つ
                while (inGameManager._P1.stepName != "Battle1")
                {
                    yield return null;
                }
                //buyButtonとmyHandButtonをすべて無効に
                uiManager.OffAllBuyButton();
                uiManager.OffAllMyHandButton();
                //backgroundをsetBoxを非表示に
                uiManager.background.SetActive (false);
                uiManager.setBox.SetActive (false);
                //相手のステップがBattle1ステップになるまで待つ
                while (inGameManager._P2.stepName != "Battle1")
                {
                    yield return null;
                }
                //相手を待つ文字のアニメショーンを中止
                uiManager.StopOPLoadingText();
                //攻撃I文字を表示
                uiManager.PlayAttackIText();
                yield return new WaitForSeconds(2);
                //バトル1回目
                yield return StartCoroutine(tankManager.Battle1Flow());

                //backgroundをsetBoxを表示に(setBoxP1を非表示)
                uiManager.background.SetActive(true);
                uiManager.setBoxP1.SetActive(false);
                uiManager.setBox.SetActive(true);
                //相手を待つ文字のアニメショーンを再生
                uiManager.PlayOPLoadingText();

                //チュートリアルである
                if (tutorialManager != null && tutorialManager.stepCounter == 12)
                {
                    yield return new WaitForSeconds(2);
                    tutorialManager.TutorialProcess_08();
                }

                while (inGameManager._P2.stepName != "Battle2")
                {
                    yield return null;
                }
                //相手を待つ文字のアニメショーンを中止
                uiManager.StopOPLoadingText();
                //backgroundをsetBoxを非表示に(setBoxP1を表示)
                uiManager.background.SetActive(false);
                uiManager.setBoxP1.SetActive(true);
                uiManager.setBox.SetActive(false);
                //攻撃II文字を表示
                uiManager.PlayAttackIIText();
                yield return new WaitForSeconds(2);
                //バトル2回目
                yield return StartCoroutine(tankManager.Battle2Flow());

                //まだ攻撃していないタンクの数を数える
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
                    //P2へ攻撃
                    yield return StartCoroutine(tankManager.PlayerAttack(canAttackTank, tankManager.P2Transform));
                }

                //チュートリアルである
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
            //相手のターン文字を表示
            uiManager.PlayOPTurnText();
            yield return new WaitForSeconds(2);

            if (turnCounter > 1)
            {
                //Defendステップに移す
                inGameManager._P1.stepName = "Defend";

                //一回buyButtonをすべて無効に
                uiManager.OffAllBuyButton();
                //TrapButton以外BuyButtonの状態を更新
                uiManager.CheckAllBuyButton();
                //MyHandButtonの状態を更新
                uiManager.CheckAllMyHandButton();
                //決定ボタンを有効に
                uiManager.decisionButton.OnActiveButton();

                //チュートリアルである
                if (tutorialManager != null && tutorialManager.stepCounter == 2)
                {
                    tutorialManager.Message_04();
                    while (tutorialManager.stepCounter == 2)
                    {
                        yield return null;
                    }
                }

                //出陣するタンクのボックスと決定ボタンを表示させる
                uiManager.background.SetActive(true);
                uiManager.setBox.SetActive(true);

                //Battle1ステップまで待つ
                while (inGameManager._P1.stepName != "Battle1")
                {
                    yield return null;
                }
                //buyButtonとmyHandButtonをすべて無効に
                uiManager.OffAllBuyButton();
                uiManager.OffAllMyHandButton();
                //backgroundをsetBoxを非表示に
                uiManager.background.SetActive(false);
                uiManager.setBox.SetActive(false);

                //チュートリアルである
                if (tutorialManager != null && tutorialManager.stepCounter == 3)
                {
                    tutorialManager.TutorialProcess_06();
                }

                //相手のステップがBattle1ステップになるまで待つ
                while (inGameManager._P2.stepName != "Battle1")
                {
                    yield return null;
                }
                //相手を待つ文字のアニメショーンを中止
                uiManager.StopOPLoadingText();
                //守備I文字を表示
                uiManager.PlayDefendIText();

                yield return new WaitForSeconds(2);
                //バトル一回目
                yield return StartCoroutine(tankManager.Battle1Flow());

                //第一回終わったら、slotDatasをクリア
                for (int i = 0; i < inGameManager._P1.slotDatas.Length; i++)
                {
                    inGameManager._P1.slotDatas[i] = "";
                }
                //決定ボタンを有効に
                uiManager.decisionButton.OnActiveButton();
                //TrapButtonの状態だけ更新
                uiManager.CheckAllTrapButton();
                
                //チュートリアルである
                if (tutorialManager != null && tutorialManager.stepCounter == 4)
                {
                    tutorialManager.Message_19();
                    while (tutorialManager.stepCounter == 4)
                    {
                        yield return null;
                    }
                }

                //backgroundをsetBoxを表示に(setBoxP2を非表示)
                uiManager.background.SetActive(true);
                uiManager.setBoxP2.SetActive(false);
                uiManager.setBox.SetActive(true);
                while (inGameManager._P1.stepName != "Battle2")
                {
                    yield return null;
                }

                //backgroundをsetBoxを非表示に(setBoxP1を表示)
                uiManager.background.SetActive(false);
                uiManager.setBoxP2.SetActive(true);
                uiManager.setBox.SetActive(false);
                //BuyButtonをすべて無効に
                uiManager.OffAllBuyButton();
                //守備II文字を表示
                uiManager.PlayDefendIIText();
                yield return new WaitForSeconds(2);
                //バトル2回目
                yield return StartCoroutine(tankManager.Battle2Flow());

                //まだ攻撃していないタンクの数を数える
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
                    //P2へ攻撃
                    yield return StartCoroutine(tankManager.PlayerAttack(canAttackTank, tankManager.P1Transform));
                }

                //チュートリアルである
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
                //相手を待つ文字のアニメショーンを再生
                uiManager.PlayOPLoadingText();
                //相手のステップがEndステップに移すまで待つ
                while (inGameManager._P2.stepName != "End")
                {
                    yield return null;
                }
                //相手を待つ文字のアニメショーンを中止
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

        //ターンの終わりにいじったパラメータを初期化する
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
            //チュートリアルである
            if (tutorialManager != null)
            {
                for (int i = 0; i < inGameManager._P2.slotDatas.Length; i++)
                {
                    inGameManager._P2.slotDatas[i] = "";
                }
                inGameManager._P2.stepName = "End";
            }
        }

        //終了文字のアニメショーンを再生
        if (inGameManager._P1.isTurn)
        {
            uiManager.PlayMyEndText();
        }
        else
        {
            
            uiManager.PlayOPEndText();
        }
        yield return new WaitForSeconds(2);
        //ターン入替
        inGameManager._P1.isTurn = !inGameManager._P1.isTurn;
        inGameManager._P2.isTurn = !inGameManager._P2.isTurn;

        if (!inGameManager.isSetGame)
        {
            //ループする
            StartCoroutine(Flow());
        }
    }
}
