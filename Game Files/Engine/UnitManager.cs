/* This file is part of Peasant's Ascension.
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

namespace Engine
{
    public static class UnitManager
    {
        // Unit manager is responsible for storing PCUs and generating monsters, as well as 
        // performing basic methods with units such as calculating damage
        public static PlayableCharacter player = new PlayableCharacter("John", CEnums.CharacterClass.warrior, "_player", true);
        public static PlayableCharacter solou = new PlayableCharacter("Solou", CEnums.CharacterClass.mage, "_solou", true);
        public static PlayableCharacter chili = new PlayableCharacter("Chili", CEnums.CharacterClass.ranger, "_chili", true);
        public static PlayableCharacter chyme = new PlayableCharacter("Chyme", CEnums.CharacterClass.monk, "_chyme", false);
        public static PlayableCharacter storm = new PlayableCharacter("Storm", CEnums.CharacterClass.assassin, "_storm", false);
        public static PlayableCharacter parsto = new PlayableCharacter("Parsto", CEnums.CharacterClass.paladin, "_parsto", false);
        public static PlayableCharacter adorine = new PlayableCharacter("Adorine", CEnums.CharacterClass.warrior, "_adorine", false);
        public static PlayableCharacter kaltoh = new PlayableCharacter("Kaltoh", CEnums.CharacterClass.bard, "_kaltoh", false);

        private static readonly Random rng = new Random();

        public static List<Monster> MonsterList = new List<Monster>()
        {
            new FireAnt(), new FrostBat(), new SparkBat(), new SludgeRat(), new GiantLandSquid(),
            new GiantCrab(), new SnowWolf(), new Beetle(), new VineLizard(), new GirthWorm(),

            new Willothewisp(), new Alicorn(), new BogSlime(),
            new SandGolem(), new Griffin(), new Harpy(), new SeaSerpent(), new NagaBowwoman(),

            new Troll(), new MossOgre(), new LesserYeti(), new RockGiant(), new GoblinArcher(),
            new Oread(), new TenguRanger(), new Naiad(), new Imp(), new Spriggan(),

            new Zombie(), new UndeadCrossbowman(), new LightningGhost(), new Mummy(), new SkeletonBoneslinger(), new WindWraith(),

            new Necromancer(), new CorruptThaumaturge(), new IceSoldier(), new FallenKnight(), new DevoutProtector(),
        }.OrderBy(_ => rng.Next()).ToList();

        // Returns ALL PCUs, alive, dead, active, and inactive
        public static List<PlayableCharacter> GetAllPCUs()
        {
            return new List<PlayableCharacter>() { player, solou, chili, chyme, storm, parsto, adorine, kaltoh };
        }

        // Returns all PCUs that are alive, regardless of whether they're active or not
        public static List<PlayableCharacter> GetAlivePCUs()
        {
            return GetAllPCUs().Where(x => x.IsAlive()).ToList();
        }

        // Returns all PCUs that are active, regardless of whether they're alive or not
        public static List<PlayableCharacter> GetActivePCUs()
        {
            return GetAllPCUs().Where(x => x.Active).ToList();
        }

        // Returns all PCUs that are both alive and active
        public static List<PlayableCharacter> GetAliveActivePCUs()
        {
            return GetAllPCUs().Where(x => x.Active && x.IsAlive()).ToList();
        }

        public static Monster GenerateMonster()
        {
            // Get a list of all the monster groups that this cell has in its MonsterGroups property
            List<CEnums.MonsterGroup> cell_groups = TileManager.FindCellWithTileID(CInfo.CurrentTile).MonsterGroups;

            // Create a new empty list of monsters
            List<Monster> monsters = MonsterList.Where(x => cell_groups.Contains(x.MonsterGroup)).ToList();

            // Choose a random monster type from the list and create a new monster out of it
            Monster chosen_monster = CMethods.GetRandomFromIterable(monsters);
            Monster new_monster = Activator.CreateInstance(chosen_monster.GetType()) as Monster;

            // Level-up the monster to increase its stats to the level of the cell that the player is in
            new_monster.MonsterLevelUp();

            // Apply multipliers to the monster based on its species, class, and par56ty difficulty
            new_monster.MonsterApplyMultipliers();

            // The new monster has been generated - we now return it
            return new_monster;
        }

        public static void CreatePlayer()
        {
            CMethods.PrintDivider();
            player.PlayerChooseName();
            player.PlayerChooseClass();
            SavefileManager.ChooseAdventureName();

            chili.Statuses.Add(CEnums.Status.poison);
            chili.Statuses.Add(CEnums.Status.blindness);
            solou.Statuses.Add(CEnums.Status.weakness);

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

            // Monk doesn't have any weapons, so we don't need to equip anything here

            else if (player.PClass == CEnums.CharacterClass.paladin)
            {
                InventoryManager.EquipItem(player, "rubber_mallet");
            }

            else if (player.PClass == CEnums.CharacterClass.bard)
            {
                InventoryManager.EquipItem(player, "kazoo");
            }

            player.PlayerCalculateStats();
            solou.PlayerCalculateStats();
            chili.PlayerCalculateStats();
            chyme.PlayerCalculateStats();
            storm.PlayerCalculateStats();
            parsto.PlayerCalculateStats();
            adorine.PlayerCalculateStats();
            kaltoh.PlayerCalculateStats();

            HealAllPCUs(true, true, true, true);
            ExplainTheSetting();
            SavefileManager.SaveTheGame();
        }

        public static int CalculateDamage(Unit attacker, Unit target, CEnums.DamageType damage_type, AttackSpell spell = null, bool do_crits = true)
        {
            // Attacker - the Unit that is attacking
            // Target - the Unit that is being attacked
            // Damage Type - the type of damage being dealt (magical, physical, or piercing)
            Random rng = new Random();

            int attack;
            int p_attack;
            int m_attack;

            int defense;
            int p_defense;
            int m_defense;

            double weapon_power;
            double armor_resist;

            CEnums.Element attacker_element;
            CEnums.Element target_element = target.DefensiveElement;

            int final_damage;

            attack = attacker.TempStats["attack"];
            p_attack = attacker.TempStats["p_attack"];
            m_attack = attacker.TempStats["m_attack"];

            defense = target.TempStats["defense"];
            p_defense = target.TempStats["p_defense"];
            m_defense = target.TempStats["m_defense"];

            if (attacker is PlayableCharacter pcu_attacker)
            {
                weapon_power = (InventoryManager.GetEquipmentItems()[pcu_attacker.PlayerID][CEnums.EquipmentType.weapon] as Weapon).Power;
            }

            else
            {
                weapon_power = Math.Min((double)attacker.Level / 50, 1);
            }

            if (target is PlayableCharacter pcu_target)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu_target.PlayerID][CEnums.EquipmentType.armor] as Armor;
                armor_resist = pcu_armor.GetEffectiveResist(pcu_target);
            }

            else
            {
                armor_resist = Math.Min((double)target.Level / 50, 1);
            }

            if (damage_type == CEnums.DamageType.physical)
            {
                final_damage = (int)((attack - (defense / 2)) * (1 + weapon_power) / (1 + armor_resist));

                // Weakeness reduces physical damage by 1/2
                if (attacker.HasStatus(CEnums.Status.weakness))
                {
                    final_damage /= 2;
                    Console.WriteLine($"{attacker.UnitName}'s weakness reduces their attack damage by half!");
                }

                attacker_element = attacker.OffensiveElement;
            }

            else if (damage_type == CEnums.DamageType.piercing)
            {
                final_damage = (int)((p_attack - (p_defense / 2)) * (1 + weapon_power) / (1 + armor_resist));

                // Blindness reduces piercing damage by 1/2
                if (attacker.HasStatus(CEnums.Status.blindness))
                {
                    final_damage /= 2;
                    Console.WriteLine($"{attacker.UnitName}'s blindness reduces their attack damage by half!");
                }

                attacker_element = attacker.OffensiveElement;
            }

            else
            {
                double spell_power;

                if (attacker is PlayableCharacter pcu)
                {
                    // Player Spell Power is equal to Charisma/100, with a maximum value of 1
                    // This formula means that spell power increases linearly from 0.01 at 1 Charisma, to 1 at 100 Charisma
                    spell_power = Math.Min((double)pcu.Attributes[CEnums.PlayerAttribute.charisma] / 100, 1);
                }

                else
                {
                    // Monster Spell Power is equal to Level/50, with a maximum value of 1
                    // This formula means that spell power increases linearly from 0.02 at level 1, to 1 at level 50
                    // All monsters from level 50 onwards have exactly 1 spell power
                    spell_power = Math.Min((double)attacker.Level/50, 1);
                }

                final_damage = (int)((m_attack - (m_defense / 2)) * (1 + spell_power) / (1 + armor_resist));
                attacker_element = spell.OffensiveElement;
            }

            if (rng.Next(0, 100) < 15 && do_crits)
            {
                final_damage = (int)(final_damage * 1.5);
                SoundManager.critical_hit.SmartPlay();
                Console.WriteLine("It's a critical hit! 1.5x damage!");

                CMethods.SmartSleep(500);
            }

            final_damage = ApplyElementalChart(attacker_element, target_element, final_damage);
            return (int)CMethods.Clamp(final_damage, 1, 999);
        }

        public static int ApplyElementalChart(CEnums.Element attacker_element, CEnums.Element target_element, int damage)
        {
            // Fire > Ice > Grass > Wind > Earth > Electricity > Water > Fire
            // Light > Dark and Dark > Light, Dark and Light resist themselves
            // Neutral element is neutral both offensively and defensively
            // All other interactions are neutral

            // If the target is weak to the attackers element, then the attack will deal 1.5x damage
            if (attacker_element == target_element.GetElementalMatchup().Item1)
            {
                return (int)(damage * 1.5);
            }

            else if (attacker_element == target_element.GetElementalMatchup().Item2)
            {
                return (int)(damage / 1.5);
            }

            return damage;
        }

        public static bool DoesAttackHit(Unit target)
        {
            Random rng = new Random();
            int target_evasion = target.TempStats["evasion"];

            if (target is PlayableCharacter pcu)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu.PlayerID][CEnums.EquipmentType.armor] as Armor;
                target_evasion -= (int)(512*pcu_armor.GetEffectivePenalty(pcu));
            }

            return target_evasion < rng.Next(0, 512);
        }

        public static void HealOnePCU(string pcu_id, bool restore_hp, bool restore_mp, bool restore_ap, bool cure_statuses)
        {
            PlayableCharacter pcu = GetAllPCUs().Single(x => x.PlayerID == pcu_id);
            if (restore_hp)
            {
                pcu.HP = pcu.MaxHP;
            }

            if (restore_mp)
            {
                pcu.MP = pcu.MaxHP;
            }

            if (restore_ap)
            {
                pcu.AP = pcu.MaxHP;
            }

            if (cure_statuses)
            {
                pcu.Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
            }

            pcu.FixAllStats();
        }

        public static void HealAllPCUs(bool restore_hp, bool restore_mp, bool restore_ap, bool cure_statuses)
        {
            GetAllPCUs().ForEach(x => HealOnePCU(x.PlayerID, restore_hp, restore_mp, restore_ap, cure_statuses));
        }

        public static StatMatrix GetAttributeMatrix(CEnums.PlayerAttribute attribute)
        {
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
            return new Dictionary<CEnums.CharacterClass, StatMatrix>()
            {
                { CEnums.CharacterClass.warrior, new StatMatrix(2, 1, 0, 3, 3, 1, 3, 1, 1, 1, 1) },
                { CEnums.CharacterClass.mage, new StatMatrix(1, 3, 0, 1, 1, 2, 1, 3, 3, 1, 2) },
                { CEnums.CharacterClass.assassin, new StatMatrix(1, 1, 0, 3, 1, 1, 2, 1, 1, 3, 3) },
                { CEnums.CharacterClass.ranger, new StatMatrix(1, 1, 0, 3, 1, 3, 1, 1, 1, 2, 3) },
                { CEnums.CharacterClass.monk, new StatMatrix(1, 2, 0, 3, 1, 1, 1, 1, 1, 3, 3) },
                { CEnums.CharacterClass.paladin, new StatMatrix(2, 2, 0, 1, 3, 1, 2, 1, 3, 1, 1) },
                { CEnums.CharacterClass.bard, new StatMatrix(1, 2, 0, 1, 1, 1, 1, 1, 2, 2, 3) }
            }[p_class];
        }

        public static StatMatrix GetClassMatrix(CEnums.CharacterClass p_class)
        {
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

        public static bool CheckForBosses()
        {
            // To-do!!
            return false;
        }

        private static void ExplainTheSetting()
        {
            List<string> setting_explanation = new List<string>()
            {
@"This story takes place in the land of Brumia, a continent surrounded by a
wall of thick fog. The fog is so thick that not even light can travel through
it, and anyone who has attempted to travel through it has failed to return.
The fog has been there as long as anyone can remember, so it's assumed to have
always been a part of this world.",

@"Brumia has 4 nations on it: The Kingdom of Harconia, a massive realm ruled by
the beloved and fair King Harconius. The Kingdom of Brescavia, an equally large
country ruled by the noble and courageous King Bascot. Koh'rin, an island 
previously separated from the mainland by the fog, now connected via an
underground tunnel. And Thex, a military nation with some of the most deadly
and skilled assassins in the land.",

@"You are a farming peasant from the village of Nearton, in the province of
Overshire, in the Kingdom of Harconia. Nearton is a small farming community 
with one of the smallest populations in the Kingdom. You have no formal combat 
training and seemingly no hope of growing beyond your current profession.
Despite this, you will soon grow to become the hero of this land.",

"It's time for your adventure to begin. It's time to ascend!"
            };

            CMethods.PrintDivider();
            CMethods.ReadStringListAsBook(setting_explanation, "The Adventure Begins");
        }
    }

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

    public abstract class Unit
    {
        /* =========================== *
         *      GENERAL PROPERTIES     *
         * =========================== */
        public CEnums.Element OffensiveElement = CEnums.Element.neutral;
        public CEnums.Element DefensiveElement = CEnums.Element.neutral;
        public List<CEnums.Status> Statuses = new List<CEnums.Status> { CEnums.Status.alive };

        public string UnitName { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public int AP { get; set; }
        public int MaxAP { get; set; }

        // These properties are protected, because we do not want them to be directly
        // accessed or manipulated by outside methods. Instead we use Unit.TempStats.
        protected int Attack { get; set; }
        protected int Defense { get; set; }
        protected int PAttack { get; set; }
        protected int PDefense { get; set; }
        protected int MAttack { get; set; }
        protected int MDefense { get; set; }
        protected int Speed { get; set; }
        protected int Evasion { get; set; }

        public Dictionary<string, int> TempStats = new Dictionary<string, int>()
        {
            { "attack", 0 },
            { "defense", 0 },
            { "p_attack", 0 },
            { "p_defense", 0 },
            { "m_attack", 0 },
            { "m_defense", 0 },
            { "speed", 0 },
            { "evasion", 0 }
        };

        /* =========================== *
         *           METHODS           *
         * =========================== */
        public bool IsAlive()
        {
            return HasStatus(CEnums.Status.alive);
        }

        public bool IsDead()
        {
            return HasStatus(CEnums.Status.dead);
        }

        public void SetTempStats()
        {
            TempStats["attack"] = Attack;
            TempStats["defense"] = Defense;
            TempStats["p_attack"] = PAttack;
            TempStats["p_defense"] = PDefense;
            TempStats["m_attack"] = MAttack;
            TempStats["m_defense"] = MDefense;
            TempStats["speed"] = Speed;
            TempStats["evasion"] = Evasion;
        }

        public bool HasStatus(CEnums.Status status)
        {
            return Statuses.Contains(status);
        }

        public void FixAllStats()
        {
            // Makes sure that that no-one ever has stats that would cause the game to malfunction.
            // e.g. no negative HP/MP/AP, no HP/MP/AP above max, etc.
            // This function also acts as a hard-cap for evasion, which is limited to a max of 256
            // (50% dodge chance). This is to prevent people from min-maxing their evasion to cheese
            // their way through the game, and also prevents monsters from being invincible.

            HP = (int)CMethods.Clamp(HP, 0, MaxHP);
            MP = (int)CMethods.Clamp(MP, 0, MaxMP);
            AP = (int)CMethods.Clamp(AP, 0, MaxAP);

            Attack = Math.Max(1, Attack);
            PAttack = Math.Max(1, PAttack);
            MAttack = Math.Max(1, MAttack);

            Defense = Math.Max(1, Defense);
            PDefense = Math.Max(1, PDefense);
            MDefense = Math.Max(1, MDefense);

            Speed = Math.Max(1, Speed);
            Evasion = (int)CMethods.Clamp(Evasion, 1, 256);

            Statuses = Statuses.Distinct().ToList();

            if (HP > 0 && !IsAlive())
            {
                Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
            }

            if (HP == 0 && !IsDead())
            {
                Statuses = new List<CEnums.Status>() { CEnums.Status.dead };
            }
        }
    }

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
         *        PLAYER METHODS       *
         * =========================== */
        public void PlayerChooseName()
        {
            const int max_chars = 25;

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

                else if (CInfo.FriendNames.Contains(chosen_name.ToLower()))
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
                    string yes_no = CMethods.SingleCharInput($"So, your name is '{chosen_name}'? [Y]es or [N]o: ").ToLower();

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
     [5] Monk: Fighter whose fists are a worthy oppenent to any blade
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
while their Charisma grants their words more power. Mages have the strongest
magical capabilities, but they are completely defenseless against physical and
piercing attacks."
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

Rangers are an "  // To-do!!
                        },

                        {
                            CEnums.CharacterClass.monk,
@"Monks belong to the guild 'The Brotherhood of the Valenfall Abbey'.
While typical monks are pacifists who commit their lives to their religion,
The Brotherhood has adapted the patience and moderation aspects for use in
combat.

Monks take a less materialistic approach to warfare, opting to only use their
fists instead of swords or bows. Their Consitution lets them directly 
manipulate chakras and auras, both through spells and abilities. Their 
Dexterity enables them to effortlessly dodge most attacks and get in swift
blows then the time is right. These strengths come at the cost of abysmal
defenses. A wise monk would opt to focus on evasion to overcome this downside."
                        },

                        {
                            CEnums.CharacterClass.paladin,
@"-Can use abilities that scale with WIS and STR
-Can learn all Healing spells and offensive Light spells
-Deals Physical Damage with Standard Attacks
-High Magical/Physical Defense
-Average MP, HP, and Pierce Defense
-Low Physical/Magical Attack, Speed, and Evasion"  // To-do!!
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
                    string yes_no = CMethods.SingleCharInput($"You wish to be a {chosen_class.EnumToString()}? [Y]es or [N]o: ").ToLower();

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

        public bool PlayerLevelUp()
        {
            if (CurrentXP >= RequiredXP)
            {
                SoundManager.levelup_music.PlayLooping();

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
                PlayerAllocateSkillPoints();
                PlayerCalculateStats();

                // true => The player leveled up
                return true;
            }

            // false => The player did not level up
            return false;
        }

        public void PlayerAllocateSkillPoints()
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
                        string yes_no = CMethods.SingleCharInput($"Increase {UnitName}'s {attribute.EnumToString()}? [Y]es or [N]o: ").ToLower();

                        if (yes_no.IsYesString())
                        {
                            PlayerIncreaseAttribute(attribute);

                            if (attribute == CEnums.PlayerAttribute.difficulty)
                            {
                                CMethods.PrintDivider();
                                Console.WriteLine("Game Difficulty increased!");
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

        public void PlayerIncreaseAttribute(CEnums.PlayerAttribute attribute)
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
Speed: {Speed}
Evasion: {Evasion}
Elements: Attacks are {OffensiveElement.EnumToString()} / Defense is {DefensiveElement.EnumToString()}
Weak to { DefensiveElement.GetElementalMatchup().Item1.EnumToString() }
Resistant to { DefensiveElement.GetElementalMatchup().Item2.EnumToString()}

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

        public void PlayerChoice(List<Monster> monster_list)
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
                    if (!PlayerChooseTarget(monster_list, $"Who should {UnitName} attack?", false, true, false, false))
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

                    if (!SpellManager.PickSpellCategory(this, monster_list))
                    {
                        PrintBattleOptions();
                        continue;
                    }

                    return;
                }

                // Ability
                else if (CurrentMove == "3")
                {
                    if (PlayerChooseAbility())
                    {
                        return;
                    }
                }

                // Use Items
                else if (CurrentMove == "4")
                {
                    CMethods.PrintDivider();

                    var x = new List<int>();
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

                    if (!BattleManager.BattlePickItem(this, monster_list))
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

        public bool PlayerExecuteMove(List<Monster> monster_list)
        {
            Random rng = new Random();
            Weapon player_weapon = InventoryManager.GetEquipmentItems()[PlayerID][CEnums.EquipmentType.weapon] as Weapon;
            SoundManager.item_pickup.Stop();

            // If the player's target is an enemy, and the target died before the player's turn began,
            // then the attack automatically redirects to a random living enemy.
            if (CurrentTarget is Monster && !CurrentTarget.IsAlive())
            {
                CurrentTarget = CMethods.GetRandomFromIterable(monster_list.Where(x => x.IsAlive()));
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

            // Basic Attack
            if (CurrentMove == "1")
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
                int attack_damage = UnitManager.CalculateDamage(this, CurrentTarget, player_weapon.DamageType);

                if (UnitManager.DoesAttackHit(CurrentTarget))
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
                if (BattleManager.TryToRunAway(this, monster_list))
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

        public bool PlayerChooseTarget(List<Monster> monster_list, string action_desc, bool target_allies, bool target_enemies, bool allow_dead, bool allow_inactive)
        {
            // A list of PCUs that are valid for targetting (could be unused if target_allies is false)
            List<PlayableCharacter> pcu_list;

            if (allow_inactive)
            {
                if (allow_dead)
                {
                    // YES to dead PCUs, YES to inactive PCUs
                    pcu_list = UnitManager.GetAllPCUs();
                }

                else
                {
                    // NO to dead PCUs, YES to inactive PCUs
                    pcu_list = UnitManager.GetAlivePCUs();
                }
            }

            else
            {
                if (allow_dead)
                {
                    // YES to dead PCUs, NO to inactive PCUs
                    pcu_list = UnitManager.GetActivePCUs();
                }

                else
                {
                    // NO to dead PCUs, NO to inactive PCUs
                    pcu_list = UnitManager.GetAliveActivePCUs();
                }
            }

            // The full list of valid targets, including both monsters and allies if applicable
            List<Unit> valid_targets = new List<Unit>();

            // Do this if both allies and enemies are valid targets (e.g. some abilities and spells)
            if (target_allies && target_enemies)
            {
                pcu_list.ForEach(x => valid_targets.Add(x));
                monster_list.ForEach(x => x.FixAllStats());
                monster_list.Where(x => x.IsAlive()).ToList().ForEach(x => valid_targets.Add(x));
            }

            // Do this if the player is allowed to target allies but not enemies (e.g. items, some spells/abilities)
            else if (target_allies && !target_enemies)
            {
                if (pcu_list.Count == 1)
                {
                    CurrentTarget = pcu_list[0];
                    return true;
                }

                pcu_list.ForEach(x => valid_targets.Add(x));
            }

            // Do this if the player is allowed to target enemies but not allies (e.g. attacks, some spells/abilities)
            else if (!target_allies && target_enemies)
            {
                if (monster_list.Count(x => x.IsAlive()) == 1)
                {
                    CurrentTarget = monster_list.Where(x => x.IsAlive()).ToList()[0];
                    return true;
                }

                monster_list.Where(x => x.IsAlive()).ToList().ForEach(x => valid_targets.Add(x));
            }

            else
            {
                throw new InvalidOperationException("Exception in 'choose_target': at least one of 'target_allies' or 'target_enemies' must be true");
            }

            CMethods.PrintDivider();
            Console.WriteLine(action_desc);

            // Print out the target list
            foreach (Tuple<int, Unit> element in CMethods.Enumerate(valid_targets))
            {
                Console.WriteLine($"      [{element.Item1 + 1}] {element.Item2.UnitName}");
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

        public bool PlayerChooseAbility()
        {
            while (true)
            {
                CMethods.PrintDivider();
                Console.WriteLine($"{UnitName}'s Abilities | {AP}/{MaxAP} AP remaining");

                // List of all abilities usable by the PCU's class
                List<Ability> a_list = AbilityManager.GetAbilityList()[PClass];

                // This is used to make sure that the AP costs of each ability line up for asthetic reasons.
                int padding = a_list.Max(x => x.AbilityName.Length);

                foreach (Tuple<int, Ability> element in CMethods.Enumerate(a_list))
                {
                    string pad = new string('-', padding - element.Item2.AbilityName.Length);
                    Console.WriteLine($"      [{element.Item1 + 1}] {element.Item2.AbilityName} {pad}-> {element.Item2.APCost} AP");
                }

                while (true)
                {
                    string chosen_ability = CMethods.FlexibleInput("Input [#] or type 'exit'): ", a_list.Count);

                    try
                    {
                        CurrentAbility = a_list[int.Parse(chosen_ability) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (chosen_ability.IsExitString())
                        {
                            CMethods.PrintDivider();
                            PrintBattleOptions();

                            return false;
                        }

                        continue;
                    }

                    // Abilities cost AP to cast, just like spells cost MP.
                    if (AP < CurrentAbility.APCost)
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"{UnitName} doesn't have enough AP to cast {CurrentAbility.AbilityName}!");
                        CMethods.PressAnyKeyToContinue();

                        break;
                    }

                    AP -= CurrentAbility.APCost;
                    CurrentAbility.BeforeAbility(this);

                    return true;
                }
            }
        }

        public void PlayerCalculateStats()
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

    public abstract class Monster : Unit
    {
        public string AttackMessage { get; set; }
        public string AsciiArt { get; set; }
        public Dictionary<string, double> ClassMultipliers { get; set; }
        public Dictionary<string, double> SpeciesMultipliers { get; set; }
        public CEnums.MonsterGroup MonsterGroup { get; set; }

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
        public void MonsterGiveStatus(int status_mp_cost)
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

            Console.WriteLine($"The {UnitName} is attempting to make {CurrentTarget.UnitName} {chosen_status.EnumToString()}!");
            CMethods.SmartSleep(750);

            if (rng.Next(0, 2) == 0)
            {
                if (CurrentTarget.HasStatus(chosen_status))
                {
                    SoundManager.debuff.SmartPlay();
                    Console.WriteLine($"...But {CurrentTarget.UnitName} is already {chosen_status.EnumToString()}!");
                }

                else
                {
                    CurrentTarget.Statuses.Add(chosen_status);
                    SoundManager.buff_spell.SmartPlay();
                    Console.WriteLine($"{CurrentTarget.UnitName} is now {chosen_status.EnumToString()}!");
                }
            }

            else
            {
                SoundManager.debuff.SmartPlay();
                Console.WriteLine($"...But {UnitName}'s attempt failed!");
            }

            MP -= status_mp_cost;
        }

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
            MaxHP = HP = (int)(HP * ClassMultipliers["hp"] * SpeciesMultipliers["hp"]);
            MaxMP = MP = (int)(MP * ClassMultipliers["mp"] * SpeciesMultipliers["mp"]);
            Attack = (int)(Attack * ClassMultipliers["attack"] * SpeciesMultipliers["attack"]);
            Defense = (int)(Defense * ClassMultipliers["defense"] * SpeciesMultipliers["defense"]);
            PAttack = (int)(PAttack * ClassMultipliers["p_attack"] * SpeciesMultipliers["p_attack"]);
            PDefense = (int)(PDefense * ClassMultipliers["p_defense"] * SpeciesMultipliers["p_defense"]);
            MAttack = (int)(MAttack * ClassMultipliers["m_attack"] * SpeciesMultipliers["m_attack"]);
            MDefense = (int)(MDefense * ClassMultipliers["m_defense"] * SpeciesMultipliers["m_defense"]);
            Speed = (int)(Speed * ClassMultipliers["speed"] * SpeciesMultipliers["speed"]);
            Evasion = (int)(Evasion * ClassMultipliers["evasion"] * SpeciesMultipliers["evasion"]);

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

            MonsterDoAbilities();
        }

        public void MonsterGetTarget()
        {
            Random rng = new Random();

            CurrentTarget = CMethods.GetRandomFromIterable(UnitManager.GetAliveActivePCUs());

            if (MonsterAbilityFlags["taunted_turn"] == BattleManager.GetTurnCounter())
            {
                CurrentTarget = MonsterAbilityFlags["taunted_user"];
            }
        }

        public void MonsterDoAbilities()
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
                Console.WriteLine($"{UnitName}'s judgment day has arrived. The darkness devours it...");
                HP = 0;
            }


        }

        public abstract void UponDefeating();

        public abstract void MonsterBattleAI();

        /* =========================== *
         *          CONSTRUCTOR        *
         * =========================== */
        protected Monster()
        {
            HP = 10;
            MP = 5;
            Attack = 6;
            Defense = 4;
            PAttack = 6;
            PDefense = 4;
            MAttack = 6;
            MDefense = 4;
            Speed = 5;
            Evasion = 3;
            Level = 1;

            IsDefending = false;
            MaxHP = MP;
            MaxMP = MP;
        }
    }

    // =========================== #
    //       MELEE MONSTERS        #
    // =========================== #
    #region
    internal abstract class MeleeMonster : Monster
    {
        public override void MonsterBattleAI()
        {
            Random rng = new Random();

            // Melee monsters have a 1 in 6 (16.667%) chance to defend
            if (rng.Next(0, 5) == 0 && !IsDefending && (MonsterAbilityFlags["taunted_turn"] != BattleManager.GetTurnCounter()))
            {
                IsDefending = true;
                Console.WriteLine($"The {UnitName} is preparing itself for enemy attacks...");
                CMethods.SmartSleep(750);

                TempStats["defense"] *= 2;
                TempStats["m_defense"] *= 2;
                TempStats["p_defense"] *= 2;

                Console.WriteLine($"The {UnitName}'s defense stats increased by 2x for one turn!");
                SoundManager.buff_spell.SmartPlay();
                return;
            }

            else if (IsDefending)
            {
                Console.WriteLine($"The {UnitName} stops defending, returning its defense stats to normal.");
                IsDefending = false;

                TempStats["defense"] /= 2;
                TempStats["m_defense"] /= 2;
                TempStats["p_defense"] /= 2;
            }

            SoundManager.sword_slash.SmartPlay();
            Console.WriteLine($"The {UnitName} {AttackMessage} {CurrentTarget.UnitName}...");
            CMethods.SmartSleep(750);

            int attack_damage = UnitManager.CalculateDamage(this, CurrentTarget, CEnums.DamageType.physical);

            if (UnitManager.DoesAttackHit(CurrentTarget))
            {
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack deals {attack_damage} damage to {CurrentTarget.UnitName}!");
                CurrentTarget.HP -= attack_damage;
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack narrowly misses {CurrentTarget.UnitName}!");
            }
        }

        protected MeleeMonster()
        {
            ClassMultipliers = new Dictionary<string, double>()
            {
                { "hp", 1.2 },            // HP
                { "mp", 1 },          // MP
                { "attack", 1.5 },      // Physical Attack
                { "defense", 1.5 },    // Physical Defense
                { "p_attack", 0.5 },    // Pierce Attack
                { "p_defense", 1.5 },  // Pierce Defense
                { "m_attack", 0.5 },    // Magical Attack
                { "m_defense", 0.5 },   // Magical Defense
                { "speed", -0.65 },         // Speed
                { "evasion", 1 }        // Evasion
            };
        }
    }

    internal sealed class GiantCrab : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public GiantCrab()
        {
            UnitName = "Giant Crab";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "snaps its massive claws at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("crab_claw", 25), new Tuple<string, int>("shell_fragment", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class BogSlime : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public BogSlime()
        {
            UnitName = "Bog Slime";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "jiggles menacingly at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("slime_vial", 25), new Tuple<string, int>("water_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Mummy : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Mummy()
        {
            UnitName = "Mummy";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "meanders over and grabs";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("burnt_ash", 25), new Tuple<string, int>("ripped_cloth", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SandGolem : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public SandGolem()
        {
            UnitName = "Sand Golem";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "begins to pile sand on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("golem_rock", 25), new Tuple<string, int>("broken_crystal", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class MossOgre : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public MossOgre()
        {
            UnitName = "Moss Ogre";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "swings a tree trunk like a club at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("bone_bag", 25), new Tuple<string, int>("monster_skull", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Troll : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Troll()
        {
            UnitName = "Troll";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "swings its mighty battleaxe at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_skull", 25), new Tuple<string, int>("eye_balls", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Griffin : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Griffin()
        {
            UnitName = "Griffin";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "swipes with its ferocious claws at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("animal_fur", 25), new Tuple<string, int>("wing_piece", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class GirthWorm : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public GirthWorm()
        {
            UnitName = "Girth Worm";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "burrows into the ground towards";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_fang", 25), new Tuple<string, int>("slime_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Zombie : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Zombie()
        {
            UnitName = "Giant Crab";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "charges and tries to bite";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_skull", 25), new Tuple<string, int>("blood_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SnowWolf : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public SnowWolf()
        {
            UnitName = "Snow Wolf";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "claws and bites at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("animal_fur", 25), new Tuple<string, int>("monster_fang", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class LesserYeti : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public LesserYeti()
        {
            UnitName = "Lesser Yeti";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "begins to maul";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("animal_fur", 25), new Tuple<string, int>("monster_fang", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SludgeRat : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public SludgeRat()
        {
            UnitName = "Sludge Rat";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "ferociously chomps at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_skull", 25), new Tuple<string, int>("rodent_tail", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SeaSerpent : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public SeaSerpent()
        {
            UnitName = "Sea Serpent";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "charges head-first into";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("serpent_scale", 25), new Tuple<string, int>("serpent_tongue", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Beetle : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Beetle()
        {
            UnitName = "Beetle";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "charges horn-first into";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("beetle_shell", 25), new Tuple<string, int>("antennae", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Harpy : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Harpy()
        {
            UnitName = "Harpy";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "dives claws-first towards";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("wing_piece", 25), new Tuple<string, int>("feathers", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class FallenKnight : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public FallenKnight()
        {
            UnitName = "Fallen Knight";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "thrusts its heavenly spear towards";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("chain_link", 25), new Tuple<string, int>("blood_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.dungeon;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class DevoutProtector : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public DevoutProtector()
        {
            UnitName = "Devout Protector";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.light;
            AttackMessage = "swings its holy hammer towards";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("angelic_essence", 25), new Tuple<string, int>("runestone", 25) };
            MonsterGroup = CEnums.MonsterGroup.dungeon;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Calculator : MeleeMonster
    {
        public override void UponDefeating()
        {

        }

        public Calculator()
        {
            UnitName = "Calculator";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "casts its mathemagical spell on";
            DropList = new List<Tuple<string, int>>() {
                new Tuple<string, int>("calculus_homework", 25),
                new Tuple<string, int>("graph_paper", 25),
                new Tuple<string, int>("protractor", 25),
                new Tuple<string, int>("ruler", 25),
                new Tuple<string, int>("textbook", 25)
            };

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }
    #endregion

    // =========================== #
    //       RANGED MONSTERS       #
    // =========================== #
    #region
    internal abstract class RangedMonster : Monster
    {
        public override void MonsterBattleAI()
        {
            Random rng = new Random();
            Console.WriteLine($"The {UnitName} {AttackMessage} {CurrentTarget.UnitName}...");
            SoundManager.aim_weapon.SmartPlay();

            CMethods.SmartSleep(750);

            int attack_damage = UnitManager.CalculateDamage(this, CurrentTarget, CEnums.DamageType.piercing);

            if (UnitManager.DoesAttackHit(CurrentTarget))
            {
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack deals {attack_damage} damage to {CurrentTarget.UnitName}!");
                CurrentTarget.HP -= attack_damage;
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack narrowly misses {CurrentTarget.UnitName}!");
            }
        }

        protected RangedMonster()
        {
            ClassMultipliers = new Dictionary<string, double>()
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
        }
    }

    internal sealed class FireAnt : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public FireAnt()
        {
            UnitName = "Fire Ant";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.fire;
            AttackMessage = "spits a firey glob of acid at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("antennae", 25), new Tuple<string, int>("burnt_ash", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class NagaBowwoman : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public NagaBowwoman()
        {
            UnitName = "Naga Bow-woman";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "fires a volley of arrows at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("serpent_scale", 25), new Tuple<string, int>("serpent_tongue", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class IceSoldier : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public IceSoldier()
        {
            UnitName = "Ice Soldier";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "fires a single hyper-cooled arrow at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("chain_link", 25), new Tuple<string, int>("blood_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.dungeon;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class FrostBat : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public FrostBat()
        {
            UnitName = "Frost Bat";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "spits a frozen glob of acid at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_fang", 25), new Tuple<string, int>("wing_piece", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SparkBat : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public SparkBat()
        {
            UnitName = "Spark Bat";
            OffensiveElement = CEnums.Element.electric;
            DefensiveElement = CEnums.Element.electric;
            AttackMessage = "spits an electrified glob of acid at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("monster_fang", 25), new Tuple<string, int>("wing_piece", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class SkeletonBoneslinger : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public SkeletonBoneslinger()
        {
            UnitName = "Skeleton Boneslinger";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "grabs a nearby bone and slings it at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("bone_bag", 25), new Tuple<string, int>("demonic_essence", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class UndeadCrossbowman : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public UndeadCrossbowman()
        {
            UnitName = "Undead Crossbowman";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "fires a bone-tipped crossbow bolt at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("chain_link", 25), new Tuple<string, int>("bone_bag", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class RockGiant : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public RockGiant()
        {
            UnitName = "Rock Giant";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "hurls a giant boulder at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("golem_rock", 25), new Tuple<string, int>("broken_crystal", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class GoblinArcher : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public GoblinArcher()
        {
            UnitName = "Goblin Archer";
            OffensiveElement = CEnums.Element.neutral;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "fires an arrow at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ripped_cloth", 25), new Tuple<string, int>("eye_balls", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class GiantLandSquid : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public GiantLandSquid()
        {
            UnitName = "Giant Land-Squid";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "shoots a black, inky substance at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ink_sack", 25), new Tuple<string, int>("slime_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class VineLizard : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public VineLizard()
        {
            UnitName = "Vine Lizard";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "spits an acidic string of vines at";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("serpent_scale", 25), new Tuple<string, int>("living_bark", 25) };
            MonsterGroup = CEnums.MonsterGroup.animal;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class TenguRanger : RangedMonster
    {
        public override void UponDefeating()
        {

        }

        public TenguRanger()
        {
            UnitName = "Tengu Ranger";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "catapults a stone javelin towards";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("wing_piece", 25), new Tuple<string, int>("feathers", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }
    #endregion

    // =========================== //
    //       MAGIC MONSTERS        //
    // =========================== //
    #region
    internal abstract class MagicMonster : Monster
    {
        public override void MonsterBattleAI()
        {
            Random rng = new Random();
            int status_mp_cost = MaxMP / 8;
            int heal_mp_cost = MaxMP / 5;
            int attack_mp_cost = MaxHP / 7;

            // If the monster is neither taunted nor silenced, it will use a spell
            if ((MonsterAbilityFlags["taunted_turn"] != BattleManager.GetTurnCounter()) || HasStatus(CEnums.Status.silence))
            {
                if (rng.Next(0, 6) == 0 && MP >= status_mp_cost)
                {
                    MonsterGiveStatus(status_mp_cost);

                    return;
                }

                // Magic heal
                else if (HP <= MaxHP / 5 && MP >= heal_mp_cost)
                {
                    Console.WriteLine($"The {UnitName} is casting a healing spell on itself...");
                    CMethods.SmartSleep(750);

                    int total_heal = Math.Max(HP / 5, 5);
                    HP += total_heal;
                    MP -= heal_mp_cost;

                    Console.WriteLine($"The {UnitName} heals itself for {total_heal} HP!");
                    SoundManager.magic_healing.SmartPlay();

                    return;
                }

                // Magical Attack
                else if (MP >= attack_mp_cost)
                {
                    SoundManager.magic_attack.SmartPlay();

                    Console.WriteLine($"The {UnitName} {AttackMessage} {CurrentTarget.UnitName}...");
                    CMethods.SmartSleep(750);

                    // UnitManager.CalculateDamage() for magical damage requires an AttackSpell as an argument, so we have
                    // to create a dummy spell.
                    AttackSpell m_spell = new AttackSpell("", "", 0, 0, new List<CEnums.CharacterClass>(), OffensiveElement);

                    int spell_damage = UnitManager.CalculateDamage(this, CurrentTarget, CEnums.DamageType.magical, spell: m_spell);

                    if (UnitManager.DoesAttackHit(CurrentTarget))
                    {
                        SoundManager.enemy_hit.SmartPlay();
                        Console.WriteLine($"The {UnitName}'s spell deals {spell_damage} damage to {CurrentTarget.UnitName}!");

                        CurrentTarget.HP -= spell_damage;
                    }

                    else
                    {
                        SoundManager.attack_miss.SmartPlay();
                        Console.WriteLine($"The {UnitName}'s spell narrowly misses {CurrentTarget.UnitName}!");
                    }

                    MP -= attack_mp_cost;

                    return;
                }
            }

            // Non-magical Attack (Pierce Damage). Only happens if taunted, silenced, or if out of mana.           
            Console.WriteLine($"The {UnitName} attacks {CurrentTarget.UnitName}...");
            SoundManager.aim_weapon.SmartPlay();

            CMethods.SmartSleep(750);
            int attack_damage = UnitManager.CalculateDamage(this, CurrentTarget, CEnums.DamageType.piercing);

            if (UnitManager.DoesAttackHit(CurrentTarget))
            {
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack deals {attack_damage} damage to {CurrentTarget.UnitName}!");
                CurrentTarget.HP -= attack_damage;
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {UnitName}'s attack narrowly misses {CurrentTarget.UnitName}!");
            }
        }

        protected MagicMonster()
        {
            ClassMultipliers = new Dictionary<string, double>()
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
        }
    }

    internal sealed class Oread : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Oread()
        {
            UnitName = "Oread";
            OffensiveElement = CEnums.Element.earth;
            DefensiveElement = CEnums.Element.earth;
            AttackMessage = "casts a basic earth spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("fairy_dust", 25), new Tuple<string, int>("eye_balls", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Willothewisp : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Willothewisp()
        {
            UnitName = "Will-o'-the-wisp";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.fire;
            AttackMessage = "casts a basic fire spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("fairy_dust", 25), new Tuple<string, int>("burnt_ash", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Naiad : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Naiad()
        {
            UnitName = "Naiad";
            OffensiveElement = CEnums.Element.water;
            DefensiveElement = CEnums.Element.water;
            AttackMessage = "casts a basic water spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("fairy_dust", 25), new Tuple<string, int>("water_vial", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Necromancer : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Necromancer()
        {
            UnitName = "Necromancer";
            OffensiveElement = CEnums.Element.dark;
            DefensiveElement = CEnums.Element.dark;
            AttackMessage = "casts a basic dark spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ripped_cloth", 25), new Tuple<string, int>("demonic_essence", 25) };
            MonsterGroup = CEnums.MonsterGroup.dungeon;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class CorruptThaumaturge : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public CorruptThaumaturge()
        {
            UnitName = "Corrupt Thaumaturge";
            OffensiveElement = CEnums.Element.ice;
            DefensiveElement = CEnums.Element.ice;
            AttackMessage = "casts a basic ice spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ripped_cloth", 25), new Tuple<string, int>("runestone", 25) };
            MonsterGroup = CEnums.MonsterGroup.dungeon;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Imp : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Imp()
        {
            UnitName = "Imp";
            OffensiveElement = CEnums.Element.fire;
            DefensiveElement = CEnums.Element.neutral;
            AttackMessage = "casts a basic fire spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("wing_piece", 25), new Tuple<string, int>("fairy_dust", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Spriggan : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Spriggan()
        {
            UnitName = "Spriggan";
            OffensiveElement = CEnums.Element.grass;
            DefensiveElement = CEnums.Element.grass;
            AttackMessage = "casts a basic grass spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("fairy_dust", 25), new Tuple<string, int>("fairy_dust", 25) };
            MonsterGroup = CEnums.MonsterGroup.humanoid;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class Alicorn : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public Alicorn()
        {
            UnitName = "Alicorn";
            OffensiveElement = CEnums.Element.light;
            DefensiveElement = CEnums.Element.light;
            AttackMessage = "casts a basic light spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("unicorn_horn", 25), new Tuple<string, int>("angelic_essence", 25) };
            MonsterGroup = CEnums.MonsterGroup.monster;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class WindWraith : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public WindWraith()
        {
            UnitName = "Wind Wraith";
            OffensiveElement = CEnums.Element.wind;
            DefensiveElement = CEnums.Element.wind;
            AttackMessage = "casts a basic wind spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ectoplasm", 25), new Tuple<string, int>("demonic_essence", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }

    internal sealed class LightningGhost : MagicMonster
    {
        public override void UponDefeating()
        {

        }

        public LightningGhost()
        {
            UnitName = "Lightning Ghost";
            OffensiveElement = CEnums.Element.electric;
            DefensiveElement = CEnums.Element.electric;
            AttackMessage = "casts a basic electric spell on";
            DropList = new List<Tuple<string, int>>() { new Tuple<string, int>("ectoplasm", 25), new Tuple<string, int>("demonic_essence", 25) };
            MonsterGroup = CEnums.MonsterGroup.undead;

            SpeciesMultipliers = new Dictionary<string, double>()
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
    }
    #endregion
}
