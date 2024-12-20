using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InfimaGames.LowPolyShooterPack.Interface;
using Random = UnityEngine.Random;
using YG;
using InfimaGames.LowPolyShooterPack;

public class TalentsController : MonoBehaviour
{
    public static TalentsController Instance;

    [field: SerializeField] public Talents Talents { get; private set; }
    [SerializeField] private TalentCardHolder _cardPref;

    private List<TalentCardHolder> cards = new();
    private List<TalentInfo> currInfo = new();
    private CharacterDisabler disabler;
    private GameObject canvas;
    private GameObject chosenC;
    private GameObject talent;

    private void Awake()
    {
        Instance = this;
        LoadSaves();
    }
    
    public string GetTalentName(int index) => Talents.TalentsInfo[index].Name;
    public List<TalentInfo> GetSelectedTalentsInfo()
    {
        return Talents.TalentsInfo.Where(t => t.Opened).ToList();
    }

    private void LoadSaves()
    {
        for (var i = 0; i < YandexGame.savesData.Talents.Length; i++)
        {
            if (YandexGame.savesData.Talents[i])
            {
                Talents.TalentsInfo[i].Open();
            }
        }
    }

    public void ClearSaves()
    {
        foreach (var t in Talents.TalentsInfo)
        {
            t.Close();
        }

        for (var i = 0; i < YandexGame.savesData.Talents.Length; i++)
        {
            YandexGame.savesData.Talents[i] = false;
        }

        //YandexGame.SaveProgress();
    }

    private void Save(int index)
    {
        YandexGame.savesData.Talents[index] = true;
        //YandexGame.SaveProgress();
    }

    private bool CheckTalents()
    {
        return Talents.TalentsInfo.Any(info => !info.Opened);
    }

    public void ShowTalents(GameObject character, GameObject talent)
    {
        this.talent = talent;
        if (!CheckTalents())
        {
            Destroy(talent);
            return;
        }

        var cs = character.GetComponent<CanvasSpawner>();
        var chosen = cs.Chosen.GetComponent<ChoseUIHolder>();

        canvas = cs.Canvas;
        chosenC = cs.Chosen;
        disabler = character.GetComponent<CharacterDisabler>();

        chosenC.SetActive(true);
        canvas.SetActive(false);
        disabler.Disable();

        for (var i = 0; i < 3; i++)
        {
            var card = SpawnCard(chosen.Parent.transform);
            if (card is null)
            {
                break;
            }
        }
    }

    private TalentCardHolder SpawnCard(Transform parent)
    {
        var info = GetRandomTalent();
        if (info is null)
            return null;
        currInfo.Add(info);
        var card = Instantiate(_cardPref, parent);

        card.SetColor(info.BackgroundColor);
        card.SetIcon(info.Sprite);
        card.SetText(info.Name, info.Description);

        card.Button.onClick.AddListener(() => ChoseTalent(info));
        cards.Add(card);

        return card;
    }

    private void ChoseTalent(TalentInfo info)
    {
        //TODO: На данный момент сохраняются таланты после их взятие - исправить сохранение на конец уровня, чтобы избежать абуза
        var index = Talents.TalentsInfo.IndexOf(info);
        Save(index);

        info.Open();
        currInfo.Clear();

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }

        cards.Clear();
        chosenC.SetActive(false);
        canvas.SetActive(true);
        disabler.Activate();
        CheckUseTalent(info, index);
        Destroy(talent);
    }

    private void CheckUseTalent(TalentInfo info, int index)
    {
        var sc = StatsController.Instance;
        var movement = sc.GetComponent<Movement>();

        if (index is 2)
        {
            movement.ApplyAddedSpeedForItem();
        }

        if (index is 9)
        {
            sc.SetNewMaxHealth(sc.Hp.Max / 2);
        }

        if (index is 21)
        {
            YandexGame.savesData.FreePurchases++;
            //YandexGame.SaveProgress();
        }

        if (index is 23)
        {
            movement.ApplySpeedFactor(.15f);
        }

        if (index is 36)
        {
            sc.SetNewMaxHealth(1);
            sc.SetNewMaxArmor(sc.Armor.Max * 3);
        }

        if (index is 40)
        {
            sc.SetNewMaxHealth(sc.Hp.Max * .5f);
            sc.SetNewMaxArmor(sc.Armor.Max * .5f);
            YandexGame.savesData.AddLive += 2;
        }

        if (index is 41)
        {
            sc.SetNewMaxHealth(sc.Hp.Max * 2f);
        }

        if (index is 50)
        {
            movement.ApplySpeedFactor(.5f);
        }

        if (index is 58)
        {
            sc.SetNewMaxHealth(sc.Hp.Max * .25f);
        }

        if (index is 62)
        {
            sc.SetNewMaxArmor(sc.Armor.Max * 1.5f);
            movement.ApplySpeedFactor(-.20f);
        }

        if (index is 64)
        {
            sc.SetNewMaxArmor(sc.Armor.Max * .25f);
        }

        CallbackController.Instance.AddCallBack(new CallbackInfo(CallbackTypes.Text, info.Name));
    }

    private TalentInfo GetRandomTalent()
    {
        var closestTalents =
            Talents.TalentsInfo
                .Where(talent => !talent.Opened && !currInfo.Contains(talent))
                .ToList();
        return closestTalents.Count > 0
            ? closestTalents[Random.Range(0, closestTalents.Count)]
            : null;
    }
}