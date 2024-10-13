using UnityEngine;

// �÷��̾� ���� ��ũ��Ʈ
public class Gatcha
{
    public int[] levelUpGatcha = new int[100]; //MAX LEVEL 100
    public int[] weaponGatcha = new int[1000];
    public int[] towerGatcha = new int[1000];
    public int[] playerGatcha = new int[1000];

    public Gatcha()
    {
        GameStart();
    }

    public void GameStart()
    {
        //50%
        // �˵� ���� ����
        // �˵� ���� ���� ����
        // �˵� ���� ������ ����
        // �˵� ���� ���ݷ� ����
        //40%
        // Ÿ�� ����
        // Ÿ�� ������ 
        //10%
        // �÷��̾� ü��
        // �÷��̾� �̵��ӵ�

        // �迭�� 0�� 50��, 1��,40��, 2�� 10 �� �߰�
        AddNumbersToArray(0, 45, levelUpGatcha); //���� ����
        AddNumbersToArray(1, 40, levelUpGatcha); //Ÿ�� ����
        AddNumbersToArray(2, 15, levelUpGatcha); //�÷��̾� ����

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 150, weaponGatcha); // ���� �߰�
        AddNumbersToArray(1, 300, weaponGatcha); // ���� ���� 
        AddNumbersToArray(2, 275, weaponGatcha); // ���� ������
        AddNumbersToArray(3, 275, weaponGatcha); // ���� ���ݷ�

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 400, towerGatcha); // Ÿ�� ����
        AddNumbersToArray(1, 600, towerGatcha); // Ÿ�� ������

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 500, playerGatcha); // �÷��̾� ü��
        AddNumbersToArray(1, 500, playerGatcha); // �÷��̾� �̵��ӵ�

        ShuffleAll(); //�迭�� ���� �� ������ ����
    }

    public void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void Shuffle(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    void ShuffleAll()
    {        
        Shuffle(levelUpGatcha);
        Shuffle(weaponGatcha);
        Shuffle(towerGatcha);
        Shuffle(playerGatcha);
    }

    void AddNumbersToArray(int number, int count, int[] TargetArr)
    {
        int currentIndex = 0;
        // �迭�� �� ������ ���ڸ� �߰�
        for (int i = 0; i < count; i++)
        {
            while (TargetArr[currentIndex] != 0)
            {
                currentIndex++;
            }
            TargetArr[currentIndex] = number;
        }
    }
}
