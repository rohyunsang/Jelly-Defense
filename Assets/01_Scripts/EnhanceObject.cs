using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectType
{
    Hp,
    Attack,
    Defense,
    AttackSpeed,
    Jelly
}


public class EnhanceObject : MonoBehaviour
{

    #region SingleTon Pattern
    public static EnhanceObject Instance { get; private set; }
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
    }
    #endregion
    public GameObject buffEffect;
    public ObjectType objectType;

    public void StageSwitch()
    {
        string stageName = UIManager.instance.selectedStageName;
        switch (stageName)
        {
            case "NormalStage1":
                objectType = ObjectType.Jelly;
                break;
            case "NormalStage2":
                objectType = ObjectType.Defense;
                break;
            case "NormalStage3":
                objectType = ObjectType.AttackSpeed;
                break;
            case "NormalStage4":
                objectType = ObjectType.Hp;
                break;
            case "NormalStage5":
                objectType = ObjectType.Jelly;
                break;
            case "NormalStage6":
                objectType = ObjectType.AttackSpeed;
                break;
            case "NormalStage7":
                objectType = ObjectType.Attack;
                break;
            case "NormalStage8":
                objectType = ObjectType.AttackSpeed;
                break;
            case "NormalStage9":
                objectType = ObjectType.Defense;
                break;
            case "NormalStage10":
                objectType = ObjectType.AttackSpeed;
                break;
        }
    }

    public void EnhancedSlime(GameObject gameObject)
    {
        Debug.Log("Enhanced");
        

        ISlime slime = gameObject.GetComponent<ISlime>();
        switch (objectType)
        {
            case ObjectType.Hp:
                slime.CurrentHP *= 1.1f;
                break;
            case ObjectType.Attack:
                slime.AttackDamage *= 1.1f;
                break;
            case ObjectType.Defense:
                slime.Defense *= 1.1f;
                break;
            case ObjectType.AttackSpeed:
                slime.AttackSpeed *= 1.1f;
                break;
        }
    }

    public void EnhancedEnemy(GameObject gameObject)
    {
        IEnemy enemy = gameObject.GetComponent<IEnemy>();
        switch (objectType)
        {
            case ObjectType.Hp:
                enemy.CurrentHP *= 1.1f;
                break;
            case ObjectType.Attack:
                enemy.AttackDamage *= 1.1f;
                break;
            case ObjectType.Defense:
                enemy.Defense *= 1.1f;
                break;
            case ObjectType.AttackSpeed:
                enemy.AttackSpeed *= 1.1f;
                break;
            
        }
    }
}

