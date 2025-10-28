using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TankManager : MonoBehaviour
{
    private InGameManager inGameManager;
    private PrefabManager prefabManager;
    private TutorialManager tutorialManager;

    public Transform P1Transform;
    public Transform P2Transform;

    [SerializeField] private Transform createPositionP1;
    [SerializeField] private Transform createPositionP2;
    [SerializeField] private List <Transform> battleSlotPositionP1;
    [SerializeField] private List <Transform> battleSlotPositionP2;
    public List <GameObject> battleTankListP1;
    public List <GameObject> battleTankListP2;

    public bool isBattling1 = false;
    public bool isBattling2 = false;
    public bool playerAttacking = false;
    private bool P1CreateComplete = false;
    private bool P2CreateComplete = false;

    private int attakTankIndex;
    private int counterAttack;

    void Start()
    {
        inGameManager = InGameManager.instance;
        prefabManager = inGameManager.prefabManager;
        tutorialManager = TutorialManager.Instance;

        battleTankListP1 = new List<GameObject>();
        battleTankListP2 = new List<GameObject>();
    }

    void Update()
    {

    }

    /// <summary>
    /// �U���^���N�Ǝ���^���N�̌��
    /// </summary>
    public IEnumerator Battle1Flow()
    {
        P1CreateComplete = false;
        P2CreateComplete = false ;

        attakTankIndex = 0;
        counterAttack = 0;

        //�^���N�𐶐����A���ׂ�
        StartCoroutine(TankCreateAndMove("P1", inGameManager._P1.slotDatas, createPositionP1, battleSlotPositionP1));
        StartCoroutine(TankCreateAndMove("P2", inGameManager._P2.slotDatas, createPositionP2, battleSlotPositionP2));

        //���ׂ�̂��I���܂ő҂�
        while (!P1CreateComplete || !P2CreateComplete)
        {
            yield return null;
        }

        //�`���[�g���A���ł���
        if (inGameManager.userManager.isTutorialMode)
        {
            if (tutorialManager.stepCounter == 3)
            {
                tutorialManager.Message_18();
                while (tutorialManager.stepCounter == 3)
                {
                    yield return null;
                }
            }
            else if (tutorialManager.stepCounter == 11)
            {
                tutorialManager.Message_32();
                while (tutorialManager.stepCounter == 11)
                {
                    yield return null;
                }
            }
        }

        //�N�̃^�[�����m�F���A�o�g�����s��
        if (inGameManager._P1.isTurn)
        {
            yield return StartCoroutine(TankAttack(battleTankListP1, battleTankListP2));
        }
        else
        {
            yield return StartCoroutine(TankAttack(battleTankListP2, battleTankListP1));
        }

        isBattling1 = false ;   //����o�g�����I���
    }

    /// <summary>
    /// Trap���g�p���āA�U����h���X�e�b�v
    /// </summary>
    public IEnumerator Battle2Flow()
    {
        List<int> removeIndex = new List<int>();
        //�������Trap�𐶐�
        if (inGameManager._P1.isTurn)
        {
            foreach (var tank in battleTankListP2)
            {
                if (tank != null)
                {
                    tank.GetComponent<Tank>().TankDestroy();
                }
            }
            battleTankListP2.Clear();
            //�����̃^�[���Ȃ瑊��̃^���N�𐶐�
            yield return StartCoroutine(TankCreateAndMove("P2", inGameManager._P2.slotDatas, createPositionP2, battleSlotPositionP2));
        }
        else
        {
            foreach (var tank in battleTankListP1)
            {
                if (tank != null)
                {
                    tank.GetComponent<Tank>().TankDestroy();
                }
            }
            battleTankListP1.Clear();
            //����̃^�[���Ȃ玩���̃^���N�𐶐�
            yield return StartCoroutine(TankCreateAndMove("P1", inGameManager._P1.slotDatas, createPositionP1, battleSlotPositionP1));
        }

        //�`���[�g���A���ł���
        if (inGameManager.userManager.isTutorialMode)
        {
            if (tutorialManager.stepCounter == 12)
            {
                tutorialManager.Message_33();
                while (tutorialManager.stepCounter == 12)
                {
                    yield return null;
                }
            }
        }

        //�N�̃^�[�����m�F���A�o�g�����s��
        if (inGameManager._P1.isTurn)
        {
            yield return StartCoroutine(TankAttack2(battleTankListP1, battleTankListP2));
        }
        else
        {
            yield return StartCoroutine(TankAttack2(battleTankListP2, battleTankListP1));
        }

        isBattling2 = false;   //����o�g�����I���
    }

    /// <summary>
    /// �܂��U���ł���^���N������΁A�v���C���[�ɍU��������
    /// </summary>
    /// <param name="canAttackTank">�܂��U���ł���^���N</param>
    /// <param name="target">�v���C���[�iP1/P2Transform�j</param>
    public IEnumerator PlayerAttack(List<Tank> canAttackTank, Transform target)
    {
        foreach (var tank in canAttackTank)
        {
            tank.Attack(target);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(3);
    }

    /// <summary>
    /// ���������^���N�Ńo�g�����s��
    /// </summary>
    /// <param name="attackTankList">�U������battleTankList</param>
    /// <param name="defendTtankList">�������battleTankList</param>
    /// <returns></returns>
    IEnumerator TankAttack(List<GameObject> attackTankList, List<GameObject> defendTankList)
    {
        int enemyIndex = 0;

        for (int i = 0; i < attackTankList.Count; i++)
        {
            attakTankIndex = i;
            for (int j = 0; j < attackTankList[i].GetComponent<Tank>().power; j++)
            {
                if (enemyIndex < defendTankList.Count)
                {
                    attackTankList[i].GetComponent<Tank>().Attack(defendTankList[enemyIndex].transform);
                    counterAttack++;
                    if (counterAttack >= defendTankList[enemyIndex].GetComponent<Tank>().power)
                    {
                        counterAttack = 0;
                        enemyIndex++;
                    }
                    yield return new WaitForSeconds(2);
                }
                if (enemyIndex >= defendTankList.Count) { break; }
            }
            if (enemyIndex >= defendTankList.Count) { break; }
        }

        yield return new WaitForSeconds(3);
    }
    IEnumerator TankAttack2(List<GameObject> attackTankList, List<GameObject> defendTankList)
    {
        int enemyIndex = 0;
        int attackNumber = 0;   //����U���ł��邩
        bool isFirst = true;   //����̍ŏ��̍U����

        for (int i = attakTankIndex; i < attackTankList.Count; i++)
        {
            if (isFirst)
            {
                isFirst = false;
                attackNumber = attackTankList[i].GetComponent<Tank>().attackNumber;
            }
            else
            {
                attackNumber = attackTankList[i].GetComponent<Tank>().power;
            }
            for (int j = 0; j < attackNumber; j++)
            {
                if (enemyIndex < defendTankList.Count)
                {
                    attackTankList[i].GetComponent<Tank>().Attack(defendTankList[enemyIndex].transform);
                    counterAttack++;
                    if (counterAttack >= defendTankList[enemyIndex].GetComponent<Tank>().power)
                    {
                        counterAttack = 0;
                        enemyIndex++;
                    }
                    yield return new WaitForSeconds(2);
                }
            }
        }
    }

    /// <summary>
    /// �^���N�𐶐����āA�ړ������鏈��
    /// </summary>
    /// <param name="player">"P1"��"P2"</param>
    /// <param name="slotDatas">inGameManager._P(1/2).slotDatas�œn��</param>
    /// <param name="createPosition">createPositionP1/P2</param>
    /// <param name="battleSlotPosition">battleSlotPositionP1/P2</param>
    /// <returns></returns>
    IEnumerator TankCreateAndMove(string player, string[] slotDatas, Transform createPosition, List<Transform> battleSlotPosition)
    {
        for (int i = 0; i < slotDatas.Length; i++)
        {
            if (slotDatas[i] != "" && slotDatas[i] != null)
            {
                //P1��P2�̐����ʒu�ƖړI�n���Ⴄ���߈ړ����@��2�ɕ�����
                if (player == "P1")
                {
                    GameObject tank = Instantiate(prefabManager.GetTankPrefab(slotDatas[i]+"_P1"),
                                createPosition.position, Quaternion.identity, battleSlotPosition[i]);
                    battleTankListP1.Add(tank);
                    //�U���̃^���N���ǂ����`�F�b�N
                    if (inGameManager._P1.isTurn)
                    {
                        tank.GetComponent<Tank>().isAttack = true;
                    }
                    //�ŏ��͉�]
                    while (true)
                    {
                        float yAngle = tank.transform.eulerAngles.y;
                        if (yAngle > 180) yAngle -= 360;
                        // -90�x���傫����Ή�]��������
                        if (yAngle > -90f)
                        {
                            tank.transform.Rotate(0, -100 * Time.deltaTime, 0);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            tank.transform.eulerAngles = new Vector3(0, -90, 0);
                            break;
                        }
                    }
                    //�ړI�n�֌������ix�����j
                    while (true)
                    {
                        if (tank.transform.position.x > battleSlotPosition[i].position.x)
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(x - (30 * Time.deltaTime), y, z);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(battleSlotPosition[i].position.x, y, z);
                            break;
                        }
                    }
                    //�J�X��������߂�
                    while (true)
                    {
                        float yAngle = tank.transform.eulerAngles.y;
                        if (yAngle > 180) yAngle -= 360;
                        // 0�x��菬������Ή�]��������
                        if (yAngle < 0f)
                        {
                            tank.transform.Rotate(0, +100 * Time.deltaTime, 0);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            tank.transform.eulerAngles = new Vector3(0, 0, 0);
                            break;
                        }
                    }
                    //�Ō�͖ړI�n�܂ňړ��iz�����j
                    while (true)
                    {
                        if (tank.transform.position.z < battleSlotPosition[i].position.z)
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(x, y, z + (30 * Time.deltaTime));
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            tank.transform.position = new Vector3(x, y, battleSlotPosition[i].position.z);
                            break;
                        }
                    }
                    tank.GetComponent<Tank>().StopSoundEngineDriving();
                }
                else if (player == "P2")
                {
                    GameObject tank = Instantiate(prefabManager.GetTankPrefab(slotDatas[i]+"_P2"),
                                createPosition.position, Quaternion.Euler(0, 180, 0), battleSlotPosition[i]);
                    battleTankListP2.Add(tank);
                    //�U���̃^���N���ǂ����`�F�b�N
                    if (inGameManager._P2.isTurn)
                    {
                        tank.GetComponent<Tank>().isAttack = true;
                    }
                    //�ŏ��͉�]
                    while (true)
                    {
                        float yAngle = tank.transform.eulerAngles.y;
                        // 90�x���傫����Ή�]��������
                        if (yAngle > 90f)
                        {
                            tank.transform.Rotate(0, -100 * Time.deltaTime, 0);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            tank.transform.eulerAngles = new Vector3(0, 90, 0);
                            break;
                        }
                    }
                    //�ړI�n�֌������ix�����j
                    while (true)
                    {
                        if (tank.transform.position.x < battleSlotPosition[i].position.x)
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(x + (30 * Time.deltaTime), y, z);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(battleSlotPosition[i].position.x, y, z);
                            break;
                        }
                    }
                    //�J�X��������߂�
                    while (true)
                    {
                        float yAngle = tank.transform.eulerAngles.y;
                        // 180�x��菬������Ή�]��������
                        if (yAngle < 180f)
                        {
                            tank.transform.Rotate(0, +100 * Time.deltaTime, 0);
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            tank.transform.eulerAngles = new Vector3(0, 180, 0);
                            break;
                        }
                    }
                    //�Ō�͖ړI�n�܂ňړ��iz�����j
                    while (true)
                    {
                        if (tank.transform.position.z > battleSlotPosition[i].position.z)
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            float z = tank.transform.position.z;
                            tank.transform.position = new Vector3(x, y, z - (30 * Time.deltaTime));
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                        else
                        {
                            float x = tank.transform.position.x;
                            float y = tank.transform.position.y;
                            tank.transform.position = new Vector3(x, y, battleSlotPosition[i].position.z);
                            break;
                        }
                    }
                    tank.GetComponent<Tank>().StopSoundEngineDriving();
                }
            }
        }
        if (player == "P1")
        {
            P1CreateComplete = true;
        }
        else
        {
            P2CreateComplete = true;
        }
    }
}
