using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;

public class GoogleSheetManager : MonoBehaviour
{
#region SingleTon Pattern
    public static GoogleSheetManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Initialize other components or variables if needed
    }
    #endregion

    // 링크 뒤 export ~ 부분을 빼고 export?format=tsv 추가하기
    const string slimeDataURL = "https://docs.google.com/spreadsheets/d/1ThIRLXCaoj25XtKjm6mFWOKnF3pqoViRYx8E-IXKGBM/export?format=tsv"; // Slime
    const string enemyDataURL = "https://docs.google.com/spreadsheets/d/14DXsg4yP4Y9zS7Le3GI-szxJ76Loj0xBkGyt_A5w_8c/export?format=tsv"; //Enemy

    [SerializeField]
    public List<Slime> slimes = new List<Slime>();

    [SerializeField]
    public List<Enemy> enemys = new List<Enemy>();

    IEnumerator Start()
    {
        //Slime
        UnityWebRequest www = UnityWebRequest.Get(slimeDataURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
        ParseSlimeData(data);

        // Enemy
        www = UnityWebRequest.Get(enemyDataURL);
        yield return www.SendWebRequest();

        string enemyData = www.downloadHandler.text;
        print(enemyData); 
        ParseEnemyData(enemyData);

    }
    void ParseSlimeData(string data)
    {
        string[] lines = data.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 번째 줄은 헤더이므로 건너뜁니다.
        {
            string[] fields = lines[i].Split('\t');
            if (fields.Length >= 12) // 필드가 충분한지 확인
            {
                Slime slime = new Slime()
                {
                    Index = int.Parse(fields[0]),
                    Name = fields[1],
                    Grade = int.Parse(fields[2]),
                    HP = float.Parse(fields[3]),
                    AttackDamage = float.Parse(fields[4]),
                    Defense = float.Parse(fields[5]),
                    AttackSpeed = float.Parse(fields[6]),
                    MoveSpeed = float.Parse(fields[7]),
                    Class = int.Parse(fields[8]),
                    AttackRange = float.Parse(fields[9]),
                    Cost = int.Parse(fields[10]),
                    KRName = fields[11],
                    DesText = fields[12],
                };
                slimes.Add(slime);
            }
        }
    }
    void ParseEnemyData(string data)
    {
        string[] lines = data.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 번째 줄은 헤더이므로 건너뜁니다.
        {
            string[] fields = lines[i].Split('\t');
            if (fields.Length >= 10) // 필드가 충분한지 확인
            {
                Enemy enemy = new Enemy()
                {
                    Index = int.Parse(fields[0]),
                    Name = fields[1],
                    HP = float.Parse(fields[2]),
                    AttackDamage = float.Parse(fields[3]),
                    Defense = float.Parse(fields[4]),
                    AttackSpeed = float.Parse(fields[5]),
                    MoveSpeed = float.Parse(fields[6]),
                    Class = int.Parse(fields[7]),
                    AttackRange = float.Parse(fields[8]),
                    DropJellyPower = int.Parse(fields[9]),
                };
                enemys.Add(enemy);
            }
        }
    }

}

[System.Serializable]
public class Slime
{
    public int Index;
    public string Name;
    public int Grade;
    public float HP;
    public float AttackDamage;
    public float Defense;
    public float AttackSpeed;
    public float MoveSpeed;
    public int Class;
    public float AttackRange;
    public int Cost;
    public string KRName;
    public string DesText;
}
[System.Serializable]
public class Enemy
{
    public int Index;
    public string Name;
    public float HP;
    public float AttackDamage;
    public float Defense;
    public float AttackSpeed;
    public float MoveSpeed;
    public int Class;
    public float AttackRange;
    public float DropJellyPower;
}