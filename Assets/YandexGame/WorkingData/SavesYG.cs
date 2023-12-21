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
        public int Money = 2000;
        public int SceneIndex;

        // -1 - не дезматч; 0 - до 100 киллов; 1 - 5 мин; 2 - 10 мин;
        public int DeatmatchType = -1;

        public bool[][] OpenedWeapons =
        {
            new[] { true, false, false, },
            new[] { false, false, false, },
            new[] { false, false, false, false, false, false, },
            new[] { true, false, false, false, },
            new[] { false, },
        };

        public bool[][] ChosenWeapons =
        {
            new[] { true, false, false, },
            new[] { false, false, false, },
            new[] { false, false, false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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
                new[] { false, false, false, false, },
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

        public int UsesLaptops = 0;
        // 0 - lvl, 1 - x,y,z
        // 0 - x, 1 - y, 2 - z;
        // -1000 - trash value
        public float[][] levelsSpawnPlaces =
        {
            new[] { -1000f, -1000f, -1000f },
            new[] { -1000f, -1000f, -1000f },
            new[] { -1000f, -1000f, -1000f },
        };

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
        }
    }
}