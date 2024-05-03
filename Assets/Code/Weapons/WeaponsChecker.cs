using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class WeaponsChecker : MonoBehaviour
{
    [SerializeField] private WarningIcon _weaponWarningIcon;
    [SerializeField] private WarningIcon _firstWarningIcon;
    [SerializeField] private WarningIcon _secondWarningIcon;
    [SerializeField] private List<WarningIcon> _sectionWarnings;

    [Space] [SerializeField] private List<WeaponsSection> _sections;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            Init();
        }
        else
        {
            YandexGame.GetDataEvent += Init;
        }
    }

    private void Init()
    {
        CurrencyController.Instanse.CrystalsChanged += CheckPossibilityBuy;
        CheckPossibilityBuy();
    }

    private void CheckPossibilityBuy()
    {
        var sections = GetPossibility();

        _weaponWarningIcon.gameObject.SetActive(sections.Any(s => s));
        _firstWarningIcon.gameObject.SetActive(GetActivityFirstWarning(sections));
        _secondWarningIcon.gameObject.SetActive(GetActivitySecondWarning(sections));
        for (var s = 0; s < sections.Length; s++)
        {
            _sectionWarnings[s].gameObject.SetActive(sections[s]);
        }
    }

    private bool[] GetPossibility()
    {
        var sections = new bool[_sections.Count];
        for (var i = 0; i < _sections.Count; i++)
        {
            var section = YandexGame.savesData.OpenedWeapons[_sections[i]._section];
            for (var j = 0; j < section.Length; j++)
            {
                if (sections[i])
                    continue;
                //Оружие закрыто. Проверка возможности его покупки.
                if (!section[j])
                {
                    sections[i] = _sections[i].Costs[j] <= YandexGame.savesData.Crystals;
                }
                // Проверка возможности покупки атачментов
                else
                {
                    var attachments = YandexGame.savesData.OpenedAttachments[j];

                    for (var atSection = 0; atSection < attachments.Length; atSection++)
                    {
                        for (var attachment = 0; attachment < attachments[atSection].Length; attachment++)
                        {
                            if (!sections[i] && !attachments[atSection][attachment])
                            {
                                switch (atSection)
                                {
                                    case 0:
                                        sections[i] = AttachmentsCostsHolder.Instance.CostsScopes[attachment] <=
                                                      YandexGame.savesData.Crystals;
                                        break;
                                    case 1:
                                        sections[i] = AttachmentsCostsHolder.Instance.CostsMuzzle[attachment] <=
                                                      YandexGame.savesData.Crystals;
                                        break;
                                    case 2:
                                        sections[i] = AttachmentsCostsHolder.Instance.CostsLaser[attachment] <=
                                                      YandexGame.savesData.Crystals;
                                        break;
                                    case 3:
                                        sections[i] = AttachmentsCostsHolder.Instance.CostsGrip[attachment] <=
                                                      YandexGame.savesData.Crystals;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return sections;
    }

    private bool GetActivityFirstWarning(bool[] sections)
    {
        for (var s = 0; s < 3; s++)
            if (sections[s])
                return true;
        return false;
    }

    private bool GetActivitySecondWarning(bool[] sections)
    {
        for (var s = 3; s < 5; s++)
            if (sections[s])
                return true;
        return false;
    }
}