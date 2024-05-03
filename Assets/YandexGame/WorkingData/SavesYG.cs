using UnityEngine.Serialization;

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

        //Сохранения забега
        public int Stage = 0;
        public int Money = 0;
        public int MainSpellMax = -1;
        public int MainSpellCount = -1;
        public int AddSpellMax = -1;
        public int AddSpellCount = -1;

        [FormerlySerializedAs("MaxArmor")] public float ArmorMax = 0;
        public float Armor = -1;

        [FormerlySerializedAs("MaxHealth")] public float HealthMax = 0;
        public float Health = -1;

        public int[] AmmunitionMax = { -1, -1, -1, -1, -1, -1, -1 };
        public int[] Ammunition = { -1, -1, -1, -1, -1, -1, -1 };

        public float DamageFactor = .0f;
        public int AddLive = 0;
        public bool IsRun = false;
        public int FreePurchases = -1;

        public float speedWalking = -1f;
        public float speedAiming = -1f;
        public float speedCrouching = -1f;
        public float speedRunning = -1f;
        //

        public int Crystals = 3000;
        public int UpgradesLevel;

        public string locale = "";

        // 0 - Rambo | 1 - Booth | 2 - Shadow
        // Rambo: Hp 210 | Armor 180 | Speed 0.8 | Ammo 1.7 | AddSpell Max 2 | Main Spell Cooldown 120 sec
        // Booth: Hp 150 | Armor 110 | Speed 1.0 | Ammo 1.2 | AddSpell Max 2 | Main Spell Cooldown 60 sec
        // Shadow: Hp 100 | Armor 80 | Speed 1.5 | Ammo 1.0 | AddSpell Max 2 |
        
        public int CharacterIndex;
        public int DifficultIndex;
  
        //65 Таланта пиздец
// | 0 +++ | Не критический урон возрастает на 20%.
// | 1 +++ | Каждая 4 атака наносит на 80% больше урона 
// | 2 +++ | При поднятие предмета, скорость передвижения вырастает на 20% на 2 секунды.
// | 3 +++ | При полном здоровье наносимый урон выше на 25%.
// | 4 +++ | Первая пуля в магазине наносит на 100% больше урона.
// | 5 +++ | Скорость передвижения возрастает на 50% при отсутствие щита.
// | 6 +++ | При поднятие патронов, так же постанавливается 2 единицы брони.
// | 7 +++ | При поднятии монет или кристаллов, так же востанавливается 1 единица здоровья.
// | 8 --- | Мощность дополнительной способности возрастает вдвое.
// | 9 +++ | Здоровье уменьшается вдвое. Получайте вдвое больше монет и кристаллов из всех источников
// | 10 --- | При перезарядке есть 30% шанс не потратить патроны.
// | 11 --- | При неподвижном состоянии урон возрастает на 50%
// | 12 +++ | +2% к скорости передвижения за каждые 10% щита.
// | 13 +++ | Все собираемые патроны, преобразовываются в патроны используемые текущим оружием.
// | 14 +++ | Получаемое лечение из всех источников увеличивается вдвое.
// | 15 +++ | +50% урона критических атак, но обычные атаки наносят на 20% меньше урона.
// | 16 --- | Получите неуязвимость к урону на 2 секунды после перезарядки.
// | 17 +++ | +1% к шансу нанести критический урон за каждые 5% отсутствующего здоровья.
// | 18 +++ | +75% к наносимому урону, если броня полная, иначе наносите на 25% меньше урона.
// | 19 --- | 25% шанс выбить патроны с поверженного противника.
// | 20 --- | Противники на расстояние 10 и более метров получает на 50% больше урона, противники находящиеся ближе, получают на 15% меньше урона.
// | 21 +++ | Следующая покупка у торговца будет бесплатна. Талант исчезает после использования.
// | 22 +++ | Начните использовать бронепластины. Бронепластина блокирует любой следующий урон, после чего ломается. Каждые 10 секунд восстановите 1 бронепластину. Максимум 3 бронепластины.
// | 23 +++ | Скорость передвижения возрастает на 15%.
// | 24 --- | 10% шанс выбить, при убийстве противника, дополнительные монеты или заряд для дополнительного навыка.
// | 25 --- | Шанс найти монеты или кристаллы в бьющихся предметах возрастает. Шанс найти все остальное - ниже.
// | 26 --- | Первый выстрел после каждых 3 секунд будет критическим.
// | 27 +-- | Атаки восстанавливают 1 единицу здоровья, если здоровье полное - восстанавливается броня. Атаки наносят на 50% меньше урона.
// | 28 --- | Шанс нанести критический урон врагу, стоящему менее, чем в 5 метрах, возрастает вдвое.
// | 29 +++ | При перезарядке рассходуется вдвое больше патронов (в первую очередь за счет не используемых типов боеприпасов). Щанс критического выстрела возрастает на 25%.
// | 30 --- | Во время перезарядки скорость перемещения увеличивается на 25%, получаемый урон меньше на 10%.
// | 31 +++ | После получение урона восстанавливается по 3 единицы здоровья в течение 5 секунд.
// | 32 +++ | При наличие зарядов дополнительного навыка наносите на 75% больше урона, при их отсутствие наносите на 25% меньше урона.
// | 33 +++ | При получение урона, собственный урон возрастает на 15% на 5 секунд.
// | 34 +++ | Получив летальный урон восстановите 10% от максимального здоровья. Навык исчезает после срабатывания.
// | 35 +++ | Получив летальный урон, станьте неуязвимым на 5 секунд, если в течение этого времени, вы не пополните здоровье, то погибните. Навык исчезает после срабатывания.
// | 36 +++ | Максимальное здоровье опускается до 1 единицы. Количество брони возрастает втрое.
// | 37 +++ | Каждую секунду 4% здоровья от максимального преобразуется в недостающую броню. В результате использования способности здоровье не может упасть ниже 20%.
// | 38 +++ | Получив летальный урон, восстановите 10% здоровья, если в течение 5 секунд после срабатывания способности вы не убьете ни одного противника, то погибните. Навык исчезает после срабатывания.
// | 39 +++ | Когда магазин пуст, вы продолжаете стрелять нанося на 2% больше урона за каждый выстрел, за каждый выстрел потеряйте 1% здоровья от максимального. Возможно погибнуть в результате использования навыка.
// | 40 +++ | Потеряйте 50% максимальной брони и 50% максимального здоровья. Получите 2 дополнительные жизни.
// | 41 +++ | Максимальное здоровье увеличивается вдвое. Получайте на 10% больше урона из всех источников.
// | 42 --- | Каждую секунду шанс критического выстрела возрастает на 5%. Сбрасывается после нанесения критического выстрела.
// | 43 --- | 5% шанс, что при получении урона врагом 5% от его максимального здоровья преобразуется в дополнительный урон.
// | 44 --- | 50% шанс восстановить 5 единиц брони при убийстве врага.
// | 45 --- | Чем быстрее ваша текущая скорость, тем выше ваше сопротивление получаемому урону. Максимум 30%.
// | 46 --- | +10% к сопротивлению урона за каждые 100 разрасходованных патронов на 5 секунд.
// | 47 --- | Награда за убийство врагов на расстояние до 7 метров выше вдвое.
// | 48 --- | +1 к максимальному здоровью за каждого убитого врага на расстояние до 7 метров.
// | 49 +++ | Получите дополнительне 15% к сопротивлению урону, когда есть броня. Станьте уязвимие к урону на 15%, когда нет брони.
// | 50 +++ | Скорость передвижения выше на 50%, основной навык восстанавливается на 150% дольше.
// | 51 +++ | Дополнительный навык можно использовать, даже когда отсутствуют снаряды, за каждое такое использование получите 25 единиц урона.
// | 52 +++ | Чем меньше патронов остается в магазине, тем выше урон оружия (максимум +75%).
// | 53 --- | Чередуйте состояния "Инь" и "Ян" каждые 12 секунд. Состояние "Инь" дает + 20% к сопротивлению урону, а состояние "Ян" дает + 50% к наносимому урону.
// | 54 --- | За каждого поверженного врага получайте +1% к урону и -0.5% к сопротивлению урону.
// | 55 +++ | При получение урона получите +20% к шансу нанести критический урон на 5 секунды.
// | 56 +++ | При использование дополнительного навыка воостановите 5% брони от ее максимума.
// | 57 +++ | Уменьшите сопротивление урону на 100%, но получаемый урон не может превышать 10% от максимального здоровья.
// | 58 +++ | Получите 100% к урону. Потеряйте 75% макимального здоровья.
// | 59 --- | Критические атаки проходят сквозь броню противников.
// | 60 +++ | +5% к урону за каждые 100 монет в запасе.
// | 61 +++ | Когда броня пробита, мгновенно восстановите 10% брони. Перезарядка 45 секунд.
// | 62 +++ | Получите +50 к максимальному щиту и +10% к сопротивлению урону. Потеряйте 20% скорости передвижения.
// | 63 +++ | +50% к шансу нанести критический урон, но максимальный шанс не может превышать 75%.
// | 64 +++ | Максимум брони уменьшается на 75%, каждую секунду восстанавливайте 1 единицу брони.

        public bool[] Talents = new bool[65];

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
                //start money |100|200|300|400|500| | Шанс найти монеты в окружение |20%|30%|45%|60%|80%|
                new[] { -1, -1, }, // +++ | +++
                //level 3 - index 2
                //Новые товары у торговца 1 и 2 уровни | Шанс получить доп монеты за убийство врагов |10%|15%|25%|30%|
                new[] { -1, -1, }, // +++ | ---
                //level 4 - index 3
                //Снижение цен у торговца |10%|20%|30%|40%|50% | Шанс удвоить найденные монеты 5%|10%|15%|20%|25%
                new[] { -1, -1, }, // +++ | +++
                //level 5 - index 4
                //Первые покупки у торговца бесплатны кол-во: 1|2|3|4|5
                new[] { -1, }, // +++
            },
            //Branch 2 - index 1
            //Battle
            new[]
            {
                //level 1 - index 0
                //Увеличение наносимого урона на 5%|10%|15%|20%|25%|30%|35%|40%|45%|50%
                new[] { 0, }, // +++
                //level 2 - index 1
                //Больше патронов на 5%|10%|15%|20%|25%|30%|35%|40%|45%|50% | Урон пистолета + 10%|20%|30%|40%|50%
                new[] { -1, -1, }, // +++ | +++
                //level 3 - index 2
                //Увеличивает скорость перезарядки на 25%|50% | Увеличивает урон основного оружия 10%|20%|30%|40%|50%
                new[] { -1, -1, }, // --- | +++
                //level 4 - index 3
                //Увеличивает урон дробовика 10%|20%|30%|40%|50% | атаки могут нанести крит 150% урона 10% шанс
                new[] { -1, -1, }, // +++ | +++
                //level 5 - index 4
                //Урон крит атак увеличивается до 175%|200%|250% | Шанс крит атаки увеличивается до 15%|20%
                new[] { -1, -1, }, // +++ | +++
            },
            //Branch 3 - index 2
            //Skill
            new[]
            {
                //level 1 - index 0
                new[] { 0, },
                //level 2 - index 1
                //Уменьшает время перезарядки способностей на 10%|20%|30%|40%|50%
                new[] { -1, }, // --- 
                //level 3 - index 2
                new[] { -2 }, // empty
                //level 4 - index 3
                //Повышает силу и эффективность способностей - Придумать что-то конкретное.
                new[] { -1, }, // --- 
                //level 5 - index 4
                new[] { -2, }, // empty
            },
            //Branch 4 - index 3
            //Survival
            new[]
            {
                //level 1 - index 0
                //Увеличивает хп на 10|20|30|40|50
                new[] { 0, }, // +++
                //level 2 - index 1
                //На уровнях можно найти Хп | На уровнях можно найти броню
                new[] { -1, -1, }, // +++ | +++
                //level 3 - index 2
                //Увеличивает лечение из всех источников на |10%|20%|30%|40%|50%| | Увеличение брони на |15|30|50|
                new[] { -1, -1, }, // +++ | +++
                //level 4 - index 3
                //Восстановления хп каждую секунду 0.5|1|2
                new[] { -1, }, // +++
                //level 5 - index 4
                //Получениемый урон меньше на |5%|10%|15%|20%|25%|
                new[] { -1, }, // +++
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
        public float EffectsVolume = -15f;
        public float StepsVolume = -30f;
        public float ShootingVolume = -30f;
        public float MusicVolume = -25f;
        
        public void SaveMoney(int value)
        {
            Money += value;
            YandexGame.SaveProgress();
        }
        
        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            //Debug.Log("UsesLaptops " + UsesLaptops);
        }
    }
}