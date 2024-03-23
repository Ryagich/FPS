using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;

public class TradingCanvas : MonoBehaviour
{
    public static TradingCanvas Instance;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private List<TradeItemInfo> _info;
    [SerializeField] private Transform _parent;
    [SerializeField] private TradeButton _buttonPref;

    [Space] [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _buyButton;

    [FormerlySerializedAs("_addButton")] [SerializeField]
    private Button _adButton;

    private List<TradeButton> buttons = new();
    private CharacterDisabler disabler;

    private void Awake()
    {
        Instance = this;
        SetUp();
        if (YandexGame.savesData.FreePurchases is -1)
        {
            YandexGame.savesData.FreePurchases = 1 + YandexGame.savesData.Upgrades[0][4][0];
            YandexGame.SaveProgress();
        }
    }

    public void SetUp()
    {
        // -1 - 1 lvl | 0 - 2 lvl | 1 - 3 lvl
        switch (YandexGame.savesData.Upgrades[0][2][0])
        {
            case -1:
                InstantiateButtons(4);
                break;
            case 0:
                InstantiateButtons(9);
                break;
            case 1:
                InstantiateButtons(10);
                break;
        }
    }

    private void InstantiateButtons(int value)
    {
        for (var i = 0; i < value; i++)
        {
            var index = i;
            var button = Instantiate(_buttonPref, _parent);
            button.SetInfo(_info[index]);
            button.Button.onClick.AddListener(() => UpdateDescription(_info[index]));
        }
    }

    public void DisableDescription()
    {
        _name.text = "";
        _description.text = "";
        _buyButton.onClick.RemoveAllListeners();
        _adButton.onClick.RemoveAllListeners();
        _buyButton.interactable = false;
        _adButton.interactable = false;
    }

    private void UpdateDescription(TradeItemInfo info)
    {
        _name.text = info.Name;
        _description.text = info.Description;

        _buyButton.onClick.RemoveAllListeners();
        _adButton.onClick.RemoveAllListeners();

        int cost;
        if (YandexGame.savesData.FreePurchases > 0)
        {
            cost = 0;
            YandexGame.savesData.FreePurchases--;
            YandexGame.SaveProgress();
        }
        else
        {
            cost = info.Cost + (int)(info.Cost * .1f * (1 + YandexGame.savesData.Upgrades[0][3][0]));
        }

        _buyButton.GetComponentInChildren<TMP_Text>().text = "Buy " + cost;

        _buyButton.interactable = !info.Opened && YandexGame.savesData.Money >= cost;
        _adButton.interactable = !info.Opened;

        _buyButton.onClick.AddListener(() => Buy(info, cost));
        _adButton.onClick.AddListener(() => AdBuy(info));
    }

    private void AdBuy(TradeItemInfo info)
    {
        YandexGame.RewVideoShow(1);
        Buy(info, 0);
    }

    private void Buy(TradeItemInfo info, int cost)
    {
        info.Open();
        YandexGame.savesData.Money -= cost;
        YandexGame.SaveProgress();

        var statsC = Character.Instance.GetComponent<StatsController>();
        var addSpellC = Character.Instance.GetComponent<AddSpellController>();
        var inventory = Character.Instance.GetInventory() as Inventory;
        var callback = CallbackController.Instance;

        switch (info.Type)
        {
            case TradeButtonType.HealthRestore:
                statsC.TakeHealth(25);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Health, "25"));
                break;
            case TradeButtonType.ArmorRestore:
                statsC.TakeArmor(25);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Armor, "25"));
                break;
            case TradeButtonType.AddSpellRestore:
                addSpellC.SetMax();
                callback.AddCallBack(new CallbackInfo(CallbackTypes.AddSpell, "1"));
                break;
            case TradeButtonType.AmmoRestore:
                inventory.TakePercentAllAmmoTypes(.2f);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                break;
            case TradeButtonType.HealthAdd:
                statsC.Hp.ChangeMax(statsC.Hp.Max + 20);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Health, "20"));
                break;
            case TradeButtonType.ArmorAdd:
                statsC.Armor.ChangeMax(statsC.Armor.Max + 15);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Armor, "15"));
                break;
            case TradeButtonType.AddSpellAdd:
                addSpellC.AddMax();
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                callback.AddCallBack(new CallbackInfo(CallbackTypes.AddSpell, "1"));
                break;
            case TradeButtonType.AmmoAdd:
                inventory.ApplyMaxAmmoFactor(.2f);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                break;
            case TradeButtonType.AddDamage:
                foreach (var weapon in (Weapon[])inventory.weapons)
                    weapon.SetDamage();
                YandexGame.savesData.DamageFactor += .2f;
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                break;
            case TradeButtonType.AddLive:
                YandexGame.savesData.AddLive++;
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
                break;
        }

        YandexGame.SaveProgress();
        UpdateDescription(info);
    }

    public void Open(GameObject character)
    {
        disabler = character.GetComponent<CharacterDisabler>();
        disabler.Disable();
        _canvas.SetActive(true);
        character.GetComponent<CanvasSpawner>().Canvas.gameObject.SetActive(false);
    }

    public void Close()
    {
        disabler.Activate();
        _canvas.SetActive(false);
        disabler.GetComponent<CanvasSpawner>().Canvas.gameObject.SetActive(true);
    }
}