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

using System.Collections.Generic;
using System;
using System.Linq;

namespace Engine
{
    public static class SpellManager
    {
        private static readonly List<AttackSpell> attack_spellbook = new List<AttackSpell>()
        {
            // Neutral
            new AttackSpell("Magic Missile", "Hurl a blast of magical energy at your enemies!",
                3, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, CEnums.Element.neutral),
            
            // Light
            new AttackSpell("Smite", "Strike down unholy beings using His Divinity's power",
                 3, 3, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.paladin }, CEnums.Element.light),

            // Dark
            new AttackSpell("Desecrate", "Call upon His Wickedness to defile holy spirits!",
                 3, 5, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.assassin }, CEnums.Element.dark),

            // Fire
            new AttackSpell("Flame Bolt", "Summon a powerful blast of fire to destroy your foes",
                3, 7, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.fire),

            // Grass
            new AttackSpell("Leaf Blade", "Summon razor-sharp blades of grass to destroy your foes!",
                3, 9, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.grass),

            // Electricity
            new AttackSpell("Spark", "Summon a powerful jolt of electricity to destroy your foes",
                3, 11, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.electric),

            // Water
            new AttackSpell("Water Blast", "Summon a large burst of water to destroy your foes",
                3, 13, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.water),

            // Earth
            new AttackSpell("Rock Slam", "Crush your enemies under a layer of solid rock!",
                3, 15, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.earth),

            // Ice
            new AttackSpell("Icicle Dagger", "Hurl a volley of supercooled icicles at your enemies!",
                3, 17, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.ice),

            // Wind
            new AttackSpell("Microburst", "Batter your enemies with powerful gusts and winds!",
                3, 19, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, CEnums.Element.wind)
        };

        private static readonly List<Spell> healing_spellbook = new List<Spell>()
        {
            new HealingSpell("Novice Healing", "Restore 20 HP or 10% of an ally's max HP, whichever is greater.",
                2, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 10, 0.05),

            new HealingSpell("Resolute Healing", "Restore 75 HP or 50% of the target's max HP, whichever is greater.",
                8, 10, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage }, 50, 0.25),

            new HealingSpell("Divine Healing", "Fully restores an ally's HP.",
                16, 20, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage }, 0, 1),

            new RandomStatusRemovalSpell("Simple Remedy", "Relieves an ally of a single random status effect.",
                5, 6, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }),

            new FullStatusRemovalSpell("Full Remedy", "Relieves an ally of all status effects.",
                15, 15, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }),

            new ReviveSpell("Resurrect", "Brings an ally back to life with 1 HP.",
                10, 9, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage })
        };

        private static readonly List<BuffSpell> buff_spellbook = new List<BuffSpell>()
        {
            new BuffSpell("Improve Quickness", 
@"Raise an ally's speed by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 2, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.assassin }, 0.15, "speed"),

            new BuffSpell("Enhance Elusiveness", 
@"Raise an ally's evasion by 15%. Stacks with multiple uses. Lasts until the end
of battle. Evasion has a cap of 256 (50% chance to dodge).",
                3, 4, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.ranger }, 0.15, "evasion"),

            // Defense Buffs
            new BuffSpell("Bolster Defenses", 
@"Raise an ally's Physical Defense by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 6, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.paladin }, 0.15, "defense"),

            new BuffSpell("Reinforce Shield",
@"Raise an ally's Magical Defense by 15%. Stacks with multiple uses. Lasts until the end
of battle.",
                3, 8, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.paladin }, 0.15, "m_defense"),

            new BuffSpell("Upgrade Block", 
@"Raise an ally's Pierce Defense by 15%. Stacks with multiple uses. Lasts until
the end of battle.", 
                3, 10, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.paladin }, 0.15, "p_defense"),

            // Attack Buffs
            new BuffSpell("Boost Strength", 
@"Raise an ally's Physical Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 12, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.warrior }, 0.15, "attack"),

            new BuffSpell("Empower Sorcery", 
@"Raise an ally's Magical Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 14, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.15, "m_attack"),

            new BuffSpell("Assist Aim",
@"Raise an ally's Pierce Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 16, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.ranger }, 0.15, "p_attack"),
        };

        public static List<Spell> GetFullSpellbook(CEnums.SpellCategory spell_category)
        {
            if (spell_category == CEnums.SpellCategory.attack)
            {
                return attack_spellbook.Select(x => x as Spell).ToList();
            }

            else if (spell_category == CEnums.SpellCategory.healing)
            {
                return healing_spellbook.Select(x => x as Spell).ToList();
            }

            else if (spell_category == CEnums.SpellCategory.buff)
            {
                return buff_spellbook.Select(x => x as Spell).ToList();
            }

            else
            {
                return attack_spellbook.Select(x => x as Spell)
                    .Concat(healing_spellbook)
                    .Concat(buff_spellbook.Select(x => x as Spell)).ToList();
            }
        }

        public static List<Spell> GetCasterSpellbook(PlayableCharacter caster, CEnums.SpellCategory spell_category)
        {
            if (spell_category == CEnums.SpellCategory.attack)
            {
                return GetOnlyAllowedSpells(caster, attack_spellbook);
            }

            else if (spell_category == CEnums.SpellCategory.healing)
            {
                return GetOnlyAllowedSpells(caster, healing_spellbook);
            }

            else if (spell_category == CEnums.SpellCategory.buff)
            {
                return GetOnlyAllowedSpells(caster, buff_spellbook);
            }

            else
            {
                return GetOnlyAllowedSpells(caster, attack_spellbook)
                    .Concat(GetOnlyAllowedSpells(caster, healing_spellbook))
                    .Concat(GetOnlyAllowedSpells(caster, buff_spellbook)).ToList();
            }
        }

        private static List<Spell> GetOnlyAllowedSpells<T>(PlayableCharacter caster, List<T> spell_list)
        {
            List<Spell> true_spell_list = spell_list.Select(x => x as Spell).ToList();

            return true_spell_list
                .Where(x => x.RequiredLevel <= caster.Level
                    && (x.AllowedClasses.Contains(caster.PClass) || x.AllowedClasses.Contains(CEnums.CharacterClass.any))).ToList();
        }

        public static bool PickSpellCategory(PlayableCharacter caster, List<Monster> monster_list)
        {
            while (true)
            {
                Console.WriteLine($"{caster.UnitName}'s Spellbook:");
                Console.WriteLine("      [1] Attack Spells");
                Console.WriteLine("      [2] Healing Spells");
                Console.WriteLine("      [3] Buff Spells");

                if (caster.CurrentSpell != null)
                {
                    Console.WriteLine($"      [4] Re-cast {caster.CurrentSpell.SpellName}");
                }

                while (true)
                {
                    string category = CMethods.SingleCharInput("Input [#] (or type 'exit'): ");
                    CEnums.SpellCategory true_category;

                    if (category.IsExitString())
                    {
                        CMethods.PrintDivider();
                        return false;
                    }

                    else if (category == "1")
                    {
                        true_category = CEnums.SpellCategory.attack;
                    }

                    else if (category == "2")
                    {
                        true_category = CEnums.SpellCategory.healing;
                    }

                    else if (category == "3")
                    {
                        true_category = CEnums.SpellCategory.buff;
                    }

                    else if (category == "4" && caster.CurrentSpell != null)
                    {
                        if (SpellTargetMenu(caster, monster_list, caster.CurrentSpell))
                        {
                            // caster.CurrentSpell is already set to the desired spell in this scenario,
                            // so we can just return right away
                            return true;
                        }

                        break;
                    }

                    else
                    {
                        continue;
                    }

                    if (GetCasterSpellbook(caster, true_category).Count == 0)
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"{caster.UnitName} doesn't know any {true_category.EnumToString()} spells!");
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();
                        break;
                    }

                    if (PickSpell(caster, true_category, monster_list))
                    {
                        return true;
                    }

                    break;
                }   
            }
        }

        public static bool PickSpell(PlayableCharacter caster, CEnums.SpellCategory category, List<Monster> monster_list)
        {
            List<Spell> chosen_spellbook = GetCasterSpellbook(caster, category).ToList();
            CMethods.PrintDivider();

            while (true)
            {
                int padding = chosen_spellbook.Max(x => x.SpellName.Length);
                Console.WriteLine($"{caster.UnitName}'s {category.EnumToString()} Spells | {caster.MP}/{caster.MaxMP} MP remaining");

                foreach (Tuple<int, Spell> element in CMethods.Enumerate(chosen_spellbook))
                {
                    string pad = new string('-', padding - element.Item2.SpellName.Length);
                    Console.WriteLine($"      [{element.Item1 + 1}] {element.Item2.SpellName} {pad}-> {element.Item2.ManaCost} MP");
                }

                while (true)
                {
                    string chosen_spell = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", chosen_spellbook.Count);

                    try
                    {
                        caster.CurrentSpell = chosen_spellbook[int.Parse(chosen_spell) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (chosen_spell.IsExitString())
                        {
                            CMethods.PrintDivider();
                            return false;
                        }

                        continue;
                    }

                    // Of course, you can't cast spells without the required amount of MP
                    if (caster.CurrentSpell.ManaCost > caster.MP)
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"{caster.UnitName} doesn't have enough MP to cast {caster.CurrentSpell.SpellName}!");
                        CMethods.PressAnyKeyToContinue();

                        break;
                    }
                    
                    if (SpellTargetMenu(caster, monster_list, caster.CurrentSpell)) 
                    {
                        return true;
                    }
                            
                    break;
                }
            }                    
        }

        public static bool SpellTargetMenu(PlayableCharacter caster, List<Monster> m_list, Spell spell)
        {
            string action_desc = $@"{spell.SpellName}: 
{spell.Description}

Who should {caster.UnitName} cast {spell.SpellName} on?";

            return caster.PlayerChooseTarget(m_list, action_desc, spell.TargetAllies, spell.TargetEnemies, spell.TargetDead, false);
        }
    }

    public abstract class Spell
    {
        public string SpellName { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
        public int RequiredLevel { get; set; }
        public List<CEnums.CharacterClass> AllowedClasses { get; set; }

        public bool TargetAllies { get; set; }
        public bool TargetEnemies { get; set; }
        public bool TargetDead { get; set; }

        public abstract void UseMagic(PlayableCharacter user);

        public void SpendMana(PlayableCharacter user)
        {
            user.MP -= ManaCost;
            user.FixAllStats();
        }

        protected Spell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, bool allies, bool enemies, bool dead)
        {
            SpellName = spell_name;
            Description = desc;
            ManaCost = mana;
            RequiredLevel = req_lvl;
            AllowedClasses = classes;
            TargetAllies = allies;
            TargetEnemies = enemies;
            TargetDead = dead;
        }
    }

    public class HealingSpell : Spell
    {
        // Healing spells will always restore a minimum of target.HP*HealthIncreasePercent.
        // e.g. A spell that heals 20 HP but has a 20% HIP will restore 20 HP for someone
        // with 45 max HP, but will restore 32 HP for someone with 160 max HP.
        // In addition to this, the target restores an additional 1 HP for every point of wisdom the user has
        public int HealthIncreaseFlat { get; set; }
        public double HealthIncreasePercent { get; set; }

        public override void UseMagic(PlayableCharacter user)
        {
            SpendMana(user);
            Unit target = user.CurrentTarget;

            int total_heal;

            if (HealthIncreaseFlat < target.MaxHP * HealthIncreasePercent)
            {
                total_heal = (int)((target.MaxHP * HealthIncreasePercent) + user.Attributes[CEnums.PlayerAttribute.wisdom]);
            }

            else
            {
                total_heal = HealthIncreaseFlat + user.Attributes[CEnums.PlayerAttribute.wisdom];
            }

            target.HP += total_heal;
            target.FixAllStats();

            if (CInfo.Gamestate == CEnums.GameState.battle)
            {
                Console.WriteLine($"{user.UnitName} is preparing to cast {SpellName}...");

                SoundManager.ability_cast.SmartPlay();
                CMethods.SmartSleep(750);

                Console.WriteLine($"Using {SpellName}, {target.UnitName} is healed by {total_heal} HP!");
                SoundManager.magic_healing.SmartPlay();
            }

            else
            {
                Console.WriteLine($"Using {SpellName}, {target.UnitName} is healed by {total_heal} HP!");
                SoundManager.magic_healing.SmartPlay();
                CMethods.PressAnyKeyToContinue();

                CMethods.PrintDivider();
            }
        }

        public HealingSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, int hp_flat, double hp_perc) :
            base(spell_name, desc, mana, req_lvl, classes, true, false, false)
        {
            HealthIncreaseFlat = hp_flat;
            HealthIncreasePercent = hp_perc;
        }
    }

    public class AttackSpell : Spell
    {
        // Damaging spells are spells that deal damage to the enemy during battle.
        // Just like normal attacks, they have a chance to miss based on
        // the enemy's evasion stat.
        public CEnums.Element OffensiveElement { get; set; }

        public override void UseMagic(PlayableCharacter user)
        {
            SpendMana(user);
            Unit target = user.CurrentTarget;
            Random rng = new Random();
            
            Console.WriteLine($"{user.UnitName} casts {SpellName} on the {target.UnitName}...");
            SoundManager.magic_attack.SmartPlay();
            CMethods.SmartSleep(750);

            int attack_damage = UnitManager.CalculateDamage(user, target, CEnums.DamageType.magical, spell: this);

            if (UnitManager.DoesAttackHit(target))
            {
                SoundManager.enemy_hit.SmartPlay();
                target.HP -= attack_damage;
                Console.WriteLine($"{user.UnitName}'s spell hits the {target.UnitName}, dealing {attack_damage} damage!");
            }

            else
            {
                SoundManager.attack_miss.SmartPlay();
                Console.WriteLine($"The {target.UnitName} narrowly dodges {user.UnitName}'s spell!");
            }
        }

        public AttackSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, CEnums.Element element) : 
            base(spell_name, desc, mana, req_lvl, classes, false, true, false)
        {
            OffensiveElement = element;
        }
    }

    public class BuffSpell : Spell
    {
        // Buffs are spells that temporarily raise the player's stats during battle. They last until the battle
        // is over, at which point the player's stats will return to normal.
        public double IncreaseAmount { get; set; }
        public string Stat { get; set; }

        public override void UseMagic(PlayableCharacter user)
        {
            SpendMana(user);
            Unit target = user.CurrentTarget;

            Console.WriteLine($"{user.UnitName} is preparing to cast {SpellName}...");
            SoundManager.ability_cast.SmartPlay();
            CMethods.SmartSleep(750);

            if (target == user)
            {
                Console.WriteLine($"{user.UnitName} raises their stats using the power of {SpellName}!");
            }

            else
            {
                Console.WriteLine($"{user.UnitName} raises {target.UnitName}'s stats using the power of {SpellName}!");
            }

            SoundManager.buff_spell.SmartPlay();

            // We write to TempStats instead of the player's actual stats so that the changes will
            // not persist after battle
            target.TempStats[Stat] = (int)(target.TempStats[Stat] * (1 + IncreaseAmount));
        }

        public BuffSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, double increase, string stat) :
            base(spell_name, desc, mana, req_lvl, classes, true, false, false)
        {
            IncreaseAmount = increase;
            Stat = stat;
        }
    }

    public class RandomStatusRemovalSpell : Spell
    {
        public override void UseMagic(PlayableCharacter user)
        {
            throw new NotImplementedException();
        }

        public RandomStatusRemovalSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes) : 
            base(spell_name, desc, mana, req_lvl, classes, true, false, false)
        {

        }
    }

    public class FullStatusRemovalSpell : Spell
    {
        public override void UseMagic(PlayableCharacter user)
        {
            throw new NotImplementedException();
        }

        public FullStatusRemovalSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes) :
            base(spell_name, desc, mana, req_lvl, classes, true, false, false)
        {

        }
    }

    public class ReviveSpell : Spell
    {
        public override void UseMagic(PlayableCharacter user)
        {
            SpendMana(user);
            Unit target = user.CurrentTarget;

            Console.WriteLine($"{user.UnitName} is preparing to cast {SpellName}...");

            SoundManager.ability_cast.SmartPlay();
            CMethods.SmartSleep(750);

            if (target.IsDead())
            {
                target.HP = 1;
                target.Statuses = new List<CEnums.Status>() { CEnums.Status.alive };
                Console.WriteLine($"Using {SpellName}, {target.UnitName} is brought back from the dead!");
                SoundManager.magic_healing.SmartPlay();
            }

            else
            {
                Console.WriteLine($"...but {target.UnitName} is already alive!");
                SoundManager.debuff.SmartPlay();
            }

            if (CInfo.Gamestate != CEnums.GameState.battle)
            {
                CMethods.PressAnyKeyToContinue();
                CMethods.PrintDivider();
            }
        }

        public ReviveSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes) :
            base(spell_name, desc, mana, req_lvl, classes, true, false, true)
        {

        }
    }
}