using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
//using Unity.Notifications.iOS;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public bool playTutorial = false;
    [Header("Panel")]
    public Slider loadResourceSlider;
    public List<GameObject> panels = new List<GameObject>();
    public GameObject tutorialPanel;

    [Header("Intro")]
    public GameObject introObj;
    public GameObject loadingObj;
    public Text dataVersionText;

    [Header("Main")]
    public GameObject mainPanel;
    public GameObject mainObj;
    public Text coinText;
    public Text diamondText;

    [Header("Enegy")]
    public Text countdownEnegyText;
    public List<GameObject> gasObjs = new List<GameObject>();


    [Header("Card Collection")]
    public GameObject cardBtnPrefab;
    public ScrollRect cardCollectionScrollRect;
    public GameObject cardPreview;


    public float maxTimeGas = 1800;
    public float timerGas = 0;

    private DataCore dc;

    private void OnEnable()
    {
        dc = DataCore.instance;
    }

    private void Start()
    {
        dataVersionText.text = "Data version." + PlayerPrefs.GetInt("game_version");
        AudioManager.instance.SetBGM(0);
        ShowAllPanel();
        AudioManager.instance.Init();
        HideAllPanel();

        if (ServiceManager.instance.isLogin)
        {
            mainObj.SetActive(true);
            mainPanel.SetActive(true);
            SetConsole();
        }
        StartCoroutine(Update1sec());
    }

    public void SetLoadData(int current, int max)
    {
        loadResourceSlider.value = (float)current / (float)max;
        if(current / max == 1)
        {
            loadResourceSlider.gameObject.SetActive(false);
            ServiceManager.instance.CheckLogin(); ;
        }
    }
    IEnumerator Update1sec()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!ServiceManager.instance.isLogin)
                continue;
            SetSpin();
            CheckGas();
            if (!spinFree)
            {
                countdownSpinFreeText.gameObject.SetActive(true);
                countdownSpinFreeText.text = FormatSeconds(maxSecSpinFree - CheckDate("spinFree"));
            }
            else
            {
                countdownSpinFreeText.gameObject.SetActive(false);
            }
        }
    }
    #region Loading

    public void EndLoading(Action method = null)
    {
        StartCoroutine(EndLoad(method));
    }
    IEnumerator EndLoad(Action method = null)
    {
        yield return new WaitForSeconds(1.4f);
        if (method != null) { method(); }
        loadingObj.SetActive(false);
    }

    #endregion

    #region Main
    public void ShowAllPanel()
    {
        panels.ForEach(p => p.SetActive(true));
    }
    public void HideAllPanel()
    {
        panels.ForEach(p => p.SetActive(false));
    }

    public void Init()
    {
        Debug.Log("Login...");
        if (playTutorial)
        {
            tutorialPanel.SetActive(true);
        }

        var sm = ServiceManager.instance;
        mainPanel.SetActive(sm.isLogin);
        mainObj.SetActive(sm.isLogin);
        introObj.SetActive(!sm.isLogin);

        if (dc.account.gas < 6)
        {
            timerGas = maxTimeGas;
            timerGas -= CheckDate("Gas");
        }
        SetGasObj();
        SetConsole();

        if (PlayerPrefs.HasKey("TravelIndex"))
        {
            currentTravel = PlayerPrefs.GetInt("TravelIndex");
            mainObj.SetActive(false);
            mainPanel.SetActive(false);
            SetTravel();
        }
    }
    #endregion

    #region Cal Count Down

    private DateTime currentDate;
    public float CheckDate(string nameBinary)
    {
        currentDate = System.DateTime.Now;
        string tempString = PlayerPrefs.GetString(nameBinary, "1");
        long tempLong = Convert.ToInt64(tempString);
        DateTime oldDate = DateTime.FromBinary(tempLong);
        TimeSpan difference = currentDate.Subtract(oldDate);
        return (float)difference.TotalSeconds;
    }


    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return String.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion

    #region Main

    public void SetConsole()
    {
        coinText.text = string.Format("{0:#,0}", dc.account.coin);
        diamondText.text = string.Format("{0:#,0}", dc.account.diamond);
    }



    private void CheckGas()
    {
        if (dc.account.gas < 6)
        {
            timerGas -= 1;
            countdownEnegyText.gameObject.SetActive(true);
            countdownEnegyText.text = FormatSeconds(timerGas);
            int maxGetGas = 6 - dc.account.gas;
            if (timerGas <= 0)
            {
                var getGas = Math.Abs((int)(timerGas / maxTimeGas));
                getGas = getGas == 0 ? 1 : getGas;
                getGas = getGas > maxGetGas ? maxGetGas : getGas;
                if (getGas < 0)
                    return;

                dc.account.gas += getGas;
                ServiceManager.instance.AddGas(getGas);
                SetGasObj();
                timerGas = maxTimeGas;
                PlayerPrefs.SetString("Gas", DateTime.Now.ToBinary().ToString());
            }
        }
        else
        {
            countdownEnegyText.gameObject.SetActive(false);
        }
    }

    public void SetGasObj()
    {
        gasObjs.ForEach(v => v.SetActive(false));
        for (int i = 0; i < dc.account.gas; i++)
        {
            gasObjs[i].SetActive(true);
        }
    }

    #endregion

    #region Gasha

    [Header("Gasha")]
    public Animator slot1;
    public Animator slot2;
    public Animator slot3;
    [System.Serializable]
    public class ArraySlot
    {
        public string[] point;
    }
    public int indexSlot1 = 0;
    public int indexSlot2 = 0;
    public int indexSlot3 = 0;
    public List<ArraySlot> indexPoint = new List<ArraySlot>();
    public GameObject alertReward;
    public GameObject gashaCoin;
    public GameObject gashaDiamond;
    public Text gashaCoinText;
    public Text gashaDiamondText;

    public bool spinFree = true;
    public GameObject spinUseDiamondObj;
    public GameObject spinFreeObj;
    public Text countdownSpinFreeText;
    public int maxSecSpinFree = 120;

    public int maxCoinGasha = 500;
    public int maxDiamondGasha = 200;
    public void SetSpin()
    {
        CheckTimeSpinFree();
        spinFreeObj.SetActive(spinFree);
        spinUseDiamondObj.SetActive(!spinFree);
    }

    public void CheckTimeSpinFree()
    {
        spinFree = maxSecSpinFree - CheckDate("spinFree") < 0;
    }

    public void Spin()
    {
        if (spinFree)
        {
            spinFree = false;
            PlayerPrefs.SetString("spinFree", System.DateTime.Now.ToBinary().ToString());
        }
        else
        {
            if (dc.account.diamond < 29)
            {
                SetAlert("เพชรของคุณไม่เพียงพอ");
                return;
            }
            ServiceManager.instance.AddCurrency(-29, 1, () => {
                dc.account.diamond -= 29;
            });
            SetConsole();
        }
        StartCoroutine(SpinSlot());
    }
    IEnumerator SpinSlot()
    {
        slot1.speed = 3;
        slot1.SetInteger("index", 0);
        slot2.speed = 3;
        slot2.SetInteger("index", 0);
        slot3.speed = 3;
        slot3.SetInteger("index", 0);
        yield return new WaitForSeconds(2f);

        indexSlot1 = Random.Range(0, 6);
        slot1.SetInteger("index", indexSlot1 + 1);
        yield return new WaitForSeconds(1f);
        indexSlot2 = Random.Range(0, 6);
        slot2.SetInteger("index", indexSlot2 + 1);
        yield return new WaitForSeconds(1f);
        indexSlot3 = Random.Range(0, 6);
        slot3.SetInteger("index", indexSlot3 + 1);
        yield return new WaitForSeconds(1f);
        CheckLinePoint();
    }
    public void CheckLinePoint()
    {
        List<int> rewardSlot = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(indexPoint[indexSlot1].point[i] + " : " + indexPoint[indexSlot2].point[i] + " : " + indexPoint[indexSlot3].point[i]);
            if (indexPoint[indexSlot1].point[i] == indexPoint[indexSlot2].point[i] && indexPoint[indexSlot2].point[i] == indexPoint[indexSlot3].point[i])
            {
                rewardSlot.Add(int.Parse(indexPoint[indexSlot1].point[i]));
            }
            else
            {
                rewardSlot.Add(-1);
            }
        }
        int rewardCoin = 0;
        int rewardDiamond = 0;
        rewardSlot.ForEach(rs =>
        {
            if (rs == 0)
            {
                rewardCoin += Random.Range(0, maxCoinGasha);
            }
            else if (rs == 1)
            {
                rewardDiamond += Random.Range(0, maxDiamondGasha);
            }
        });
        if (rewardCoin != 0 || rewardDiamond != 0)
        {
            var sm = ServiceManager.instance;
            alertReward.SetActive(true);
            if (rewardCoin != 0)
            {
                gashaCoin.SetActive(true);
                gashaCoinText.text = rewardCoin.ToString();
                sm.AddCurrency(rewardCoin, 0, () => {
                    dc.account.coin += rewardCoin;
                });
            }
            else
            {
                gashaCoin.SetActive(false);
            }
            if (rewardDiamond != 0)
            {
                gashaDiamond.SetActive(true);
                gashaDiamondText.text = rewardDiamond.ToString();
                sm.AddCurrency(rewardDiamond, 1, () => {
                    dc.account.diamond += rewardDiamond;
                });
            }
            else
            {
                gashaDiamond.SetActive(false);
            }
            SetConsole();
        }
    }

    #endregion

    #region CardCollection
    public void SetCardCollection()
    {
        cardCollectionScrollRect.content.DetachChildren();
        dc.invenCardsId.ForEach(cId =>
        {
            var card = Instantiate(cardBtnPrefab, cardCollectionScrollRect.content);
            if(dc.cards[cId].loadImg)
                card.GetComponent<Image>().sprite = dc.imgs["cp" + cId];
            var id = cId;
            card.GetComponent<Button>().onClick.AddListener(() => { SetPreviewCard(id); });
            if (dc.cards[cId].minigame != 0)
            {
                card.transform.GetChild(0).gameObject.SetActive(true);
                card.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { GotoMiniGame(dc.cards[cId].minigame); });
            }
            else
            {
                card.transform.GetChild(0).gameObject.SetActive(false);
            }
        });
    }
    #endregion

    #region Shop
    [Header("Shop")]
    public GameObject ShopObj;
    public GameObject itemShopPrefab;
    public ScrollRect itemScrollRect;

    public GameObject detailObj;
    public Image detailImage;
    public Text detailText;
    public Text priceText;
    public Image detailTypeCurrencyImage;
    public Button purchaseBtn;

    public GameObject confirmObj;
    public Image confirmItemImg;
    public Button purchaseConfirmBtn;

    public Sprite[] typeCurrencySprite;

    public int currentTypeShop = 0;
    public void SetItemShop(int type)
    {
        ShopObj.SetActive(true);
        currentTypeShop = type;
        ClearObjInParent(itemScrollRect.content);
        var itemFilter = dc.items.Values.Where(i => i.category_id == type).ToList();
        itemFilter.ForEach(i =>
        {
            var item = Instantiate(itemShopPrefab.GetComponent<ShopItemObj>(), itemScrollRect.content);
            item.image.sprite = dc.imgs["i" + i.id];
            item.price.text = i.category_id == 0 ? i.price + " THB" : i.price.ToString();
            item.typeCurrencyImg.sprite = i.category_id == 0 ? typeCurrencySprite[2] : typeCurrencySprite[i.currency];
            var has = dc.invenItemsId.Contains(i.id);
            item.checkObj.SetActive(dc.invenItemsId.Contains(i.id));
            var id = i.id;
            item.GetComponent<Button>().onClick.AddListener(() => { SetItemShopDetail(id, has); });
        });
    }

    public void SetItemShopDetail(string id, bool has)
    {
        detailObj.SetActive(true);
        var i = dc.items[id];
        detailImage.sprite = dc.imgs["i" + id];
        detailText.text = i.name;

        priceText.text = i.category_id == 0 ? i.price + " THB" : i.price.ToString();
        detailTypeCurrencyImage.sprite = i.category_id == 0 ? typeCurrencySprite[2] : typeCurrencySprite[i.currency];

        purchaseBtn.onClick.AddListener(() => { SetConfirmPurchase(id); });
        purchaseBtn.gameObject.SetActive(!has);
    }
    public void SetConfirmPurchase(string id)
    {
        confirmObj.SetActive(true);
        confirmItemImg.sprite = dc.imgs["i" + id];
        purchaseConfirmBtn.onClick.RemoveAllListeners();
        purchaseConfirmBtn.onClick.AddListener(() => {
            if (currentTypeShop == 0)
            {
                PurchaseMoney(dc.items[id].price, () =>
                {
                    ServiceManager.instance.PuchaseDiamond(dc.items[id].price, dc.items[id].diamond, confirmObj, () => {
                        moneyPurchasePanel.SetActive(false);
                    });
                });
            }
            else
            {
                ServiceManager.instance.Puchase(dc.items[id].price, dc.items[id].currency, id, confirmObj);
            }
        });
    }
    #endregion

    #region Custom
    [Header("Custom")]
    public GameObject customPanel;
    public GameObject customObj;

    public GameObject customItemPrefab;
    public ScrollRect customScrollRect;

    public CharacterObj characterCustom;
    public GameObject unlinkBtn;
    public int catIndex = 0;

    public GameObject itemReqCustomPrefab;
    public ScrollRect itemReqScrollRect;
    private List<ItemReqCustomObj> listItemReqCustom = new List<ItemReqCustomObj>();
    public void SetItemCustom(int index)
    {
        catIndex = index;
        ClearObjInParent(customScrollRect.content);
        var itemIdUsing = PlayerPrefs.GetString("s" + catIndex);
        List<Item> list;
        list = dc.items.Values.Where(i => i.category_id == index).ToList();
        list = list.Where(i => dc.invenItemsId.Contains(i.id) && itemIdUsing != i.id).ToList();
        list.ForEach(i =>
        {
            var item = Instantiate(customItemPrefab, customScrollRect.content);
            item.GetComponent<Image>().sprite = dc.imgs["i" + i.id];
            item.GetComponent<Button>().onClick.AddListener(() => { SelectItem(i.id); });
        });
        unlinkBtn.SetActive(itemIdUsing != "");
    }

    public void SetItemRequestCustom()
    {
        listItemReqCustom.Clear();
        ClearObjInParent(itemReqScrollRect.content);
        foreach (var itemId in dc.stage[currentStage].requestItemIds)
        {
            if (itemId == "0")
                continue;
            var item = Instantiate(itemReqCustomPrefab.GetComponent<ItemReqCustomObj>(), itemReqScrollRect.content);
            item.Set(itemId, dc.items[itemId].category_id, dc.imgs["i" + itemId]);
            listItemReqCustom.Add(item);
        }
        CheckRequestCustom();
    }
    private void CheckRequestCustom()
    {
        bool nextTu = true;
        listItemReqCustom.ForEach(item =>
        {
            bool check = CheckItemRequest(item.catType, item.id);
            item.Check(check);
            if (!check)
            {
                nextTu = check;
            }
        });
        var tc = FindObjectOfType<TutorialCore>();
        if (tc && nextTu)
        {
            customPanel.SetActive(false);
            customObj.SetActive(false);
            requestObj.SetActive(true);
            requestPanel.SetActive(true);
            SetStageRequest();
            tc.tutorialObj.SetActive(true);
            tc.OnNextStep();
        }
    }


    public void SelectItem(string id)
    {
        PlayerPrefs.SetString("s" + catIndex, id);
        characterCustom.SetCharacter(dc.items);
        unlinkBtn.SetActive(true);
        SetItemCustom(catIndex);
        CheckRequestCustom();

        if (catIndex == 6) characterCustom.lineCamera.SetActive(true);
    }

    public void Unlink()
    {
        PlayerPrefs.SetString("s" + catIndex, "");
        characterCustom.SetCharacter(dc.items);
        SetItemCustom(catIndex);
        unlinkBtn.SetActive(false);
        CheckRequestCustom();

        if (catIndex == 6) characterCustom.lineCamera.SetActive(false);
    }

    #endregion

    #region Stage

    [Header("Stage")]
    public GameObject requestPanel;
    public GameObject requestObj;

    public List<StageBtnObj> stageBtn = new List<StageBtnObj>();

    public List<GameObject> stageObjs = new List<GameObject>();
    public List<GameObject> stagesObjsWait = new List<GameObject>();

    public ScrollRect requestScrollRect;
    public GameObject itemReqPrefab;
    public Text stageNameText;

    public int currentStage = 0;
    public Button travelBtn;

    public GameObject unlockConfirm;
    public int stageIdUnlock = 0;

    public void SetBtnStage()
    {
        for (int stageId = 2; stageId < stageBtn.Count + 2; stageId++)
        {
            var b = stageBtn[stageId - 2];
            var price = dc.stage[stageId - 1].unlock_price;
            if (dc.account.achieve_stage.Contains(stageId.ToString()))
            {
                b.Set(false, 0, () => { });
            }
            else
            {
                var sID = stageId;
                b.Set(true, price, () => {
                    stageIdUnlock = sID;
                    unlockConfirm.SetActive(true);
                });
            }
        }
    }

    public void Unlock()
    {
        var s = dc.stage[stageIdUnlock - 1];
        PurchaseMoney(s.unlock_price, () => {
            ServiceManager.instance.PuchaseStage(s.unlock_price, stageIdUnlock.ToString(), unlockConfirm, () => {
                AddCard(stageIdUnlock.ToString());
                moneyPurchasePanel.SetActive(false);
                SetBtnStage();
            });
        });

    }

    public void AddCard(string stageId)
    {
        Array.Resize(ref dc.account.achieve_stage, dc.account.achieve_stage.Length + 1);
        dc.account.achieve_stage[dc.account.achieve_stage.Length - 1] = stageId;
    }

    public bool CheckStageFullCard(int stage)
    {
        var listC = dc.cards.Values.Where(c => c.stage == stage).ToList();
        var invenlistC = listC.Where(c => dc.invenCardsId.Contains(c.id)).ToList();
        return invenlistC.Count == listC.Count && listC.Count != 0;
    }

    public void ToStagePanel(int index)
    {
        currentStage = index;
        SetStageRequest();
        AudioManager.instance.SetBGM(index == 0 ? 2 : 1);
    }
    public void SetStageRequest()
    {
        CloseStage();
        stageObjs[currentStage].SetActive(true);
        stageNameText.text = dc.stage[currentStage].name;
        ClearObjInParent(requestScrollRect.content);
        bool canTravel = true;
        foreach (var itemId in dc.stage[currentStage].requestItemIds)
        {
            if (itemId == "0")
                continue;
            var itemReq = Instantiate(itemReqPrefab.GetComponent<ItemRequestObj>(), requestScrollRect.content);
            itemReq.img.sprite = dc.imgs["i" + itemId];
            itemReq.itemText.text = dc.items[itemId].name;
            bool check = CheckItemRequest(dc.items[itemId].category_id, itemId);
            itemReq.checkObj.SetActive(check);
            if (!check)
            {
                canTravel = check;
            }
        }
        travelBtn.interactable = canTravel;
        //travelBtn.gameObject.SetActive(canTravel);
        travelBtn.onClick.RemoveAllListeners();
        travelBtn.onClick.AddListener(() => { GoTravel(currentStage); });

        if (playTutorial)
        {
            travelBtn.onClick.RemoveListener(() => { FindObjectOfType<TutorialCore>().OnNextStep(); });
            travelBtn.onClick.AddListener(() => {
                FindObjectOfType<TutorialCore>().OnNextStep();
            });
        }
    }
    public bool CheckItemRequest(int catType, string itemIdReq)
    {
        return PlayerPrefs.GetString("s" + catType) == itemIdReq;
    }

    public void CloseStage()
    {
        stageObjs.ForEach(s => s.SetActive(false));
    }
    #endregion

    #region Travel

    [Header("Travel")]
    public GameObject travelPanel;
    public GameObject travelObj;
    public List<GameObject> travelScene = new List<GameObject>();
    public Text travelName;

    public GameObject travelGasObj;

    public Text deepText;
    public Text gasText;
    public Text timerText;

    public int currentTravel = 0;


    public void GoTravel(int index)
    {
        if (index == 1)
        {
            var useGas = 2;
            if (dc.account.gas < useGas)
            {
                SetAlert("แก๊สออกซิเจนของคุณไม่เพียงพอ");
                return;
            }
            PlayerPrefs.SetString("Gas", System.DateTime.Now.ToBinary().ToString());
            dc.account.gas -= useGas + 1;
            ServiceManager.instance.AddGas(-2);
        }
        requestPanel.SetActive(false);
        requestObj.SetActive(false);

        PlayerPrefs.SetInt("TravelIndex", index);
        PlayerPrefs.SetString("Travel" + index, System.DateTime.Now.ToBinary().ToString());
        //SetNotiApp(dc.stage[index].name,(int)dc.stage[index].range_time);
        SetTravel();

    }
    public void SetTravel()
    {
        AudioManager.instance.SetBGM(currentTravel == 0 ? 2 : 3);
        travelPanel.SetActive(true);
        travelObj.SetActive(true);
        currentTravel = PlayerPrefs.GetInt("TravelIndex");
        travelName.text = dc.stage[currentTravel].name;
        travelScene.ForEach(s => s.SetActive(false));
        travelScene[currentTravel].SetActive(true);

        StartCoroutine(CountDownTravel(dc.stage[currentTravel].range_time, CheckDate("Travel" + currentTravel)));
        if (currentTravel == 1)
        {
            travelGasObj.SetActive(true);
            gasText.text = "";
        }
        else
        {
            travelGasObj.SetActive(false);
        }
    }
    IEnumerator CountDownTravel(float maxTime, float timer)
    {
        while (timer < maxTime)
        {
            yield return new WaitForSeconds(1);
            timer += 1;
            timerText.text = FormatSeconds(timer);
            deepText.text = ((int)timer).ToString();
            if (currentTravel == 1)
            {
                gasText.text = ((int)timer).ToString();
            }
        }
        TravelSuccess();
    }

    #endregion

    #region Travel Success

    [Header("Travel Success")]
    public GameObject travelSuccessPanel;
    public GameObject rewardCardPrefab;
    public ScrollRect rewardCardScrollRect;

    public Button miniGameBtn;
    public Text rewardCoinText;
    public Text rewardDiamondText;

    private int rewardCoin = 0;
    private int rewardDiamond = 0;
    public GameObject nextStage;
    private List<string> rewardCardIds = new List<string>();
    public void TravelSuccess()
    {
        AudioManager.instance.SetBGM(4);
        travelPanel.SetActive(false);
        travelSuccessPanel.SetActive(true);
        nextStage.SetActive(false);
        if (PlayerPrefs.HasKey("rewardCoin"))
        {
            rewardCoin = PlayerPrefs.GetInt("rewardCoin");
        }
        else
        {
            rewardCoin = Random.Range(0, 300);
            PlayerPrefs.SetInt("rewardCoin", rewardCoin);
        }

        if (PlayerPrefs.HasKey("rewardDiamond"))
        {
            rewardDiamond = PlayerPrefs.GetInt("rewardDiamond");
        }
        else
        {
            rewardDiamond = Random.Range(0, 50);
            PlayerPrefs.SetInt("rewardDiamond", rewardDiamond);
        }

        rewardCoinText.text = "+ " + rewardCoin;
        rewardDiamondText.text = "+ " + rewardDiamond;

        rewardCardIds.Clear();
        var list = dc.cards.Values.Where(c => c.stage == currentTravel + 1 && !dc.invenCardsId.Contains(c.id)).ToList();
        char[] delimiterChars = { ',' };
        if (PlayerPrefs.HasKey("rewardCardIds"))
        {
            var cIds = PlayerPrefs.GetString("rewardCardIds");
            var inx = cIds.Split(delimiterChars).ToList();
            inx.ForEach(i => {
                rewardCardIds.Add(i);
            });
        }
        else
        {
            if (list.Count > 0)
            {
                var rdAmountCard = Random.Range(1, 3);
                Debug.Log("Reward card " + rdAmountCard);
                for (int i = 0; i < rdAmountCard; i++)
                {
                    var indexCard = Random.Range(0, list.Count);
                    var card = list[indexCard];
                    if (!rewardCardIds.Contains(card.id))
                    {
                        rewardCardIds.Add(card.id);
                    }
                }
                var cardIdsString = string.Join(",", rewardCardIds.ToArray());
                PlayerPrefs.SetString("rewardCardIds", cardIdsString);
            }
        }

        ClearObjInParent(rewardCardScrollRect.content);
        var miniGame = 0;
        rewardCardIds.ForEach(id =>
        {
            var card = dc.cards[id];
            var rewardCard = Instantiate(rewardCardPrefab, rewardCardScrollRect.content);
            rewardCard.GetComponent<Image>().sprite = dc.imgs["cp" + card.id];
            rewardCard.GetComponent<Button>().onClick.AddListener(() => { SetPreviewCard(card.id); });
            if (card.minigame != 0)
            {
                miniGame = card.minigame;
            }
        });
        if (miniGame != 0)
        {
            miniGameBtn.gameObject.SetActive(true);
            miniGameBtn.GetComponent<Animator>().SetBool("isActive", true);
            //miniGameBtn.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            miniGameBtn.interactable = true;
            miniGameBtn.onClick.RemoveAllListeners();
            miniGameBtn.onClick.AddListener(() => { GotoMiniGame(miniGame); });
        }
        else
        {
            miniGameBtn.gameObject.SetActive(false);
            miniGameBtn.GetComponent<Animator>().SetBool("isActive", false);
            //miniGameBtn.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            miniGameBtn.interactable = false;
        }
        rewardCardIds.ForEach(c => {
            if (!dc.invenCardsId.Contains(c))
            {
                dc.invenCardsId.Add(c);
                if (CheckStageFullCard(currentTravel + 1) && dc.stage.Count > currentTravel)
                {
                    AudioManager.instance.SetAudio(AudioManager.instance.winClip);
                    nextStage.SetActive(true);
                    ServiceManager.instance.AddStage(currentTravel + 2);
                    AddCard((currentTravel + 2).ToString());
                    //SetAlert("ยินดีด้วย คุณได้เปิดด่าน \"" + dc.stage[currentTravel + 1].name + "\"");
                }
            }
        });
        if (CheckStageFullCard(currentTravel + 1)){
            nextStage.SetActive(true);
        }
    }

    public void RecieveReward()
    {
        var sm = ServiceManager.instance;

        PlayerPrefs.DeleteKey("rewardCoin");
        PlayerPrefs.DeleteKey("rewardDiamond");
        PlayerPrefs.DeleteKey("rewardCardIds");
        PlayerPrefs.DeleteKey("TravelIndex");
        PlayerPrefs.DeleteKey("Travel" + currentTravel);
        rewardCardIds.ForEach(c => {
            sm.AddCard(c);
        });

        sm.AddCurrency(rewardCoin, 0, () => {
        });
        dc.account.coin += rewardCoin;
        sm.AddCurrency(rewardDiamond, 1, () => {
        });
        dc.account.diamond += rewardDiamond;

        travelObj.SetActive(false);

        AudioManager.instance.SetBGM(1);
        Init();
    }

    public void GotoMiniGame(int indexGame)
    {
        SceneManager.LoadScene(indexGame, LoadSceneMode.Single);
    }

    #endregion

    #region Preview Card

    [Header("Preview Card")]
    public GameObject previewCardPanel;
    public Image previewCardImg;

    public void SetPreviewCard(string cardId)
    {
        previewCardPanel.SetActive(true);
        previewCardImg.sprite = dc.imgs["cp" + cardId];
    }

    #endregion

    #region Alert
    [Header("Alert")]
    public GameObject alertPanel;
    public Text alertText;

    public void SetAlert(string msg)
    {
        Debug.Log("alert: " + msg);
        alertPanel.SetActive(true);
        alertText.text = msg;
    }
    #endregion

    #region MoneyPurchase

    [Header("MoneyPurchase")]
    public GameObject moneyPurchasePanel;
    public Text pricePurchaseText;
    public Button confirmPurchaseBtn;

    public void PurchaseMoney(int cash, Action method)
    {
        moneyPurchasePanel.SetActive(true);
        pricePurchaseText.text = "฿ " + cash;
        confirmPurchaseBtn.onClick.RemoveAllListeners();
        confirmPurchaseBtn.onClick.AddListener(() => { method(); });
    }

    #endregion


    public static Transform ClearObjInParent(Transform transform)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        return transform;
    }
}
static class Extensions
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}