using UnityEngine;

// 플레이어 하위 스크립트
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
        // 궤도 무기 증가
        // 궤도 무기 공속 증가
        // 궤도 무기 레인지 증가
        // 궤도 무기 공격력 증가
        //40%
        // 타워 공속
        // 타워 레인지 
        //10%
        // 플레이어 체력
        // 플레이어 이동속도

        // 배열에 0을 50개, 1을,40개, 2를 10 개 추가
        AddNumbersToArray(0, 45, levelUpGatcha); //무기 관련
        AddNumbersToArray(1, 40, levelUpGatcha); //타워 관련
        AddNumbersToArray(2, 15, levelUpGatcha); //플레이어 관련

        //합이 1000이 되도록
        AddNumbersToArray(0, 150, weaponGatcha); // 무기 추가
        AddNumbersToArray(1, 300, weaponGatcha); // 무기 공속 
        AddNumbersToArray(2, 275, weaponGatcha); // 무기 레인지
        AddNumbersToArray(3, 275, weaponGatcha); // 무기 공격력

        //합이 1000이 되도록
        AddNumbersToArray(0, 400, towerGatcha); // 타워 공속
        AddNumbersToArray(1, 600, towerGatcha); // 타워 레인지

        //합이 1000이 되도록
        AddNumbersToArray(0, 500, playerGatcha); // 플레이어 체력
        AddNumbersToArray(1, 500, playerGatcha); // 플레이어 이동속도

        ShuffleAll(); //배열에 랜덤 요쇼가 오도록 섞음
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
        // 배열의 빈 공간에 숫자를 추가
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
