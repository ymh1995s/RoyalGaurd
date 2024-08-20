using UnityEngine;

public static class Gatcha
{
    static public int[] levelUpGatcha = new int[100]; //MAX LEVEL 100
    static public int[] weaponGatcha = new int[1000];
    static public int[] towerGatcha = new int[1000];
    static public int[] playerGatcha = new int[1000];

    static public void GameStart()
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

        //TODO :�۷ι� ����, ���ο� ����ü ��

        // �迭�� 0�� 50��, 1��,40��, 2�� 10 �� �߰�
        AddNumbersToArray(0, 50, levelUpGatcha); //���� ����
        AddNumbersToArray(1, 40, levelUpGatcha); //Ÿ�� ����
        AddNumbersToArray(2, 10, levelUpGatcha); //�÷��̾� ����

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 250, weaponGatcha); // ���� ����
        AddNumbersToArray(1, 250, weaponGatcha); // ���� ���� 
        AddNumbersToArray(2, 250, weaponGatcha); // ���� ������
        AddNumbersToArray(3, 250, weaponGatcha); // ���� ���ݷ�

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 500, towerGatcha); // Ÿ�� ����
        AddNumbersToArray(1, 500, towerGatcha); // Ÿ�� ������

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 500, playerGatcha); // �÷��̾� ü��
        AddNumbersToArray(1, 500, playerGatcha); // �÷��̾� �̵��ӵ�

        ShuffleAll(); //�迭�� ���� �� ������ ����
    }

    static public void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    static public void Shuffle(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    static void ShuffleAll()
    {        
        Shuffle(levelUpGatcha);
        Shuffle(weaponGatcha);
        Shuffle(towerGatcha);
        Shuffle(playerGatcha);
    }

    static void AddNumbersToArray(int number, int count, int[] TargetArr)
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
