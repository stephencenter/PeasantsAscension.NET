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

namespace Game
{
    public static class BattleManager
    {
        /* =========================== *
         *            FIELDS           *
         * =========================== */
        private static int turn_counter;
        private static List<Monster> monster_list = new List<Monster>();

        /* =========================== *
         *     GETTERS AND SETTERS     *
         * =========================== */
        public static int GetTurnCounter()
        {
            return turn_counter;
        }

        public static List<Monster> GetMonsterList()
        {
            return monster_list;
        }

        public static void SetMonsterList(List<Monster> new_list)
        {
            monster_list = new_list;
        }

        /* =========================== *
         *        BATTLE SYSTEM        *
         * =========================== */
        public static void BattleSystem(bool is_bossfight)
        {
            Random rng = new Random();
            GameLoopManager.Gamestate = CEnums.GameState.battle;
            List<PlayableCharacter> active_pcus = UnitManager.GetActivePCUs();
            turn_counter = 0;

            // Alert the player that a battle has begun
            if (is_bossfight)
            {
                Console.WriteLine($"The legendary {monster_list[0].UnitName} has awoken!");
                MusicPlayer.PlaySong(SoundManager.battle_music_boss, -1);
            }

            else
            {
                if (monster_list.Count == 1)
                {
                    Console.WriteLine($"A {monster_list[0].UnitName} suddenly appeared out of nowhere!");
                }

                else if (monster_list.Count == 2)
                {
                    Console.WriteLine($"A {monster_list[0].UnitName} and 1 other monster suddenly appeared out of nowhere!");
                }

                else if (monster_list.Count > 2)
                {
                    Console.WriteLine($"A {monster_list[0].UnitName} and {monster_list.Count - 1} other monsters suddenly appeared out of nowhere!");
                }

                MusicPlayer.PlaySong(monster_list[0].MonsterGroup.GetMonsterSong(), -1);
            }

            CMethods.SmartSleep(500);

            // Create a temporary copy of all of the player's stats. These copies are what will be modified in-battle by 
            // spells, abilities, etc. so that they will return to normal after battle (although they in fact were never 
            // touched to begin with)
            active_pcus.ForEach(x => x.SetTempStats());
            monster_list.ForEach(x => x.SetTempStats());

            // While all active party members are alive, continue the battle
            while (monster_list.Any(x => x.HP > 0) && active_pcus.Any(x => x.HP > 0))
            {
                turn_counter++;

                // Display the stats for every battle participant
                DisplayBattleStats(active_pcus);

                // Iterate through each active players
                foreach (PlayableCharacter character in UnitManager.GetAliveActivePCUs())
                {
                    if (0 < character.HP && character.HP <= character.TempStats.MaxHP * 0.20)
                    {
                        Console.WriteLine($"Warning: {character.UnitName}'s HP is low, heal as soon as possible!");
                        SoundManager.health_low.SmartPlay();
                        CMethods.SmartSleep(1333);
                    }

                    character.ChooseBattleAction();

                    if (character != UnitManager.GetAliveActivePCUs().Last())
                    {
                        CMethods.PrintDivider();
                    }
                }

                // Create a list of all units in the battle
                List<Unit> speed_list = new List<Unit>();
                active_pcus.ForEach(speed_list.Add);
                monster_list.ForEach(speed_list.Add);

                // Sort this list by 
                speed_list = speed_list.OrderByDescending(
                    x => x.HasStatus(CEnums.Status.paralyzation) ? x.TempStats.Speed / 2 : x.TempStats.Speed
                ).ToList();

                // Iterate through each unit in the battle from fastest to slowest
                foreach (Unit unit in speed_list)
                {
                    if (unit.IsAlive())
                    {
                        if (monster_list.All(x => x.HP <= 0) || active_pcus.All(x => x.HP <= 0))
                        {
                            break;
                        }

                        CMethods.PrintDivider();

                        // Leave the battle if the player runs away
                        if (unit is PlayableCharacter pcu)
                        {
                            if (!pcu.ExecuteBattleAction())
                            {
                                return;
                            }
                        }

                        else if (unit is Monster monster)
                        {
                            monster.MonsterExecuteMove();
                        }
                    }

                    // If any unit died on this turn, set their health to 0 and set their status as 'dead'
                    foreach (Unit other_unit in speed_list)
                    {
                        if (other_unit is PlayableCharacter && other_unit.HP <= 0 && other_unit.IsAlive())
                        {
                            other_unit.FixAllStats();
                            CMethods.SmartSleep(250);
                            SoundManager.ally_death.SmartPlay();

                            Console.WriteLine($"{other_unit.UnitName} has fallen to the monsters!");
                        }

                        else if (other_unit is Monster && other_unit.HP <= 0 && other_unit.IsAlive())
                        {
                            other_unit.FixAllStats();
                            CMethods.SmartSleep(250);
                            SoundManager.enemy_death.SmartPlay();

                            Console.WriteLine($"The {other_unit.UnitName} was defeated!");
                        }
                    }

                    if (monster_list.Any(x => x.HP > 0) && unit.HP > 0)
                    {
                        CMethods.PressAnyKeyToContinue();
                    }
                }
            }

            // Determine the results of the battle and react accordingly
            AfterBattle(active_pcus, is_bossfight);
        }

        public static void AfterBattle(List<PlayableCharacter> active_pcus, bool is_bossfight)
        {
            CMethods.PrintDivider();

            foreach(PlayableCharacter pcu in active_pcus)
            {
                pcu.FixAllStats();
            }

            foreach(Monster monster in monster_list)
            {
                monster.FixAllStats();
            }

            if (active_pcus.Any(x => x.IsAlive()))
            {
                MusicPlayer.PlaySong(SoundManager.victory_music, -1);

                if (is_bossfight)
                {
                    Console.WriteLine($"The mighty {monster_list[0].UnitName} has been slain!\n");
                    // CInfo.DefeatedBosses.Add(monster_list[0].PlayerID);
                    monster_list[0].UponDefeating();
                }

                else
                {
                    Console.WriteLine($"The {monster_list[0].UnitName} falls to the ground dead as a stone.\n");
                }

                DetermineLoot(active_pcus);

                bool leveled_up = false;
                foreach (PlayableCharacter pcu in active_pcus)
                {
                    if (pcu.CheckForLevelUp())
                    {
                        leveled_up = true;
                    }
                }

                if (leveled_up)
                {
                    CMethods.PrintDivider();
                    SavefileManager.WouldYouLikeToSave();
                }

                SoundManager.PlayCellMusic();
            }

            else
            {
                MusicPlayer.PlaySong(SoundManager.gameover_music, -1);

                Console.WriteLine($"Despite your best efforts, the {monster_list[0].UnitName} has killed your party.");
                CMethods.PrintDivider();

                bool auto_yes = GameLoopManager.AutoPlay;
                while (true)
                {
                    string y_n = auto_yes ? "y" : CMethods.SingleCharInput("Do you wish to continue playing? ");

                    if (y_n.IsYesString())
                    {
                        CInfo.CurrentTile = CInfo.RespawnTile;
                        UnitManager.HealAllPCUs(true, true, true, true);
                        SoundManager.PlayCellMusic();

                        return;
                    }

                    else if (y_n.IsNoString())
                    {
                        while (true)
                        {
                            string y_n2 = CMethods.SingleCharInput("Are you sure you want to quit? ");

                            if (y_n2.IsYesString())
                            {
                                auto_yes = true;
                                break;
                            }

                            else if (y_n2.IsNoString())
                            {
                                Environment.Exit(0);
                            }
                        }
                    }
                }
            }
        }

        public static void DetermineLoot(List<PlayableCharacter> active_pcus)
        {
            // Calculate XP drops
            int expr_drops = 0;
            foreach (Monster monster in monster_list)
            {
                expr_drops += Math.Max(Math.Max(1, monster.DroppedXP), (int)Math.Pow(1.5, monster.Level));
            }

            foreach (PlayableCharacter pcu in active_pcus)
            {
                pcu.CurrentXP += expr_drops;
                Console.WriteLine($"{pcu.UnitName} gained {expr_drops} XP");
            }

            // Calculate gold drops
            foreach (Monster monster in monster_list)
            {
                int gold = Math.Max(Math.Max(1, monster.DroppedGold), 2 * monster.Level);
                Console.WriteLine($"The {monster.UnitName} dropped {gold} GP");
                CInfo.GP += gold;
            }

            // Get all monster drops
            foreach (Monster monster in monster_list)
            {
                if (monster.DroppedItem != null || monster.MonsterSetDroppedItem())
                {
                    Console.WriteLine($"The {monster.UnitName} dropped a {ItemManager.FindItemWithID(monster.DroppedItem).ItemName}");
                    InventoryManager.AddItemToInventory(monster.DroppedItem);
                }
            }

            CMethods.PressAnyKeyToContinue();
        }

        public static bool TryToRunAway(Unit runner)
        {
            Random rng = new Random();

            Console.WriteLine($"Your party tries to make a run for it...");
            SoundManager.foot_steps.SmartPlay();
            CMethods.SmartSleep(750);

            int chance;
            int s_monster = monster_list.Max(x => x.TempStats.Speed);
            int e_monster = monster_list.Max(x => x.TempStats.Evasion);

            // Running has a 30% chance of success if the runner is paralyzed
            if (runner.HasStatus(CEnums.Status.paralyzation))
            {
                chance = 30;
            }

            // Running has a 70% chance of success if the runner:
            //     1. Has a higher speed than the fastest monster, but a lower evasion than the most evasive monster
            //     2. Has a higher evasion than the most evasive monster, but a lower speed than the fastest monster
            else if ((runner.TempStats.Speed > s_monster) != (runner.TempStats.Evasion > e_monster))
            {
                chance = 70;
            }

            // Running has an 90% chance of success if the runner is both:
            //    1. Faster than the fastest monster
            //    2. More evasive than the most evasive monster
            else if ((runner.TempStats.Speed > s_monster) && (runner.TempStats.Evasion > e_monster))
            {
                chance = 90;
            }

            // In all other scenarios, running has a 50% chance to succeed
            else
            {
                chance = 50;
            }

            if (rng.Next(0, 100) < chance)
            {
                SoundManager.buff_spell.SmartPlay();
                Console.WriteLine("Your party managed to escape!");
                CMethods.PressAnyKeyToContinue();

                return true;
            }

            else
            {
                SoundManager.debuff.SmartPlay();
                Console.WriteLine("Your party's escape attempt failed!");
                return false;
            }
        }

        public static bool BattlePickItem(PlayableCharacter pcu)
        {

            // The player can use items from the Consumables category of their inventory during battles
            while (true)
            {
                List<string> item_ids = InventoryManager.DisplayInventory(CEnums.InvCategory.consumables, false);
                List<Item> consumables = item_ids.Select(ItemManager.FindItemWithID).ToList();

                while (true)
                {
                    string chosen = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", consumables.Count).ToLower();

                    try
                    {
                        pcu.CurrentItem = consumables[int.Parse(chosen) - 1] as Consumable;
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

                    if (ItemManager.ConsumableTargetMenu(pcu, pcu.CurrentItem))
                    {
                        return true;
                    }

                    break;
                }
            }
        }

        public static void DisplayTeamStats(List<Unit> unit_list)
        {
            int player_pad1 = unit_list.Max(x => x.UnitName.Length);
            int player_pad2 = unit_list.Max(x => $"{x.HP}/{x.TempStats.MaxHP} HP".Length);
            int player_pad3 = unit_list.Max(x => $"{x.MP}/{x.TempStats.MaxMP} MP".Length);

            foreach (Unit unit in unit_list)
            {
                string pad1 = new string(' ', player_pad1 - unit.UnitName.Length);
                string pad2 = new string(' ', player_pad2 - $"{unit.HP}/{unit.TempStats.MaxHP} HP".Length);
                string pad3 = new string(' ', player_pad3 - $"{unit.MP}/{unit.TempStats.MaxMP} MP".Length);

                string status_list = "";
                foreach (CEnums.Status status in unit.Statuses)
                {
                    if (string.IsNullOrEmpty(status_list))
                    {
                        status_list = status.EnumToString();
                    }

                    else
                    {
                        status_list = string.Join(", ", new List<string>() { status_list, status.EnumToString() });
                    }
                }

                Console.WriteLine($"  {unit.UnitName}{pad1} | {unit.HP}/{unit.TempStats.MaxHP} HP {pad2}| {unit.MP}/{unit.TempStats.MaxMP} MP {pad3}| LVL: {unit.Level} | STATUS: {status_list}");
            }
        }

        public static void DisplayBattleStats(List<PlayableCharacter> active_pcus)
        {
            foreach (PlayableCharacter unit in active_pcus)
            {
                unit.FixAllStats();
            }

            foreach(Monster unit in monster_list)
            {
                unit.FixAllStats();
            }

            CMethods.PrintDivider();

            Console.WriteLine("Your party: ");
            DisplayTeamStats(active_pcus.Cast<Unit>().ToList());

            Console.WriteLine("\nEnemy team: ");
            DisplayTeamStats(monster_list.Cast<Unit>().ToList());

            CMethods.PrintDivider();
        }
    }

    public static class BattleCalculator
    {
        public static int CalculateSpellDamage(Unit attacker, Unit target, AttackSpell spell = null)
        {
            // Damage is amplified by Charisma and Magic Attack, and is reduced by Magic Defense and armor
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
                spell_power = Math.Min((double)attacker.Level / 50, 1);
            }

            double armor_resist;
            if (target is PlayableCharacter pcu_target)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu_target.PlayerID][CEnums.EquipmentType.armor] as Armor;
                armor_resist = pcu_armor.GetEffectiveResist(pcu_target);
            }

            else
            {
                armor_resist = Math.Min((double)target.Level / 50, 1);
            }

            int final_damage = (int)((attacker.TempStats.MAttack - (target.TempStats.MDefense / 2)) * (1 + spell_power) / (1 + armor_resist));
            final_damage = CheckForCrit(final_damage);
            final_damage = ApplyElementalChart(spell.OffensiveElement, target.DefensiveElement, final_damage);

            return (int)CMethods.Clamp(final_damage, 1, 999);
        }

        public static int CalculateAttackDamage(Unit attacker, Unit target, CEnums.DamageType damage_type)
        {
            // Damage is amplified by Weapon Power and damage type attack, and is reduced the damage type defense and armor
            double weapon_power;
            if (attacker is PlayableCharacter pcu_attacker)
            {
                weapon_power = (InventoryManager.GetEquipmentItems()[pcu_attacker.PlayerID][CEnums.EquipmentType.weapon] as Weapon).Power;
            }

            else
            {
                weapon_power = Math.Min((double)attacker.Level / 50, 1);
            }

            double armor_resist;
            if (target is PlayableCharacter pcu_target)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu_target.PlayerID][CEnums.EquipmentType.armor] as Armor;
                armor_resist = pcu_armor.GetEffectiveResist(pcu_target);
            }

            else
            {
                armor_resist = Math.Min((double)target.Level / 50, 1);
            }

            int final_damage;
            if (damage_type == CEnums.DamageType.physical)
            {
                final_damage = (int)((attacker.TempStats.Attack - (target.TempStats.Defense / 2)) * (1 + weapon_power) / (1 + armor_resist));
                final_damage = CheckForWeakness(attacker, final_damage);
            }

            else if (damage_type == CEnums.DamageType.piercing)
            {
                final_damage = (int)((attacker.TempStats.PAttack - (target.TempStats.PDefense / 2)) * (1 + weapon_power) / (1 + armor_resist));
                final_damage = CheckForBlindness(attacker, final_damage);
            }

            else
            {
                final_damage = (int)((attacker.TempStats.MAttack - (target.TempStats.MDefense / 2)) * (1 + weapon_power) / (1 + armor_resist));
            }

            final_damage = CheckForCrit(final_damage);
            final_damage = ApplyElementalChart(attacker.OffensiveElement, target.DefensiveElement, final_damage);
            return (int)CMethods.Clamp(final_damage, 1, 999);
        }

        public static int CalculateRawDamage(int base_damage, CEnums.Element attacker_element, Unit target, CEnums.DamageType damage_type)
        {
            // Damage is amplified by nothing and is reduced by the given damage type's defense stat and armor
            double armor_resist;
            if (target is PlayableCharacter pcu_target)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu_target.PlayerID][CEnums.EquipmentType.armor] as Armor;
                armor_resist = pcu_armor.GetEffectiveResist(pcu_target);
            }

            else
            {
                armor_resist = Math.Min((double)target.Level / 50, 1);
            }

            int final_damage;
            if (damage_type == CEnums.DamageType.physical)
            {
                final_damage = (int)((base_damage - (target.TempStats.Defense / 2)) / (1 + armor_resist));
            }

            else if (damage_type == CEnums.DamageType.piercing)
            {
                final_damage = (int)((base_damage - (target.TempStats.PDefense / 2)) / (1 + armor_resist));
            }

            else
            {
                final_damage = (int)((base_damage - (target.TempStats.MDefense / 2)) / (1 + armor_resist));
            }

            final_damage = ApplyElementalChart(attacker_element, target.DefensiveElement, final_damage);
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
            // Roll a dice to see whether or not an attack hits based on the target's evasion
            Random rng = new Random();
            int target_evasion = target.TempStats.Evasion;

            if (target is PlayableCharacter pcu)
            {
                Armor pcu_armor = InventoryManager.GetEquipmentItems()[pcu.PlayerID][CEnums.EquipmentType.armor] as Armor;
                target_evasion -= (int)(512 * pcu_armor.GetEffectivePenalty(pcu));
            }

            return target_evasion < rng.Next(0, 512);
        }

        public static int CheckForCrit(int damage)
        {
            Random rng = new Random();

            if (rng.Next(0, 100) < 15)
            {
                damage = (int)(damage * 1.5);
                SoundManager.critical_hit.SmartPlay();
                Console.WriteLine("It's a critical hit! 1.5x damage!");

                CMethods.SmartSleep(500);
            }

            return damage;
        }

        public static int CheckForWeakness(Unit attacker, int damage)
        {
            // Weakeness reduces physical damage by 1/2
            if (attacker.HasStatus(CEnums.Status.weakness))
            {
                damage /= 2;
                Console.WriteLine($"{attacker.UnitName}'s weakness reduces their attack damage by half!");
            }

            return damage;
        }

        public static int CheckForBlindness(Unit attacker, int damage)
        {
            // Blindness reduces piercing damage by 1/2
            if (attacker.HasStatus(CEnums.Status.blindness))
            {
                damage /= 2;
                Console.WriteLine($"{attacker.UnitName}'s blindness reduces their attack damage by half!");
            }

            return damage;
        }
    }
}
