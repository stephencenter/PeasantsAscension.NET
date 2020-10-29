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
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace Game
{
    public static class ItemManager
    {
        private static readonly List<Item> item_list = new List<Item>()
        {
            /* =========================== *
             *         CONSUMABLES         *
             * =========================== */
            #region
            // Potions -- Health
            new HealthManaPotion("Basic Potion",
"A generic potion that restores 20 HP when consumed.", 15, 20, 0, "s_potion"),

            new HealthManaPotion("Enhanced Potion",
"A more potent potion that restores 50 HP when consumed.", 30, 50, 0, "m_potion"),

            new HealthManaPotion("Strong Potion",
"A powerful potion that restores 100 HP when consumed.", 60, 100, 0, "l_potion"),

            new HealthManaPotion("Super Potion",
"A super powerful potion that restores 200 HP when consumed.", 120, 200, 0, "x_potion"),

            // Potions -- Mana
            new HealthManaPotion("Basic Elixir",
"A generic elixir that restores 10 MP when consumed.", 10, 0, 10, "s_elixir"),

            new HealthManaPotion("Enhanced Elixir",
"A more potent elixir that restores 25 MP when consumed.", 20, 0, 25, "m_elixir"),

            new HealthManaPotion("Grand Elixir",
"A powerful elixir that restores 50 MP when consumed.", 40, 0, 50, "l_elixir"),

            new HealthManaPotion("Extreme Elixir",
"A super powerful elixir that restores 100 MP when consumed.", 80, 0, 100, "x_elixir"),

            // Potions -- Both
            new HealthManaPotion("Minor Rejuvenation Potion",
"A basic mixture that restores 20 HP and 10 MP when consumed.", 30, 20, 10, "s_rejuv"),

            new HealthManaPotion("Refined Rejuvenation Potion",
"A higher quality mixture that restores 50 HP and 25 MP when consumed.", 60, 50, 25, "m_rejuv"),

            new HealthManaPotion("Mighty Rejuvenation Potion",
"A super powerful mixture that restores 100 HP and 50 MP when consumed.", 120, 100, 50, "l_rejuv"),

            // Potions - Status
            new StatusPotion("Potion of Allowing Speech",
@"A potion designed to enable the usage of damaged vocal chords.Relieves a party
member of the 'Silenced' debuff.", 50, CEnums.Status.silence, "silence_pot"),

            new StatusPotion("Potion of Curing Disease",
@"A potion designed to cure even the most deadly of illnesses.Relieves a party
member of the 'Poisoned' debuff.", 50, CEnums.Status.poison, "poison_pot"),

            new StatusPotion("Potion of Regaining Strength",
@"A potion designed to help regain lost muscle - mass and stamina.Relieves a party
member of the 'Weakened' debuff.", 50, CEnums.Status.weakness, "weakness_pot"),

            new StatusPotion("Potion of Enabling Sight",
@"A potion designed to help the blind regain their eyesight.Relieves a party
member of the 'Blinded' debuff.", 50, CEnums.Status.blindness, "blindness_pot"),

            new StatusPotion("Potion of Inducing Motion",
@"A potion designed to cure minor paralysis in most of the body.Relieves a party
member of the 'Paralyzed' debuff.", 50, CEnums.Status.paralyzation, "paralyze_pot"),

            // Potions - Alchemy
            new RandomPositiveEffectPotion("Pleasant Potion I",
@"A potion that can only be obtained through alchemy. Has a random weak 
positive effect when used on an ally. Made using 'strange' ingredients.", 100, 1, "pleasantpot1"),

            new RandomPositiveEffectPotion("Pleasant Potion II",
@"A potion that can only be obtained through alchemy. Has a random medium 
positive effect when used on an ally. Made using 'strange' ingredients.", 100, 2, "pleasantpot2"),

            new RandomPositiveEffectPotion("Pleasant Potion III",
@"A potion that can only be obtained through alchemy. Has a random strong 
positive effect when used on an ally. Made using 'strange' ingredients.", 100, 3, "pleasantpot3"),

            new RandomNegativeEffectPotion("Nasty Potion I",
@"A potion that can only be obtained through alchemy. Has a random weak 
negative effect when used on an enemy. Made using 'natural' ingredients.", 100, 1, "nastypot1"),

            new RandomNegativeEffectPotion("Nasty Potion II",
@"A potion that can only be obtained through alchemy. Has a random medium 
negative effect when used on an enemy. Made using 'natural' ingredients.", 100, 2, "nastypot2"),

            new RandomNegativeEffectPotion("Nasty Potion III",
@"A potion that can only be obtained through alchemy. Has a random strong 
negative effect when used on an enemy. Made using 'natural' ingredients.", 100, 3, "nastypot3"),

            new MultiTargetNuke("Grenade Potion I",
@"A potion that can only be obtained through alchemy. Deals 12 physical damage to
all enemies in the battle. Made using 'flowing' ingredients.", 100, 12, "grenadepot1"),

            new MultiTargetNuke("Grenade Potion II",
@"A potion that can only be obtained through alchemy. Deals 24 physical damage to
all enemies in the battle. Made using 'flowing' ingredients.", 100, 24, "grenadepot2"),

            new MultiTargetNuke("Grenade Potion III",
@"A potion that can only be obtained through alchemy. Deals 36 physical damage to
all enemies in the battle. Made using 'flowing' ingredients.", 100, 36, "grenadepot3"),

            new SingleTargetNuke("Missile Potion I",
@"A potion that can only be obtained through alchemy. Deals 24 physical damage to
a single target enemy. Made using 'rigid' ingredients.", 100, 24, "missilepot1"),

            new SingleTargetNuke("Missile Potion II",
@"A potion that can only be obtained through alchemy. Deals 48 physical damage to
a single target enemy. Made using 'rigid' ingredients.", 100, 48, "missilepot2"),

            new SingleTargetNuke("Missile Potion III",
@"A potion that can only be obtained through alchemy. Deals 72 physical damage to
a single target enemy. Made using 'rigid' ingredients.", 100, 72, "missilepot3"),

            new XPGoldPotion("Greed Potion I",
@"A potion that can only be obtained through alchemy. Used on an ally to convert 
up to 100 XP into 50 GP. Made using 'dark' ingredients.", 0, 50, -100, "greedpot1"),

            new XPGoldPotion("Greed Potion II",
@"A potion that can only be obtained through alchemy. Used on an ally to convert 
up to 250 XP into 125 GP. Made using 'dark' ingredients.", 0, 125, -250, "greedpot2"),

            new XPGoldPotion("Greed Potion III",
@"A potion that can only be obtained through alchemy. Used on an ally to convert 
up to 500 XP into 250 GP. Made using 'dark' ingredients.", 0, 250, -500, "greedpot3"),

            new XPGoldPotion("Temperance Potion I",
@"A potion that can only be obtained through alchemy. Used on an ally to convert
up to 100 GP into 50 XP. Made using 'mystic' ingredients.", 0, -100, 50, "temppot1"),

            new XPGoldPotion("Temperance Potion II",
@"A potion that can only be obtained through alchemy. Used on an ally to convert
up to 250 GP into 125 XP. Made using 'mystic' ingredients.", 0, -250, 125, "temppot2"),

            new XPGoldPotion("Temperance Potion III",
@"A potion that can only be obtained through alchemy. Used on an ally to convert
up to 500 GP into 250 XP. Made using 'mystic' ingredients.", 0, -500, 250, "temppot3"),

            new GameCrashPotion("Game Crash Potion",
@"Instantly crashes the game when used. Speaking of which, why would drink this?
Maybe you think I'm lying. Maybe you think it will grant you an ultra-powerful
weapon, or maybe it will make you level 100, or maybe it will instantly defeat
an important boss coming up. Well you'd be wrong, it really does just crash the
game. That's all this potion does, you wasted those mathematical ingredients on
this useless potion. You could have sold those for money, unlike this potion
which has no sale value. Instead you made a potion whose only purpose is to
crash the game. You probably don't believe me, do you? You think I'm lying and
you're gonna drink this thing regardless of what I tell you. Well fine, but
at least save the game before you do, and don't yell at me if you didn't and
your progress is lost.", 0, "gamecrashpot"),
            #endregion

            /* =========================== *
             *           WEAPONS           *
             * =========================== */
            #region
            new Weapon("Fists", "The oldest weapon known to man.",
                0, 0, CEnums.DamageType.physical, CEnums.CharacterClass.any, CEnums.Element.neutral, "weapon_fists"),

            // Weapons -- Warrior
            new Weapon("Iron Hoe",
@"Not much of a weapon, unless you really hate dirt.
-5% Damage Increase
-Only usable by Warriors", 10, 0.05, CEnums.DamageType.physical, CEnums.CharacterClass.warrior, CEnums.Element.neutral, "iron_hoe"),

            new Weapon("Bronze Sword",
@"A light yet sturdy sword smelted from bronze.
-10% Damage Increase
-Only usable by Warriors", 150, 0.1, CEnums.DamageType.physical, CEnums.CharacterClass.warrior, CEnums.Element.neutral, "bronze_sword"),

            new Weapon("Steel Spear",
@"A fair-sized spear crafted from well made steel.
-30% Damage Increase
-Only usable by Warriors", 600, 0.3, CEnums.DamageType.physical, CEnums.CharacterClass.warrior, CEnums.Element.neutral, "steel_spear"),

            new Weapon("Grand Battleaxe",
@"A massive, titanium battleaxe that probably weighs more than most of the
monsters it kills.
-50% Damage Increase
-Only usable by Warriors", 1200, 0.5, CEnums.DamageType.physical, CEnums.CharacterClass.warrior, CEnums.Element.neutral, "titan_axe"),

            // Weapons -- Mage 
            new Weapon("Magical Twig",
@"Not actually magical but it makes you feel powerful when you use it.
-5% Damage Increase
-Only usable by Mages", 10, 0.05, CEnums.DamageType.piercing, CEnums.CharacterClass.mage, CEnums.Element.neutral, "magical_twig"),

            new Weapon("Oak Wand",
@"A wooden wand imbued with simple magical abilities.
-10% Damage Increase
-Only usable by Mages", 150, 0.1, CEnums.DamageType.piercing, CEnums.CharacterClass.mage, CEnums.Element.neutral, "oak_staff"),

            new Weapon("Arcane Spellbook",
@"An intermediate spellbook for combat purposes.
-30% Damage Increase
-Only usable by Mages", 600, 0.3, CEnums.DamageType.piercing, CEnums.CharacterClass.mage, CEnums.Element.neutral, "arcane_spellbook"),

            new Weapon("Staff of the Ancients",
@"A powerful staff enchanted with ancient magic.
-50% Damage Increase
-Only usable by Mages", 1200, 0.5, CEnums.DamageType.piercing, CEnums.CharacterClass.mage, CEnums.Element.neutral, "ancient_staff"),

            // Weapons -- Assassin 
            new Weapon("Stone Dagger",
@"A stone knife used to shear sheep and cut meat.
-5% Damage Increase
-Only usable by Assassins", 10, 0.05, CEnums.DamageType.physical, CEnums.CharacterClass.assassin, CEnums.Element.neutral, "stone_dagger"),

            new Weapon("Serrated Knife",
@"A durable knife made of iron with one side made jagged.
-10% Damage Increase
-Only usable by Assassins", 150, 0.1, CEnums.DamageType.physical, CEnums.CharacterClass.assassin, CEnums.Element.neutral, "serated_knife"),

            new Weapon("Stiletto",
@"A long, cross-shaped knife perfect for piercing the hearts of your enemies.
-30% Damage Increase
-Only usable by Assassins", 600, 0.3, CEnums.DamageType.physical, CEnums.CharacterClass.assassin, CEnums.Element.neutral, "stiletto"),

            new Weapon("Mythril Shortblade",
@"A longer, curved knife made of mythril. It was the preferred weapon of the
rangers hundreds of years ago.
-50% Damage Increase
-Only usable by Assassins", 1200, 0.5, CEnums.DamageType.physical, CEnums.CharacterClass.assassin, CEnums.Element.neutral, "mythril_spellbook"),

            // Weapons -- Ranger 
            new Weapon("Sling Shot",
@"A weapon that could scare even the mightiest of tin-cans.
-5% Damage Increase
-Only usable by Rangers", 10, 0.05, CEnums.DamageType.piercing, CEnums.CharacterClass.ranger, CEnums.Element.neutral, "sling_shot"),

            new Weapon("Cherry Short Bow",
@"A small bow made for beginner archers. Very reliable.
Made from the wood of a cherry tree.
-10% Damage Increase
-Only usable by Rangers", 150, 0.1, CEnums.DamageType.piercing, CEnums.CharacterClass.ranger, CEnums.Element.neutral, "short_bow"),

            new Weapon("Ashen Long Bow",
@"A much more impressive bow capable of accuracy at long distances.
Made from the wood of an ash tree.
-30% Damage Increase
-Only usable by Rangers", 600, 0.3, CEnums.DamageType.piercing, CEnums.CharacterClass.ranger, CEnums.Element.neutral, "long_bow"),

            new Weapon("Osage Compound Bow",
@"A bow with strange contraptions on it that improve its usability.
Made from the wood of the osage orange tree.
-50% Damage Increase
-Only usable by Rangers", 1200, 0.5, CEnums.DamageType.piercing, CEnums.CharacterClass.ranger, CEnums.Element.neutral, "compound_bow"),

            // Weapons -- Monk
            // Monks do not use weapons

            // Weapons -- Paladin 
            new Weapon("Rubber Mallet",
@"This can barely hammer nails, what do you expect to kill with it?
-5% Damage Increase
-Only usable by Paladins", 10, 0.05, CEnums.DamageType.physical, CEnums.CharacterClass.paladin, CEnums.Element.neutral, "rubber_mallet"),

            new Weapon("Holy Mace",
@"An well-made iron mace great for exercising His Divinity's will.
-10% Damage Increase
-Only usable by Paladins", 150, 0.1, CEnums.DamageType.physical, CEnums.CharacterClass.paladin, CEnums.Element.neutral, "holy_mace"),

            new Weapon("Hammer of Might",
@"A hammer often used by holy warriors to smash their foes.
-30% Damage Increase
-Only usable by Paladins", 600, 0.3, CEnums.DamageType.physical, CEnums.CharacterClass.paladin, CEnums.Element.neutral, "might_hammer"),

            new Weapon("Night's Bane",
@"A powerful, sacred hammer. High-rank paladins in the ancient times were given
one of these, and their holy light has kept them safe even until now.
-50% Damage Increase
-Only usable by Paladins", 1200, 0.5, CEnums.DamageType.physical, CEnums.CharacterClass.paladin, CEnums.Element.neutral, "night_bane"),

            // Weapons -- Bard 
            new Weapon("Kazoo",
@"A wooden kazoo that does more to annoy your enemies than damage them.
-5% Damage Increase
-Only usable by Bards", 10, 0.05, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "kazoo"),

            new Weapon("Flute",
@"A good-quality flute made out of wood and silver.
-10% Damage Increase
-Only usable by Bards", 150, 0.1, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "flute"),

            new Weapon("Snare Drum",
@"A marching drum that inspires courage in the hearts of your allies.
-30% Damage Increase
-Only usable by Bards", 300, 0.3, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "snare_drum"),

            new Weapon("Trumpet",
@"A mighty brass trumpet that can be heard blaring from miles away.
-30% Damage Increase
-Only usable by Bards", 600, 0.3, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "trumpet"),

            new Weapon("Bagpipes",
@"A ridiculously loud and extravagent bagpipe made from plaid fabric. Your
allies will probably hate you if you use this.
-50% Damage Increase
-Only usable by Bards", 1200, 0.5, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "bagpipes"),

            new Weapon("Lyre",
@"An extravagant lyre that produces the most beautiful tune when plucked.
-50% Damage Increase
-Only usable by Bards", 1200, 0.5, CEnums.DamageType.piercing, CEnums.CharacterClass.bard, CEnums.Element.neutral, "lyre"),
            #endregion

            /* =========================== *
             *            ARMOR            *
             * =========================== */   
            #region
            new Armor("None", "You should probably get some armor.",
                0, 0, 0, new List<CEnums.CharacterClass>(), new List<CEnums.CharacterClass>(), "no_armor"),

            // Light Armor
            new Armor("Light Armor",
@"Armor made from leather, perfect for those who need to move quickly.
-Resistance: 10%
-Evasion Penalty: 10%
-Good for Assassin and Ranger (1.5x Resist, 0.67x Penalty)
-Bad for Mage and Monk (0.67x Resist, 1.5x Penalty)", 100, 0.1, 0.1,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.assassin, CEnums.CharacterClass.ranger },
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, "light_armor"),

            // Medium Armor
            new Armor("Medium Armor",
@"Cloth clothes with chainmail underneath for a balance of speed and defense.
-Resistance: 20%
-Evasion Penalty: 15%
-Good for Warrior and Ranger (1.5x Resist, 0.67x Penalty)
-Bad for Mage, Monk, and Bard (0.67x Resist, 1.5x Penalty)", 200, 0.2, 0.15,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.warrior, CEnums.CharacterClass.ranger },
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk, CEnums.CharacterClass.bard }, "medium_armor"),

            // Heavy Armor
            new Armor("Heavy Armor",
@"Armor made from thick plates, perfect for those in the thick of battle.
-Resistance: 30%
-Evasion Penalty: 20%
-Good for Warrior and Paladin (1.5x Resist, 0.67x Penalty)
-Bad for all other classes (0.67x Resist, 1.5x Penalty)", 400, 0.3, 0.2,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.warrior, CEnums.CharacterClass.paladin },
                      new List<CEnums.CharacterClass>()
                      {
                          CEnums.CharacterClass.mage,
                          CEnums.CharacterClass.monk,
                          CEnums.CharacterClass.assassin,
                          CEnums.CharacterClass.ranger,
                          CEnums.CharacterClass.bard,
                      }, "heavy_armor"),

            // Fancy Robes
            new Armor("Fancy Robes",
@"Armor made from leather, perfect for those who need to move quickly.
-Resistance: 5%
-Evasion Penalty: 5%
-Good for Mage and Monk (1.5x Resist, 0.67x Penalty)
-Bad for Warrior and Paladin (0.67x Resist, 1.5x Penalty)", 75, 0.05, 0.05,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk },
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.warrior, CEnums.CharacterClass.paladin }, "fancy_robes"),

            // Dragonhide Armor
            new Armor("Dragonhide Armor",
@"Armor made from leather, perfect for those who need to move quickly.
-Resistance: 15%
-Evasion Penalty: 15%
-Good for Assassin and Ranger (1.5x Resist, 0.67x Penalty)
-Bad for Mage and Monk (0.67x Resist, 1.5x Penalty)", 250, 0.15, 0.15,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.assassin, CEnums.CharacterClass.ranger },
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.mage, CEnums.CharacterClass.monk }, "dragon_armor"),

            // Festive Clothes
            new Armor("Festive Clothes",
@"Armor made from leather, perfect for those who need to move quickly.
-Resistance: 5%
-Evasion Penalty: 2%
-Good for Bard (1.5x Resist, 0.67x Penalty)
-Bad for Warrior and Paladin (0.67x Resist, 1.5x Penalty)", 50, 0.05, 0.02,
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.monk },
                      new List<CEnums.CharacterClass>() { CEnums.CharacterClass.warrior, CEnums.CharacterClass.paladin }, "festive_clothes"),
            #endregion

            /* =========================== *
             *         ACCESSORIES         *
             * =========================== */
            #region
            new ElementAccessory("None", "You should probably get an elemental accessory.", 0, CEnums.Element.neutral, "no_elem_access"),

            // Element Accessories
            new ElementAccessory("Aquatic Amulet",
@"An amulet that imbues its wearer with the power of water. Causes fire
attacks to deal less damage to the wearer, and electric attacks to deal more.", 375, CEnums.Element.water, "water_amulet"),

            new ElementAccessory("Infernal Amulet",
@"An amulet that imbues its wearer with the power of fire. Causes ice
attacks to deal less damage to the wearer, and water attacks to deal more.", 375, CEnums.Element.fire, "fire_amulet"),

            new ElementAccessory("Glacial Amulet",
@"An amulet that imbues its wearer with the power of ice. Causes grass
attacks to deal less damage to the wearer, and fire attacks to deal more.", 375, CEnums.Element.ice, "ice_amulet"),

            new ElementAccessory("Verdant Amulet",
@"An amulet that imbues its wearer with the power of grass. Causes wind
attacks to deal less damage to the wearer, and ice attacks to deal more.", 375, CEnums.Element.grass, "grass_amulet"),

            new ElementAccessory("Tempestuous Amulet",
@"An amulet that imbues its wearer with the power of wind. Causes earth
attacks to deal less damage to the wearer, and grass attacks to deal more.", 375, CEnums.Element.wind, "wind_amulet"),

            new ElementAccessory("Ground Amulet",
@"An amulet that imbues its wearer with the power of earth. Causes electric
attacks to deal less damage to the wearer, and wind attacks to deal more.", 375, CEnums.Element.earth, "earth_amulet"),

            new ElementAccessory("Galvanic Amulet",
@"An amulet that imbues its wearer with the power of electric. Causes water
attacks to deal less damage to the wearer, and ground attacks to deal more.", 375, CEnums.Element.electric, "electric_amulet"),

            new ElementAccessory("Divine Amulet",
@"An amulet that imbues its wearer with the power of light. Causes light
attacks to deal less damage to the wearer, and dark attacks to deal more.", 375, CEnums.Element.light, "light_amulet"),

            new ElementAccessory("Umbral Amulet",
@"An amulet that imbues its wearer with the power of dark. Causes dark
attacks to deal less damage to the wearer, and light attacks to deal more.", 375, CEnums.Element.dark, "dark_amulet"),

            // Ammunition
            new Ammunition("Standard Ammo",
@"If you're a ranger, you might want to invest in different ammo types to 
increase your versatility.", 0, 1.0, 1.0, 1.0, null, "no_ammunition"),

            new Ammunition("Volitile Ammo",
@"Incredibly volitle ammunition. They have a 30% crit-rate instead of 15%, but
they only have 90% accuracy instead of 100%. Only usable by rangers.", 125, 1, 0.9, 2.0, null, "vol_ammunition"),

            new Ammunition("Stable Ammo",
@"A very safe choice of ammo. They deal 110% damage per hit, but a 0% crit-rate.
Only usable by rangers.", 125, 1.1, 1.0, 0.0, null, "safe_ammunition"),

            #endregion

            /* =========================== *
             *            TOOLS            *
             * =========================== */
            #region
            new Shovel("Expert Mining Tool",
@"It's a shovel. The name is just a marketing gimmick. Used to dig for gems
on the overworld. Gems have pre-determined locations and do not respawn - there 
is no luck involved with this tool.", 150, "shovel_tool"),

            new FastTravelAtlas("Fast Travel Atlas",
@"A convenient tome that allows teleportation between towns. These aren't
being made anymore, after having been banned by the King due to its use in
many recent abductions and murders. Most of the pages appear to be missing.", 0, "fast_map"),

            new MonsterEncyclopedia("Monster Encyclopedia",
@"A book containing information on monsters. When used in battle, this will
identify the stats and weaknesses of an enemy. Has no use outside of battle.", 200, "monster_book"),

            new PocketAlchemyLab("Pocket Alchemy Lab",
@"A nifty little Pocket Alchemy Lab! Somehow all of the necessary tools to
convert everyday ingredients into useful potions can fit in your pocket.
There are six flavors of ingredients, and each flavor corresponds to a specific
potion. Combine three ingredients to make a potion. The ratio of flavors used
determines the probability of getting each flavor potion. The quantity of the
prevailing ingredient determines the potion strength.", 200, "pocket_lab"),

            new MusicBox("Portable Musicbox",
@"Somehow this small device has the ability to play music without need for a
bard or instruments. Select a folder full of music on your computer and this
device will replace the in-game music with your tunes!", 250, "musicbox"),

            // Lockpicks
            new LockpickKit("Copper Lockpick Kit",
@"A wooden lockpick kit with a 30% chance to open chests. Chests can be found
by sneaking into houses in towns, or inside dungeons. This kit will not break,
so you don't need to buy multiple of them.", 30, 30, "copper_lockpick"),

            new LockpickKit("Bronze Lockpick Kit",
@"A copper lockpick kit with a 45% chance to open chests. Chests can be found
by sneaking into houses in towns, or inside dungeons. This kit will not break,
so you don't need to buy multiple of them.", 200, 45, "bronze_lockpick"),

            new LockpickKit("Iron Lockpick Kit",
@"An iron lockpick kit with a 60% chance to open chests. Chests can be found
by sneaking into houses in towns, or inside dungeons. This kit will not break,
so you don't need to buy multiple of them.", 300, 60, "iron_lockpick"),

            new LockpickKit("Steel Lockpick Kit",
@"A steel lockpick kit with a 75% chance to open chests. Chests can be found
by sneaking into houses in towns, or inside dungeons. This kit will not break,
so you don't need to buy multiple of them.", 500, 75, "steel_lockpick"),

            new LockpickKit("Mythril Lockpick Kit",
@"A mythril lockpick kit with a 90% chance to open chests. Chests can be found
by sneaking into houses in towns, or inside dungeons. This kit will not break,
so you don't need to buy multiple of them.", 700, 90, "mythril_lockpick"),
            #endregion

            /* =========================== *
             *         OTHER STUFF         *
             * =========================== */
            #region
            new QuestItem("Message from Joseph", "A neatly written message addressed to Philliard.", 0, "message_joseph"),

            new QuestItem("Message from Philliard", "A neatly written message addressed to Joseph.", 0, "message_philliard"),

            // Gems & Valuables
            new Valuable("Pearl", "A valuable pearl. This could probably be sold for quite a bit.", 875, "pearl_gem"),

            new Valuable("Ruby", "A valuable ruby. This could be sold for quite a bit.", 875, "ruby_gem"),

            new Valuable("Sapphire", "A valuable sapphire. This could probably be sold for quite a bit.", 875, "sapphire_gem"),

            new Valuable("Emerald", "A valuable emerald. This could probably be sold for quite a bit.", 875, "emerald_gem"),

            new Valuable("Citrine", "A valuable citrine. This could probably be sold for quite a bit.", 875, "citrine_gem"),

            new Valuable("Jade", "A valuable jade. This could probably be sold for quite a bit.", 875, "jade_gem"),

            new Valuable("Opal", "A valuable opal. This could probably be sold for quite a bit.", 875, "opal_gem"),

            new Valuable("Onyx", "A valuable onyx. This could probably be sold for quite a bit.", 875, "onyx_gem"),

            new Valuable("Diamond", "A valuable diamond. This could probably be sold for quite a bit.", 875, "diamond_gem"),

            new Valuable("Amethyst", "A valuable amethyst. This could probably be sold for quite a bit.", 875, "amethyst_gem"),

            new Valuable("Topaz", "A valuable topaz. This could probably be sold for quite a bit.", 875, "topaz_gem"),

            new Valuable("Garnet", "A valuable garnet. This could probably be sold for quite a bit.", 875, "garnet_gem"),

            new Valuable("Quartz", "A valuable quartz. This could probably be sold for quite a bit.", 875, "quartz_gem"),

            new Valuable("Zircon", "A valuable zircon. This could probably be sold for quite a bit.", 875, "zircon_gem"),

            new Valuable("Agate", "A valuable agate. This could probably be sold for quite a bit.", 875, "agate_gem"),

            new Valuable("Aquamarine", "A valuable aquamarine. This could probably be sold for quite a bit.", 875, "aquamarine_gem"),

            // ALCHEMY INGREDIENTS - Dropped by monsters, used to make potions
            // Strange
            new Ingredient("Broken Crystal",
@"A chunk of crystal too powdery to be of any value. Could have useful alchemical
applications. Has a 'Strange' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.strange, "broken_crystal"),

            new Ingredient("Chain links",
@"A couple joined links of chain made from steel. Could have useful alchemical
applications. Has a 'Strange' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.strange, "chain_link"),

            new Ingredient("Bag of Bones",
@"A bag full of various bones from a now deceased creature. Could have useful
alchemical applications. Has a 'Strange' alchemical flavor. Combine with two
other ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.strange, "bone_bag"),

            new Ingredient("Ripped Cloth",
@"A thick, torn cloth made out of an unknown fabric. Could have useful alchemical
applications. Has a 'Strange' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.strange, "ripped_cloth"),

            new Ingredient("Living Bark",
@"This bark has a fleshy texture to it. Could have useful alchemical
applications. Has a 'Strange' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.strange, "living_bark"),

            // Mystic
            new Ingredient("Demonic Essence",
@"A strange orb that exudes a terrifying aura. Could have useful alchemical
applications. Has a 'Mystic' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mystic, "demonic_essence"),

            new Ingredient("Angelic Essence",
@"A strange orb that radiates an incredible aura. Could have useful alchemical
applications. Has a 'Mystic' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mystic, "angelic_essence"),

            new Ingredient("Strange Runestone",
@"Strange stones with even stranger symbols on it. Could have useful alchemical
applications. Has a 'Mystic' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mystic, "runestone"),

            new Ingredient("Unicorn Horn",
@"A tough and shiny horn from a mythical creature. Could have useful alchemical
applications. Has a 'Mystic' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mystic, "unicorn_horn"),

            new Ingredient("Fairy Dust",
@"Dust from a fairy. It has strange, magical properties. Could have useful
alchemical applications. Has a 'Mystic' alchemical flavor. Combine with two
other ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mystic, "fairy_dust"),

            // Rigid
            new Ingredient("Crab Claw",
@"A reddish claw from a giant crab. Could have useful alchemical applications.
Has a 'Rigid' alchemical flavor. Combine with two other ingredients in a
Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.rigid, "crab_claw"),

            new Ingredient("Shell Fragment",
@"A broken fragment of a once-beautiful shell. Could have useful alchemical
applications. Has a 'Rigid' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.rigid, "shell_fragment"),

            new Ingredient("Golem Rock",
@"A small rock that seems to glow slightly. Could have useful alchemical
applications. Has a 'Rigid' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.rigid, "golem_rock"),

            new Ingredient("Beetle Shell",
@"A bluish shell from a large beetle. Could have useful alchemical applications.
Has a 'Rigid' alchemical flavor. Combine with two other ingredients in a
Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.rigid, "beetle_shell"),

            new Ingredient("Monster Skull",
@"A broken skull from a strange creature. Could have useful alchemical
applications. Has a 'Rigid' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.rigid, "monster_skull"),

            // Flowing
            new Ingredient("Vial of Slime",
@"A small glass vial filled with gooey slime. Could have useful alchemical
applications. Has a 'Flowing' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.flowing, "slime_vial"),

            new Ingredient("Vial of Blood",
@"A small glass vial filled with the blood of some creature. Could have useful
alchemical applications. Has a 'Flowing' alchemical flavor. Combine with two
other ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.flowing, "blood_vial"),

            new Ingredient("Vial of Water",
@"A small glass vial filled with enchanted water. Could have useful alchemical
applications. Has a 'Flowing' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.flowing, "water_vial"),

            new Ingredient("Ink Sack",
@"A small pouch full of an inky substance. Could have useful alchemical
applications. Has a 'Flowing' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.flowing, "ink_sack"),

            new Ingredient("Ectoplasm",
@"The gooey remains from a terrifying apparition. Could have useful alchemical
applications. Has a  'Flowing' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.flowing, "ectoplasm"),

            // Dark
            new Ingredient("Burnt Ash",
@"The ashy remains of a once-living creature. Could have useful alchemical
applications. Has a 'Dark' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.dark, "burnt_ash"),

            new Ingredient("Monster Fang",
@"The sharp fang of a frightening creature. Could have useful alchemical
applications. Has a 'Dark' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.dark, "monster_fang"),

            new Ingredient("Gooey Antennae",
@"A pair of antennae from a massive, slimy insect. Could have useful alchemical
applications. Has a 'Dark' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.dark, "antennae"),

            new Ingredient("Eyeballs",
@"The visual receptors of some disgusting creature. Could have useful alchemical
applications. Has a 'Dark' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.dark, "eye_balls"),

            new Ingredient("Serpent Scale",
@"A rough scale from an unknown reptile. Could have useful alchemical
applications. Has a 'Dark' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.dark, "serpent_scale"),

            // Natural
            new Ingredient("Wing Piece",
@"A piece of wing from a flying creature. Could have useful alchemical
applications. Has a 'Natural' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.natural, "wing_piece"),

            new Ingredient("Animal Fur",
@"A wet clump of fur from a strange animal. Could have useful alchemical
applications. Has a 'Natural' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.natural, "animal_fur"),

            new Ingredient("Rodent Tail",
@"The detached tail of a hideous rodent. Could have useful alchemical
applications. Has a 'Natural' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.natural, "rodent_tail"),

            new Ingredient("Serpent Tongue",
@"A dried-up tongue from a slithery serpent. Could have useful alchemical
applications. Has a 'Natural' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.natural, "serpent_tongue"),

            new Ingredient("Feathers",
@"A veiny feather from an unknown avian creature. Could have useful alchemical
applications. Has a 'Natural' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.natural, "feathers"),

            // Mathematical
            new Ingredient("Calculus Homework",
@"A load of random symbols and gibberish. Could have useful alchemical
applications. Has a 'Mathematical' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mathematical, "c_homework"),

            new Ingredient("Graph Paper",
@"Useful paper for graphing points and lines. Could have useful alchemical
applications. Has a 'Mathematical' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mathematical, "g_paper"),

            new Ingredient("Ruler",
@"A piece of wood with lines on it. Neat! Could have useful alchemical
applications. Has a 'Mathematical' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mathematical, "ruler"),

            new Ingredient("Protractor and Compass",
@"Instruments used to make shapes and angles. Could have useful alchemical
applications. Has a 'Mathematical' alchemical flavor. Combine with two other
ingredients in a Pocket Alchemy Lab to make a potion.", 25, CEnums.Flavor.mathematical, "protractor")
            #endregion
        };

        public static List<Item> GetItemList()
        {
            return item_list;
        }

        public static Item FindItemWithID(string item_id)
        {
            return item_list.Single(x => x.ItemID == item_id);
        }

        public static bool VerifyItemExists(string item_id)
        {
            return item_list.Any(x => x.ItemID == item_id);
        }

        public static bool ConsumableTargetMenu(PlayableCharacter user, Consumable item)
        {
            string action_desc = $@"{item.ItemName}: 
{item.Description}

Who should {user.UnitName} use the {item.ItemName} on?";

            return user.PlayerChooseTarget(action_desc, item.TargetMapping);
        }

        public static bool EquipmentTargetMenu(PlayableCharacter user, Equipment item)
        {
            TargetMapping t_map = new TargetMapping(true, true, false, true);
            string action_desc = $@"{item.ItemName}: 
{item.Description}

Who should equip the {item.ItemName}?";

            return user.PlayerChooseTarget(action_desc, t_map);
        }
    }

    public abstract class Item
    {
        // The basic item class. Items are stored in the "inventory" dictionary. All
        // item-subclasses inherit from this class.
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public bool IsImportant { get; set; }
        public CEnums.InvCategory Category { get; set; }
        public string ItemID { get; set; }

        public abstract void UseItem(PlayableCharacter user);

        // Constructor
        protected Item(string name, string desc, int value, bool imp, CEnums.InvCategory cat, string item_id)
        {
            ItemName = name;
            Description = desc;
            Value = value;
            IsImportant = imp;
            Category = cat;
            ItemID = item_id;
        }
    }

    /* =========================== *
     *        MISCELLANEOUS        *
     * =========================== */
    #region
    public class QuestItem : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            Console.WriteLine($"You look at the {ItemName}.");
            Console.WriteLine("It looks very important, you should definitely hold on to it.");
            CMethods.PressAnyKeyToContinue();
        }

        public QuestItem(string name, string desc, int value, string item_id) : base(name, desc, value, true, CEnums.InvCategory.quest, item_id)
        {

        }
    }

    public class Valuable : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            Console.WriteLine($"You admire the valuable {ItemName}.");
            CMethods.PressAnyKeyToContinue();
        }

        public Valuable(string name, string desc, int value, string item_id) : base(name, desc, value, false, CEnums.InvCategory.misc, item_id)
        {

        }
    }

    public class Ingredient : Item
    {
        public CEnums.Flavor Flavor { get; set; }

        public override void UseItem(PlayableCharacter user)
        {
            Console.WriteLine($"You look at the {ItemName}.");
            Console.WriteLine("This could come in handy for making potions.");
            CMethods.PressAnyKeyToContinue();
        }

        public Ingredient(string name, string desc, int value, CEnums.Flavor flavor, string item_id) : base(name, desc, value, false, CEnums.InvCategory.misc, item_id)
        {
            Flavor = flavor;
        }
    }
    #endregion

    /* =========================== *
     *          EQUIPMENT          *
     * =========================== */
    #region
    public abstract class Equipment : Item
    {
        public CEnums.EquipmentType EquipType { get; set; }

        // Constructor
        protected Equipment(string name, string desc, int value, CEnums.InvCategory cat, string item_id) :
            base(name, desc, value, false, cat, item_id)
        {

        }
    }

    public class Weapon : Equipment
    {
        // Items that increase your damage by a percentage.
        public double Power { get; set; }
        public CEnums.DamageType DamageType { get; set; }
        public CEnums.CharacterClass PClass { get; set; }
        public CEnums.Element Element { get; set; }

        public override void UseItem(PlayableCharacter user)
        {
            PlayableCharacter equipper = user.CurrentTarget as PlayableCharacter;

            if (equipper.PClass == PClass)
            {
                InventoryManager.EquipItem(equipper, ItemID);
                Console.WriteLine($"{equipper.UnitName} equips the {ItemName}.");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                Console.WriteLine($"{equipper.UnitName} must be a {PClass.EnumToString()} to equip this.");
                CMethods.PressAnyKeyToContinue();
            }
        }

        // Constructor
        public Weapon(string name, string desc, int value, double power, CEnums.DamageType d_type, CEnums.CharacterClass p_class, CEnums.Element element, string item_id) : base(name, desc, value, CEnums.InvCategory.weapons, item_id)
        {
            Power = power;
            DamageType = d_type;
            PClass = p_class;
            Element = element;
            EquipType = CEnums.EquipmentType.weapon;
        }
    }

    public class Armor : Equipment
    {
        // Items that give the player a percent resistance to all damage,
        // but reduce player speed and evasion to compensate
        public double Resist { get; set; }
        public double Penalty { get; set; }

        // Proficient classes get a 1.5x increase in resist, and a 1.5x decrease in penalty
        // Non-proficient classes get a 1.5x decrease in resist, and a 1.5x increase in penalty
        public List<CEnums.CharacterClass> ProficientClasses { get; set; }
        public List<CEnums.CharacterClass> NonProficientClasses { get; set; }

        public override void UseItem(PlayableCharacter user)
        {
            PlayableCharacter equipper = user.CurrentTarget as PlayableCharacter;

            InventoryManager.EquipItem(equipper, ItemID);
            Console.WriteLine($"{equipper.UnitName} equips the {ItemName}.");

            if (ProficientClasses.Contains(equipper.PClass))
            {
                Console.WriteLine($"It feels light and comfortable.");
            }

            else if (NonProficientClasses.Contains(equipper.PClass))
            {
                Console.WriteLine($"It feels bulky and cumbersome.");
            }

            CMethods.PressAnyKeyToContinue();
        }

        public double GetEffectiveResist(PlayableCharacter equipper)
        {
            if (ProficientClasses.Contains(equipper.PClass))
            {
                return Resist * 1.5;
            }

            else if (NonProficientClasses.Contains(equipper.PClass))
            {
                return Resist / 1.5;
            }

            return Resist;
        }

        public double GetEffectivePenalty(PlayableCharacter equipper)
        {
            if (ProficientClasses.Contains(equipper.PClass))
            {
                return Penalty / 1.5;
            }

            else if (NonProficientClasses.Contains(equipper.PClass))
            {
                return Penalty * 1.5;
            }

            return Penalty;
        }

        // Constructor
        public Armor(string name, string desc, int value, double resist, double penatly, List<CEnums.CharacterClass> prof_classes, List<CEnums.CharacterClass> nonprof_classes, string item_id) :
            base(name, desc, value, CEnums.InvCategory.armor, item_id)
        {
            Resist = resist;
            Penalty = penatly;
            ProficientClasses = prof_classes;
            NonProficientClasses = nonprof_classes;
        }
    }

    public class ElementAccessory : Equipment
    {
        // Gives the player an element used when taking damage
        public CEnums.Element Element { get; set; }

        public override void UseItem(PlayableCharacter user)
        {
            PlayableCharacter equipper = user.CurrentTarget as PlayableCharacter;
            InventoryManager.EquipItem(equipper, ItemID);
            Console.WriteLine($"{equipper.UnitName} equips the {ItemName}.");
            Console.WriteLine($"Their defensive element is now {Element.EnumToString()}.");
            CMethods.PressAnyKeyToContinue();
            equipper.CalculateStats();
        }

        // Constructor
        public ElementAccessory(string name, string desc, int value, CEnums.Element element, string item_id) : base(name, desc, value, CEnums.InvCategory.accessories, item_id)
        {
            Element = element;
            EquipType = CEnums.EquipmentType.elem_accessory;
        }
    }

    public class Ammunition : Equipment
    {
        public double DamageMultiplier { get; set; }    // Defaults to 1
        public double AccuracyMultiplier { get; set; }  // Defaults to 1
        public double CritRateMultipier { get; set; }   // Defaults to 1
        public CEnums.Status? Status { get; set; }      // Defaults to null

        public override void UseItem(PlayableCharacter user)
        {
            PlayableCharacter equipper = user.CurrentTarget as PlayableCharacter;

            if (equipper.PClass == CEnums.CharacterClass.ranger)
            {
                InventoryManager.EquipItem(equipper, ItemID);
                Console.WriteLine($"{equipper.UnitName} equips the {ItemName}.");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                Console.WriteLine($"Only rangers can equip ammunition.");
                CMethods.PressAnyKeyToContinue();
            }
        }

        // Constructor
        public Ammunition(string name, string desc, int value, double damage, double accuracy, double critrate, CEnums.Status? status, string item_id) : base(name, desc, value, CEnums.InvCategory.accessories, item_id)
        {
            DamageMultiplier = damage;
            AccuracyMultiplier = accuracy;
            CritRateMultipier = critrate;
            Status = status;
            EquipType = CEnums.EquipmentType.ammunition;
        }
    }
    #endregion

    /* =========================== *
     *         CONSUMABLES         *
     * =========================== */
    #region
    public abstract class Consumable : Item
    {
        // Consumables can be used in battle, and are used by one PCU targeting another unit
        public TargetMapping TargetMapping { get; set; }
        public bool Disposable { get; set; }

        public sealed override void UseItem(PlayableCharacter user)
        {
            Unit target = user.CurrentTarget;

            SoundManager.potion_brew.SmartPlay();
            Console.WriteLine($"{target.UnitName} uses the {ItemName}...");
            CMethods.SmartSleep(750);

            PerformItemFunction(user, target);

            if (GameLoopManager.Gamestate != CEnums.GameState.battle)
            {
                CMethods.PressAnyKeyToContinue();
            }

            if (Disposable)
            {
                InventoryManager.RemoveItemFromInventory(ItemID);
            }
        }

        public abstract void PerformItemFunction(PlayableCharacter user, Unit target);

        // Constructor
        protected Consumable(string name, string desc, int value, TargetMapping t_mapping, bool disposable, string item_id) :
            base(name, desc, value, false, CEnums.InvCategory.consumables, item_id)
        {
            TargetMapping = t_mapping;
            Disposable = disposable;
        }
    }

    public class HealthManaPotion : Consumable
    {
        // Items that restore your HP, MP, or both
        public int Health { get; set; }
        public int Mana { get; set; }

        private static readonly TargetMapping healthmana_mapping = new TargetMapping(true, true, false, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            SoundManager.magic_healing.SmartPlay();

            if (Health > 0)
            {
                target.HP += Health;
                Console.WriteLine($"{target.UnitName} restored {Health} HP using the {ItemName}!");
            }

            if (Mana > 0)
            {
                target.MP += Mana;
                Console.WriteLine($"{target.UnitName} restored {Mana} MP using the {ItemName}!");
            }

            target.FixAllStats();
        }

        // Constructor
        public HealthManaPotion(string name, string desc, int value, int heal, int mana, string item_id) :
            base(name, desc, value, healthmana_mapping, true, item_id)
        {
            Health = heal;
            Mana = mana;
        }
    }

    public class StatusPotion : Consumable
    {
        public CEnums.Status Status { get; set; }

        private static readonly TargetMapping status_mapping = new TargetMapping(true, true, false, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            if (target.HasStatus(Status))
            {
                target.Statuses = target.Statuses.Where(x => x != Status).ToList();
                Console.WriteLine($"{target.UnitName} is no longer {Status.EnumToString().ToLower()}!");
                SoundManager.magic_healing.SmartPlay();
            }

            else
            {
                Console.WriteLine($"...but {target.UnitName} wasn't {Status.EnumToString().ToLower()}!");
                SoundManager.debuff.SmartPlay();
            }

            target.FixAllStats();
        }

        // Constructor
        public StatusPotion(string name, string desc, int value, CEnums.Status status, string item_id) :
            base(name, desc, value, status_mapping, true, item_id)
        {
            Status = status;
        }
    }

    public class RandomPositiveEffectPotion : Consumable
    {
        public int EffectStrength { get; set; }

        private static readonly TargetMapping positive_mapping = new TargetMapping(true, true, false, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            throw new NotImplementedException();
        }

        // Constructor
        public RandomPositiveEffectPotion(string name, string desc, int value, int strength, string item_id) :
            base(name, desc, value, positive_mapping, true, item_id)
        {
            EffectStrength = strength;
        }
    }

    public class RandomNegativeEffectPotion : Consumable
    {
        public int EffectStrength { get; set; }

        private static readonly TargetMapping negative_mapping = new TargetMapping(false, false, true, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            throw new NotImplementedException();
        }

        // Constructor
        public RandomNegativeEffectPotion(string name, string desc, int value, int strength, string item_id) :
            base(name, desc, value, negative_mapping, true, item_id)
        {
            EffectStrength = strength;
        }
    }

    public class SingleTargetNuke : Consumable
    {
        public int Damage { get; set; }

        private static readonly TargetMapping bomb_mapping = new TargetMapping(false, false, true, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            int attack_damage = BattleCalculator.CalculateRawDamage(Damage, CEnums.Element.neutral, target, CEnums.DamageType.physical);

            if (BattleCalculator.DoesAttackHit(target))
            {
                Console.WriteLine($"The {ItemName} hits the {target.UnitName}, dealing {attack_damage} damage!");
                SoundManager.enemy_hit.SmartPlay();
                target.HP -= attack_damage;
            }

            else
            {
                Console.WriteLine($"The {target.UnitName} narrowly avoids the {ItemName}!");
                SoundManager.attack_miss.SmartPlay();
            }
        }

        // Constructor
        public SingleTargetNuke(string name, string desc, int value, int damage, string item_id) :
            base(name, desc, value, bomb_mapping, true, item_id)
        {
            Damage = damage;
        }
    }

    public class MultiTargetNuke : Consumable
    {
        public int Damage { get; set; }

        private static readonly TargetMapping bomb_mapping = new TargetMapping(true, false, false, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            foreach (Monster the_monster in BattleManager.GetMonsterList())
            {
                int attack_damage = BattleCalculator.CalculateRawDamage(Damage, CEnums.Element.neutral, the_monster, CEnums.DamageType.physical);

                if (BattleCalculator.DoesAttackHit(the_monster))
                {
                    Console.WriteLine($"The {ItemName} hits the {the_monster.UnitName}, dealing {attack_damage} damage!");
                    SoundManager.enemy_hit.SmartPlay();
                    the_monster.HP -= attack_damage;
                }

                else
                {
                    Console.WriteLine($"The {the_monster.UnitName} narrowly avoids the {ItemName}!");
                    SoundManager.attack_miss.SmartPlay();
                }
            }
        }

        // Constructor
        public MultiTargetNuke(string name, string desc, int value, int damage, string item_id) :
            base(name, desc, value, bomb_mapping, true, item_id)
        {
            Damage = damage;
        }
    }

    public class XPGoldPotion : Consumable
    {
        public int GoldChange { get; set; }
        public int XPChange { get; set; }

        private static readonly TargetMapping xpgold_mapping = new TargetMapping(true, true, false, true);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            SoundManager.buff_spell.SmartPlay();

            if (GoldChange > 0)
            {
                Console.WriteLine($"{target.UnitName} gained {GoldChange} GP!");
            }

            else
            {
                Console.WriteLine($"{target.UnitName} lost {GoldChange} GP!");
            }

            if (XPChange > 0)
            {
                Console.WriteLine($"{target.UnitName} gained {XPChange} XP!");
            }

            else
            {
                Console.WriteLine($"{target.UnitName} lost {XPChange} XP!");
            }

            PlayableCharacter pcu = target as PlayableCharacter;

            // Apply the XP/GP changes
            pcu.CurrentXP += XPChange;
            CInfo.GP += GoldChange;

            // Make sure they don't go below 0
            pcu.CurrentXP = Math.Max(0, pcu.CurrentXP);
            CInfo.GP = Math.Max(0, CInfo.GP);

            // Check if the target leveled up
            pcu.CheckForLevelUp();
        }

        // Constructor
        public XPGoldPotion(string name, string desc, int value, int gold_change, int xp_change, string item_id) :
            base(name, desc, value, xpgold_mapping, true, item_id)
        {
            GoldChange = gold_change;
            XPChange = xp_change;
        }
    }

    public class GameCrashPotion : Consumable
    {
        private static readonly TargetMapping gamecrash_mapping = new TargetMapping(true, false, false, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            throw new DivideByZeroException("You asked for this.");
        }

        // Constructor
        public GameCrashPotion(string name, string desc, int value, string item_id) :
            base(name, desc, value, gamecrash_mapping, true, item_id)
        {

        }
    }

    public class MonsterEncyclopedia : Consumable
    {
        // Monster Encyclopedias are considered consumables, because they are
        // targeted and usable during battles

        private static readonly TargetMapping book_mapping = new TargetMapping(false, false, true, false);

        public override void PerformItemFunction(PlayableCharacter user, Unit target)
        {
            if (GameLoopManager.Gamestate != CEnums.GameState.battle)
            {
                Console.WriteLine("There are no monsters to identify!");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                CMethods.PrintDivider();
                SoundManager.buff_spell.SmartPlay();
                target.FixAllStats();
                Console.WriteLine($@"-{target.UnitName}'s Stats-
Level {target.Level}
Statuses: {string.Join(", ", target.Statuses.Select(x => x.EnumToString()))}

HP: {target.HP}/{target.TempStats.MaxHP} / MP: {target.MP}/{target.TempStats.MaxMP}
Physical: {target.TempStats.Attack} Attack / {target.TempStats.Defense} Defense
Magical: {target.TempStats.MAttack} Attack / {target.TempStats.MDefense} Defense
Piercing: {target.TempStats.PAttack} Attack / {target.TempStats.PDefense} Defense
Speed: {target.TempStats.Speed} / Evasion: {target.TempStats.Evasion}
Elements: Attacks are {target.OffensiveElement.EnumToString()} / Defense is {target.DefensiveElement.EnumToString()}
Weak to { target.DefensiveElement.GetElementalMatchup().Item1.EnumToString() } / Resistant to { target.DefensiveElement.GetElementalMatchup().Item2.EnumToString()}");
            }
        }

        public MonsterEncyclopedia(string name, string desc, int value, string item_id) :
            base(name, desc, value, book_mapping, false, item_id)
        {

        }
    }
    #endregion

    /* =========================== *
     *            TOOLS            *
     * =========================== */
    #region
    public class Shovel : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            if (GameLoopManager.Gamestate == CEnums.GameState.town)
            {
                Console.WriteLine("What, here? You can't just start digging up a town!");
                CMethods.PressAnyKeyToContinue();
                return;
            }

            List<Tuple<string, bool>> gem_list = TileManager.FindTileWithID(CInfo.CurrentTile).GemList;
            Item the_gem;
            try
            {
                the_gem = ItemManager.FindItemWithID(gem_list.First(x => !x.Item2).Item1);
            }

            catch (InvalidOperationException)
            {
                the_gem = null;
            }

            Console.WriteLine("Digging...");
            SoundManager.foot_steps.SmartPlay();
            CMethods.SmartSleep(825);

            Console.WriteLine("Digging...");
            SoundManager.foot_steps.SmartPlay();
            CMethods.SmartSleep(825);

            Console.WriteLine("Still digging...");
            SoundManager.foot_steps.SmartPlay();
            CMethods.SmartSleep(825);

            if (the_gem != null) 
            {
                SoundManager.unlock_chest.SmartPlay();
                Console.WriteLine($"Eureka, you found a {the_gem.ItemName}! It looks very valuable.");
                CMethods.PressAnyKeyToContinue();
                InventoryManager.AddItemToInventory(the_gem.ItemID);

                // Flag the gem as obtained
                gem_list.Remove(gem_list.First(x => x.Item1 == the_gem.ItemID && !x.Item2));
                gem_list.Add(new Tuple<string, bool>(the_gem.ItemID, true));
            }

            else
            {
                Console.WriteLine("No luck, your party didn't find anything.");
                CMethods.PressAnyKeyToContinue();
            }
        }

        // Constructor
        public Shovel(string name, string desc, int value, string item_id) : base(name, desc, value, true, CEnums.InvCategory.tools, item_id)
        {

        }
    }

    public class FastTravelAtlas : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            if (GameLoopManager.Gamestate == CEnums.GameState.town)
            {
                Console.WriteLine("You can't use the Fast Travel Atlas in a town!");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                ChooseProvince();
            }
        }

        private static void ChooseProvince()
        {
            List<Province> avail_provs = TileManager.GetProvinceList().Take(CInfo.AtlasStrength).ToList();

            if (avail_provs.Count == 1)
            {
                ChooseCell(avail_provs[0]);
                return;
            }

            while (true)
            {
                Console.WriteLine($"Available Provinces [Pages {CInfo.AtlasStrength}]/12: ");

                int counter = 0;
                foreach (Province prov in avail_provs)
                {
                    Console.WriteLine($"      [{counter + 1}] {prov.ProvinceName}");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", avail_provs.Count);
                    Province chosen;

                    try
                    {
                        chosen = avail_provs[int.Parse(choice) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (choice.IsExitString())
                        {
                            CMethods.PrintDivider();
                            return;
                        }

                        continue;
                    }

                    CMethods.PrintDivider();
                    ChooseCell(chosen);

                    return;
                }
            }
        }

        private static void ChooseCell(Province prov)
        {
            while (true)
            {
                Console.WriteLine($"Locations in {prov.ProvinceName}:");
                List<Cell> cell_list = prov.CellList.Select(TileManager.FindCellWithID).ToList();

                int counter = 0;
                foreach (Cell cell in cell_list)
                {
                    Console.WriteLine($"      [{counter + 1}] {cell.CellName}");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", cell_list.Count);
                    Cell chosen;

                    try
                    {
                        chosen = cell_list[int.Parse(choice) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (choice.IsExitString())
                        {
                            CMethods.PrintDivider();
                            return;
                        }

                        continue;
                    }

                    CMethods.PrintDivider();

                    if (WarpYesOrNo(chosen))
                    {
                        return;
                    }

                    break;
                }
            }
        }

        public static bool WarpYesOrNo(Cell cell)
        {
            while (true)
            {
                string yes_no = CMethods.SingleCharInput($"Warp to {cell.CellName}? ").ToLower();

                if (yes_no.IsYesString())
                {
                    DoTheWarp(cell);
                    return true;
                }

                if (yes_no.IsNoString())
                {
                    CMethods.PrintDivider();
                    return false;
                }
            }
        }

        public static void DoTheWarp(Cell cell)
        {
            if (CInfo.HasTeleported)
            {
                CMethods.PrintDivider();
                Console.WriteLine("Your party peers into the Fast Travel Atlas and begins to phase out of reality.");
                Console.WriteLine("Upon waking you're exactly where you wanted to be.");
                CMethods.PressAnyKeyToContinue();
            }

            else
            {
                CMethods.PrintDivider();
                Console.WriteLine("You begin to feel strange - your body feels light and all you hear is silence.");
                Console.WriteLine("Your vision starts going blank... All of your senses quickly turning off until");
                Console.WriteLine("you're left with nothing but your thoughts...");
                CMethods.PressAnyKeyToContinue();
                Console.WriteLine("...");
                CMethods.SmartSleep(1000);
                Console.WriteLine("...");
                CMethods.SmartSleep(1000);
                Console.WriteLine("...");
                CMethods.SmartSleep(1000);
                SoundManager.enemy_hit.SmartPlay();
                Console.WriteLine("CRASH! Your senses re-emerge as you land on... Oh, you're exactly where");
                Console.WriteLine("you wanted to be!");
                CMethods.PressAnyKeyToContinue();
            }

            CInfo.HasTeleported = true;
            CInfo.CurrentTile = cell.PrimaryTile;
            SoundManager.PlayCellMusic();
        }

        // Constructor
        public FastTravelAtlas(string name, string desc, int value, string item_id) : base(name, desc, value, true, CEnums.InvCategory.tools, item_id)
        {

        }
    }

    public class LockpickKit : Item
    {
        public int LockpickPower { get; set; }

        public override void UseItem(PlayableCharacter user)
        {
            Console.WriteLine($"You look at the {ItemName}.");
            Console.WriteLine("This could be used to open chests in dungeons. Or to rob houses...");
            CMethods.PressAnyKeyToContinue();
        }

        // Constructor
        public LockpickKit(string name, string desc, int value, int power, string item_id) : base(name, desc, value, true, CEnums.InvCategory.tools, item_id)
        {
            LockpickPower = power;
        }
    }

    public class PocketAlchemyLab : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            List<Ingredient> chosen_ingredients = new List<Ingredient>();
            Dictionary<CEnums.Flavor, List<Ingredient>> available_flavors = new Dictionary<CEnums.Flavor, List<Ingredient>>();

            foreach (Item item in InventoryManager.GetInventoryItems()[CEnums.InvCategory.misc])
            {
                if (item is Ingredient ingredient)
                {
                    if (available_flavors.ContainsKey(ingredient.Flavor))
                    {
                        available_flavors[ingredient.Flavor].Add(ingredient);
                    }

                    else
                    {
                        available_flavors[ingredient.Flavor] = new List<Ingredient>() { ingredient };
                    }
                }
            }

            if (available_flavors.Values.SelectMany(x => x).Count() < 3)
            {
                Console.WriteLine("You need at least three ingredients to make a potion!");
                CMethods.PressAnyKeyToContinue();

                return;
            }

            while (true)
            {
                Console.WriteLine("Flavors in your inventory: ");

                List<CEnums.Flavor> flavor_list = available_flavors.Keys.ToList();

                int counter = 0;
                foreach (CEnums.Flavor flavor in flavor_list)
                {
                    Console.WriteLine($"      [{counter + 1}] {flavor.EnumToString()}");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.SingleCharInput("Input [#] (or type 'exit'): ").ToLower();
                    List<Ingredient> chosen;

                    try
                    {
                        chosen = available_flavors[flavor_list[int.Parse(choice) - 1]];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (choice.IsExitString())
                        {
                            foreach (Ingredient ingredient in chosen_ingredients)
                            {
                                InventoryManager.AddItemToInventory(ingredient.ItemID);
                            }

                            return;
                        }

                        continue;
                    }

                    Tuple<bool, Ingredient> tuple = PickIngredient(chosen);
                    CMethods.PrintDivider();

                    if (tuple.Item1)
                    {
                        chosen_ingredients.Add(tuple.Item2);
                        available_flavors[tuple.Item2.Flavor].Remove(tuple.Item2);

                        if (available_flavors[tuple.Item2.Flavor].Count == 0)
                        {
                            available_flavors.Remove(tuple.Item2.Flavor);
                        }

                        Console.WriteLine($"Added a {tuple.Item2.ItemName} to the mix.");
                    }

                    if (chosen_ingredients.Count != 3)
                    {
                        Console.WriteLine($"{3 - chosen_ingredients.Count} ingredients left to pick!");

                        break;
                    }

                    else
                    {
                        Console.WriteLine("All ingredients added! Time to start brewing!");
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();

                        BrewPotion(chosen_ingredients);
                        return;
                    }
                }
            }
        }

        private static Tuple<bool, Ingredient> PickIngredient(List<Ingredient> ingredients)
        {
            CMethods.PrintDivider();
            Console.WriteLine($"{ingredients[0].Flavor.EnumToString()} Ingredients: ");

            int counter = 0;
            foreach (Ingredient ingredient in ingredients)
            {
                Console.WriteLine($"      [{counter + 1}] {ingredient.ItemName}");
                counter++;
            }

            while (true)
            {
                string choice = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", ingredients.Count);
                Ingredient chosen;

                try
                {
                    chosen = ingredients[int.Parse(choice) - 1];
                }

                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    if (choice.IsExitString())
                    {
                        return new Tuple<bool, Ingredient>(false, null);
                    }

                    continue;
                }

                InventoryManager.RemoveItemFromInventory(chosen.ItemID);

                return new Tuple<bool, Ingredient>(true, chosen);
            }
        }

        private static void BrewPotion(List<Ingredient> ingredients)
        {
            Dictionary<CEnums.Flavor, List<string>> flavor_map = new Dictionary<CEnums.Flavor, List<string>>()
            {
                { CEnums.Flavor.strange, new List<string>() { "pleasantpot1", "pleasantpot2", "pleasantpot2" } },
                { CEnums.Flavor.mystic, new List<string>() { "nastypot1", "nastypot2", "nastypot3" } },
                { CEnums.Flavor.rigid, new List<string>() { "grenadepot1", "grenadepot2", "grenadepot3" } },
                { CEnums.Flavor.flowing, new List<string>() { "missilepot1", "missilepot2", "missilepot3" } },
                { CEnums.Flavor.dark, new List<string>() { "greedpot1", "greedpot2", "greedpot3" } },
                { CEnums.Flavor.natural, new List<string>() { "temppot1", "temppot2", "temppot3" } },
                { CEnums.Flavor.mathematical, new List<string>() { "gamecrashpot", "gamecrashpot", "gamecrashpot" } }
            };

            List<CEnums.Flavor> added_flavors = ingredients.Select(x => x.Flavor).ToList();
            CEnums.Flavor chosen_flavor = CMethods.GetRandomFromIterable(added_flavors);
            int chosen_power = added_flavors.Count(x => x == chosen_flavor);
            string chosen_potion = flavor_map[chosen_flavor][chosen_power - 1];

            Console.WriteLine("Brewing...");
            SoundManager.potion_brew.SmartPlay();
            CMethods.SmartSleep(1000);
            Console.WriteLine("Brewing...");
            SoundManager.potion_brew.SmartPlay();
            CMethods.SmartSleep(1000);
            Console.WriteLine("Brewing...");
            SoundManager.potion_brew.SmartPlay();
            CMethods.SmartSleep(1000);

            SoundManager.unlock_chest.SmartPlay();
            InventoryManager.AddItemToInventory(chosen_potion);
            Console.WriteLine($"Success! You brewed a {ItemManager.FindItemWithID(chosen_potion).ItemName}!");
            CMethods.PressAnyKeyToContinue();
        }

        public PocketAlchemyLab(string name, string desc, int value, string item_id) :
            base(name, desc, value, false, CEnums.InvCategory.tools, item_id)
        {

        }
    }

    public class MusicBox : Item
    {
        public override void UseItem(PlayableCharacter user)
        {
            Console.WriteLine($"Musicbox is currently {(MusicPlayer.MusicboxIsPlaying ? "on" : "off")}");
            Console.WriteLine($"Musicbox is set to {MusicPlayer.MusicboxMode.EnumToString()}");

            if (string.IsNullOrEmpty(MusicPlayer.MusicboxFolder))
            {
                Console.WriteLine($"Musicbox is set to play music from {MusicPlayer.MusicboxFolder}/");
            }

            else
            {
                Console.WriteLine("Musicbox does not have a directory set");
            }

            ChooseOption();
        }

        private static void ChooseOption()
        {
            CMethods.PrintDivider();

            while (true)
            {
                Console.WriteLine("What should you do with the Musicbox?");
                Console.WriteLine($"      [1] Turn {(MusicPlayer.MusicboxIsPlaying ? "off" : "on")}");
                Console.WriteLine("      [2] Change play order");
                Console.WriteLine("      [3] Set music directory");

                while (true)
                {
                    string choice = CMethods.SingleCharInput("Input [#] (or type 'exit'): ");

                    if (choice == "1")
                    {
                        if (!string.IsNullOrEmpty(MusicPlayer.MusicboxFolder))
                        {
                            MusicPlayer.MusicboxIsPlaying = !MusicPlayer.MusicboxIsPlaying;

                            if (MusicPlayer.MusicboxIsPlaying)
                            {
                                MusicPlayer.MusicboxThread = new Thread(RunPlaylist);
                                MusicPlayer.StopMusic();
                                MusicPlayer.MusicboxThread.Start();
                            }

                            else
                            {
                                MusicPlayer.MusicboxThread.Abort();
                                SoundManager.PlayCellMusic();
                            }

                            CMethods.PrintDivider();
                            Console.WriteLine($"You turn {(MusicPlayer.MusicboxIsPlaying ? "on" : "off")} the musicbox");
                            CMethods.PressAnyKeyToContinue();
                            CMethods.PrintDivider();

                            break;
                        }

                        else
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine("You need to set a music directory first!");
                            CMethods.PressAnyKeyToContinue();
                            CMethods.PrintDivider();

                            break;
                        }
                    }

                    else if (choice == "2")
                    {
                        CMethods.PrintDivider();
                        ChoosePlayOrder();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice == "3")
                    {
                        CMethods.PrintDivider();
                        ChooseMusicDirectory();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice.IsExitString())
                    {
                        return;
                    }
                }
            }
        }

        private static void ChoosePlayOrder()
        {
            Console.WriteLine("Which setting do you want for the musicbox?");
            Console.WriteLine("      [1] A -> Z");
            Console.WriteLine("      [2] Z -> A");
            Console.WriteLine("      [3] Shuffle");

            while (true)
            {
                string choice = CMethods.SingleCharInput("Input [#] (or type 'exit'): ");

                if (choice.IsExitString())
                {
                    return;
                }

                else if (choice == "1")
                {
                    MusicPlayer.MusicboxMode = CEnums.MusicboxMode.AtoZ;
                    CMethods.PrintDivider();
                    Console.WriteLine("Musicbox set to play from A -> Z");
                }

                else if (choice == "2")
                {
                    MusicPlayer.MusicboxMode = CEnums.MusicboxMode.ZtoA;
                    CMethods.PrintDivider();
                    Console.WriteLine("Musicbox set to play from Z -> A");
                }

                else if (choice == "3")
                {
                    MusicPlayer.MusicboxMode = CEnums.MusicboxMode.shuffle;
                    CMethods.PrintDivider();
                    Console.WriteLine("Musicbox set to shuffle mode");
                }

                else 
                {
                    continue;
                }

                if (MusicPlayer.MusicboxIsPlaying)
                {
                    Console.WriteLine("You'll need to restart your musicbox to apply this change.");
                }

                CMethods.PressAnyKeyToContinue();

                return;
            }
        }

        private static void ChooseMusicDirectory()
        {
            Console.WriteLine("Note that the portable musicbox can only play .wav files");

            while (true)
            {
                string folder = CMethods.MultiCharInput("Type the directory path, type 'crawl', or type 'exit': ");

                if (folder.IsExitString())
                {
                    return;
                }

                if (folder.ToLower() == "crawl")
                {
                    CMethods.PrintDivider();
                    folder = SelectRoot();

                    if (string.IsNullOrEmpty(folder))
                    {
                        CMethods.PrintDivider();
                        continue;
                    }
                }

                if (!Directory.Exists(folder))
                {
                    CMethods.PrintDivider();
                    Console.WriteLine($"That is not a valid directory.");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();

                    continue;
                }

                CMethods.PrintDivider();
                if (Directory.GetFiles(folder).Any(x => x.EndsWith(".wav")))
                {
                    MusicPlayer.MusicboxFolder = folder;
                    Console.WriteLine($"Directory set to {folder}");

                    if (MusicPlayer.MusicboxIsPlaying)
                    {
                        Console.WriteLine("You'll need to restart your musicbox to apply this change.");
                    }

                    CMethods.PressAnyKeyToContinue();

                    return;
                }

                else 
                {
                    Console.WriteLine("Couldn't find any .wav files in that directory.");
                }
            }
        }

        private static string SelectRoot()
        {
            List<string> drive_list = DriveInfo.GetDrives().Select(x => x.Name).ToList();

            if (drive_list.Count > 1)
            {
                while (true)
                {
                    Console.WriteLine("Select a drive: ");

                    int counter = 0;
                    foreach (string drive in drive_list)
                    {
                        Console.WriteLine($"      [{counter + 1}] {drive}");
                        counter++;
                    }

                    while (true)
                    {
                        string chosen = CMethods.FlexibleInput("Input [#] (or type back): ", drive_list.Count);

                        try
                        {
                            chosen = drive_list[int.Parse(chosen) - 1];
                        }

                        catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                        {
                            if (chosen.IsExitString())
                            {
                                return "";
                            }

                            continue;
                        }

                        return FileExplorer(chosen);
                    }
                }
            }

            else 
            {
                return FileExplorer(drive_list[0]);
            }
        }
        
        private static string FileExplorer(string root)
        {
            List<string> current_path = new List<string>() { root };

            while (true)
            {
                CMethods.PrintDivider();

                List<string> available_dirs;

                try
                {
                    available_dirs = Directory.GetDirectories(string.Join("\\", current_path)).ToList();
                }

                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("You do not have permission to access that folder.");
                    CMethods.PressAnyKeyToContinue();
                    current_path.RemoveAt(current_path.Count - 1);

                    continue;
                }

                Console.WriteLine($"Current Path: {string.Join("\\", current_path)}");

                int counter = 0;
                foreach (string path in available_dirs)
                {
                    Console.WriteLine($"      [{counter + 1}] {path.Split('\\').Last()}");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.FlexibleInput("Input [#], type 'yes' to choose this folder, or type 'back': ", available_dirs.Count).ToLower();

                    try
                    {
                        choice = available_dirs[int.Parse(choice) - 1].Split('\\').Last();
                        current_path.Add(choice);

                        break;
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (choice.IsYesString())
                        {
                            return string.Join("\\", current_path);
                        }

                        else if (choice.IsExitString())
                        {
                            if (current_path.Count > 1)
                            {
                                current_path.RemoveAt(current_path.Count - 1);
                                break;
                            }

                            else
                            {
                                return "";
                            }
                        }
                    }
                }
            }
        }
        
        private static void RunPlaylist()
        {
            List<string> file_list = Directory.EnumerateFiles(MusicPlayer.MusicboxFolder).Where(x => x.EndsWith(".wav")).ToList();

            if (MusicPlayer.MusicboxMode == CEnums.MusicboxMode.AtoZ)
            {
                file_list = file_list.OrderBy(x => x).ToList();
            }

            if (MusicPlayer.MusicboxMode == CEnums.MusicboxMode.ZtoA)
            {
                file_list = file_list.OrderByDescending(x => x).ToList();
            }

            if (MusicPlayer.MusicboxMode == CEnums.MusicboxMode.shuffle)
            {
                file_list = CMethods.ShuffleIterable(file_list).ToList();
            }

            while (true)
            {
                foreach (string file in file_list)
                {
                    SoundPlayer player = new SoundPlayer();

                    try
                    {
                        player.SoundLocation = file;
                        player.Load();
                        player.PlaySync();
                    }

                    catch (Exception ex) when (ex is FileNotFoundException || ex is InvalidOperationException) { }

                    finally
                    {
                        player.Dispose();
                    }
                }
            }
        }
        
        public MusicBox(string name, string desc, int value, string item_id) : 
            base(name, desc, value, false, CEnums.InvCategory.tools, item_id)
        {

        }
    }
    #endregion
}
