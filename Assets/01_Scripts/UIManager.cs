using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
#region SingleTon Pattern
    public static UIManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [Header("TopBar")]
    public GameObject stageScreenBackButton;
    public TextMeshProUGUI actionPointText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI jellyStoneText;

    [Header("MainScreen")]

    [Header("StageScreen")]
    public GameObject stageScreen;  
    public GameObject imageEnemyExplain;  // 적군 설명
    public GameObject imageStageStory;  // 스테이지 번호마다 스토리가 바뀌도록 만들어야 함
    public GameObject settingScreenMain;
    public string selectedStageName = null;
    public int stageActionPoint;
    public TextMeshProUGUI actionPointTextStageScreen;
    public GameObject InsufficientAPInfo;

    public TextMeshProUGUI stageText;
    public TextMeshProUGUI stageDesText;

    public GameObject[] stageStars;
    public GameObject[] stageButtons;
    public GameObject[] enemyIcons;

    public GameObject enemyInfoPanel;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyDesText;

    //public Texture2D clearedStageImage;
    //public Texture2D clearedChaosStageImage;
    public Sprite clearedStageImage;
    public Sprite clearedChaosStageImage;

    public bool currentModeIsNormal = true;

    [Header("HUDScreen")]
    public GameObject HUDsettingScreen;
    public GameObject stageFailScreen;
    public GameObject stageClearScreen;
    public GameObject epicSlimeSkillIcons;
    public GameObject[] epicSlimeSkillTextures;
    public GameObject[] epicSlimeSkillCostText;
    public GameObject shiningIcon;
    public GameObject HUDBackGroundLight;
    public GameObject lockImage;

    public GameObject[] iconBlackImages;
    public GameObject[] slimeSpawnIcons;
    private List<int> originalSiblingIndexes = new List<int>();


    public Texture2D AngelSlimeSkillIcon;
    public Texture2D DevilSlimeSkillIcon;
    public Texture2D WitchSlimeSkillIcon;
    public Texture2D SkullSlimeSkillIcon;

    public Texture2D GhostSlimeSkillIcon;
    public Texture2D LizardSlimeSkillIcon;
    public Texture2D WizardSlimeSkillIcon;
    public Texture2D GrassSlimeSkillIcon;
    public Texture2D CatSlimeSkillIcon;

    public GameObject objectShining;
    public GameObject objectDesImage;
    public TextMeshProUGUI objectDesText;
    public GameObject objectImage;

    [Header("HUDClearScreen")]
    public GameObject leftStar;
    public GameObject middelStar;
    public GameObject rightStar;
    public GameObject jellyImageHUD;
    public TextMeshProUGUI jellyTextHUD;
    public GameObject goldImageHUD;
    public TextMeshProUGUI goldTextHUD;

    [Header("PickUpScreen")]
    public GameObject pickUpScreen;  //슬라임 선택창
    public GameObject notFullSlimeInfo;
    public GameObject onlyOneLegendSlimeInfo;

    [Header("Shop")]
    public GameObject shopScreen;
    public GameObject shopScreenBackButton;
    public GameObject LimitedSalePanel;
    public GameObject AdsShopPanel;
    public GameObject CashShopPanel;

    public GameObject purchaseInfoPanel;
    public int needGold;
    public int needJelly;
    public int purchasedGold;
    public int purchasedActionPoint;
    public TextMeshProUGUI itemDesText;

    public GameObject purchaseFailInfoPanel;
    public GameObject adSuccessInfo;

    [Header("Collection")]
    public GameObject collectionScreen;
    public GameObject preButtonCollectionScreen;

    [Header("Scenario")]
    public GameObject scenarioScreen;

    [Header("Etc")]
    public GameObject UIBackGround; // black transparency 100
    public GameObject UIBackGroundLight; 

    [Header("DontDestroy")]
    public GameObject slimeSpawnManager;
    public GameObject mainUI; //메인씬용 스크린
    public GameObject battleHUDScreen;  //HUD 스크린
    public GameObject uIManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject slimeManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject playerSkillManager;
    public GameObject stageManager;
    public GameObject enhancedObjectManager;
    public GameObject enemySpawnManager;
    public GameObject collectionManager;
    public GameObject aliveSlime;
    public GameObject aliveEnemy;
    public GameObject currenyManager;
    public GameObject dataManager;
    public GameObject lobbySlimeManager;
    public GameObject shopManager;
    public GameObject dayManager;
    public GameObject adManager;
    public GameObject iAPManager;
    public GameObject actionPointManager;
    public GameObject audioManager;
    public GameObject scenarioManager;
    public GameObject tutorialManager;

    public void UIClickSound()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_UI_ClickSound);
    }
    public void PurchaseSound()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);
    }

    #region TapBar

    public void AsycCurrenyUI()
    {
        actionPointText.text = CurrenyManager.Instance.actionPoint.ToString("N0");
        goldText.text = CurrenyManager.Instance.gold.ToString("N0");
        jellyStoneText.text = CurrenyManager.Instance.jellyStone.ToString("N0");
    }

    #endregion

    

    //메인 스크린
    #region MainScreen 
    public void OnClickBattleButton() //배틀 버튼 누르면
    {
        UIClickSound();
        UIBackGround.SetActive(true);
        stageScreen.SetActive(true); //스테이지 화면 열기
        stageScreenBackButton.SetActive(true);
    }

    #endregion

    #region SettingScreenMain

    public void OffSettingScreenMain()
    {
        UIClickSound();
        settingScreenMain.SetActive(false);
        UIBackGroundLight.SetActive(false);
    }
    public void OnSettingScreenMain()
    {
        UIClickSound();
        settingScreenMain.SetActive(true);
        UIBackGroundLight.SetActive(true);
    }
    public void OnClick_OpenURL() // 이용 약관
    {
        UIClickSound();
        Application.OpenURL("https://sites.google.com/view/jellybangeodae"); //연결원하는 사이트
    }
    public void OnClickOpenPersonalURL() // 개인 정보 취급 방침
    {
        UIClickSound();
        Application.OpenURL("https://sites.google.com/view/jellybangeodae"); //연결원하는 사이트
    }

    #endregion

    //스테이지 스크린
    #region StageScreen 

    public void ChangeMode()
    {
        UIClickSound();
        if (currentModeIsNormal)
        {
            for (int i = 0; i < 10; i++)
            {
                stageButtons[i].SetActive(false);
            }
            for (int i = 10; i < 20; i++)
            {
                stageButtons[i].SetActive(true);
            }
            currentModeIsNormal = false;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                stageButtons[i].SetActive(true);
            }
            for (int i = 10; i < 20; i++)
            {
                stageButtons[i].SetActive(false);
            }
            currentModeIsNormal = true;
        }
         
    }

    public void ShowEnemyInfoText(UnityEngine.UI.Button button)
    {
        UIClickSound();
        switch (button.name)
        {
            case "TurtleShell":
                enemyName.text = "뿔보";
                enemyDesText.text = "태어날 때부터 뾰족했던 등껍질 때문에 가까이 다가가기 힘든 몬스터. \n뾰족한 등껍질로 몸을 보호할 수 있지만 매끈한 등껍질을 원해서 슬라임의 젤리를 원한다.";
                break;
            case "OneEyedSlime":
                enemyName.text = "조거";
                enemyDesText.text = "하나 달린 눈이 컴플렉스인 특출난 것 없는 몬스터.";
                break;
            case "Cactus":
                enemyName.text = "미스선";
                enemyDesText.text = "몸에 돋은 가시가 너무 부끄러운 아가씨로, 가시 박힌 몸을 숨기기 위해 항상 화분에 숨어 있다. \n뾰족한 가시로 공격을 하지만 화분 때문에 멀리 가지는 못 한다.";
                break;
            case "Fishman":
                enemyName.text = "피쉬맨";
                enemyDesText.text = "원래는 귀여운 물고기였으나 오염된 젤리를 먹고 흉측하게 변해버린 몬스터.\n삼지창으로 바로 앞에 있는 슬라임을 공격하면 깨끗한 젤리를 얻어 다시 돌아갈 수 있을 것이라 믿는다.";
                break;
            case "Mushroom":
                enemyName.text = "모쉬모쉬";
                enemyDesText.text = "귀여워보이지만 오염된 젤리를 먹고 독버섯이 되어버렸다. \n다시 돌아가고 싶어!";
                break;
            case "ChestMonster":
                enemyName.text = "미믹";
                enemyDesText.text = "보물상자인척 슬라임을 꾀어 잡아먹는 악명 높은 몬스터. \n슬라임의 젤리가 주식이고 상자답게 몸이 단단하다.";
                break;
            case "Spider":
                enemyName.text = "아라고네";
                enemyDesText.text = "독을 멀리까지 내뿜어 슬라임의 젤리를 오염시킨다. \n8개의 다리가 컴플렉스라 매끈한 슬라임을 부러워한다.";
                break;
            case "Skeleton":
                enemyName.text = "해골";
                enemyDesText.text = "슬라임의 피부를 부러워하는 해골은 슬라임 젤리를 얻어 로션 대신 바르려고 한다.";
                break;
            case "CrabMonster":
                enemyName.text = "대게";
                enemyDesText.text = "오염된 젤리로 인해 흉측해진 집게다리를 감추고 싶어 한다. \n과거 단단한 몸과 아름다운 집게다리로 미스터 대게 수상 경력이 있다.";
                break;
            case "RatAssassin":
                enemyName.text = "자객";
                enemyDesText.text = "못생긴 외모로 가만히 있어도 미움을 받는 자객은 슬라임을 질투해 \n숨을 죽이고 슬라임의 바로 앞까지 달려가 공격한다.";
                break;
            case "Werewolf":
                enemyName.text = "웨어울프";
                enemyDesText.text = "저주를 받아 늑대인간이 되어 버린 몬스터. \n살금살금 슬라임에게 접근해 물고 놓아주지 않는다.";
                break;
            case "Salamander":
                enemyName.text = "살라맨더";
                enemyDesText.text = "징그럽다는 이유로 언제나 미움 받는 살라맨더는 \n귀여운 동물이 되고 싶어 젤리석을 훔치려고 한다.";
                break;
            case "Dragon":
                enemyName.text = "드라고";
                enemyDesText.text = "비만으로 브레스도 못 뿜고 제대로 날지도 못하는 드래곤 몬스터. \n어찌나 살이 쪘는지 공격을 맞아도 튕겨져 나온다. 슬라임 젤리가 그렇게 다이어트에 좋다면서?";
                break;
            case "Beholder":
                enemyName.text = "비홀더";
                enemyDesText.text = "마법 연구가 실패하여 흉측한 모습이 됐다. \n슬라임 왕국 마법 협회에 모습을 돌려줄 방법이 있다고 믿는다. 비록 실패했지만 마법력은 뛰어나다.";
                break;
            case "Orc":
                enemyName.text = "오크";
                enemyDesText.text = "무리지어 사는 습성이 있는 오크 부대는 먹을 게 \n많은 슬라임 킹덤의 땅을 탐낸다.";
                break;
            case "Cyclops":
                enemyName.text = "싸이클롭스";
                enemyDesText.text = "저주로 인해 한개 남은 눈이 실명 되었지만, \n기척을 읽는데 뛰어난 재능이 있어 보이지 않는 눈의 암살자가 되었다.";
                break;
            case "MonsterPlant":
                enemyName.text = "한입만";
                enemyDesText.text = "언젠가는 아름다운 장미가 되고 싶어 한다. \n하지만 닥치는대로 집어 삼켜 비만이 되었고 살 때문에 맞아도 아픔을 느끼지 못한다.";
                break;
            case "StingRay":
                enemyName.text = "하늘가오리";
                enemyDesText.text = "흉측한 이빨이 없는 슬라임을 질투하며 멀리서부터 슬라임만 보이면 공격을 한다.";
                break;
            case "Specter":
                enemyName.text = "팬텀";
                enemyDesText.text = "평생 귀여워지고 싶다는 소원을 품고 있었으나 이루지 못하고 죽었다. \n전생의 소심한 성격 때문인지 가까이서 공격은 하지 않고 멀리서 원한을 풀려고 한다.";
                break;
            case "Golem":
                enemyName.text = "골렘";
                enemyDesText.text = "마왕 부대의 대표 탱커로, 울퉁불퉁한 자신의 몸이 컴플렉스였으나 \n슬라임의 공격을 어느정도 막을 수 있다는 사실을 알게된 후로는 꽤나 마음에 들어한다.";
                break;
            case "BlackKnight":
                enemyName.text = "비숍";
                enemyDesText.text = "골렘의 충직한 신하로서 말이 없고 비밀스럽다. \n블랙나이트의 암살 실력은 마왕 부대 최고라는 소문이 있다.";
                break;
            case "NagaWizard":
                enemyName.text = "리자드맨 마법사";
                enemyDesText.text = "리자드슬라임의 천적으로, 오로지 리자드슬라임을 잡기 위해 마법을 배웠다.";
                break;
            case "LizardWarrior":
                enemyName.text = "리자드맨 대장";
                enemyDesText.text = "리자드맨 마법사가 곤경에 처하면 안정적인 실력으로 그의 곁을 지킨다.";
                break;
            case "BishopKnight":
                enemyName.text = "블랙나이트";
                enemyDesText.text = "멀리서 빠른 공격 속도로 슬라임을 압박한다. \n리자드맨 마법사보다도 빠르지만 체력 훈련을 게을리 했다.";
                break;
            default:
                enemyName.text = "Unknown";
                enemyDesText.text = "";
                break;
        }
        enemyInfoPanel.SetActive(true);
    }

    public void ShowEnemyIcon()  //UI Sound 안넣음
    {
        foreach(GameObject enemyIcon in enemyIcons)
        {
            enemyIcon.SetActive(false);
        }

        switch (selectedStageName)
        {
            case "NormalStage1":
                enemyIcons[0].SetActive(true);
                enemyIcons[1].SetActive(true);
                stageText.text = "[STAGE 1]";
                stageDesText.text = "정신없이 끌려왔지만 슬라임의 왕으로서 첫 전투에 참여 해야한다.\n일단 살아남아보자!";
                stageActionPoint = 7;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "NormalStage2":
                enemyIcons[0].SetActive(true);
                enemyIcons[1].SetActive(true);
                enemyIcons[2].SetActive(true);
                stageText.text = "[STAGE 2]";
                stageDesText.text = "아직도 미지의 시계인 이곳에서 돌아갈 방법을 찾고있지만 쉽지않다.\n일단은 전투에 익숙해지도록하자 ㅠㅠ";
                stageActionPoint = 8;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();

                break;
            case "NormalStage3":
                enemyIcons[0].SetActive(true);
                enemyIcons[1].SetActive(true);
                enemyIcons[2].SetActive(true);
                enemyIcons[3].SetActive(true);
                stageText.text = "[STAGE 3]";
                stageDesText.text = "아인슬타인과 이야기를 나누었지만 답답함은 여전하다.\n\n그나저나 물에 비친 내 모습을 보니 조금.. 귀여울지도?";
                stageActionPoint = 9;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();

                break;
            case "NormalStage4":
                enemyIcons[0].SetActive(true);
                enemyIcons[1].SetActive(true);
                enemyIcons[2].SetActive(true);
                enemyIcons[24].SetActive(true); // Boss
                stageText.text = "[STAGE 4]";
                stageDesText.text = "마왕군단이 마침내 대규모 공격을 시작했다.\n하늘을 뒤덮는 그림자.. 난생 처음보는 공포스러운 광경\n\n과연 나는 여기서 살아남을 수 있을까..";
                stageActionPoint = 19;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();


                break;
            case "NormalStage5":
                enemyIcons[3].SetActive(true);
                enemyIcons[4].SetActive(true);
                enemyIcons[5].SetActive(true);
                enemyIcons[6].SetActive(true);
                stageText.text = "[STAGE 5]";
                stageDesText.text = "드래곤에게서 살아남았지만 흔들리는 멘탈\n보스인줄 알았는데 이게 끝이 아니다!";
                stageActionPoint = 13;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();


                break;
            case "NormalStage6":
                enemyIcons[3].SetActive(true);
                enemyIcons[4].SetActive(true);
                enemyIcons[7].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[11].SetActive(true);
                stageText.text = "[STAGE 6]";
                stageDesText.text = "최근 수상한 움직임을 보이고있는 마왕군 부대장이 격파된 후 분위기가 변했다.";
                stageActionPoint = 14;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();

                break;
            case "NormalStage7":
                enemyIcons[6].SetActive(true);
                enemyIcons[7].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[13].SetActive(true);
                enemyIcons[24].SetActive(true); // Boss
                stageText.text = "[STAGE 7]";
                stageDesText.text = "드디어 마왕군 대장이 움직이기 시작했다.\n대장의 정체는 아무도 모르지만 최근 마왕군은 죽어도 되살아난다는 소문이 돌고있다..";
                stageActionPoint = 25;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();


                break;
            case "NormalStage8":
                enemyIcons[6].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[11].SetActive(true);
                enemyIcons[13].SetActive(true);
                enemyIcons[16].SetActive(true);
                stageText.text = "[STAGE 8]";
                stageDesText.text = "네크로맨서마저 물리쳤고 슬라임대장도 다행히 무사하다.\n그리고.. 서서히 풀리는 젤리석의 비밀";
                stageActionPoint = 18;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();


                break;
            case "NormalStage9":
                enemyIcons[7].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[12].SetActive(true);
                enemyIcons[16].SetActive(true);
                stageText.text = "[STAGE 9]";
                stageDesText.text = "점점 더 강력해져가는 마왕군단을 상대로 고전하는 슬라임왕국.\n이번에도 지켜낼 수 있을까?";
                stageActionPoint = 22;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();


                break;
            case "NormalStage10":
                enemyIcons[6].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[11].SetActive(true);
                enemyIcons[24].SetActive(true); // Boss
                stageText.text = "[STAGE 10]";
                stageDesText.text = "마침내 마지막 전투만 남겨두고있다.\n" ;
                stageActionPoint = 45;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();

                break;
            case "ChaosStage1":
                enemyIcons[3].SetActive(true);
                enemyIcons[4].SetActive(true);
                enemyIcons[7].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[11].SetActive(true);
                stageText.text = "[STAGE 1]";
                stageDesText.text = "";
                stageActionPoint = 11;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage2":
                enemyIcons[6].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[10].SetActive(true);
                enemyIcons[11].SetActive(true);
                enemyIcons[16].SetActive(true);
                stageText.text = "[STAGE 2]";
                stageDesText.text = "";
                stageActionPoint = 12;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage3":
                enemyIcons[7].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[10].SetActive(true);
                enemyIcons[12].SetActive(true);
                stageText.text = "[STAGE 3]";
                stageDesText.text = "";
                stageActionPoint = 13;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage4":
                enemyIcons[6].SetActive(true);
                enemyIcons[7].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[13].SetActive(true);
                enemyIcons[24].SetActive(true);
                stageText.text = "[STAGE 4]";
                stageDesText.text = "";
                stageActionPoint = 36;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage5":
                enemyIcons[6].SetActive(true);
                enemyIcons[8].SetActive(true);
                enemyIcons[9].SetActive(true);
                enemyIcons[10].SetActive(true);
                enemyIcons[11].SetActive(true);
                stageText.text = "[STAGE 5]";
                stageDesText.text = "";
                stageActionPoint = 20;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage6":
                enemyIcons[14].SetActive(true);
                enemyIcons[15].SetActive(true);
                enemyIcons[16].SetActive(true);
                enemyIcons[19].SetActive(true);
                stageText.text = "[STAGE 6]";
                stageDesText.text = "";
                stageActionPoint = 22;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage7":
                enemyIcons[16].SetActive(true);
                enemyIcons[17].SetActive(true);
                enemyIcons[18].SetActive(true);
                enemyIcons[24].SetActive(true);
                stageText.text = "[STAGE 7]";
                stageDesText.text = "";
                stageActionPoint = 48;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage8":
                enemyIcons[16].SetActive(true);
                enemyIcons[19].SetActive(true);
                enemyIcons[22].SetActive(true);
                enemyIcons[23].SetActive(true);
                stageText.text = "[STAGE 8]";
                stageDesText.text = "";
                stageActionPoint = 26;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage9":
                enemyIcons[19].SetActive(true);
                enemyIcons[20].SetActive(true);
                enemyIcons[21].SetActive(true);
                enemyIcons[22].SetActive(true);
                enemyIcons[23].SetActive(true);
                stageText.text = "[STAGE 9]";
                stageDesText.text = "";
                stageActionPoint = 32;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            case "ChaosStage10":
                enemyIcons[16].SetActive(true);
                enemyIcons[19].SetActive(true);
                enemyIcons[20].SetActive(true);
                enemyIcons[22].SetActive(true);
                enemyIcons[24].SetActive(true);
                stageText.text = "[STAGE 10]";
                stageDesText.text = "";
                stageActionPoint = 70;
                actionPointTextStageScreen.text = "행동력 소모 " + stageActionPoint.ToString();
                break;
            default:
                Debug.LogError("Unknown stage: " + selectedStageName);
                break;
        }
    }

    public void OffStageScreen() //스테이지 나가기 버튼 누르면
    {
        UIClickSound();
        stageScreen.SetActive(false); //스테이지 화면 닫기
        UIBackGround.SetActive(false);
    }

   
    public void OnClickButtonEnemy() //적군을 터치하면
    {
        UIClickSound();
        imageEnemyExplain.SetActive(true); //해당 적군의 설명이 보임
    }

    public void OnClickImageEnemyButton() //적군설명을 터치하면
    {
        UIClickSound();
        imageEnemyExplain.SetActive(false); //해당 적군의 설명이 사라짐
    }
    public void OnClickStageScreenStartButton() //스테이지 시작하기 버튼 누르면
    {
        UIClickSound();
        if (CurrenyManager.Instance.actionPoint < stageActionPoint)
        {
            InsufficientAPInfo.SetActive(true);
        }
        else
        {
            pickUpScreen.SetActive(true); //픽업화면 열기
            enemyInfoPanel.SetActive(false);
            OffImageStageStory();
        }
    }

    public void OnClickStageButton(UnityEngine.UI.Button button) //스테이지를 터치하면 
    {
        UIClickSound();

        selectedStageName = button.name;

        if (StageManager.Instance.CanEnterStage(selectedStageName))
        {
            imageStageStory.SetActive(true); //해당 스테이지의 스토리가 보임
        }
    }
    public void OffImageStageStory()
    {
        UIClickSound();
        imageStageStory.SetActive(false);
    }

    #endregion

    //픽업 스크린
    #region PickUpScreen



    public void OffPickUpScreen() 
    {
        UIClickSound();
        pickUpScreen.SetActive(false); //픽업화면 닫기
        InitSlimePickUp();
    }

    private void InitSlimePickUp()
    {
        SlimeManager.instance.InitSlimeSlot();
        SlimeManager.instance.InitSlimeIconCheckImageSetActiveFalse();
    }

    public void OnClickStartButtonPickUpScreen()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_BattleStartSound);
        if (SlimeManager.instance.FindFirstEmptySlot() == -1)
        {
            ChangeCanvasToHUD();
            SlimeManager.instance.SelectedSlimes();
            SlimeManager.instance.InitHUDSlimeButton();
            GameManager.Instance.ChangeScene(selectedStageName);
            GameManager.Instance.ResumeGame();
            notFullSlimeInfo.SetActive(false);
        }
        else
        {
            notFullSlimeInfo.SetActive(true);
        }
    }

    public void ChangeCanvasToHUD() //스타트 버튼을 누르면. 픽업씬 스타트 , HUD 설정>리스타트에서도 사용
    {
        HUDsettingScreen.SetActive(false);
        slimeSpawnManager.SetActive(true);//스폰 매니저 켜주기 (젤리력을 위함)
        battleHUDScreen.gameObject.SetActive(true); //HUD화면 캔버스 켜주기
        mainUI.gameObject.SetActive(false); //메인화면 캔버스 켜주기
    }
    #endregion

    #region HUDScreen

    public void OnOffObjectDesText()
    {
        if (!objectShining.activeSelf) return;

        if (objectDesImage.activeSelf)
        {
            objectDesImage.SetActive(false);
        }
        else
        {
            objectDesImage.SetActive(true);
        }
    }

    public void ShuffleSlimeIcon()
    {
        // Save original sibling indexes
        originalSiblingIndexes.Clear();
        foreach (var icon in slimeSpawnIcons)
        {
            originalSiblingIndexes.Add(icon.transform.GetSiblingIndex());
        }

        // Shuffle the slime spawn icons array
        for (int i = 0; i < slimeSpawnIcons.Length; i++)
        {
            GameObject temp = slimeSpawnIcons[i];
            int randomIndex = Random.Range(i, slimeSpawnIcons.Length);
            slimeSpawnIcons[i] = slimeSpawnIcons[randomIndex];
            slimeSpawnIcons[randomIndex] = temp;
        }

        // Apply the shuffled order by setting the sibling index based on the new order
        for (int i = 0; i < slimeSpawnIcons.Length; i++)
        {
            slimeSpawnIcons[i].transform.SetSiblingIndex(i);
        }
    }

    public void ResetOrder()
    {
        // Reset to the original order based on saved sibling indexes
        for (int i = 0; i < originalSiblingIndexes.Count; i++)
        {
            slimeSpawnIcons[i].transform.SetSiblingIndex(originalSiblingIndexes[i]);
        }
    }

    public void UnvisibleSlimeSpawnIcon()
    {
        foreach(GameObject icon in iconBlackImages)
        {
            icon.SetActive(true);
        }

        Invoke("VisibleSlimeSpawnIcon", 7f);
    }
    public void VisibleSlimeSpawnIcon()
    {
        foreach (GameObject icon in iconBlackImages)
        {
            icon.SetActive(false);
        }
    }


    public void OnClickSpeedButton()
    {
        if (!shiningIcon.activeSelf)
        {
            shiningIcon.SetActive(true);
            GameManager.Instance.DoubleTimeScale();
        }
        else
        {
            shiningIcon.SetActive(false);
            GameManager.Instance.OriginTimeScale();
        }

    }

    public void OnOffEpicSlimeSkill()
    {
        UIClickSound();
        epicSlimeSkillIcons.SetActive(!epicSlimeSkillIcons.activeSelf);
    }

    
    public void OnStageClearScreen()
    {
        stageClearScreen.SetActive(true);
    }
    public void OnClickHomeButtonStageClearScreen()
    {
        UIClickSound();
        stageClearScreen.SetActive(false);
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();
    }
    public void OnStageFailScreen()
    {
        stageFailScreen.SetActive(true);
    }
    public void OnClickHomeButtonStageFailScreen()
    {
        UIClickSound();
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        
        OnDestroyObjects();
    }

    #endregion

    //SettingScreen 안의 버튼 이벤트들 
    #region SettingScreenHUD 

    public void HUDSettingButton() //설정창 들어가기 버튼
    {
        UIClickSound();
        GameManager.Instance.PauseGame();
        HUDsettingScreen.SetActive(true);
        HUDBackGroundLight.SetActive(true);
    }
    public void HUDResumeButton()  //이어서 하기 버튼
    {
        UIClickSound();
        HUDsettingScreen.SetActive(false);
        if (shiningIcon.activeSelf)
        {
            GameManager.Instance.DoubleTimeScale();
        }
        else
        {
            GameManager.Instance.ResumeGame();
        }
        HUDBackGroundLight.SetActive(false);
    }

    public void OnClickRetryButton() //
    {
        UIClickSound();
        HUDsettingScreen.SetActive(false);

        //Init
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();

        GameManager.Instance.goPickUpStage = true;
    }

    public void OnClickHelpButton()
    {
        UIClickSound();
        Debug.Log("OnClickHelpButton");
    }
    public void OnClickHomeButton()
    {
        UIClickSound();
        HUDsettingScreen.SetActive(false);
        //Init
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();
    }

    #endregion

    #region ShopScreen

    public void JellyRefreshButton()
    {
        UIClickSound();
        if (CurrenyManager.Instance.jellyStone - 10 >= 0)
        {
            ShopManager.Instance.purchasePanel.SetActive(false);
            ShopManager.Instance.purchaseSuccessPanel.SetActive(false);
            ShopManager.Instance.purchaseFailPanel.SetActive(false);

            SlimeManager.instance.RefreshShopSlimes();
            CurrenyManager.Instance.jellyStone -= 10;
            AsycCurrenyUI();
            DataManager.Instance.JsonSave();
        }
        else
        {
            // ShopManager.Instance.purchaseFailPanel.SetActive(true);
        }

    }
    public void GoldRefreshButton()
    {
        UIClickSound();
        if (CurrenyManager.Instance.gold - 3000 >= 0 && DayManager.Instance.currentGoldRefresh > 0)
        {
            DayManager.Instance.currentGoldRefresh--;

            ShopManager.Instance.purchasePanel.SetActive(false);
            ShopManager.Instance.purchaseSuccessPanel.SetActive(false);
            ShopManager.Instance.purchaseFailPanel.SetActive(false);

            SlimeManager.instance.RefreshShopSlimes();
            CurrenyManager.Instance.gold -= 3000;
            AsycCurrenyUI();
            DayManager.Instance.AsyncGoldRefreshText();
            DataManager.Instance.JsonSave();
        }
        else
        {
            // ShopManager.Instance.purchaseFailPanel.SetActive(true);
        }

    }

    public void OnShopScreen()
    {
        UIClickSound();
        UIBackGround.SetActive(true);
        shopScreen.SetActive(true);
        shopScreenBackButton.SetActive(true);
        OnClickLimitSaleButton(); // Init Shop Store
    }
    public void OffShopScreen()
    {
        UIClickSound();
        UIBackGround.SetActive(false);
        shopScreen.SetActive(false);
        shopScreenBackButton.SetActive(false);
    }

    public void OnClickLimitSaleButton()
    {
        UIClickSound();
        LimitedSalePanel.SetActive(true);
        AdsShopPanel.SetActive(false);
        CashShopPanel.SetActive(false);
    }
    public void OnClickAddShopButton()
    {
        UIClickSound();
        LimitedSalePanel.SetActive(false);
        AdsShopPanel.SetActive(true);
        CashShopPanel.SetActive(false);
    }
    public void OnClickCashStoreButton()
    {
        UIClickSound();
        LimitedSalePanel.SetActive(false);
        AdsShopPanel.SetActive(false);
        CashShopPanel.SetActive(true);
    }

    #endregion

    // shop 안의 버튼 이벤트들 
    #region shop purchase
    public void OnClickFreeGoldButton()
    {
        UIClickSound();

        Debug.Log("광고 버튼 눌림 ");
        if(!DataManager.Instance.isAdPass)
            AdManager.instance.ShowAds(0); // 임시로 골드는 0, 젤리는 1 
        else
        {
            if (DayManager.Instance.goldAd <= 0)
                return;
            CurrenyManager.Instance.gold += 5000; // 일단 100골드 줘 봄 
            DayManager.Instance.goldAd--; // 한개 깍음

            DataManager.Instance.JsonSave(); // 바아로 저장 
            UIManager.instance.AsycCurrenyUI();
            adSuccessInfo.SetActive(true);

        }

    }
    public void OnClickFreeJellyButton()
    {
        UIClickSound();

        if (!DataManager.Instance.isAdPass)
            AdManager.instance.ShowAds(1); // 임시로 골드는 0, 젤리는 1 
        else
        {
            if (DayManager.Instance.jellyStoneAd <= 0)
                return;
            CurrenyManager.Instance.jellyStone += 20; 
            DayManager.Instance.jellyStoneAd--; // 한개 깍음

            DataManager.Instance.JsonSave(); // 저장 
            UIManager.instance.AsycCurrenyUI();
            adSuccessInfo.SetActive(true);

        }
    }
    public void OnClickFreeActionPointButton()
    {
        UIClickSound();

        if (!DataManager.Instance.isAdPass)
            AdManager.instance.ShowAds(2); // 임시로 골드는 0, 젤리는 1 
        else
        {
            if (DayManager.Instance.actionPointAd <= 0)
                return;
            CurrenyManager.Instance.actionPoint += 20; //  
            if (CurrenyManager.Instance.actionPoint > 180) CurrenyManager.Instance.actionPoint = 180;
            DayManager.Instance.actionPointAd--; // 한개 깍음

            DataManager.Instance.JsonSave(); // 바아로 저장 
            UIManager.instance.AsycCurrenyUI();
            adSuccessInfo.SetActive(true);
        }
    }

    public void BuyGoldPackage(int needJelly)
    {
        UIClickSound();

        this.needJelly = needJelly;
        purchasedActionPoint = 0;
        purchasedGold = 0;

        if (CurrenyManager.Instance.jellyStone >= needJelly)
        {
            purchaseInfoPanel.SetActive(true);
        }
        else
        {
            purchaseFailInfoPanel.SetActive(true);
        }

        if (needJelly == 30)
        {
            itemDesText.text = "골드 묶음\r\n<#08A200>10,000</color>";
            purchasedGold = 10000;
        }
        else if (needJelly == 80)
        {
            itemDesText.text = "골드 가방\r\n<#08A200>30,000</color> <#FF0F00>+ 10%</color>";

            purchasedGold = 33000;
        }
        else if (needJelly == 120)
        {
            itemDesText.text = "골드 상자\r\n<#08A200>50,000</color> <#FF0F00>+ 30%</color>";

            purchasedGold = 65000;
        }
        else if(needJelly == 250)
        {
            itemDesText.text = "골드 박스\r\n<#08A200>100,000</color> <#FF0F00>+ 50%</color>";

            purchasedGold = 150000;
        }
        else
        {
            Debug.Log("error + not jelly type");
        }
    }
    public void BuyActionPointPackage(int needJellyOrGold)
    {
        UIClickSound();
        this.needGold = 0;
        this.needJelly = 0;
        purchasedActionPoint = 0;
        purchasedGold = 0;

        if (needJellyOrGold > 500)
        {
            this.needGold = needJellyOrGold;
        }
        else
        {
            this.needJelly = needJellyOrGold;
        }

        if (CurrenyManager.Instance.jellyStone >= needJelly && CurrenyManager.Instance.gold >= needGold)
        {
            purchaseInfoPanel.SetActive(true);
        }
        else
        {
            purchaseFailInfoPanel.SetActive(true);
        }

        if (needJelly == 80)
        {
            itemDesText.text = "행동력 가방\r\n<#1192C5>60개</color> <#FF0F00>+ 30%</color>";

            purchasedActionPoint = 78;
        }
        else if (needJelly == 120)
        {
            itemDesText.text = "행동력 상자\r\n<#1192C5>100개</color> <#FF0F00>+ 50%</color>";

            purchasedActionPoint = 150;
        }
        else if (needGold == 9000)
        {
            itemDesText.text = "행동력 주머니\r\n<#1192C5>20개</color>";

            purchasedActionPoint = 20;
        }
        else if (needGold == 20000)
        {
            itemDesText.text = "행동력 묶음\r\n<#1192C5>50개</color> <#FF0F00>+ 10%</color>";

            purchasedActionPoint = 60;
        }
        else
        {
            Debug.Log("error + not jelly type");
        }
    }

    public void OnClickPurchasePanelYesButton()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);

        CurrenyManager.Instance.jellyStone -= needJelly;
        CurrenyManager.Instance.gold -= needGold;
        CurrenyManager.Instance.gold += purchasedGold;
        CurrenyManager.Instance.actionPoint += purchasedActionPoint;
        if (CurrenyManager.Instance.actionPoint > 180) CurrenyManager.Instance.actionPoint = 180;

        AsycCurrenyUI();
        DataManager.Instance.JsonSave();
    }





    #endregion

    #region CollectionScreen

    public void OnCollectionScreen()
    {
        UIClickSound();

        UIBackGround.SetActive(true);
        preButtonCollectionScreen.SetActive(true);
        collectionScreen.SetActive(true);
    }

    public void OffCollectionScreen()
    {
        UIClickSound();

        UIBackGround.SetActive(false);
        preButtonCollectionScreen.SetActive(false);
        collectionScreen.SetActive(false);
    }


    #endregion

    void OnDestroyObjects()
    {
        //스스로를 파괴하지 않으면 UI연결이 끊기는 문제 발생
        Destroy(mainUI);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnManager);//메인씬 중복방지용 파괴
        Destroy(battleHUDScreen);//메인씬 중복방지용 파괴
        Destroy(uIManager); //전체연결 없어짐 방지
        Destroy(slimeManager);//슬라임 컨텐트 ~ 버튼 연결 없어짐 방지
        Destroy(playerSkillManager);
        Destroy(stageManager);
        Destroy(enhancedObjectManager);
        Destroy(enemySpawnManager);
        Destroy(collectionManager);
        Destroy(aliveSlime);
        Destroy(aliveEnemy);
        Destroy(currenyManager);
        Destroy(dataManager);
        Destroy(lobbySlimeManager);
        Destroy(shopManager);
        Destroy(dayManager);
        Destroy(adManager);
        Destroy(iAPManager);
        Destroy(actionPointManager);
        Destroy(audioManager);
        Destroy(scenarioManager);
        Destroy(tutorialManager);
    }
}