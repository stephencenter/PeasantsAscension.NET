using System.Collections.Generic;
using System;
using System.Linq;

namespace Data
{
    public static class SpellManager
    {
        private static readonly List<AttackSpell> attack_spellbook = new List<AttackSpell>()
        {
            // Neutral
            new AttackSpell("Magical Shot", "Hurl a small ball of magical energy at your enemies! (25% Spell Power)",
                3, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.25, CEnums.Element.neutral, "no_elem_1"),

            new AttackSpell("Magical Burst", "Shatter your enemy with a wave of magical energy! (50% Spell Power)",
                9, 11, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 0.5, CEnums.Element.neutral, "no_elem_2"),

            new AttackSpell("Magical Blast", "Annihilate your enemies with a blast of magical energy! (100% Spell Power)",
                18, 23, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.neutral, "no_elem_3"),

            // Fire
            new AttackSpell("Flame Bolt", "Summon a small fireball to destroy your foes! (25% Spell Power)",
                3, 2, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.25, CEnums.Element.fire, "fire_elem_1"),

            new AttackSpell("Fierce Blaze", "Summon a powerful flame to destroy your foes! (50% Spell Power)",
                10, 12, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 0.5, CEnums.Element.fire, "fire_elem_2"),
            
            new AttackSpell("Grand Inferno", "Unleash a monstrous blaze destroy your foes! (100% Spell Power)",
                20, 24, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.fire, "fire_elem_3"),

            // Grass
            new AttackSpell("Leaf Blade", "Summon razor-sharp blades of grass to destroy your foes! (25% Spell Power)",
                3, 2, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any },0.25, CEnums.Element.grass, "grass_elem_1"),

            new AttackSpell("Grass Grenade", "Summon a small explosion to destroy your foes! (50% Spell Power)",
                10, 12, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage },0.5, CEnums.Element.grass, "grass_elem_2"),

            new AttackSpell("Vine Storm", "Unleash a frenzy of powerful vines to destroy your foes! (100% Spell Power)",
                20, 24, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.grass, "grass_elem_3"),

            // Electricity
            new AttackSpell("Spark", "Summon a small spark to destroy your foes! (25% Spell Power)",
                3, 3, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any },0.25, CEnums.Element.electric, "elec_elem_1"),

            new AttackSpell("Powerful Jolt", "Summon a powerful jolt of energy to destroy your foes! (50% Spell Power)",
                10, 13, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage },0.5, CEnums.Element.electric, "elec_elem_2"),

            new AttackSpell("Superior Storm", "Unleash a devastating lightning storm to destroy your foes! (100% Spell Power)",
                20, 25, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.electric, "elec_elem_3"),

            // Water
            new AttackSpell("Drizzle", "Summon a small to destroy your foes! (25% Spell Power)",
                3, 3, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any },0.25, CEnums.Element.water, "water_elem_1"),

            new AttackSpell("Water Blast", "Summon a large burst of water to destroy your foes! (50% Spell Power)",
                10, 13, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage },0.5, CEnums.Element.water, "water_elem_2"),

            new AttackSpell("Tsunami", "Unleash a terrifying barrage of waves upon your foes! (100% Spell Power)",
                20, 25, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.water, "water_elem_3"),

            // Earth
            new AttackSpell("Mud Toss", "Summon a small ball of mud to throw at your foes! (25% Spell Power)",
                3, 4, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any },0.25, CEnums.Element.earth, "earth_elem_1"),

            new AttackSpell("Rock Slam", "Crush your enemies under a layer of solid rock! (50% Spell Power)",
                10, 13, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage },0.5, CEnums.Element.earth, "earth_elem_2"),

            new AttackSpell("Earthquake", "Wreck havoc on your enemies with a powerful earthquake! (100% Spell Power)",
                20, 25, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.earth, "earth_elem_3"),

            // Ice
            new AttackSpell("Icicle Dagger", "Hurl a volley of supercooled icicles at your enemies! (25% Spell Power)",
                3, 4, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any },0.25, CEnums.Element.ice, "ice_elem_1"),

            new AttackSpell("Hailstorm", "Rain ice upon your enemies with unrelenting force! (50% Spell Power)",
                11, 14, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage },0.5, CEnums.Element.ice, "ice_elem_2"),

            new AttackSpell("Blizzard", "Devastate your enemies with a terrifying flurry of ice and wind! (100% Spell Power)",
                23, 26, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.ice, "ice_elem_3"),

            // Wind
            new AttackSpell("Minor Gust", "Batter your enemies with powerful gusts and winds! (25% Spell Power)",
                3, 4, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.25, CEnums.Element.wind, "wind_elem_1"),

            new AttackSpell("Microburst", "Decimate your foes with a powerful blast of wind! (50% Spell Power)",
                11, 14, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 0.5, CEnums.Element.wind, "wind_elem_2"),

            new AttackSpell("Cyclone", "Demolish all that stand in your path with a terrifying tornado! (100% Spell Power)",
                23, 26, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage }, 1, CEnums.Element.wind, "wind_elem_3"),

            // Light
            new AttackSpell("Purify", "Call upon His Divinity to cast out evil creatures! (25% Spell Power)!",
                 3, 5, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.25, CEnums.Element.light, "light_elem_1"),

            new AttackSpell("Holy Smite", "Strike down unholy beings using His Divinity's power! (50% Spell Power)",
                 11, 15, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.paladin }, 0.5, CEnums.Element.light, "light_elem_2"),

            new AttackSpell("Moonbeam", "Utterly destroy evil creatures with holy rays from the moon! (100% Spell Power)",
                 23, 27, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.paladin }, 1, CEnums.Element.light, "light_elem_3"),

            // Dark
            new AttackSpell("Evil Curse", "Call upon His Wickedness to harm holy creatures! (25% Spell Power)",
                 3, 5, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.25, CEnums.Element.dark, "dark_elem_1"),

            new AttackSpell("Desecration", "Defile holy spirits with an evil aura! (50% Spell Power)",
                 11, 15, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.assassin }, 0.5, CEnums.Element.dark, "dark_elem_2"),

            new AttackSpell("Unholy Rend", "Annihilate holy creatures with a sundering blow! (100% Spell Power)",
                 23, 27, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.assassin }, 1, CEnums.Element.dark, "dark_elem_3")
        };

        private static readonly List<Spell> healing_spellbook = new List<Spell>()
        {
            new HealingSpell("Novice Healing",
@"Restore a small amount of an ally's HP using holy magic. Heals 10 HP or 5% of
the target's max HP, whichever is greater.",
                2, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 10, 0.05, "healing_1"),

            new HealingSpell("Adept Healing",
@"Restore a moderate amount of an ally's HP using holy magic. Heals 25 HP or 20%
of the target's max HP, whichever is greater.",
                5, 8, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 25, 0.2, "healing_2"),
            
            // This tier can only be learned by Paladins and Mages
            new HealingSpell("Advanced Healing",
@"Restore a large amount of an ally's HP using holy magic. Heals 70 HP or 50%
of the target's max HP, whichever is greater.",
                10, 20, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage }, 70, 0.5, "healing_3"),

            new HealingSpell("Divine Healing",
@"Restore a very large amount of an ally's HP using holy magic. Heals 125 HP or
75% of the target's max HP, whichever is greater.",
                25, 35, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage }, 125, 0.75, "healing_4"),

            new StatusRemovalSpell("Relieve Afflictions", "Relieves an ally of a single random status effect.",
                5, 7, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, "relieve"),

            new ReviveSpell("Resurrect", "Brings an ally back to life with 1 HP.",
                10, 9, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.paladin, CEnums.CharacterClass.mage }, "revive")
        };

        private static readonly List<BuffSpell> buff_spellbook = new List<BuffSpell>()
        {
            new BuffSpell("Minor Quickness", 
@"Raise an ally's speed by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "speed", "speed_1"),

            new BuffSpell("Minor Evade", 
@"Raise an ally's evasion by 15%. Stacks with multiple uses. Lasts until the end
of battle. Evasion has a cap of 256 (50% chance to dodge).", 
                3, 1, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "evasion", "evad_1"),

            new BuffSpell("Adept Quickness", 
@"Raise an ally's speed by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 10, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "speed", "speed_2"),

            new BuffSpell("Adept Evade", 
@"Raise an ally's evasion by 30%. Stacks with multiple uses. Lasts until the end
of battle. Evasion has a cap of 256 (50% chance to dodge).", 
                6, 10, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "evad", "evad_2"),

            // Defense Buffs
            new BuffSpell("Minor Defend", 
@"Raise an ally's Physical Defense by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 3, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "defense", "defend_1"),

            new BuffSpell("Minor Shield", 
@"Raise an ally's Magical Defense by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 5, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "m_defense", "shield_1"),

            new BuffSpell("Minor Block", 
@"Raise an ally's Pierce Defense by 15%. Stacks with multiple uses. Lasts until
the end of battle.", 
                3, 7, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "p_defense", "block_1"),

            new BuffSpell("Adept Defend", 
@"Raise an ally's Physical Defense by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 14, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "defense", "defend_2"),

            new BuffSpell("Adept Shield", 
@"Raise an ally's Magical Defense by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 16, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "m_defense", "shield_2"),

            new BuffSpell("Adept Block", 
@"Raise an ally's Pierce Defense by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 18, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "p_defense", "block_2"),

            // Attack Buffs
            new BuffSpell("Minor Strengthen", 
@"Raise an ally's Physical Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 2, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "attack", "strength_1"),

            new BuffSpell("Minor Empower", 
@"Raise an ally's Magical Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 4, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "m_attack", "power_1"),

            new BuffSpell("Minor Aim",
@"Raise an ally's Pierce Attack by 15%. Stacks with multiple uses. Lasts until the end
of battle.", 
                3, 6, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.any }, 0.15, "p_attack", "aim_1"),

            new BuffSpell("Adept Strengthen", 
@"Raise an ally's Physical Attack by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 13, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "attack", "strength_2"),

            new BuffSpell("Adept Empower", 
@"Raise an ally's Magical Attack by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 15, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "m_attack", "power_2"),

            new BuffSpell("Adept Aim", 
@"Raise an ally's Pierce Attack by 30%. Stacks with multiple uses. Lasts until the end
of battle.", 
                6, 17, new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, 0.3, "p_attack", "aim_2"),
        };

        public static List<Spell> GetSpellbook(PlayableCharacter caster, CEnums.SpellCategory spell_category)
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
                        if (caster.CurrentSpell is HealingSpell || caster.CurrentSpell is BuffSpell)
                        {
                            caster.PlayerGetTarget(monster_list, $"Who should {caster.UnitName} cast {caster.CurrentSpell.SpellName} on?", true, false, false, false);
                        }

                        else
                        {
                            caster.PlayerGetTarget(monster_list, $"Who should {caster.UnitName} cast {caster.CurrentSpell.SpellName} on?", false, true, false, false);
                        }

                        return true;
                    }

                    else
                    {
                        continue;
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
            List<Spell> chosen_spellbook = GetSpellbook(caster, category).ToList();
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

                    if (CInfo.Gamestate == CEnums.GameState.battle)
                    {
                        if (caster.CurrentSpell is HealingSpell || caster.CurrentSpell is BuffSpell)
                        {
                            if (caster.PlayerGetTarget(monster_list, $"Who should {caster.UnitName} cast {caster.CurrentSpell.SpellName} on?", true, false, false, false))
                            {
                                return true;
                            }
                            
                            break;
                        }

                        else
                        {
                            if (caster.PlayerGetTarget(monster_list, $"Who should {caster.UnitName} cast {caster.CurrentSpell.SpellName} on?", false, true, false, false)) 
                            {
                                return true;
                            }
                            
                            break;
                        }
                    }

                    else
                    {
                        if (caster.PlayerGetTarget(monster_list, $"Who should {caster.UnitName} cast {caster.CurrentSpell.SpellName} on?", true, false, false, false))
                        {
                            caster.CurrentSpell.UseMagic(caster);
                        }

                        break;
                    }
                }
            }                    
        }
    }

    public abstract class Spell
    {
        public string SpellName { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
        public int RequiredLevel { get; set; }
        public List<CEnums.CharacterClass> AllowedClasses { get; set; }
        public string SpellID { get; set; }

        public abstract void UseMagic(PlayableCharacter user);

        public void SpendMana(PlayableCharacter user)
        {
            user.MP -= ManaCost;
            user.FixAllStats();
        }

        protected Spell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, string spell_id)
        {
            SpellName = spell_name;
            Description = desc;
            ManaCost = mana;
            RequiredLevel = req_lvl;
            AllowedClasses = classes;
            SpellID = spell_id;
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
                CMethods.PrintDivider();

                Console.WriteLine($"Using {SpellName}, {target.UnitName} is healed by {total_heal} HP!");
                SoundManager.magic_healing.SmartPlay();
                CMethods.PressAnyKeyToContinue();

                CMethods.PrintDivider();
            }
        }

        public HealingSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, int hp_flat, double hp_perc, string spell_id) : 
            base(spell_name, desc, mana, req_lvl, classes, spell_id)
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
        public double SpellPower { get; set; }
        public CEnums.Element OffensiveElement { get; set; }

        public override void UseMagic(PlayableCharacter user)
        {
            SpendMana(user);
            Unit target = user.CurrentTarget;
            Random rng = new Random();
            
            Console.WriteLine($"{user.UnitName} casts {SpellName} on the {target.UnitName}...");
            SoundManager.magic_attack.SmartPlay();
            CMethods.SmartSleep(750);

            int attack_damage = UnitManager.CalculateDamage(user, target, CEnums.DamageType.magical, spell_power: SpellPower);

            if (target.Evasion < rng.Next(0, 512))
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

        public AttackSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, double spell_power, CEnums.Element element, string spell_id) : 
            base(spell_name, desc, mana, req_lvl, classes, spell_id)
        {
            SpellPower = spell_power;
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

        public BuffSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, double increase, string stat, string spell_id) : 
            base(spell_name, desc, mana, req_lvl, classes, spell_id)
        {
            IncreaseAmount = increase;
            Stat = stat;
        }
    }

    public class StatusRemovalSpell : Spell
    {
        public override void UseMagic(PlayableCharacter user)
        {
            throw new NotImplementedException();
        }

        public StatusRemovalSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, string spell_id) : 
            base(spell_name, desc, mana, req_lvl, classes, spell_id)
        {

        }
    }

    public class ReviveSpell : Spell
    {
        public override void UseMagic(PlayableCharacter user)
        {
            throw new NotImplementedException();
        }

        public ReviveSpell(string spell_name, string desc, int mana, int req_lvl, List<CEnums.CharacterClass> classes, string spell_id) :
            base(spell_name, desc, mana, req_lvl, classes, spell_id)
        {

        }
    }
}