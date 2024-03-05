using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения
        public int SceneIndex;

        public int Money = 3000;
        public int Crystals = 3000;
        public int UpgradesLevel = 0;

        public string locale = "";

        // -1 - не дезматч; 0 - до 100 киллов; 1 - 5 мин; 2 - 10 мин;
        public int CharacterIndex = 0;

        //-2 - empty |-1 - close |0 - first level...
        public int[][][] Upgrades =
        {
            //Branch 1 - index 0
            //Expedition
            new[]
            {
                //level 1 - index 0
                new[] { 0, },
                //level 2 - index 1
                new[] { -1, -1, },
                //level 3 - index 2
                new[] {  -1, -1, },
                //level 4 - index 3
                new[] {  -1, -1, },
                //level 5 - index 4
                new[] {  -1, },
            },
            //Branch 2 - index 1
            //Battle
            new[]
            {
                //level 1 - index 0
                new[] { 0, },
                //level 2 - index 1
                new[] { -1, -1, },
                //level 3 - index 2
                new[] {  -1, -1, },
                //level 4 - index 3
                new[] {  -1, -1, },
                //level 5 - index 4
                new[] {  -1, -1, },
            },
            //Branch 3 - index 2
            //Skill
            new[]
            {
                //level 1 - index 0
                new[] { 0, },
                //level 2 - index 1
                new[] { -1,},
                //level 3 - index 2
                new[] { -1}, // empty
                //level 4 - index 3
                new[] {  -1, },
                //level 5 - index 4
                new[] {  -1,}, // empty
            },
            //Branch 4 - index 3
            //Survival
            new[]
            {
                //level 1 - index 0
                new[] { 0, },
                //level 2 - index 1
                new[] { -1, -1, },
                //level 3 - index 2
                new[] { -1, -1, },
                //level 4 - index 3
                new[] { -1, },
                //level 5 - index 4
                new[] { -1,},
            },
        };

        public bool[][] OpenedWeapons =
        {
            new[] { true, false, false, },
            new[] { false, false, false, },
            new[] { false, false, false, false, false, },
            new[] { true, false, false, false, },
            new[] { false, },
        };

        public bool[][] ChosenWeapons =
        {
            new[] { true, false, false, },
            new[] { false, false, false, },
            new[] { false, false, false, false, false, },
            new[] { true, false, false, false, },
            new[] { false, },
        };

        //0 - Weapon; 1 - AttachmentsSection; 2 - Attachment
        //0 - Scopes; 1 - Muzzles; 2 - Lasers; 3 - Grips;
        public bool[][][] OpenedAttachments =
        {
            //Auto Rifles
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            //Rifles
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            //Submachine Guns
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            //Pistols
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
            //Shotguns
            new[]
            {
                //Scopes
                new[] { false, false, false, false, false, false, false, false, },
                //Muzzles
                new[] { true, false, false, false, },
                //Lasers
                new[] { false, false, },
                //Grips
                new[] { false, false, false, },
            },
        };

        public int[][] ChosenAttachments =
        {
            //Auto Rifles
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            //Rifles
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            //Submachine Guns
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            //Pistols
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            new[]
                { -1, 0, -1, -1, },
            //Shotguns
            new[]
                { -1, 0, -1, -1, },
        };

        //Setings
        public float SensitivityX = 1.25f;
        public float SensitivityY = 1.25f;

        //Audio
        public float MasterVolume = -10f;
        public float UIVolume = -30f;
        public float EffectsVolume = -30f;
        public float StepsVolume = -30f;
        public float ShootingVolume = -30f;

        public void SaveMoney(int value)
        {
            Money += value;
            YandexGame.SaveProgress();
        }

        // 0 - lvl, 1 - x,y,z
        // 0 - x, 1 - y, 2 - z;
        // -1000 - trash value
        public float[][] levelsSpawnPlaces =
        {
            new[] { -1000f, -1000f, -1000f },
            new[] { -1000f, -1000f, -1000f },
            new[] { -1000f, -1000f, -1000f },
        };

        public bool[] OpenedLevels =
        {
            true, false,
        };

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            //Debug.Log("UsesLaptops " + UsesLaptops);
        }
    }
}