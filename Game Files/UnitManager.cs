﻿/* This file is part of Peasant's Ascension.
 * 
 * Peasant's Ascension is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Peasant's Ascension is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Peasant's Ascension.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Game
{
    public static class UnitManager
    {
        /* =========================== *
         *            FIELDS           *
         * =========================== */
        #region
        // Unit manager is responsible for storing PCUs and generating monsters, as well as 
        // performing basic methods with units such as calculating damage
        public static PlayableCharacter player = new PlayableCharacter("John", CEnums.CharacterClass.warrior, "_player", true);
        public static PlayableCharacter solou = new PlayableCharacter("Solou", CEnums.CharacterClass.mage, "_solou", false);
        public static PlayableCharacter chili = new PlayableCharacter("Chili", CEnums.CharacterClass.ranger, "_chili", false);
        public static PlayableCharacter chyme = new PlayableCharacter("Chyme", CEnums.CharacterClass.monk, "_chyme", false);
        public static PlayableCharacter storm = new PlayableCharacter("Storm", CEnums.CharacterClass.assassin, "_storm", false);
        public static PlayableCharacter tavlosk = new PlayableCharacter("Tavlosk", CEnums.CharacterClass.paladin, "_tavlosk", false);
        public static PlayableCharacter adorine = new PlayableCharacter("Adorine", CEnums.CharacterClass.warrior, "_adorine", false);
        public static PlayableCharacter kaltoh = new PlayableCharacter("Kaltoh", CEnums.CharacterClass.bard, "_kaltoh", false);

        // A list of all bosses in the game
        public static List<Boss> BossList = new List<Boss>() { new MasterSlime() };

        public static List<Monster> MonsterList = CMethods.ShuffleIterable(new List<Monster>()
        {
            new FireAnt(), new FrostBat(), new SparkBat(), new SludgeRat(), new GiantLandSquid(),
            new GiantCrab(), new SnowWolf(), new Beetle(), new VineLizard(), new GirthWorm(),
            new Willothewisp(), new Alicorn(), new BogSlime(),
            new SandGolem(), new Griffin(), new Harpy(), new SeaSerpent(), new NagaBowwoman(),
            new Troll(), new MossOgre(), new LesserYeti(), new RockGiant(), new GoblinArcher(),
            new Oread(), new TenguRanger(), new Naiad(), new Imp(), new Spriggan(),
            new Zombie(), new UndeadCrossbowman(), new LightningGhost(), new Mummy(), new SkeletonBoneslinger(), new WindWraith(),
            new Necromancer(), new CorruptThaumaturge(), new IceSoldier(), new FallenKnight(), new DevoutProtector(),
        }).ToList();
        #endregion

        /* =========================== *
         *      RETRIEVAL METHODS      *
         * =========================== */
        #region
        public static List<PlayableCharacter> GetAllPCUs()
        {
            // Returns ALL PCUs, alive, dead, active, and inactive
            return new List<PlayableCharacter>() { player, solou, chili, chyme, storm, tavlosk, adorine, kaltoh };
        }

        public static List<PlayableCharacter> GetAlivePCUs()
        {
            // Returns all PCUs that are alive, regardless of whether they're active or not
            return GetAllPCUs().Where(x => x.IsAlive()).ToList();
        }

        public static List<PlayableCharacter> GetActivePCUs()
        {
            // Returns all PCUs that are active, regardless of whether they're alive or not
            return GetAllPCUs().Where(x => x.Active).ToList();
        }

        public static List<PlayableCharacter> GetAliveActivePCUs()
        {
            // Returns all PCUs that are both alive and active
            return GetAllPCUs().Where(x => x.Active && x.IsAlive()).ToList();
        }

        public static Boss FindBossWithID(string boss_id)
        {
            // Returns the boss with the specified boss_id
            return BossList.First(x => x.BossID == boss_id);
        }

        public static StatMatrix GetAttributeMatrix(CEnums.PlayerAttribute attribute)
        {
            // These stat matrixes define how many additional stat points 
            // your character gets when they put a point into an attribute            
            return new Dictionary<CEnums.PlayerAttribute, StatMatrix>()
            {
                { CEnums.PlayerAttribute.strength, new StatMatrix(0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0) },
                { CEnums.PlayerAttribute.intelligence, new StatMatrix(0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0) },
                { CEnums.PlayerAttribute.dexterity, new StatMatrix(0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1) },
                { CEnums.PlayerAttribute.perception, new StatMatrix(0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1) },
                { CEnums.PlayerAttribute.constitution, new StatMatrix(1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0) },
                { CEnums.PlayerAttribute.wisdom, new StatMatrix(0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0) },
                { CEnums.PlayerAttribute.charisma, new StatMatrix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) }
            }[attribute];
        }

        public static StatMatrix GetLevelUpMatrix(CEnums.CharacterClass p_class)
        {
            // These stat matrixes determine how many additional stat points
            // your character gets upon level-up, depending on your class
            return new Dictionary<CEnums.CharacterClass, StatMatrix>()
            {
                { CEnums.CharacterClass.warrior, new StatMatrix(2, 1, 0, 3, 3, 1, 3, 1, 1, 1, 1) },
                { CEnums.CharacterClass.mage, new StatMatrix(1, 3, 0, 1, 1, 2, 1, 3, 3, 1, 2) },
                { CEnums.CharacterClass.assassin, new StatMatrix(1, 1, 0, 3, 1, 1, 2, 1, 1, 3, 2) },
                { CEnums.CharacterClass.ranger, new StatMatrix(1, 1, 0, 1, 1, 3, 1, 1, 1, 2, 3) },
                { CEnums.CharacterClass.monk, new StatMatrix(1, 2, 0, 3, 1, 1, 1, 1, 1, 3, 3) },
                { CEnums.CharacterClass.paladin, new StatMatrix(2, 2, 0, 1, 3, 1, 2, 1, 3, 1, 1) },
                { CEnums.CharacterClass.bard, new StatMatrix(1, 2, 0, 1, 1, 1, 1, 1, 2, 2, 3) }
            }[p_class];
        }

        public static StatMatrix GetClassMatrix(CEnums.CharacterClass p_class)
        {
            // These stat matrixes determine how many additional stat points
            // your character will get when they spec into these classes
            return new Dictionary<CEnums.CharacterClass, StatMatrix>()
            {
                { CEnums.CharacterClass.warrior, new StatMatrix(5, -1, 0, 3, 3, 0, 2, 0, 0, -1, -1) },
                { CEnums.CharacterClass.mage, new StatMatrix(1, 6, 0, 0, 0, 1, 0, 4, 3, 0, 0) },
                { CEnums.CharacterClass.assassin, new StatMatrix(2, 1, 0, 3, 2, 0, 0, 1, 0, 4, 2) },
                { CEnums.CharacterClass.ranger, new StatMatrix(0, 2, 0, 0, 0, 4, 0, 0, 2, 3, 3) },
                { CEnums.CharacterClass.monk, new StatMatrix(2, 2, 0, 3, -1, 0, 0, 0, 2, 3, 3) },
                { CEnums.CharacterClass.paladin, new StatMatrix(3, 4, 0, 3, 3, 0, 3, 3, 3, -2, -2) },
                { CEnums.CharacterClass.bard, new StatMatrix(-1, 3, 0, 0, -1, 0, -1, 3, 1, 0, 0) }
            }[p_class];
        }
        #endregion

        /* =========================== *
         *        OTHER METHODS        *
         * =========================== */
        public static void CreatePlayer()
        {
            CMethods.PrintDivider();
            player.PlayerChooseName();
            player.PlayerChooseClass();
            SavefileManager.ChooseAdventureName();

            if (player.PClass == CEnums.CharacterClass.warrior)
            {
                InventoryManager.EquipItem(player, "iron_hoe");
            }

            else if (player.PClass == CEnums.CharacterClass.assassin)
            {
                InventoryManager.EquipItem(player, "stone_dagger");
            }

            else if (player.PClass == CEnums.CharacterClass.ranger)
            {
                InventoryManager.EquipItem(player, "sling_shot");
            }

            else if (player.PClass == CEnums.CharacterClass.mage)
            {
                InventoryManager.EquipItem(player, "magical_twig");
            }

            else if (player.PClass == CEnums.CharacterClass.monk)
            {
                // Monk doesn't have any weapons, so we don't need to equip anything here
            }

            else if (player.PClass == CEnums.CharacterClass.paladin)
            {
                InventoryManager.EquipItem(player, "rubber_mallet");
            }

            else if (player.PClass == CEnums.CharacterClass.bard)
            {
                InventoryManager.EquipItem(player, "kazoo");
            }

            player.CalculateStats();
            solou.CalculateStats();
            chili.CalculateStats();
            chyme.CalculateStats();
            storm.CalculateStats();
            tavlosk.CalculateStats();
            adorine.CalculateStats();
            kaltoh.CalculateStats();

            HealAllPCUs(true, true, true, true);
            GameLoopManager.ExplainTheSetting();
            SavefileManager.SaveTheGame();
        }

        public static List<Monster> GetRandomEncounter(int max_monsters=3)
        {
            Random rng = new Random();
            
            // How many monsters will be generated
            int num_monsters = 1;

            // Chance of generating an additional monster. This chance goes
            // down by half for every monster generated
            double chance = 75.0;  

            while (num_monsters < max_monsters)
            {
                int x = rng.Next(1, 100);
                if (x < chance)
                {
                    num_monsters++;
                    chance /= 2;
                }
                
                else
                {
                    break;
                }
            }

            List<Monster> monster_list = new List<Monster>();

            for (int i=0; i<num_monsters; i++)
            {
                // Get a list of all the monster groups that this cell has in its MonsterGroups property
                List<CEnums.MonsterGroup> cell_groups = TileManager.FindCellWithTileID(CInfo.CurrentTile).MonsterGroups;

                // Create a list of all the monsters possible to fight on the current tile
                List<Monster> monsters = MonsterList.Where(x => cell_groups.Contains(x.MonsterGroup)).ToList();

                // Choose a random monster type from the list and create a new monster out of it
                Monster chosen_monster = CMethods.GetRandomFromIterable(monsters);
                Monster new_monster = Activator.CreateInstance(chosen_monster.GetType()) as Monster;

                // Level-up the monster to increase its stats to the level of the cell that the player is in
                new_monster.MonsterLevelUp();

                // Apply multipliers to the monster based on its species, class, and party difficulty
                new_monster.MonsterApplyMultipliers();

                monster_list.Add(new_monster);
            }

            // The monster encounter has been generated. It has not yet been assigned to  
            // BattleManager.monster_list, put this list in BattleManager.SetMonsterList() to set it.
            return monster_list;
        }

        public static List<Monster> GetBossEncounter(string boss_id)
        {
            Boss the_boss = FindBossWithID(boss_id);
            var boss_fight = new List<Monster>() { the_boss }.Concat(the_boss.Lackies);
            return boss_fight.ToList();
        }

        public static void HealAllPCUs(bool restore_hp, bool restore_mp, bool restore_ap, bool cure_statuses)
        {
            // Completely revitalizes all PCUs
            GetAllPCUs().ForEach(x => x.FullyHealUnit(restore_hp, restore_mp, restore_ap, cure_statuses));
        }

        public static bool CheckForBosses()
        { 
            foreach (string boss_id in TileManager.FindTileWithID(CInfo.CurrentTile).BossList)
            {
                Boss boss = FindBossWithID(boss_id);
                if (!boss.Defeated && boss.Active)
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("You feel the presence of an unknown entity...");

                    while (true)
                    {
                        string yes_no = CMethods.SingleCharInput($"Do you investigate? ").ToLower();

                        if (yes_no.IsYesString())
                        {
                            BattleManager.SetMonsterList(GetBossEncounter(boss_id));
                            CMethods.PrintDivider();
                            BattleManager.BattleSystem(is_bossfight: true);

                            return true;
                        }

                        else if (yes_no.IsNoString())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

    /* =========================== *
     *       NON-UNIT TYPES        *
     * =========================== */
    public class StatMatrix
    {
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int MaxAP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int PAttack { get; set; }
        public int PDefense { get; set; }
        public int MAttack { get; set; }
        public int MDefense { get; set; }
        public int Speed { get; set; }
        public int Evasion { get; set; }

        public StatMatrix(int maxhp, int maxmp, int maxap, int attk, int dfns, int pattk, int pdfns, int mattk, int mdfns, int spd, int evad)
        {
            MaxHP = maxhp;
            MaxMP = maxmp;
            MaxAP = maxap;
            Attack = attk;
            Defense = dfns;
            PAttack = pattk;
            PDefense = pdfns;
            MAttack = mattk;
            MDefense = mdfns;
            Speed = spd;
            Evasion = evad;
        }
    }

    public class TargetMapping
    {
        public bool TargetSelf { get; set; }
        public bool TargetAllies { get; set; }
        public bool TargetEnemies { get; set; }
        public bool TargetDead { get; set; }

        public TargetMapping(bool t_self, bool t_allies, bool t_enemies, bool t_dead)
        {
            TargetSelf = t_self;
            TargetAllies = t_allies;
            TargetEnemies = t_enemies;
            TargetDead = t_dead;
        }
    }

    /* =========================== *
     *       STANDARD UNITS        *
     * =========================== */
    public abstract class Unit
    {
        public string UnitName { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int AP { get; set; }

        // These properties are protected, because we do not want them to be directly
        // accessed or manipulated by outside methods. Instead we use Unit.TempStats.
        protected int MaxHP { get; set; }
        protected int MaxMP { get; set; }
        protected int MaxAP { get; set; }
        protected int Attack { get; set; }
        protected int Defense { get; set; }
        protected int PAttack { get; set; }
        protected int PDefense { get; set; }
        protected int MAttack { get; set; }
        protected int MDefense { get; set; }
        protected int Speed { get; set; }
        protected int Evasion { get; set; }

        public CEnums.Element OffensiveElement { get; set; }
        public CEnums.Element DefensiveElement { get; set; }
        public List<CEnums.Status> Statuses { get; set; }

        public StatMatrix TempStats = new StatMatrix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /* =========================== *
         *           METHODS           *
         * =========================== */
        #region
        public void SetTempStats()
        {
            TempStats.MaxHP = MaxHP;
            TempStats.MaxMP = MaxMP;
            TempStats.MaxAP = MaxAP;
            TempStats.Attack = Attack;
            TempStats.Defense = Defense;
            TempStats.PAttack = PAttack;
            TempStats.PDefense = PDefense;
            TempStats.MAttack = MAttack;
            TempStats.MDefense = MDefense;
            TempStats.Speed = Speed;
            TempStats.Evasion = Evasion;
        }

        public void FixAllStats()
        {
            // Makes sure that that no-one ever has stats that would cause the game to malfunction.
            // e.g. no negative HP/MP/AP, no HP/MP/AP above max, etc.
            // This function also acts as a hard-cap for evasion, which is limited to a max of 256
            // (50% dodge chance). This is to prevent people from min-maxing their evasion to cheese
            // their way through the game, and also prevents monsters from being invincible.

            // Fix true stats
            MaxHP = Math.Max(1, MaxHP);
            MaxMP = Math.Max(1, MaxMP);
            MaxAP = Math.Max(1, MaxAP);

            Attack = Math.Max(1, Attack);
            PAttack = Math.Max(1, PAttack);
            MAttack = Math.Max(1, MAttack);

            Defense = Math.Max(1, Defense);
            PDefense = Math.Max(1, PDefense);
            MDefense = Math.Max(1, MDefense);

            Speed = Math.Max(1, Speed);
            Evasion = (int)CMethods.Clamp(Evasion, 1, 256);

            // Fix temp stats
            TempStats.MaxHP = Math.Max(1, TempStats.MaxHP);
            TempStats.MaxMP = Math.Max(1, TempStats.MaxMP);
            TempStats.MaxAP = Math.Max(1, TempStats.MaxAP);

            TempStats.Attack = Math.Max(1, TempStats.Attack);
            TempStats.PAttack = Math.Max(1, TempStats.PAttack);
            TempStats.MAttack = Math.Max(1, TempStats.MAttack);

            TempStats.Defense = Math.Max(1, TempStats.Defense);
            TempStats.PDefense = Math.Max(1, TempStats.PDefense);
            TempStats.MDefense = Math.Max(1, TempStats.MDefense);

            TempStats.Speed = Math.Max(1, TempStats.Speed);
            TempStats.Evasion = (int)CMethods.Clamp(TempStats.Evasion, 1, 256);

            // Fix HP/MP/AP
            HP = (int)CMethods.Clamp(HP, 0, MaxHP);
            MP = (int)CMethods.Clamp(MP, 0, MaxMP);
            AP = (int)CMethods.Clamp(AP, 0, MaxAP);

            // Fix statuses
            Statuses = Statuses.Distinct().ToList();

            if (HP > 0 && !IsAlive())
            {
                Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
            }

            if (HP == 0 && !IsDead())
            {
                Statuses = new List<CEnums.Status>() { CEnums.Status.dead };
            }

            // Fix XP
            if (this is PlayableCharacter pcu)
            {
                pcu.CurrentXP = Math.Max(0, pcu.CurrentXP);
            }
        }

        public bool HasStatus(CEnums.Status status)
        {
            return Statuses.Contains(status);
        }

        public bool IsAlive()
        {
            return HasStatus(CEnums.Status.alive);
        }

        public bool IsDead()
        {
            return HasStatus(CEnums.Status.dead);
        }

        public void FullyHealUnit(bool restore_hp, bool restore_mp, bool restore_ap, bool cure_statuses)
        {
            if (restore_hp)
            {
                HP = MaxHP;
            }

            if (restore_mp)
            {
                MP = MaxMP;
            }

            if (restore_ap)
            {
                AP = MaxAP;
            }

            if (cure_statuses)
            {
                Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
            }

            FixAllStats();
        }
        #endregion

        /* =========================== *
         *         CONSTRUCTOR         *
         * =========================== */
        protected Unit()
        {
            Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
        }
    }

    /* =========================== *
     *     PLAYABLE CHARACTERS     *
     * =========================== */
        public class PlayableCharacter : Unit
    {
        public CEnums.CharacterClass PClass { get; set; }
        public bool Active { get; set; }
        public int CurrentXP { get; set; }
        public int RequiredXP { get; set; }
        public int RemainingSkillpoints { get; set; }
        public string CurrentMove { get; set; }
        public Unit CurrentTarget { get; set; }
        public Ability CurrentAbility { get; set; }
        public Spell CurrentSpell { get; set; }
        public Consumable CurrentItem { get; set; }
        public string PlayerID { get; set; }

        public Dictionary<CEnums.PlayerAttribute, int> Attributes = new Dictionary<CEnums.PlayerAttribute, int>()
        {
            { CEnums.PlayerAttribute.strength, 0 },
            { CEnums.PlayerAttribute.intelligence, 0 },
            { CEnums.PlayerAttribute.dexterity, 0 },
            { CEnums.PlayerAttribute.perception, 0 },
            { CEnums.PlayerAttribute.constitution, 0 },
            { CEnums.PlayerAttribute.wisdom, 0 },
            { CEnums.PlayerAttribute.charisma, 0 }
        };

        public Dictionary<string, dynamic> PlayerAbilityFlags = new Dictionary<string, dynamic>()
        {
            {"ascend_used", false },
            {"berserk", false },
            {"rolling", false }
        };

        /* =========================== *
         *     CHARACTER GENERATION    *
         * =========================== */
        #region
        public void PlayerChooseName()
        {
            const int max_chars = 25;
            List<string> FriendNames = new List<string>()
            {
                "apollo kalar", "apollokalar", "apollo_kalar",
                "flygon jones", "flygonjones", "flygon_jones",
                "starkiller106024", "starkiller", "star killer",
                "atomic vexal", "vexal", "wave vex",
                "therichpig", "therichpig64", "spaghettipig64", "spaghettipig", "pastahog", "pastahog64",
                "theaethersplash", "the aether splash", "aethersplash", "aether splash"
            };

            Console.WriteLine("Rules for naming your character: ");
            Console.WriteLine("-No symbols, except for spaces dashes and underscores");
            Console.WriteLine($"-Name has a max length of {max_chars} characters");
            Console.WriteLine("-This is the only character you get to name - choose wisely!");
            Console.WriteLine();

            while (true)
            {
                string chosen_name = CMethods.MultiCharInput("What is your name, young adventurer? ");

                // This line removes all characters that are not alphanumeric, spaces, dashes, or underscores
                // We also remove repeated spaces like "Hello    world" => "Hello world"
                // Finally we .Trim() to remove leading or ending whitespace like "    Hello world    " => "Hello world"
                chosen_name = Regex.Replace(Regex.Replace(chosen_name, @"[^\w\s\-]*", ""), @"\s+", " ").Trim();

                if (string.IsNullOrEmpty(chosen_name))
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("What was that? I couldn't hear you, speak up!");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();
                    continue;
                }

                else if (chosen_name.Length > max_chars)
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("No hero would have a name that long, try to be a little more to the point!");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();
                    continue;
                }

                if (chosen_name.ToLower() == "y")
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("Your name's y, eh? Must be in a hurry.");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();
                }

                else if (FriendNames.Contains(chosen_name.ToLower()))
                {
                    CMethods.PrintDivider();
                    Console.WriteLine($"Ah, {chosen_name}! My dear friend, it is great to see you again!");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();

                    // If you choose of the the friend names, then it will enable calculators to spawn in the game.
                    UnitManager.MonsterList.Add(new Calculator());
                }

                else if (chosen_name.ToLower() == "frisk")
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("Frisk? Sorry, no hard mode for you in this game.");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();
                }

                while (true)
                {
                    string yes_no = CMethods.SingleCharInput($"So, your name is '{chosen_name}'? ").ToLower();

                    if (yes_no.IsYesString())
                    {
                        UnitName = chosen_name;
                        CMethods.PrintDivider();
                        return;
                    }

                    else if (yes_no.IsNoString())
                    {

                        CMethods.PrintDivider();
                        break;
                    }
                }
            }
        }

        public void PlayerChooseClass()
        {
            while (true)
            {
                Console.WriteLine($@"{UnitName}, which class would you like to train as?
     [1] Warrior: Excellent soldier, good balance of offense and defense
     [2] Mage: Master of the arcane arts
     [3] Assassin: Proficient in both stealth and murder
     [4] Ranger: Fast and evasive, good with a bow
     [5] Monk: Fighter whose fists are a worthy opponent to any blade
     [6] Paladin: Holy knight whose healing prowess is unmatched
     [7] Bard: True team-player and master musician");

                string chosen_number = CMethods.SingleCharInput("Input[#]: ");
                string class_desc;
                CEnums.CharacterClass chosen_class;

                try
                {
                    chosen_class = new Dictionary<string, CEnums.CharacterClass>()
                    {
                        {"1", CEnums.CharacterClass.warrior},
                        {"2", CEnums.CharacterClass.mage},
                        {"3", CEnums.CharacterClass.assassin},
                        {"4", CEnums.CharacterClass.ranger},
                        {"5", CEnums.CharacterClass.monk},
                        {"6", CEnums.CharacterClass.paladin},
                        {"7", CEnums.CharacterClass.bard}
                    }[chosen_number];

                    class_desc = new Dictionary<CEnums.CharacterClass, string>()
                    {
                        {
                            CEnums.CharacterClass.warrior,
@"Warriors belong to the guild 'The Knights of Principalia'. Over the centuries,
Principalia has become the premiere site for improving ones swordplay and 
mastering the art of the Warrior. 

Warriors are the jack of all trades in the combat world. By relying on a
combination of Strength and Constutition, they deal considerable physical
damage while being well-protected from physical and pierce attacks.
Their major drawback is their abysmal magical prowess, completely vulnerable
to spells while being unable to effectively use them themselves."
                        },

                        {
                            CEnums.CharacterClass.mage,
@"Mages belong to the guild 'The Sorcerers of Parceon'. Thousands of years
ago, the Father of All Magic cast his first spell, and his gift has since been
passed down through generations.

Mages do not use weapons to fight, instead they use gestures and words. Their
supreme Intelligence allows them to learn and cast every spell ever conceived,
while their Charisma grants their words more power. The Mage's abundant supply
of magical energy and access to every spell means that they are possibly the
most versitle class available. However, this comes with the drawback of being 
completely defenseless against physical and piercing attacks."
                        },

                        {
                            CEnums.CharacterClass.assassin,
@"Assassins belong to the guild 'The Valician Nightcrawlers'. Innovating on
the strategies used by the rogues of yore, Assassins combine their speed and
melee attacks with some new tricks learned from the Mages.

Assassins are a terrifying foe on the battlefield. Their Dexterity allows
them to dish out powerful, swift attacks to their foes, while their 
Intelligence grants them the use of dark magic and disabling abilities.
A skilled Assassin can completely cripple their opponents. However, their
powerful offense has almost no defense backing it up, making the Assassin a 
true glass cannon."
                        },

                        {
                            CEnums.CharacterClass.ranger,
@"Rangers belong to the guild 'The Overshire Watchmen'. A unique take on the
standard archer class, Rangers are significantly more versitle as a result of
their training with the Watchmen.

Their Perception grants them exceptional damage with their piercing attacks, 
while their Dexterity allows for extraordinary speed and evasion, rivaling 
that of the Assassin. However, the true source of the Ranger's power comes
from their ammunition. By equipping different ammo types, Rangers can empower
their attacks with higher critical chance, accuracy, or on-hit status effects.
All rangers should have many types of ammunition on them at once so they
can pick the right tool for the job."
                        },

                        {
                            CEnums.CharacterClass.monk,
@"Monks belong to the guild 'The Brotherhood of the Valenfall Abbey'.
While typical monks are pacifists who commit their lives to their religion,
The Brotherhood has adapted their patience and moderation for use in combat.

Monks take a less materialistic approach to warfare, opting to only use their
fists instead of swords or bows. Their Consitution lets them directly 
manipulate chakras and auras, both through spells and abilities. Their 
Dexterity enables them to effortlessly dodge most attacks and get in swift
blows then the time is right. These strengths come at the cost of abysmal
defenses. A wise monk would opt to focus on evasion to overcome this downside."
                        },

                        {
                            CEnums.CharacterClass.paladin,
@"Paladins belong to the guild 'The Cezuric Devout', from the town of Cezera.
For ages their holy light and unwavering commitment to defending all that is
good has protected that town, keeping it safe from evil.

Paladins are incredibly tough fighters. Their Strength gives them unmatched
resistance to attacks, shrugging off both swords and spells, while their
Wisdom gives them access to all healing and light attack spells. Paladin's
possess many abilities that help keep their party alive and ready to smite
down evil. The downside is that Paladins lack an effective means to deal 
damage. However, no true Paladin would let that discourage them from defending
the peace."
                        },

                        {
                            CEnums.CharacterClass.bard,
@"Bards belong to the guild 'The Dancing Jester Inn', from the town of
Southford. While Bards have been a staple of the tavern scene for millenia,
The Dancing Jesters were the first to show their potential on the battlefield.

Bards play an important role in supporting the team. Rather than directly
attacking opponents, they use their Charisma to boost morale with inspiring
songs and melodies. Their Perception also has helped them gather vast musical 
knowledge over their career, allowing them to have 6 abilities instead of the
usual 4. All of this comes at a cost, as Bards tend to have both offense and 
defense far below average. However, with frequent and clever use of their
songs, it's possible to overcome these weaknesses and turn your team into an
unstoppable threat."
                        }
                    }[chosen_class];
                }

                catch (KeyNotFoundException)
                {
                    CMethods.PrintDivider();
                    continue;
                }

                CMethods.PrintDivider();
                Console.WriteLine($"Information about {chosen_class.EnumToString()}s: ");
                Console.WriteLine(class_desc);
                Console.WriteLine();

                while (true)
                {
                    string yes_no = CMethods.SingleCharInput($"You wish to be a {chosen_class.EnumToString()}? ").ToLower();

                    if (yes_no.IsYesString())
                    {
                        CMethods.PrintDivider();
                        PClass = chosen_class;
                        return;
                    }

                    else if (yes_no.IsNoString())
                    {
                        CMethods.PrintDivider();
                        break;
                    }
                }
            }
        }

        public bool CheckForLevelUp()
        {
            if (CurrentXP >= RequiredXP)
            {
                MusicPlayer.PlaySong(SoundManager.levelup_music, -1);

                while (CurrentXP >= RequiredXP)
                {
                    Level++;
                    RemainingSkillpoints += 3;

                    CMethods.PrintDivider();
                    Console.WriteLine($"{UnitName} has advanced to level {Level}!");

                    // Get a list of all the spells in the game
                    List<Spell> new_spells = SpellManager.GetCasterSpellbook(this, CEnums.SpellCategory.all);

                    // Filter this list to only include the spells that the player was not previously able to use, and 
                    // that are usable by the player's class
                    new_spells = new_spells.Where(x => x.RequiredLevel == Level).ToList();

                    if (new_spells.Count > 0)
                    {
                        // Prompt the player of their new spells.
                        foreach (Spell spell in new_spells)
                        {
                            SoundManager.item_pickup.SmartPlay();
                            Console.WriteLine($"{UnitName} has learned a new spell: {spell.SpellName}!");
                        }

                        CMethods.PressAnyKeyToContinue();
                    }

                    CurrentXP -= RequiredXP;
                    RequiredXP = (int)(Math.Pow(Level * 1.75, 1.75) - Level);
                    FixAllStats();
                }

                // The player restores all their health and mana when they level up
                HP = MaxHP;
                MP = MaxMP;
                Statuses = new List<CEnums.Status>() { CEnums.Status.alive };

                CMethods.PrintDivider();
                AllocateSkillPoints();
                CalculateStats();

                // true => The player leveled up
                return true;
            }

            // false => The player did not level up
            return false;
        }

        public void CalculateStats()
        {
            // Call this function after the PCU levels up, or after the PCU is loaded at the beginning of the game
            StatMatrix base_matrix = new StatMatrix(20, 5, 10, 8, 5, 8, 5, 8, 5, 6, 3);

            MaxHP = base_matrix.MaxHP
                + UnitManager.GetClassMatrix(PClass).MaxHP
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).MaxHP);

            MaxMP = base_matrix.MaxMP
                + UnitManager.GetClassMatrix(PClass).MaxMP
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).MaxMP);

            MaxAP = base_matrix.MaxAP
                + UnitManager.GetClassMatrix(PClass).MaxAP
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).MaxAP);

            Attack = base_matrix.Attack
                + UnitManager.GetClassMatrix(PClass).Attack
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).Attack);

            Defense = base_matrix.Defense
                + UnitManager.GetClassMatrix(PClass).Defense
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).Defense);

            PAttack = base_matrix.PAttack
                + UnitManager.GetClassMatrix(PClass).PAttack
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).PAttack);

            PDefense = base_matrix.PDefense
                + UnitManager.GetClassMatrix(PClass).PDefense
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).PDefense);

            MAttack = base_matrix.MAttack
                + UnitManager.GetClassMatrix(PClass).MAttack
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).MAttack);

            MDefense = base_matrix.MDefense
                + UnitManager.GetClassMatrix(PClass).MDefense
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).MDefense);

            Speed = base_matrix.Speed
                + UnitManager.GetClassMatrix(PClass).Speed
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).Speed);

            Evasion = base_matrix.Evasion
                + UnitManager.GetClassMatrix(PClass).Evasion
                + ((Level - 1) * UnitManager.GetLevelUpMatrix(PClass).Evasion);

            foreach (KeyValuePair<CEnums.PlayerAttribute, int> kvp in Attributes)
            {
                MaxHP += UnitManager.GetAttributeMatrix(kvp.Key).MaxHP * kvp.Value;
                MaxMP += UnitManager.GetAttributeMatrix(kvp.Key).MaxMP * kvp.Value;
                Attack += UnitManager.GetAttributeMatrix(kvp.Key).Attack * kvp.Value;
                Defense += UnitManager.GetAttributeMatrix(kvp.Key).Defense * kvp.Value;
                PAttack += UnitManager.GetAttributeMatrix(kvp.Key).PAttack * kvp.Value;
                PDefense += UnitManager.GetAttributeMatrix(kvp.Key).PDefense * kvp.Value;
                MAttack += UnitManager.GetAttributeMatrix(kvp.Key).MAttack * kvp.Value;
                MDefense += UnitManager.GetAttributeMatrix(kvp.Key).MDefense * kvp.Value;
                Speed += UnitManager.GetAttributeMatrix(kvp.Key).Speed * kvp.Value;
                Evasion += UnitManager.GetAttributeMatrix(kvp.Key).Evasion * kvp.Value;
            }

            OffensiveElement = (InventoryManager.GetEquipmentItems()[PlayerID][CEnums.EquipmentType.weapon] as Weapon).Element;
            DefensiveElement = (InventoryManager.GetEquipmentItems()[PlayerID][CEnums.EquipmentType.elem_accessory] as ElementAccessory).Element;

            FixAllStats();
            SetTempStats();
        }

        private void AllocateSkillPoints()
        {
            while (RemainingSkillpoints > 0)
            {
                Console.WriteLine($"{UnitName} has {RemainingSkillpoints} skill point{(RemainingSkillpoints > 1 ? "s" : "")} left to spend.");
                Console.WriteLine(@"Choose an attribute to increase:
      [1] Strength, the attribute of Warriors and Paladins
      [2] Intelligence, The attribute of Mages and Assassins
      [3] Dexterity, the attribute of Assassins and Rangers
      [4] Perception, the attribute of Rangers and Bards
      [5] Constitution, the attribute of Monks and Warriors
      [6] Wisdom, the attribute of Paladins and Monks
      [7] Charisma, the attribute of Bards and Mages
      [8] Fate, the forgotten attribute
      [9] Difficulty, the forbidden attribute");

                while (true)
                {
                    string chosen = CMethods.SingleCharInput("Input [#]: ");

                    CEnums.PlayerAttribute attribute;
                    string message;

                    if (chosen == "1")
                    {
                        attribute = CEnums.PlayerAttribute.strength;
                        message = @"Increasing STRENGTH will provide:
    +1 Physical Attack
    +1 Physical Defense
    +1 Pierce Defense
    +Warrior Ability Power
    +Paladin Ability Power";
                    }

                    else if (chosen == "2")
                    {
                        attribute = CEnums.PlayerAttribute.intelligence;
                        message = @"Increasing INTELLIGENCE will provide:
    +1 Magical Attack
    +1 Magical Defense
    +1 MP
    +Mage Ability Power
    +Assassin Ability Power";
                    }

                    else if (chosen == "3")
                    {
                        attribute = CEnums.PlayerAttribute.dexterity;
                        message = @"Increasing DEXTERITY will provide:
    +1 Physical Attack
    +1 Speed
    +1 Evasion
    +Assassin Ability Power
    +Ranger Ability Power";
                    }


                    else if (chosen == "4")
                    {
                        attribute = CEnums.PlayerAttribute.perception;
                        message = @"Increasing PERCEPTION will provide:
    +1 Pierce Attack
    +1 Pierce Defense
    +1 Evasion
    +Ranger Ability Power
    +Bard Ability Power";
                    }

                    else if (chosen == "5")
                    {
                        attribute = CEnums.PlayerAttribute.constitution;
                        message = @"Increasing CONSTITUTION will provide:
    +1 HP
    +1 Physical Defense
    +1 Pierce Defense
    +1 Magical Defense
    +Monk Ability Power
    +Warrior Ability Power";
                    }

                    else if (chosen == "6")
                    {
                        attribute = CEnums.PlayerAttribute.wisdom;
                        message = @"Increasing WISDOM will provide:
    +1 HP restored from healing spells
    +2 MP
    +Paladin Ability Power
    +Monk Ability Power";
                    }

                    else if (chosen == "7")
                    {
                        attribute = CEnums.PlayerAttribute.charisma;
                        message = @"Increasing CHARISMA will provide:
    +0.5% better deals at shops (only the highest CHARISMA in party applies)
    +1% Spell Damage (caps at 100%)
    +Bard Ability Power
    +Mage Ability Power";
                    }

                    else if (chosen == "8")
                    {
                        attribute = CEnums.PlayerAttribute.fate;
                        message = @"Increasing FATE will provide:
    +1 to a random attribute (won't choose DIFFICULTY or FATE)
    +1 to a second random attribute (won't choose DIFFICULTY or FATE)
    +Knowledge that your destiny is predetermined and nothing matters";
                    }

                    else if (chosen == "9")
                    {
                        attribute = CEnums.PlayerAttribute.difficulty;
                        message = @"Increasing DIFFICULTY will provide:
    +0.5% Enemy Physical Attack (applies to entire party)
    +0.5% Enemy Pierce Attack (applies to entire party)
    +0.5% Enemy Magical Attack (applies to entire party)
    +More challenging experience";
                    }

                    else
                    {
                        continue;
                    }

                    CMethods.PrintDivider();

                    if (attribute == CEnums.PlayerAttribute.difficulty || attribute == CEnums.PlayerAttribute.fate)
                    {
                        Console.WriteLine($"Current {attribute.EnumToString()}: {CInfo.Difficulty}");
                    }

                    else
                    {
                        Console.WriteLine($"Current {attribute.EnumToString()}: {Attributes[attribute]}");
                    }

                    Console.WriteLine(message);
                    CMethods.PrintDivider();

                    while (true)
                    {
                        string yes_no = CMethods.SingleCharInput($"Increase {UnitName}'s {attribute.EnumToString()}? ").ToLower();

                        if (yes_no.IsYesString())
                        {
                            IncreaseAttribute(attribute);

                            if (attribute == CEnums.PlayerAttribute.difficulty)
                            {
                                CMethods.PrintDivider();
                                Console.WriteLine("The enemies of your world have grown in power!");
                                CMethods.PressAnyKeyToContinue();
                            }

                            else if (attribute != CEnums.PlayerAttribute.fate)
                            {
                                CMethods.PrintDivider();
                                Console.WriteLine($"{UnitName}'s {attribute.EnumToString()} has increased!");
                                CMethods.PressAnyKeyToContinue();
                            }

                            // The player has spent a point now, subtract one from RemainingSkillpoints
                            RemainingSkillpoints--;

                            if (RemainingSkillpoints > 0)
                            {
                                CMethods.PrintDivider();
                            }

                            break;
                        }

                        else if (yes_no.IsNoString())
                        {
                            CMethods.PrintDivider();
                            break;
                        }
                    }

                    break;
                }
            }

            Console.WriteLine($"\n{UnitName} is out of skill points.");
        }

        private void IncreaseAttribute(CEnums.PlayerAttribute attribute)
        {
            if (attribute == CEnums.PlayerAttribute.difficulty)
            {
                CInfo.Difficulty++;
            }

            else if (attribute == CEnums.PlayerAttribute.fate)
            {
                // Fate gives you 1 point in two randomly chosen attributes. Can choose the same attribute twice.
                // Cannot choose Fate or Difficulty as the attribute.
                List<CEnums.PlayerAttribute> skill_list = new List<CEnums.PlayerAttribute>()
                {
                    CEnums.PlayerAttribute.strength,
                    CEnums.PlayerAttribute.intelligence,
                    CEnums.PlayerAttribute.dexterity,
                    CEnums.PlayerAttribute.perception,
                    CEnums.PlayerAttribute.constitution,
                    CEnums.PlayerAttribute.wisdom,
                    CEnums.PlayerAttribute.charisma,
                };

                CEnums.PlayerAttribute rand_attr1 = CMethods.GetRandomFromIterable(skill_list);
                CEnums.PlayerAttribute rand_attr2 = CMethods.GetRandomFromIterable(skill_list);

                Attributes[rand_attr1]++;
                Attributes[rand_attr2]++;

                CMethods.PrintDivider();
                Console.WriteLine($"{UnitName} gained one point in {rand_attr1.EnumToString()} from FATE!");
                Console.WriteLine($"{UnitName} gained one point in {rand_attr2.EnumToString()} from FATE!");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                Attributes[attribute]++;
            }
        }
        #endregion

        /* =========================== *
         *        BATTLE METHODS       *
         * =========================== */
        #region
        public void ChooseBattleAction()
        {
            PrintBattleOptions();

            while (true)
            {
                string c_move = CMethods.SingleCharInput("Input [#]: ");

                try
                {
                    CurrentMove = c_move[0].ToString();
                }

                catch (IndexOutOfRangeException)
                {
                    continue;
                }

                // Attack
                if (CurrentMove == "1")
                {
                    TargetMapping t_map = new TargetMapping(false, false, true, false);
                    if (!PlayerChooseTarget($"What should {UnitName} attack?", t_map))
                    {
                        PrintBattleOptions();
                        continue;
                    }

                    return;
                }

                // Magic
                else if (CurrentMove == "2")
                {
                    CMethods.PrintDivider();

                    // Silence is a status ailment that prevents using spells
                    if (HasStatus(CEnums.Status.silence))
                    {
                        SoundManager.debuff.SmartPlay();
                        Console.WriteLine($"{UnitName} can't use spells when silenced!");
                        CMethods.PressAnyKeyToContinue();
                        PrintBattleOptions();

                        continue;
                    }

                    if (!SpellManager.PickSpellCategory(this))
                    {
                        PrintBattleOptions();
                        continue;
                    }

                    return;
                }

                // Ability
                else if (CurrentMove == "3")
                {
                    if (!AbilityManager.ChooseAbility(this))
                    {
                        PrintBattleOptions();
                        continue;
                    }

                    return;
                }

                // Use Items
                else if (CurrentMove == "4")
                {
                    CMethods.PrintDivider();

                    if (InventoryManager.GetInventoryItems()[CEnums.InvCategory.consumables].Count == 0)
                    {
                        SoundManager.debuff.SmartPlay();
                        Console.WriteLine("Your party has no consumables!");

                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();
                        PrintBattleOptions();

                        continue;
                    }

                    if (HasStatus(CEnums.Status.muted))
                    {
                        SoundManager.debuff.SmartPlay();
                        Console.WriteLine($"{UnitName} is muted and can't use items.");

                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();
                        PrintBattleOptions();

                        continue;
                    }

                    if (!BattleManager.BattlePickItem(this))
                    {
                        PrintBattleOptions();
                        continue;
                    }

                    return;
                }

                // Run
                else if (CurrentMove == "5")
                {
                    return;
                }
            }
        }

        public bool ExecuteBattleAction()
        {
            if (BeforeBattleAction())
            {
                return true;
            }

            // Basic Attack
            if (CurrentMove == "1")
            {
                AttackAction(InventoryManager.GetEquipmentItems()[PlayerID][CEnums.EquipmentType.weapon] as Weapon);
            }

            // Use Magic
            else if (CurrentMove == "2")
            {
                CurrentSpell.UseMagic(this);
            }

            // Use Ability
            else if (CurrentMove == "3")
            {
                CurrentAbility.UseAbility(this);
            }

            // Use Item
            else if (CurrentMove == "4")
            {
                CurrentItem.UseItem(this);
            }

            // Run away
            else if (CurrentMove == "5")
            {
                if (BattleManager.TryToRunAway(this))
                {
                    SoundManager.PlayCellMusic();
                    return false;
                }
            }

            // This only happens if the PCU was resurrected on the turn, and therefore didn't get
            // a chance to choose an action
            else
            {
                Console.WriteLine($"{UnitName} wasn't prepared!");
                SoundManager.debuff.SmartPlay();
            }

            CurrentMove = null;
            return true;
        }

        public void AttackAction(Weapon player_weapon)
        {
            if (SoundManager.bard_sounds.Keys.Contains(player_weapon.ItemID))
            {
                SoundManager.bard_sounds[player_weapon.ItemID].SmartPlay();
                Console.WriteLine($"{UnitName} starts playing their {player_weapon.ItemName} at the {CurrentTarget.UnitName}...");
            }

            else if (player_weapon.DamageType == CEnums.DamageType.physical)
            {
                SoundManager.sword_slash.SmartPlay();
                Console.WriteLine($"{UnitName} fiercely attacks the {CurrentTarget.UnitName} using their {player_weapon.ItemName}...");
            }

            else
            {
                SoundManager.aim_weapon.SmartPlay();
                Console.WriteLine($"{UnitName} aims carefully at the {CurrentTarget.UnitName} using their {player_weapon.ItemName}...");
            }

            CMethods.SmartSleep(750);
            int attack_damage = BattleCalculator.CalculateAttackDamage(this, CurrentTarget, player_weapon.DamageType);

            if (BattleCalculator.DoesAttackHit(CurrentTarget))
            {
                Console.WriteLine($"{UnitName}'s attack connects with the {CurrentTarget.UnitName}, dealing {attack_damage} damage!");
                SoundManager.enemy_hit.SmartPlay();
                CurrentTarget.HP -= attack_damage;
            }

            else
            {
                Console.WriteLine($"The {CurrentTarget.UnitName} narrowly avoids {UnitName}'s attack!");
                SoundManager.attack_miss.SmartPlay();
            }
        }

        public bool BeforeBattleAction()
        {
            Random rng = new Random();
            SoundManager.item_pickup.Stop();

            // If the player's target is an enemy, and the target died before the player's turn began,
            // then the attack automatically redirects to a random living enemy.
            if (CurrentTarget is Monster && !CurrentTarget.IsAlive())
            {
                CurrentTarget = CMethods.GetRandomFromIterable(BattleManager.GetMonsterList().Where(x => x.IsAlive()));
            }

            Console.WriteLine($"-{UnitName}'s Turn-");

            // PCUs regenerate 1 Action Point per turn, unless they used an ability that turn
            if (CurrentMove != "3")
            {
                AP++;
            }

            if (HasStatus(CEnums.Status.poison))
            {
                SoundManager.poison_damage.SmartPlay();

                int poison_damage = HP / 5;
                HP -= poison_damage;

                Console.WriteLine($"{UnitName} took {poison_damage} damage from poison!");

                if (HP <= 0)
                {
                    return true;
                }
            }

            foreach (CEnums.Status status in Statuses)
            {
                // There is a 12.5% chance each turn per status ailment to be relived of that status ailment
                // Only one status can be cleared per turn
                if (status != CEnums.Status.alive && rng.Next(0, 8) == 0)
                {
                    SoundManager.buff_spell.SmartPlay();
                    Statuses.Remove(status);
                    Console.WriteLine($"{UnitName} is no longer {status.EnumToString()}!");

                    break;
                }
            }

            return false;
        }

        public bool PlayerChooseTarget(string action_desc, TargetMapping t_map)
        {
            // Create the list of valid PCU targets (might go unused if t_map.TargetAllies is false)
            List<PlayableCharacter> pcu_list;
            if (t_map.TargetDead)
            {
                pcu_list = UnitManager.GetActivePCUs().Where(x => x != this).ToList();
            }

            else
            {
                pcu_list = UnitManager.GetAliveActivePCUs().Where(x => x != this).ToList();
            }

            // The full list of valid targets, including both monsters and allies if applicable
            List<Unit> valid_targets = new List<Unit>();

            if (t_map.TargetSelf)
            {
                valid_targets.Add(this);
            }

            if (t_map.TargetAllies)
            {
                pcu_list.ForEach(x => valid_targets.Add(x));
            }

            if (t_map.TargetEnemies && BattleManager.GetMonsterList() != null)
            {                
                foreach (Monster monster in BattleManager.GetMonsterList())
                {
                    if (monster.IsAlive() || t_map.TargetDead)
                    {
                        monster.FixAllStats();
                        valid_targets.Add(monster);
                    }
                }
            }

            // If the valid_targets list has either 1 or 0 elements, then do something special
            if (valid_targets.Count == 0)
            {
                CMethods.PrintDivider();
                Console.WriteLine("There are no valid targets for that move.");
                CMethods.PressAnyKeyToContinue();
                return false;
            }

            else if (valid_targets.Count == 1)
            {
                CurrentTarget = valid_targets[0];
                return true;
            }

            CMethods.PrintDivider();
            Console.WriteLine(action_desc);

            // Print out the target list
            int counter = 0;
            foreach (Unit unit in valid_targets)
            {
                Console.WriteLine($"      [{counter + 1}] {unit.UnitName}");
                counter++;
            }

            while (true)
            {
                string chosen = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", valid_targets.Count);

                try
                {
                    CurrentTarget = valid_targets[int.Parse(chosen) - 1];
                }

                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    if (chosen.IsExitString())
                    {
                        CMethods.PrintDivider();
                        return false;
                    }

                    continue;
                }

                return true;
            }
        }

        private void PrintBattleOptions()
        {
            Console.WriteLine($"Pick {UnitName}'s Move:");
            Console.WriteLine("      [1] Standard Attack");
            Console.WriteLine("      [2] Use Magic");
            Console.WriteLine("      [3] Use Abilities");
            Console.WriteLine("      [4] Use Items");
            Console.WriteLine("      [5] Run");
        }
        #endregion

        /* =========================== *
         *        OTHER METHDOS        *
         * =========================== */
        #region
        public void PlayerViewStats()
        {
            FixAllStats();
            Console.WriteLine($@"-{UnitName}'s Stats-
Level {Level} {PClass.EnumToString()}
Statuses: {string.Join(", ", Statuses.Select(x => x.EnumToString()))}
XP: {CurrentXP}/{RequiredXP} / GP: {CInfo.GP}

HP: {HP}/{MaxHP} / MP: {MP}/{MaxMP} / AP: {AP}/{MaxAP}
Physical: {Attack} Attack / {Defense} Defense
Magical: {MAttack} Attack / {MDefense} Defense
Piercing: {PAttack} Attack / {PDefense} Defense
Speed: {Speed} / Evasion: {Evasion}
Elements: Attacks are {OffensiveElement.EnumToString()} / Defense is {DefensiveElement.EnumToString()}
Weak to { DefensiveElement.GetElementalMatchup().Item1.EnumToString() } / Resistant to { DefensiveElement.GetElementalMatchup().Item2.EnumToString()}

Strength: {Attributes[CEnums.PlayerAttribute.strength]}
Intelligence: {Attributes[CEnums.PlayerAttribute.intelligence]} 
Dexterity: {Attributes[CEnums.PlayerAttribute.dexterity]}
Perception: {Attributes[CEnums.PlayerAttribute.perception]}
Constitution: {Attributes[CEnums.PlayerAttribute.constitution]}
Wisdom: {Attributes[CEnums.PlayerAttribute.wisdom]}
Charisma: {Attributes[CEnums.PlayerAttribute.charisma]}
Difficulty: {CInfo.Difficulty}");

            CMethods.PressAnyKeyToContinue();
        }
        #endregion

        /* =========================== *
         *          CONSTRUCTOR        *
         * =========================== */
        public PlayableCharacter(string name, CEnums.CharacterClass p_class, string player_id, bool active)
        {
            UnitName = name;
            AP = MaxAP = 10;
            Level = 1;
            CurrentXP = 0;
            RequiredXP = 3;
            PClass = p_class;
            PlayerID = player_id;
            Active = active;
        }
    }

    /* =========================== *
     *           MONSTERS          *
     * =========================== */
    public abstract class Monster : Unit
    {

        public string AttackMessage { get; set; }
        public Dictionary<string, double> ClassMultiplier { get; set; }
        public Dictionary<string, double> SpeciesMultiplier { get; set; }
        public CEnums.MonsterGroup MonsterGroup { get; set; }
        public CEnums.DamageType SAttackType { get; set; }
        public MediaWrapper SAttackSound { get; set; }

        // A list of droppable items
        // Tuple.Item1 is the Item ID, Tuple.Item2 is droprate out of 100 
        // Ex: 75 => 75% chance of being added to the item pool
        // If multiple items get added to the item pool, a random one is chosen
        public List<Tuple<string, int>> DropList { get; set; }

        public string DroppedItem { get; set; }
        public int DroppedGold { get; set; }
        public int DroppedXP { get; set; }

        public Dictionary<string, dynamic> MonsterAbilityFlags = new Dictionary<string, dynamic>()
        {
            {"poison_pow", 0},
            {"poison_dex", 0},
            {"knockout_turns", 0},
            {"judgment_day", 0},
            {"taunted_turn", 0},
            {"taunted_user", null},
            {"drained", false},
            {"disarmed", false}
        };

        public bool IsDefending { get; set; }
        public Unit CurrentTarget { get; set; }

        /* =========================== *
         *        MONSTER METHODS      *
         * =========================== */
        #region
        public bool MonsterSetDroppedItem()
        {
            Random rng = new Random();

            List<string> item_pool = new List<string>();
            foreach (Tuple<string, int> item in DropList)
            {
                if (rng.Next(0, 101) < item.Item2)
                {
                    item_pool.Add(item.Item1);
                }
            }

            if (item_pool.Count > 0)
            {
                DroppedItem = CMethods.GetRandomFromIterable(item_pool);
                return true;
            }

            return false;
        }

        public void MonsterLevelUp()
        {
            Random rng = new Random();
            int minlvl = TileManager.FindCellWithTileID(CInfo.CurrentTile).MinMonsterLevel;
            int maxlvl = TileManager.FindCellWithTileID(CInfo.CurrentTile).MaxMonsterLevel;

            // We add 1 to maxlvl because rng.Next's upper bound is not inclusive
            Level = rng.Next(minlvl, maxlvl + 1);

            for (int i = 0; i < Level; i++)
            {
                HP += 3;
                MP += 2;
                Attack += 3;
                Defense += 2;
                PAttack += 3;
                PDefense += 2;
                MAttack += 3;
                MDefense += 2;
                Speed += 2;
                Evasion += 2;
            }

            MaxHP = HP;
            MaxMP = MP;
        }

        public void MonsterApplyMultipliers()
        {
            MaxHP = HP = (int)(HP * ClassMultiplier["hp"] * SpeciesMultiplier["hp"]);
            MaxMP = MP = (int)(MP * ClassMultiplier["mp"] * SpeciesMultiplier["mp"]);
            Attack = (int)(Attack * ClassMultiplier["attack"] * SpeciesMultiplier["attack"]);
            Defense = (int)(Defense * ClassMultiplier["defense"] * SpeciesMultiplier["defense"]);
            PAttack = (int)(PAttack * ClassMultiplier["p_attack"] * SpeciesMultiplier["p_attack"]);
            PDefense = (int)(PDefense * ClassMultiplier["p_defense"] * SpeciesMultiplier["p_defense"]);
            MAttack = (int)(MAttack * ClassMultiplier["m_attack"] * SpeciesMultiplier["m_attack"]);
            MDefense = (int)(MDefense * ClassMultiplier["m_defense"] * SpeciesMultiplier["m_defense"]);
            Speed = (int)(Speed * ClassMultiplier["speed"] * SpeciesMultiplier["speed"]);
            Evasion = (int)(Evasion * ClassMultiplier["evasion"] * SpeciesMultiplier["evasion"]);

            Attack += (int)(Attack * 0.005 * CInfo.Difficulty);
            MAttack += (int)(MAttack * 0.005 * CInfo.Difficulty);
            PAttack += (int)(PAttack * 0.005 * CInfo.Difficulty);
        }

        public void MonsterExecuteMove()
        {
            // Base Turn
            SoundManager.item_pickup.Stop();
            MonsterGetTarget();

            Console.WriteLine($"-{UnitName}'s Turn-");

            if (!(MonsterAbilityFlags["knockout_turns"] > 0))
            {
                MonsterBattleAI();
            }

            else
            {
                Console.WriteLine($"The {UnitName} is asleep!");
            }

            ManageAbilityFlags();
        }

        public void MonsterGetTarget()
        {
            if (MonsterAbilityFlags["taunted_turn"] == BattleManager.GetTurnCounter())
            {
                CurrentTarget = MonsterAbilityFlags["taunted_user"];
            }

            else
            {
                CurrentTarget = CMethods.GetRandomFromIterable(UnitManager.GetAliveActivePCUs());
            }
        }

        public void ManageAbilityFlags()
        {
            Random rng = new Random();

            // Sleep prevents the unit from acting
            if (MonsterAbilityFlags["knockout_turns"] > 0)
            {
                // Number of turns remaining for knockout gas goes down by 1 each turn
                MonsterAbilityFlags["knockout_turns"]--;

                // If decrementing knockout_turns caused it to equal 0, then wake up
                if (MonsterAbilityFlags["knockout_turns"] == 0)
                {
                    CMethods.SmartSleep(500);
                    SoundManager.buff_spell.SmartPlay();
                    Statuses.Remove(CEnums.Status.sleep);
                    Console.WriteLine($"The {UnitName} woke up!");
                }

                else if (rng.Next(0, 100) < 10)
                {
                    CMethods.SmartSleep(500);
                    SoundManager.buff_spell.SmartPlay();
                    Statuses.Remove(CEnums.Status.sleep);
                    Console.WriteLine($"The {UnitName} woke up early!");
                }
            }

            // Poison deals damage per turn
            if (HasStatus(CEnums.Status.poison))
            {
                int poison_damage = (MonsterAbilityFlags["poison_pow"] * MaxHP) + MonsterAbilityFlags["poison_dex"];
                HP -= poison_damage;
                SoundManager.poison_damage.SmartPlay();
                Console.WriteLine($"The {UnitName} took {poison_damage} from poison!");
            }

            // Judgment day instantly kills the unit if the wait timer expires
            if (MonsterAbilityFlags["judgment_day"] == BattleManager.GetTurnCounter())
            {
                CMethods.SmartSleep(500);
                Console.WriteLine($"{UnitName}'s judgment day has arrived. The light devours it...");
                HP = 0;
            }
        }

        public abstract void UponDefeating();

        public abstract void MonsterBattleAI();
        #endregion

        /* =========================== *
         *          CONSTRUCTOR        *
         * =========================== */
        protected Monster()
        {
            HP = 8;
            MP = 5;
            Attack = 5;
            Defense = 3;
            PAttack = 5;
            PDefense = 3;
            MAttack = 5;
            MDefense = 3;
            Speed = 4;
            Evasion = 2;

            IsDefending = false;
            MaxHP = MP;
            MaxMP = MP;
        }
    }

    internal static class MonsterDifferentiation
    {
        /* =========================== *
         *           ENEMY AI          *
         * =========================== */
        #region
        public static void GenericMeleeAI(Monster me)
        {

            Random rng = new Random();

            ExitDefensiveStance(me);

            // Melee monsters have a 1 in 6 (16.667%) chance to defend
            if (rng.Next(0, 5) == 0 && !me.IsDefending && (me.MonsterAbilityFlags["taunted_turn"] != BattleManager.GetTurnCounter()))
            {
                EnterDefensiveStance(me);
                return;
            }

            DoStandardAttack(me);
        }

        public static void GenericRangedAI(Monster me)
        {
            DoStandardAttack(me);
        }

        public static void GenericMagicAI(Monster me)
        {
            Random rng = new Random();
            int status_mp_cost = me.TempStats.MaxMP / 8;
            int heal_mp_cost = me.TempStats.MaxMP / 5;
            int attack_mp_cost = me.TempStats.MaxMP / 7;

            // If the monster is neither taunted nor silenced, it will use a spell
            if ((me.MonsterAbilityFlags["taunted_turn"] != BattleManager.GetTurnCounter()) || me.HasStatus(CEnums.Status.silence))
            {
                if (rng.Next(0, 6) == 0 && me.MP >= status_mp_cost)
                {
                    GiveStatusAilment(me);
                    me.MP -= status_mp_cost;

                    return;
                }

                // Magic heal
                else if (me.HP <= me.TempStats.MaxHP / 5 && me.MP >= heal_mp_cost)
                {
                    UseHealingSpell(me);
                    me.MP -= heal_mp_cost;

                    return;
                }

                // Magical Attack
                else if (me.MP >= attack_mp_cost)
                {
                    BasicAttackSpell(me);
                    me.MP -= attack_mp_cost;

                    return;
                }
            }

            // Non-magical Attack (Pierce Damage). Only happens if taunted, silenced, or if out of mana.
            DoStandardAttack(me);
        }
        #endregion

        /* =========================== *
         *        AI COMPONENTS        *
         * =========================== */
        #region
        public static void DoStandardAttack(Monster me)
        {
            me.SAttackSound.SmartPlay();
            Console.WriteLine($"The {me.UnitName} {me.AttackMessage} {me.CurrentTarget.UnitName}...");
            CMethods.SmartSleep(750);

            int attack_damage = BattleCalculator.CalculateAttackDamage(me, me.CurrentTarget, me.SAttackType);

            if (BattleCalculator.DoesAttackHit(me.CurrentTarget))
            {
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine($"The {me.UnitName}'s attack deals {attack_damage} damage to {me.CurrentTarget.UnitName}!");
                me.CurrentTarget.HP -= attack_damage;
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {me.UnitName}'s attack narrowly misses {me.CurrentTarget.UnitName}!");
            }
        }

        public static void EnterDefensiveStance(Monster me)
        {
            me.IsDefending = true;
            Console.WriteLine($"The {me.UnitName} is preparing itself for enemy attacks...");
            CMethods.SmartSleep(750);

            me.TempStats.Defense *= 2;
            me.TempStats.PDefense *= 2;
            me.TempStats.MDefense *= 2;

            Console.WriteLine($"The {me.UnitName}'s defense stats increased by 2x for one turn!");
            SoundManager.buff_spell.SmartPlay();
        }

        public static bool ExitDefensiveStance(Monster me)
        {
            if (me.IsDefending)
            {
                Console.WriteLine($"The {me.UnitName} defense stats to normal.");

                me.IsDefending = false;
                me.TempStats.Defense /= 2;
                me.TempStats.PDefense /= 2;
                me.TempStats.MDefense /= 2;

                return true;
            }

            return false;
        }

        public static void BasicAttackSpell(Monster me)
        {
            SoundManager.magic_attack.SmartPlay();

            Console.WriteLine($"The {me.UnitName} casts a basic {me.OffensiveElement} spell on {me.CurrentTarget.UnitName}...");
            CMethods.SmartSleep(750);

            // UnitManager.CalculateDamage() for magical damage requires an AttackSpell as an argument, so we have
            // to create a dummy spell.
            AttackSpell m_spell = new AttackSpell("", "", 0, 0, new List<CEnums.CharacterClass>(), me.OffensiveElement);

            int spell_damage = BattleCalculator.CalculateSpellDamage(me, me.CurrentTarget, m_spell);

            if (BattleCalculator.DoesAttackHit(me.CurrentTarget))
            {
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine($"The {me.UnitName}'s spell deals {spell_damage} damage to {me.CurrentTarget.UnitName}!");

                me.CurrentTarget.HP -= spell_damage;
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {me.UnitName}'s spell narrowly misses {me.CurrentTarget.UnitName}!");
            }
        }

        public static void GiveStatusAilment(Monster me)
        {
            Random rng = new Random();
            List<CEnums.Status> status_list = new List<CEnums.Status>()
            {
                CEnums.Status.blindness,
                CEnums.Status.muted,
                CEnums.Status.paralyzation,
                CEnums.Status.silence,
                CEnums.Status.weakness,
                CEnums.Status.poison
            };

            CEnums.Status chosen_status = CMethods.GetRandomFromIterable(status_list);

            Console.WriteLine($"The {me.UnitName} casts a spell to make {me.CurrentTarget.UnitName} {chosen_status.EnumToString()}...");
            SoundManager.ability_cast.SmartPlay();
            CMethods.SmartSleep(750);

            if (rng.Next(0, 2) == 0)
            {
                if (me.CurrentTarget.HasStatus(chosen_status))
                {
                    SoundManager.debuff.SmartPlay();
                    Console.WriteLine($"...But {me.CurrentTarget.UnitName} is already {chosen_status.EnumToString()}!");
                }

                else
                {
                    me.CurrentTarget.Statuses.Add(chosen_status);
                    SoundManager.buff_spell.SmartPlay();
                    Console.WriteLine($"{me.CurrentTarget.UnitName} is now {chosen_status.EnumToString()}!");
                }
            }

            else
            {
                SoundManager.debuff.SmartPlay();
                Console.WriteLine($"...But {me.UnitName}'s attempt failed!");
            }
        }

        public static void UseHealingSpell(Monster me)
        {
            Console.WriteLine($"The {me.UnitName} is casting a healing spell on itself...");
            CMethods.SmartSleep(750);

            int total_heal = Math.Max(me.HP / 5, 5);
            me.HP += total_heal;

            Console.WriteLine($"The {me.UnitName} heals itself for {total_heal} HP!");
            SoundManager.magic_healing.SmartPlay();
        }
        #endregion

        /* =========================== *
         *       CLASS MULTIPLIERS     *
         * =========================== */
        #region
        public static Dictionary<string, double> MeleeMultiplier = new Dictionary<string, double>()
        {
            { "hp", 1.2 },         // HP
            { "mp", 1 },           // MP
            { "attack", 1.5 },     // Physical Attack
            { "defense", 1.5 },    // Physical Defense
            { "p_attack", 0.5 },   // Pierce Attack
            { "p_defense", 1.5 },  // Pierce Defense
            { "m_attack", 0.5 },   // Magical Attack
            { "m_defense", 0.5 },  // Magical Defense
            { "speed", 0.65 },     // Speed
            { "evasion", 1 }       // Evasion
        };

        public static Dictionary<string, double> RangedMultiplier = new Dictionary<string, double>()
        {
            { "hp", 0.9 },        // HP
            { "mp", 1 },          // MP
            { "attack", 0.8 },    // Physical Attack
            { "defense", 0.8 },   // Physical Defense
            { "p_attack", 1.5 },  // Pierce Attack
            { "p_defense", 1.2 }, // Pierce Defense
            { "m_attack", 0.8 },  // Magical Attack
            { "m_defense", 1 },   // Magical Defense
            { "speed", 1.5 },     // Speed
            { "evasion", 1.5 }    // Evasion
        };

        public static Dictionary<string, double> MagicMultiplier = new Dictionary<string, double>()
        {
            { "hp", 1 },            // HP
            { "mp", 1.5 },          // MP
            { "attack", 0.5 },      // Physical Attack
            { "defense", 0.65 },    // Physical Defense
            { "p_attack", 0.5 },    // Pierce Attack
            { "p_defense", 0.65 },  // Pierce Defense
            { "m_attack", 1.5 },    // Magical Attack
            { "m_defense", 1.5 },   // Magical Defense
            { "speed", 1 },         // Speed
            { "evasion", 1 }        // Evasion
        };
        #endregion
    }

    // Melee Monsters
    #region
    internal sealed class GiantCrab : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public GiantCrab()
        {
            UnitName = "Giant Crab";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "snaps its massive claws at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("crab_claw", 25), 
                new Tuple<string, int>("shell_fragment", 25) 
            };
        }
    }

    internal sealed class BogSlime : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public BogSlime()
        {
            UnitName = "Bog Slime";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "jiggles menacingly at";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("slime_vial", 25), 
                new Tuple<string, int>("water_vial", 25) 
            };
        }
    }

    internal sealed class Mummy : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Mummy()
        {
            UnitName = "Mummy";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "meanders over and grabs";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("burnt_ash", 25), 
                new Tuple<string, int>("ripped_cloth", 25) 
            };
        }
    }

    internal sealed class SandGolem : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public SandGolem()
        {
            UnitName = "Sand Golem";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "begins to pile sand on";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("golem_rock", 25), 
                new Tuple<string, int>("broken_crystal", 25) 
            };
        }
    }

    internal sealed class MossOgre : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public MossOgre()
        {
            UnitName = "Moss Ogre";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "swings a tree trunk like a club at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("bone_bag", 25), 
                new Tuple<string, int>("monster_skull", 25) 
            };
        }
    }

    internal sealed class Troll : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Troll()
        {
            UnitName = "Troll";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "swings its mighty battleaxe at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_skull", 25), 
                new Tuple<string, int>("eye_balls", 25) 
            };
        }
    }

    internal sealed class Griffin : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Griffin()
        {
            UnitName = "Griffin";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "swipes with its ferocious claws at";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("animal_fur", 25), 
                new Tuple<string, int>("wing_piece", 25) 
            };
        }
    }

    internal sealed class GirthWorm : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public GirthWorm()
        {
            UnitName = "Girth Worm";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "burrows into the ground towards";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_fang", 25), 
                new Tuple<string, int>("slime_vial", 25) 
            };
        }
    }

    internal sealed class Zombie : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Zombie()
        {
            UnitName = "Giant Crab";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "charges and tries to bite";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_skull", 25), 
                new Tuple<string, int>("blood_vial", 25) 
            };
        }
    }

    internal sealed class SnowWolf : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public SnowWolf()
        {
            UnitName = "Snow Wolf";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "claws and bites at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("animal_fur", 25), 
                new Tuple<string, int>("monster_fang", 25) 
            };
        }
    }

    internal sealed class LesserYeti : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public LesserYeti()
        {
            UnitName = "Lesser Yeti";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "begins to maul";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("animal_fur", 25), 
                new Tuple<string, int>("monster_fang", 25) 
            };
        }
    }

    internal sealed class SludgeRat : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public SludgeRat()
        {
            UnitName = "Sludge Rat";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "ferociously chomps at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_skull", 25), 
                new Tuple<string, int>("rodent_tail", 25) 
            };
        }
    }

    internal sealed class SeaSerpent : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public SeaSerpent()
        {
            UnitName = "Sea Serpent";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "charges head-first into";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("serpent_scale", 25), 
                new Tuple<string, int>("serpent_tongue", 25) 
            };
        }
    }

    internal sealed class Beetle : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Beetle()
        {
            UnitName = "Beetle";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "charges horn-first into";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("beetle_shell", 25), 
                new Tuple<string, int>("antennae", 25) 
            };
        }
    }

    internal sealed class Harpy : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Harpy()
        {
            UnitName = "Harpy";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "dives claws-first towards";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("wing_piece", 25), 
                new Tuple<string, int>("feathers", 25) 
            };
        }
    }

    internal sealed class FallenKnight : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public FallenKnight()
        {
            UnitName = "Fallen Knight";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "thrusts its heavenly spear towards";
            MonsterGroup = CEnums.MonsterGroup.dungeon;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("chain_link", 25), 
                new Tuple<string, int>("blood_vial", 25) 
            };
        }
    }

    internal sealed class DevoutProtector : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public DevoutProtector()
        {
            UnitName = "Devout Protector";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.light;
            AttackMessage = "swings its holy hammer towards";
            MonsterGroup = CEnums.MonsterGroup.dungeon;
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>()
            {
                new Tuple<string, int>("angelic_essence", 25),
                new Tuple<string, int>("runestone", 25)
            };
        }
    }

    internal sealed class Calculator : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }

        public Calculator()
        {
            UnitName = "Calculator";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "casts its mathemagical spell on";
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>()
            {
                new Tuple<string, int>("calculus_homework", 25),
                new Tuple<string, int>("graph_paper", 25),
                new Tuple<string, int>("protractor", 25),
                new Tuple<string, int>("ruler", 25),
                new Tuple<string, int>("textbook", 25)
            };
        }
    }
    #endregion

    // Ranged Monsters
    #region
    internal sealed class FireAnt : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public FireAnt()
        {
            UnitName = "Fire Ant";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.fire;
            AttackMessage = "spits a firey glob of acid at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("antennae", 25), 
                new Tuple<string, int>("burnt_ash", 25) 
            };
        }
    }

    internal sealed class NagaBowwoman : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public NagaBowwoman()
        {
            UnitName = "Naga Bow-woman";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "fires a volley of arrows at";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("serpent_scale", 25), 
                new Tuple<string, int>("serpent_tongue", 25) 
            };
        }
    }

    internal sealed class IceSoldier : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public IceSoldier()
        {
            UnitName = "Ice Soldier";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "fires a single hyper-cooled arrow at";
            MonsterGroup = CEnums.MonsterGroup.dungeon;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("chain_link", 25), 
                new Tuple<string, int>("blood_vial", 25) 
            };
        }
    }

    internal sealed class FrostBat : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public FrostBat()
        {
            UnitName = "Frost Bat";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "spits a frozen glob of acid at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_fang", 25), 
                new Tuple<string, int>("wing_piece", 25) 
            };
        }
    }

    internal sealed class SparkBat : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public SparkBat()
        {
            UnitName = "Spark Bat";
            OffensiveElement = CEnums.Element.electric;
            DefensiveElement = CEnums.Element.electric;
            AttackMessage = "spits an electrified glob of acid at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("monster_fang", 25), 
                new Tuple<string, int>("wing_piece", 25) 
            };
        }
    }

    internal sealed class SkeletonBoneslinger : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public SkeletonBoneslinger()
        {
            UnitName = "Skeleton Boneslinger";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "grabs a nearby bone and slings it at";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("bone_bag", 25), 
                new Tuple<string, int>("demonic_essence", 25) 
            };
        }
    }

    internal sealed class UndeadCrossbowman : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public UndeadCrossbowman()
        {
            UnitName = "Undead Crossbowman";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "fires a bone-tipped crossbow bolt at";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("chain_link", 25), 
                new Tuple<string, int>("bone_bag", 25) 
            };
        }
    }

    internal sealed class RockGiant : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public RockGiant()
        {
            UnitName = "Rock Giant";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "hurls a giant boulder at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("golem_rock", 25), 
                new Tuple<string, int>("broken_crystal", 25) 
            };
        }
    }

    internal sealed class GoblinArcher : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public GoblinArcher()
        {
            UnitName = "Goblin Archer";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "fires an arrow at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("ripped_cloth", 25), 
                new Tuple<string, int>("eye_balls", 25) 
            };
        }
    }

    internal sealed class GiantLandSquid : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public GiantLandSquid()
        {
            UnitName = "Giant Land-Squid";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "shoots a black, inky substance at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("ink_sack", 25), 
                new Tuple<string, int>("slime_vial", 25) 
            };
        }
    }

    internal sealed class VineLizard : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public VineLizard()
        {
            UnitName = "Vine Lizard";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "spits an acidic string of vines at";
            MonsterGroup = CEnums.MonsterGroup.animal;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("serpent_scale", 25), 
                new Tuple<string, int>("living_bark", 25) 
            };
        }
    }

    internal sealed class TenguRanger : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericRangedAI(this);
        }

        public TenguRanger()
        {
            UnitName = "Tengu Ranger";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "catapults a stone javelin towards";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.RangedMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("wing_piece", 25),
                new Tuple<string, int>("feathers", 25) 
            };
        }
    }
    #endregion

    // Magic Monsters
    #region
    internal sealed class Oread : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Oread()
        {
            UnitName = "Oread";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("fairy_dust", 25), 
                new Tuple<string, int>("eye_balls", 25) 
            };
        }
    }

    internal sealed class Willothewisp : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Willothewisp()
        {
            UnitName = "Will-o'-the-wisp";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.fire;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("fairy_dust", 25), 
                new Tuple<string, int>("burnt_ash", 25) 
            };
        }
    }

    internal sealed class Naiad : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Naiad()
        {
            UnitName = "Naiad";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("fairy_dust", 25), 
                new Tuple<string, int>("water_vial", 25) 
            };
        }
    }

    internal sealed class Necromancer : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Necromancer()
        {
            UnitName = "Necromancer";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.dungeon;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("ripped_cloth", 25), 
                new Tuple<string, int>("demonic_essence", 25) 
            };
        }
    }

    internal sealed class CorruptThaumaturge : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public CorruptThaumaturge()
        {
            UnitName = "Corrupt Thaumaturge";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.dungeon;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            {
                new Tuple<string, int>("ripped_cloth", 25), 
                new Tuple<string, int>("runestone", 25) 
            };
        }
    }

    internal sealed class Imp : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Imp()
        {
            UnitName = "Imp";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("wing_piece", 25), 
                new Tuple<string, int>("fairy_dust", 25) 
            };
        }
    }

    internal sealed class Spriggan : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Spriggan()
        {
            UnitName = "Spriggan";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.humanoid;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("fairy_dust", 25),
                new Tuple<string, int>("fairy_dust", 25) 
            };
        }
    }

    internal sealed class Alicorn : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public Alicorn()
        {
            UnitName = "Alicorn";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.light;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.monster;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("unicorn_horn", 25), 
                new Tuple<string, int>("angelic_essence", 25) 
            };
        }
    }

    internal sealed class WindWraith : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public WindWraith()
        {
            UnitName = "Wind Wraith";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("ectoplasm", 25), 
                new Tuple<string, int>("demonic_essence", 25) 
            };
        }
    }

    internal sealed class LightningGhost : Monster
    {
        public override void UponDefeating()
        {

        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMagicAI(this);
        }

        public LightningGhost()
        {
            UnitName = "Lightning Ghost";
            OffensiveElement = CEnums.Element.electric;
            DefensiveElement = CEnums.Element.electric;
            AttackMessage = "fires a weak projectile at";
            MonsterGroup = CEnums.MonsterGroup.undead;
            SAttackType = CEnums.DamageType.piercing;
            SAttackSound = SoundManager.aim_weapon;
            ClassMultiplier = MonsterDifferentiation.MagicMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };

            DropList = new List<Tuple<string, int>>() 
            { 
                new Tuple<string, int>("ectoplasm", 25), 
                new Tuple<string, int>("demonic_essence", 25) 
            };
        }
    }
    #endregion

    /* =========================== *
     *           BOSSES            *
     * =========================== */
    #region
    public abstract class Boss : Monster
    {
        // Bosses drop a static amount of XP and Gold when they're killed, instead of a
        // dynamic amount based on their level like normal enemies
        public int XPDrops { get; set; }
        public int GoldDrops { get; set; }

        // Determines whether you are given the option to fight the boss when you encounter
        // their tile, or if the fight is forced upon you
        public bool IsMandatory { get; set; }

        // Lackies are other enemies present in the fight that are allied with the boss. This 
        // allows for fights with 1 boss and multiple fodder enemies, or even multiple bosses
        // in one fight!
        public List<Monster> Lackies { get; set; }

        // If true, then the lackies must be defeated for the fight to complete, otherwise
        // only the main boss enemy is required
        public bool AreLackiesRequired { get; set; }

        public bool Active { get; set; }
        public bool Defeated { get; set; }
        public string BossID { get; set; }
    }

    internal sealed class MasterSlime : Boss
    {
        public MasterSlime()
        {
            BossID = "master_slime";
            Active = true;
            Defeated = false;
            Lackies = new List<Monster>() { };

            UnitName = "Master Slime";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "jiggles ferociously towards";
            SAttackType = CEnums.DamageType.physical;
            SAttackSound = SoundManager.sword_slash;
            MonsterGroup = CEnums.MonsterGroup.monster;
            ClassMultiplier = MonsterDifferentiation.MeleeMultiplier;

            SpeciesMultiplier = new Dictionary<string, double>()
            {
                { "hp", 1 },         // HP
                { "mp", 1 },         // MP
                { "attack", 1 },     // Physical Attack
                { "defense", 1 },    // Physical Defense
                { "p_attack", 1 },   // Pierce Attack
                { "p_defense", 1 },  // Pierce Defense
                { "m_attack", 1 },   // Magical Attack
                { "m_defense", 1 },  // Magical Defense
                { "speed", 1 },      // Speed
                { "evasion", 1 }     // Evasion
            };
        }

        public override void UponDefeating()
        {
            throw new NotImplementedException();
        }

        public override void MonsterBattleAI()
        {
            MonsterDifferentiation.GenericMeleeAI(this);
        }
    }

    #endregion
}
