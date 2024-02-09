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
            case "Stage1":
                objectType = ObjectType.Jelly;
                break;
            case "Stage2":
                objectType = ObjectType.Defense;
                break;
            case "Stage3":
                objectType = ObjectType.AttackSpeed;
                break;
            case "Stage4":
                objectType = ObjectType.Hp;
                break;
            case "Stage5":
                objectType = ObjectType.Jelly;
                break;
            case "Stage6":
                objectType = ObjectType.Attack;
                break;
            case "Stage7":
                objectType = ObjectType.Attack;
                break;
            case "Stage8":
                objectType = ObjectType.AttackSpeed;
                break;
            case "Stage9":
                objectType = ObjectType.Defense;
                break;
            case "Stage10":
                objectType = ObjectType.AttackSpeed;
                break;
        }
    }

    public void EnhancedSlime(GameObject gameObject)
    {
        Debug.Log("Enhanced");

        Instantiate(buffEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);

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
                Instantiate(buffEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                break;
            case ObjectType.Attack:
                enemy.AttackDamage *= 1.1f;
                Instantiate(buffEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                break;
            case ObjectType.Defense:
                enemy.Defense *= 1.1f;
                Instantiate(buffEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                break;
            case ObjectType.AttackSpeed:
                enemy.AttackSpeed *= 1.1f;
                Instantiate(buffEffect, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                break;
            
        }
    }
}
