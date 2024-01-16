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
    const string URL = "https://docs.google.com/spreadsheets/d/1ThIRLXCaoj25XtKjm6mFWOKnF3pqoViRYx8E-IXKGBM/export?format=tsv";
    
    [SerializeField]
    public List<Slime> slimes = new List<Slime>();
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
        ParseData(data);
    }
    void ParseData(string data)
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
                    Type = int.Parse(fields[3]),
                    HP = int.Parse(fields[4]),
                    Attack = int.Parse(fields[5]),
                    Defense = int.Parse(fields[6]),
                    AttackSpeed = float.Parse(fields[7]),
                    Speed = int.Parse(fields[8]),
                    Range = fields[9],
                    Cost = int.Parse(fields[10]),
                    Target = int.Parse(fields[11])
                };
                slimes.Add(slime);
            }
        }
    }
    public int GetCostByName(string name)
    {
        Slime slimeData = slimes.FirstOrDefault(slime => slime.Name == name);
        if (slimeData != null)
        {
            return slimeData.Cost;
        }
        else
        {
            // 해당 이름의 Slime을 찾지 못한 경우, 원하는 처리를 추가하세요.
            return -1; // 예를 들어 오류 처리 등을 할 수 있습니다.
        }
    }
}
[System.Serializable]
public class Slime
{
    public int Index;
    public string Name;
    public int Grade;
    public int Type;
    public int HP;
    public int Attack;
    public int Defense;
    public float AttackSpeed;
    public int Speed;
    public string Range;
    public int Cost;
    public int Target;
}