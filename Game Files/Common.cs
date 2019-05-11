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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Game
{
    // Here we store classes that contain useful methods, enums, and data 
    // that would not fit in any other class
    public static class CMethods
    {
        private static readonly Random rng = new Random();

        // Input Methods
        public static string SingleCharInput(string prompt, bool arrow_keys_to_direction = false)
        {
            // Immediately returns the next key the user presses without them needing to press enter
            // Used when you KNOW the player will only have 9 or less options to choose from
            Console.Write(prompt);

            if (GameLoopManager.AutoPlay)
            {
                return DebugInput();
            }

            string character;
            ConsoleKeyInfo key = Console.ReadKey();

            if (arrow_keys_to_direction)
            {
                character = MapArrowKeysToDirection(key);
            }

            else
            {
                character = key.KeyChar.ToString();
            }

            Console.WriteLine();

            if (SettingsManager.do_blips)
            {
                SoundManager.item_pickup.SmartPlay();
            }

            return character;
        }

        public static string MultiCharInput(string prompt)
        {
            // Requires you to press enter before your string is accepted
            // Used when typing in a name or selecting from a list of 
            // more than 9 items
            Console.Write(prompt);

            if (GameLoopManager.AutoPlay)
            {
                return DebugInput();
            }

            string x = Console.ReadLine();

            if (SettingsManager.do_blips)
            {
                SoundManager.item_pickup.SmartPlay();
            }

            return x;
        }

        public static string FlexibleInput(string prompt, int option_count)
        {
            // FlexibleInput allows you to use SCI when the option list is small, and MCI
            // when it is big
            // Used when you don't know how many options there will be, such as when selecting
            // inventory items or spells
            if (option_count < 10)
            {
                return SingleCharInput(prompt);
            }

            return MultiCharInput(prompt);
        }

        public static void PressAnyKeyToContinue(string prompt = "\nPress any key to continue ")
        {
            // Use this when you don't care what key the user hits, you just need
            // them to hit one
            Console.Write(prompt);

            if (!GameLoopManager.AutoPlay)
            {
                Console.ReadKey(true);
            }

            if (SettingsManager.do_blips)
            {
                SoundManager.item_pickup.SmartPlay();
            }

            Console.WriteLine();
        }

        private static string DebugInput()
        {
            string chosen = GetRandomFromIterable("abcdefghijklmnopqrstuvwxyz1234567890").ToString();
            Console.WriteLine(chosen);
            return chosen;
        }

        private static string MapArrowKeysToDirection(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                return "n";
            }

            else if (key.Key == ConsoleKey.DownArrow)
            {
                return "s";
            }

            else if (key.Key == ConsoleKey.RightArrow)
            {
                return "e";
            }

            else if (key.Key == ConsoleKey.LeftArrow)
            {
                return "w";
            }

            else
            {
                return key.KeyChar.ToString();
            }
        }

        // Extension methods
        public static bool IsExitString(this string the_string)
        {
            List<string> ValidExitStrings = new List<string>() { "e", "x", "exit", "b", "back", "cancel" };
            return ValidExitStrings.Contains(the_string.ToLower());
        }

        public static bool IsYesString(this string the_string)
        {
            List<string> ValidYesStrings = new List<string>() { "y", "ye", "yes", "yup", "yeah", "ya", "yeh", "yah", "yea", "yeehaw" };
            return ValidYesStrings.Contains(the_string.ToLower());
        }

        public static bool IsNoString(this string the_string)
        {
            List<string> ValidNoStrings = new List<string>() { "n", "no", "nope", "nah", "nuh uh", "nay", "negative" };
            return ValidNoStrings.Contains(the_string.ToLower());
        }

        // Other Methods
        public static void SmartSleep(int milliseconds)
        {
            // Reduce the duration of the sleep to 0.1 seconds if debugging is set to true
            if (GameLoopManager.AutoPlay)
            {
                return;
            }

            else
            {
                Thread.Sleep(milliseconds);
            }

            // Clear the Key Buffer so that all inputs made during the Thread.Sleep() will be ignored\
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public static T GetRandomFromIterable<T>(IEnumerable<T> iterable)
        {
            return iterable.ToList()[rng.Next(iterable.Count())];
        }

        public static IList<T> ShuffleIterable<T>(IList<T> iterable)
        {
            int n = iterable.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = iterable[k];
                iterable[k] = iterable[n];
                iterable[n] = value;
            }

            return iterable;
        }

        public static void PrintDivider()
        {
            Console.WriteLine(new string(SettingsManager.divider_char, SettingsManager.divider_size));
        }

        public static List<string> SplitBy79(string the_string, int num = 79)
        {
            List<string> sentences = new List<string>();
            string current_sentence = "";

            foreach (string word in the_string.Split())
            {
                if ((current_sentence + word).Length > num)
                {
                    sentences.Add(Regex.Replace(current_sentence.Trim(), @"\s+", " "));
                    current_sentence = "";
                }

                current_sentence = string.Concat(new List<string>() { current_sentence, " ", word });
            }

            if (!string.IsNullOrEmpty(current_sentence))
            {
                sentences.Add(Regex.Replace(current_sentence.Trim(), @"\s+", " "));
            }

            return sentences;
        }

        public static void ReadStringListAsBook(List<string> pages, string title)
        {
            // Takes a list of strings (paragraphs/pages) and outputs one at a time.
            // You can cycle through them by pressing 1 (previous) and 2 (next)
            // Useful for dumping lore on the player, or implementing books
            int current_page = 0;

            while (true)
            {
                Console.WriteLine($"Page {current_page + 1} of {pages.Count} from '{title}'\n");
                Console.WriteLine(pages[current_page]);
                PrintDivider();

                if (current_page > 0)
                {
                    Console.WriteLine("      [1] Previous Page");
                }

                else
                {
                    Console.WriteLine("      [1] (There is no previous page)");
                }

                if (pages.Count - 1 > current_page)
                {
                    Console.WriteLine("      [2] Next Page");
                }
                
                else
                {
                    Console.WriteLine("      [2] (There is no next page)");
                }

                while (true)
                {
                    string choice = SingleCharInput("Input [#] (or type 'exit'): ");

                    if (choice == "1" && current_page > 0)
                    {
                        current_page--;
                        PrintDivider();
                        break;
                    }

                    else if (choice == "2" && pages.Count - 1 > current_page)
                    {
                        current_page++;
                        PrintDivider();
                        break;
                    }

                    else if (choice.IsExitString())
                    {
                        return;
                    }
                }
            }
        }

        public static double Clamp(double value, double min, double max)
        {
            // If value < min, returns min, if value > max, returns max. Otherwise, returns value
            // Used to impose both an upper and lower bound on Stats - for example, Evasion must be between 1 and 256.
            return Math.Max(min, Math.Min(max, value));
        }
    }

    public static class CEnums
    {
        public enum Status
        {
            [Description("Silenced")] silence = 0,
            [Description("Poisoned")] poison,
            [Description("Weakened")] weakness,
            [Description("Blinded")] blindness,
            [Description("Paralyzed")] paralyzation,
            [Description("Asleep")] sleep,
            [Description("Muted")] muted,
            [Description("Alive")] alive,
            [Description("Dead")] dead
        }

        public enum Element
        {
            [Description("Fire")] fire = 0,
            [Description("Water")] water,
            [Description("Electric")] electric,
            [Description("Earth")] earth,
            [Description("Wind")] wind,
            [Description("Grass")] grass,
            [Description("Ice")] ice,
            [Description("Light")] light,
            [Description("Dark")] dark,
            [Description("Neutral")] neutral,

            // This is used as the "weakness" and "resistance" of the Neutral element. 
            // It is not a real element and should not be assigned to monsters/items/pcus
            [Description("None")] none  
        }

        public enum CharacterClass
        {
            [Description("Warrior")] warrior = 0,
            [Description("Mage")] mage,
            [Description("Assassin")] assassin,
            [Description("Ranger")] ranger,
            [Description("Monk")] monk,
            [Description("Paladin")] paladin,
            [Description("Bard")] bard,
            [Description("Any")] any
        }

        public enum EquipmentType
        {
            [Description("Armor")] armor = 0,
            [Description("Weapon")] weapon,
            [Description("E. Accessory")] elem_accessory,
            [Description("Ammunition")] ammunition
        }

        public enum DamageType
        {
            [Description("Physical")] physical = 0,
            [Description("Piercing")] piercing,
            [Description("Magical")] magical
        }

        public enum GameState
        {
            [Description("Overworld")] overworld = 0,
            [Description("Battle")] battle,
            [Description("Town")] town
        }

        public enum MusicboxMode
        {
            [Description("A to Z")] AtoZ = 0,
            [Description("Z to A")] ZtoA,
            [Description("Shuffle")] shuffle
        }

        public enum SpellCategory
        {
            [Description("Buff")] buff = 0,
            [Description("Attack")] attack,
            [Description("Healing")] healing,
            [Description("All")] all
        }

        public enum InvCategory
        {
            [Description("Quest Items")] quest = 0,
            [Description("Consumables")] consumables,
            [Description("Weapons")] weapons,
            [Description("Armor")] armor,
            [Description("Tools")] tools,
            [Description("Accessories")] accessories,
            [Description("Misc Items")] misc
        }

        public enum MonsterGroup
        {
            [Description("Animal")] animal = 0,
            [Description("Monster")] monster,
            [Description("Humanoid")] humanoid,
            [Description("Undead")] undead,
            [Description("Dungeon")] dungeon
        }

        public enum PlayerAttribute
        {
            [Description("Strength")] strength = 0,
            [Description("Intelligence")] intelligence,
            [Description("Dexterity")] dexterity,
            [Description("Perception")] perception,
            [Description("Constitution")] constitution,
            [Description("Wisdom")] wisdom,
            [Description("Charisma")] charisma,
            [Description("Fate")] fate,
            [Description("Difficulty")] difficulty
        }

        public enum Flavor
        {
            [Description("Rigid")] rigid = 0,
            [Description("Flowing")] flowing,
            [Description("Dark")] dark,
            [Description("Mystic")] mystic,
            [Description("Natural")] natural,
            [Description("Strange")] strange,
            [Description("Mathematical")] mathematical
        }

        public enum SoundType
        {
            [Description("Sound FX")] soundfx,
            [Description("Music")] music
        }

        public static string EnumToString(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(value.GetType(), value);

            FieldInfo field = type.GetField(name);
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
            {
                return attr.Description;
            }

            ExceptionLogger.LogException($"Enum '{value}' does not have a description set.", new InvalidOperationException());
            return "ERROR";
        }

        public static Tuple<Element, Element> GetElementalMatchup(this Element element)
        {
            // element_matchup[element].Item1 is the Element that element is weak to
            // element_matchup[element].Item2 is the Element that element is resistant to
            Dictionary<Element, Tuple<Element, Element>> element_chart = new Dictionary<Element, Tuple<Element, Element>>
            {
                { Element.fire, new Tuple<Element, Element>(Element.water, Element.ice)},
                { Element.water, new Tuple<Element, Element>(Element.electric, Element.fire) },
                { Element.electric, new Tuple<Element, Element>(Element.earth, Element.water) },
                { Element.earth, new Tuple<Element, Element>(Element.wind, Element.electric) },
                { Element.wind, new Tuple<Element, Element>(Element.grass, Element.earth) },
                { Element.grass, new Tuple<Element, Element>(Element.ice, Element.wind) },
                { Element.ice, new Tuple<Element, Element>(Element.fire, Element.grass) },
                { Element.light, new Tuple<Element, Element>(Element.dark, Element.light) },
                { Element.dark, new Tuple<Element, Element>(Element.light, Element.dark) },
                { Element.neutral, new Tuple<Element, Element>(Element.none, Element.none) }
            };

            return element_chart[element];
        }

        public static string GetMonsterSong(this MonsterGroup m_group)
        {
            return new Dictionary<MonsterGroup, string>()
            {
                { MonsterGroup.animal, SoundManager.battle_music_animal },
                { MonsterGroup.humanoid, SoundManager.battle_music_humanoid },
                { MonsterGroup.monster, SoundManager.battle_music_monster },
                { MonsterGroup.undead, SoundManager.battle_music_undead },
                { MonsterGroup.dungeon, SoundManager.battle_music_dungeon }

            }[m_group];
        }
    }

    public static class CInfo
    {
        public static List<string> DefeatedBosses = new List<string>();
        public static int GP = 20;
        public static int Difficulty = 0;
        public static int AtlasStrength = 1;
        public static string CurrentTile = "nearton_s";
        public static string RespawnTile = "nearton_tile";
        public static bool DoSpawns = true;
        public static bool HasTeleported = false;
    }
}
