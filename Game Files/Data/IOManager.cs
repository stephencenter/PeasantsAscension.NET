﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Configuration;

namespace Data
{
    public static class SavefileManager
    {
        // Save Files
        public const string sav_gems = "gems.json";                      // Acquired Gems
        public const string sav_equipment = "equipment.json";            // Equipment
        public const string sav_inventory = "inventory.json";            // Inventory
        public const string sav_boss_flags = "boss_flags.json";          // Misc Boss Info
        public const string sav_game_info = "game_info.json";            // Party Info
        public const string sav_dialogue_flags = "dialogue_flags.json";  // Quests & Dialogue
        public const string sav_chests = "chests.json";                  // Chest Info

        // PCU Save Files
        public const string sav_player = "player_stats.json";    // Player Stats
        public const string sav_solou = "solou_stats.json";      // Solou's Stats
        public const string sav_chili = "chili_stats.json";      // Chili's Stats
        public const string sav_chyme = "chyme_stats.json";      // Chyme's Stats
        public const string sav_parsto = "parsto_stats.json";    // Parsto's Stats
        public const string sav_adorine = "adorine_stats.json";  // Adorine's Stats
        public const string sav_storm = "storm_stats.json";      // Storm's Stats
        public const string sav_kaltoh = "kaltoh_stats.json";    // Kaltoh's Stats

        public const string base_dir = "Save Files";
        public const string temp_dir = "temp";
        public static string adventure_name;

        public static void SetAdventureName()
        {
            // This function asks the player for an "adventure name". This is the
            // name of the directory in which his/her save files will be stored.

            while (true)
            {
                // Certain OSes don't allow certain characters, so this removes those characters
                // and replaces them with whitespace. The player is then asked if this is okay.
                string adventure = CMethods.MultiCharInput("Finally, what do you want to name this adventure? ");

                // This line removes all characters that are not alphanumeric, spaces, dashes, or underscores
                // We also remove repeated spaces like "Hello    world" => "Hello world"
                // Finally we .Trim() to remove leading or ending whitespace like "    Hello world    " => "Hello world"
                adventure = Regex.Replace(Regex.Replace(adventure, @"[^\w\s\-]*", ""), @"\s+", " ").Trim();

                // Make sure the adventure name isn't blank
                if (string.IsNullOrEmpty(adventure))
                {
                    continue;
                }

                // You also can't use "temp", because this is reserved for other features
                else if (adventure == "temp")
                {
                    Console.WriteLine("Please choose a different name, that one definitely won't do!");
                    CMethods.PressAnyKeyToContinue();
                    continue;
                }

                // Make sure that the folder doesn't already exist
                else if (Directory.Exists(adventure))
                {
                    Console.WriteLine("I've already read about adventures with that name; be original!");
                    CMethods.PressAnyKeyToContinue();
                    continue;
                }

                // Max adventure name length is 35
                else if (adventure.Length > 35)
                {
                    Console.WriteLine("That adventure name is far too long, it would never catch on!");
                    CMethods.PressAnyKeyToContinue();
                    continue;
                }

                while (true)
                {
                    string yes_no = CMethods.SingleCharInput($"You want your adventure to be remembered as '{adventure}'? [Y]es or [N]o: ").ToLower();

                    if (yes_no.IsYesString())
                    {
                        adventure_name = adventure;
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

        public static void WouldYouLikeToSave()
        {
            while (true)
            {
                string yes_no = CMethods.SingleCharInput("Do you wish to save your progress? [Y]es or [N]o: ").ToLower();

                if (yes_no.IsYesString())
                {
                    Console.WriteLine("Saving...");
                    CMethods.SmartSleep(100);
                    SaveTheGame();
                    Console.WriteLine("Game has been saved!");
                    CMethods.PressAnyKeyToContinue();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    return;
                }
            }
        }

        public static void SaveTheGame()
        {
            // Make sure there isn't already a temp directory
            if (Directory.Exists($"{base_dir}/{temp_dir}"))
            {
                Directory.Delete($"{base_dir}/{temp_dir}", true);
            }

            // Create a temp directory to store the save data in, so that if the saving fails data isn't corrupted
            Directory.CreateDirectory($"{base_dir}/{temp_dir}");

            // Save everything as JSON objects inside .json files, and store them in the temp directory
            JSONSerializer.SerializeEverything();

            // Delete the existing save file
            if (Directory.Exists($"{base_dir}/{adventure_name}"))
            {
                Directory.Delete($"{base_dir}/{adventure_name}", true);
            }

            // Create the save file folder
            Directory.CreateDirectory($"{base_dir}/{adventure_name}");

            // Move all the files from the temp directory to the save file folder
            foreach (string path in Directory.GetFiles($"{base_dir}/{temp_dir}", "*.json"))
            {
                string file = path.Split('\\').Last();
                File.Move(path, $"{base_dir}/{adventure_name}/{file}");
            }


            Directory.Delete($"{base_dir}/{temp_dir}", true);

            // Create a file with a disclaimer that warns against manually editing save files
            File.WriteAllText($"{base_dir}/README.txt", @"-IMPORTANT NOTE-
Editing these .json files is a VERY easy way to corrupt your save file.
Unless you are familiar with the inner-workings of the game and know
how to read/edit .json files, it's highly recommended that you turn away.");
        }

        public static void LoadTheGame()
        {
            while (true)
            {
                Console.WriteLine("Searching for existing save files...");
                CMethods.SmartSleep(100);

                if (!Directory.Exists(base_dir))
                {
                    NoSaveFilesFound();
                    return;
                }

                Dictionary<string, List<string>> save_files = new Dictionary<string, List<string>>();
                List<string> save_file_components = new List<string>()
                {
                    sav_gems,
                    sav_equipment,
                    sav_inventory,
                    sav_boss_flags,
                    sav_game_info,
                    sav_dialogue_flags,
                    sav_chests,
                    sav_player,
                    sav_solou,
                    sav_chili,
                    sav_chyme,
                    sav_parsto,
                    sav_adorine,
                    sav_storm,
                    sav_kaltoh
                };

                foreach (string path in Directory.GetDirectories(base_dir))
                {
                    if (save_file_components.All(x => File.Exists($"{path}/{x}")))
                    {
                        // ...then set the dictionary key equal to the newly-formatted save file names
                        string folder_name = path.Split('\\').Last();
                        save_files[folder_name] = save_file_components.Select(x => $"{base_dir}/{folder_name}/{x}").ToList();
                    }
                }

                if (save_files.Count == 0)
                {
                    NoSaveFilesFound();
                    return;
                }

                CMethods.PrintDivider();
                Console.WriteLine($"Found {save_files.Count} existing save files: ");

                // Print the list of save files
                foreach (Tuple<int, string> element in CMethods.Enumerate(save_files.Keys))
                {
                    Console.WriteLine($"      [{element.Item1 + 1}] {element.Item2}");
                }

                while (true)
                {
                    string chosen = CMethods.FlexibleInput("Input [#] (or type [c]reate new): ", save_files.Count);

                    try
                    {
                        adventure_name = save_files.Keys.ToList()[int.Parse(chosen) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        // Let the player create a new save file
                        if (chosen.StartsWith("c"))
                        {
                            CMethods.PrintDivider();
                            UnitManager.CreatePlayer();
                            return;
                        }

                        continue;
                    }

                    CMethods.PrintDivider();
                    Console.WriteLine($"Loading Save File: '{adventure_name}'...");
                    CMethods.SmartSleep(100);

                    if (!JSONDeserializer.DeserializeEverything())
                    {
                        break;
                    }

                    Console.WriteLine("Game loaded!");
                    return;
                }
            } 
        }

        public static void NoSaveFilesFound()
        {
            Console.WriteLine("No save files found. Starting new game...");
            CMethods.SmartSleep(100);
            CMethods.PrintDivider();
            UnitManager.CreatePlayer();
        }
    }

    public static class SettingsManager
    {
        // Settings
        public static float music_vol = 1;
        public static float sound_vol = 1;
        public static char divider_char = '-';
        public static int divider_size = 25;
        public static bool do_blips = true;

        public static void ApplySettings()
        {

        }

        public static void ChangeSoundVolume()
        {
            /*
            while True:
                c_volume = save_load.music_vol if mode == "music" else save_load.sound_vol

                print(f"{mode.title()} Volume determines how loud the {mode} is. 0 is silent, 100 is loud")
                print(f'{mode.title()} Volume is currently set to {int(c_volume*100)}%')

                do_thing = True
                while do_thing:
                    new_vol = main.s_input('Input # (or type "back"): ').ToLower()

                    if new_vol in ['e', 'x', 'exit', 'b', 'back']:
                        return
                    try:
                        // Convert the player's input into an integer between 0 and 100
                        new_vol = max(0, min(100, int(new_vol)))

                    except ValueError:
                        continue

                    print('-'*save_load.divider_size)
                    while True:
                        y_n = main.s_input(f"{mode.title()} Volume will be set to {new_vol}%, is that okay? | Y/N: ").ToLower()

                        if y_n.startswith("y"):
                            if mode == "music":
                                save_load.music_vol = new_vol/100
                                pygame.mixer.music.set_volume(new_vol/100)

                            else if mode == "sound":
                                save_load.sound_vol = new_vol/100
                                sounds.change_volume()

                            config = configparser.ConfigParser()

                            if not os.path.isfile("../settings.cfg"):
                                with open("../settings.cfg", mode= 'w') as f:
                                    f.write(save_load.settings_file)

                            config.read("../settings.cfg")
                            config.set("settings", f"{mode}_vol", str(new_vol))

                            with open("../settings.cfg", mode= "w") as g:
                                config.write(g)

                            print('-' * save_load.divider_size)
                            print(f'{mode.title()} Volume set to {new_vol}%.')
                            main.s_input("\nPress enter/return ")

                            return

                        else if y_n.startswith("n"):
                            print('-'*save_load.divider_size)
                            do_thing = False
                            break */

        }

        public static void ChangeDividerChar()
        {
            Console.WriteLine("Dividers are long strings of characters that seperate different bits of text.");
            Console.WriteLine("You can change the character that these dividers are made of.");
            Console.WriteLine($"Dividers are currently comprised of this character: {divider_char}\n");

            while (true)
            {
                char character = CMethods.SingleCharInput("Please input the new character you'd like to use (default is - ): ")[0];

                if (character < 33 || character > 126)
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("Sorry, that's not a valid character for the divider.");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();

                    continue;
                }

                divider_char = character;
                UpdateSetting("divider_char", divider_char.ToString());

                CMethods.PrintDivider();
                Console.WriteLine($"The divider character has been changed to {divider_char}.");
                CMethods.PressAnyKeyToContinue();

                return;
            }
        }

        public static void ChangeDividerSize()
        {
            Console.WriteLine("Dividers are long strings of characters that seperate different bits of text.");
            Console.WriteLine("You can change the number of characters. Max is 79, min is 5.");
            Console.WriteLine($"Current divider size: {divider_size} characters\n");

            while (true)
            {
                string new_size = CMethods.MultiCharInput("How long should the dividers be? (default is 25): ");

                if (!int.TryParse(new_size, out int true_new_size))
                {
                    continue;
                }

                if (true_new_size > 79 || true_new_size < 5)
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("Number must be between 5 and 79.");
                    CMethods.PressAnyKeyToContinue();
                    CMethods.PrintDivider();

                    continue;
                }

                divider_size = true_new_size;
                UpdateSetting("divider_size", divider_size.ToString());

                CMethods.PrintDivider();
                Console.WriteLine($"Divider Size set to {true_new_size}.");
                CMethods.PressAnyKeyToContinue();

                return;
            }
        }

        public static void ToggleBlips()
        {
            Console.WriteLine("Blips are the sounds that the game make when you press enter.");
            Console.WriteLine("They can get annoying, so you have the option to turn them off.");
            Console.WriteLine($"Blips are currently {(do_blips ? "enabled" : "disabled")}\n");

            var appSettings = ConfigurationManager.AppSettings;

            while (true)
            {
                string yes_no = CMethods.SingleCharInput("Toggle Blips? [Y]es or [N]o: ");

                if (yes_no.IsYesString())
                {
                    if (do_blips)
                    {
                        SoundManager.item_pickup.Stop();
                    }

                    else
                    {
                        SoundManager.item_pickup.SmartPlay();
                    }

                    do_blips = !do_blips;
                    UpdateSetting("do_blips", do_blips.ToString());

                    CMethods.PrintDivider();
                    Console.WriteLine($"Blips are now {(do_blips ? "enabled" : "disabled")}.");
                    CMethods.PressAnyKeyToContinue();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    return;
                }
            }
        }

        private static void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }

    public static class ExceptionLogger
    {
        public static void LogException(string error_desc, Exception ex)
        {
            using (StreamWriter file = new StreamWriter("error_history.log", true))
            {
                file.WriteLine($"-------------------------");
                file.WriteLine($"{error_desc} at {GetCurrentDate()} using version {CInfo.GameVersion}");
                file.WriteLine($"    {ex.Message}\n");
                file.WriteLine($"{ex.StackTrace}");
            }
        }

        private static string GetCurrentDate()
        {
            return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }

    public static class JSONSerializer
    {
        public static bool SerializeEverything()
        {
            try
            {
                SerializeGems();
                SerializeEquipment();
                SerializeInventory();
                SerializeDialogueFlags();
                SerializeBossFlags();
                SerializeChestFlags();
                SerializePartyMemebers();
                SerializeGameInfo();

                return true;
            }

            catch (Exception ex)
            {
                ExceptionLogger.LogException("Error saving game", ex);
                Console.WriteLine("There was an error saving. Error message can be found in error_history.log");
                CMethods.PressAnyKeyToContinue();

                return false;
            }
        }

        private static void SerializeGameInfo()
        {
            Dictionary<string, dynamic> game_info = new Dictionary<string, dynamic>()
            {
                { "musicbox_mode", CInfo.MusicboxMode },
                { "defeated_bosses", CInfo.DefeatedBosses },
                { "gp", CInfo.GP },
                { "difficulty", CInfo.Difficulty },
                { "atlas_strength", CInfo.AtlasStrength },
                { "musicbox_folder", CInfo.MusicboxFolder },
                { "current_tile", CInfo.CurrentTile },
                { "respawn_tile", CInfo.RespawnTile },
                { "do_spawns", CInfo.DoSpawns },
                { "has_teleported", CInfo.HasTeleported }
            };

            string gameinfo_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_game_info}";
            File.WriteAllText(gameinfo_string, JsonConvert.SerializeObject(game_info, Formatting.Indented));
        }

        private static void SerializePartyMemebers()
        {
            // We have to reset some values to null, because they are unserializable
            UnitManager.ResetTemporaryProperties();

            string player_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_player}";
            File.WriteAllText(player_string, JsonConvert.SerializeObject(UnitManager.player, Formatting.Indented));

            string solou_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_solou}";
            File.WriteAllText(solou_string, JsonConvert.SerializeObject(UnitManager.solou, Formatting.Indented));

            string chili_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_chili}";
            File.WriteAllText(chili_string, JsonConvert.SerializeObject(UnitManager.chili, Formatting.Indented));

            string chyme_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_chyme}";
            File.WriteAllText(chyme_string, JsonConvert.SerializeObject(UnitManager.chyme, Formatting.Indented));

            string parsto_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_parsto}";
            File.WriteAllText(parsto_string, JsonConvert.SerializeObject(UnitManager.parsto, Formatting.Indented));

            string adorine_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_adorine}";
            File.WriteAllText(adorine_string, JsonConvert.SerializeObject(UnitManager.adorine, Formatting.Indented));

            string storm_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_storm}";
            File.WriteAllText(storm_string, JsonConvert.SerializeObject(UnitManager.storm, Formatting.Indented));

            string kaltoh_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_kaltoh}";
            File.WriteAllText(kaltoh_string, JsonConvert.SerializeObject(UnitManager.kaltoh, Formatting.Indented));
        }

        private static void SerializeInventory()
        {
            string inventory_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_inventory}";
            File.WriteAllText(inventory_string, JsonConvert.SerializeObject(InventoryManager.inventory, Formatting.Indented));
        }

        private static void SerializeEquipment()
        {
            string equipment_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_equipment}";
            File.WriteAllText(equipment_string, JsonConvert.SerializeObject(InventoryManager.equipment, Formatting.Indented));
        }

        private static void SerializeDialogueFlags()
        {
            string dialogue_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_dialogue_flags}";
            File.WriteAllText(dialogue_string, "to-do!!");
        }

        private static void SerializeBossFlags()
        {
            string boss_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_boss_flags}";
            File.WriteAllText(boss_string, "to-do!!");
        }

        private static void SerializeChestFlags()
        {
            string chest_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_chests}";
            File.WriteAllText(chest_string, "to-do!!");
        }

        private static void SerializeGems()
        {
            string gem_string = $"{SavefileManager.base_dir}/{SavefileManager.temp_dir}/{SavefileManager.sav_gems}";
            File.WriteAllText(gem_string, "to-do!!");
        }
    }

    public static class JSONDeserializer
    {
        public static bool DeserializeEverything()
        {
            try
            {
                DeserializeGems();
                DeserializeEquipment();
                DeserializeInventory();
                DeserializeDialogueFlags();
                DeserializeBossFlags();
                DeserializeChestFlags();
                DeserializePartyMemebers();
                DeserializeGameInfo();

                return true;
            }

            catch (Exception ex)
            {
                ExceptionLogger.LogException("Error loading game", ex);
                Console.WriteLine("There was an error loading the game. Error message can be found in error_history.log");
                CMethods.PressAnyKeyToContinue();

                return false;
            }
        }

        private static void DeserializeGameInfo()
        {
            string gameinfo_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_game_info}";
            Dictionary<string, dynamic> game_info = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(gameinfo_string));

            CInfo.MusicboxMode = (CEnums.MusicboxMode)game_info["musicbox_mode"];
            CInfo.DefeatedBosses = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(game_info["defeated_bosses"]));
            CInfo.GP = (int)game_info["gp"];
            CInfo.Difficulty = (int)game_info["difficulty"];
            CInfo.AtlasStrength = (int)game_info["atlas_strength"];
            CInfo.MusicboxFolder = game_info["musicbox_folder"];
            CInfo.CurrentTile = game_info["current_tile"];
            CInfo.RespawnTile = game_info["respawn_tile"];
            CInfo.DoSpawns = game_info["do_spawns"];
            CInfo.HasTeleported = game_info["has_teleported"];
        }

        private static void DeserializePartyMemebers()
        {
            string player_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_player}";
            UnitManager.player = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(player_string));

            string solou_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_solou}";
            UnitManager.solou = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(solou_string));

            string chili_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_chili}";
            UnitManager.chili = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(chili_string));

            string chyme_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_chyme}";
            UnitManager.chyme = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(chyme_string));

            string parsto_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_parsto}";
            UnitManager.parsto = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(parsto_string));

            string adorine_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_adorine}";
            UnitManager.adorine = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(adorine_string));

            string storm_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_storm}";
            UnitManager.storm = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(storm_string));

            string kaltoh_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_kaltoh}";
            UnitManager.kaltoh = JsonConvert.DeserializeObject<PlayableCharacter>(File.ReadAllText(kaltoh_string));
        }

        private static void DeserializeInventory()
        {
            string inventory_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_inventory}";
            InventoryManager.inventory = JsonConvert.DeserializeObject<Dictionary<CEnums.InvCategory, List<string>>>(File.ReadAllText(inventory_string));
        }

        private static void DeserializeEquipment()
        {
            string equipment_string = $"{SavefileManager.base_dir}/{SavefileManager.adventure_name}/{SavefileManager.sav_equipment}";
            InventoryManager.equipment = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<CEnums.EquipmentType, string>>>(File.ReadAllText(equipment_string));
        }

        private static void DeserializeDialogueFlags()
        {

        }

        private static void DeserializeBossFlags()
        {

        }

        private static void DeserializeChestFlags()
        {

        }

        private static void DeserializeGems()
        {

        }
    }
}