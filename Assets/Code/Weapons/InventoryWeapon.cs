using System;
using InfimaGames.LowPolyShooterPack;
using NaughtyAttributes;
using UnityEngine;
using YG;

public class InventoryWeapon : MonoBehaviour
{
    //0 - Scopes; 1 - Muzzles; 2 - Lasers; 3 - Grips;
    public GameObject[][] Attachments { get; private set; }
    public WeaponAttachmentManager Manager { get; private set; }

    [field: SerializeField] public int WeaponIndex { get; private set; }

    public void Init()
    {
        var attachmentManager = GetComponent<WeaponAttachmentManager>();
        Attachments = new[]
        {
            Array.ConvertAll(attachmentManager.GetScopes,
                s => s.gameObject),
            Array.ConvertAll(attachmentManager.GetMuzzle,
                s => s.gameObject),
            Array.ConvertAll(attachmentManager.GetLaser,
                s => s.gameObject),
            Array.ConvertAll(attachmentManager.GetGrip,
                s => s.gameObject),
        };
        Manager = GetComponent<WeaponAttachmentManager>();
    }

    public void ChosenCurrentAttachments()
    {
        //0 - Weapon; 1 - AttachmentsSection -> Attachments
        //0 - Scopes; 1 - Muzzles; 2 - Lasers; 3 - Grips;
        Debug.Log(WeaponIndex);
        ShowAttachments();
        Manager.SetScope(YandexGame.savesData.ChosenAttachments[WeaponIndex][0]);
        Manager.SetMuzzle(YandexGame.savesData.ChosenAttachments[WeaponIndex][1]);
        Manager.SetLaser(YandexGame.savesData.ChosenAttachments[WeaponIndex][2]);
        Manager.SetGrip(YandexGame.savesData.ChosenAttachments[WeaponIndex][3]);
    }

    [Button("Attachments")]
    public void ShowAttachments()
    {
        var ca = YandexGame.savesData.ChosenAttachments;
        var str = "ChosenAttachments \n";
        for (int i = 0; i < ca.Length; i++)
        {
            str += "[] \n {";
            for (int j = 0; j < ca[i].Length; j++)
            {
                str += ca[i][j] + ", ";
            }

            str += "}; \n";
        }

        Debug.Log(str);
    }
}