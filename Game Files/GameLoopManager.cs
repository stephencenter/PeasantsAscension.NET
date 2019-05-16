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

namespace Game
{
    public static class GameLoopManager
    {
        public static int StepsWithoutBattle = 0;
        public static CEnums.GameState Gamestate = CEnums.GameState.overworld;
        public static bool AutoPlay = false;
        public static readonly bool FullRelease = false;
        public const string GameVersion = "v0.1";

        /* =========================== *
         *        INITIALIZATION       *
         * =========================== */
        #region
        public static void RunChecks()
        {
            // RunChecks() are some simple tests that help catch simple programming errors,
            // such as using an ID multiple times or using an invalid ID.
            // This method is NOT ran if FullRelease is set to true, because running 
            // these checks causes the game to take longer to load up, and all of the issues
            // this method checks for should be fixed before release anyway.

            if (FullRelease)
            {
                AutoPlay = false;
                return;
            }

            /* =========================== *
             *        MONSTER CHECKS       *
             * =========================== */
            // Check that all monsters have real items assigned to them
            foreach (Monster monster in UnitManager.MonsterTypes)
            {
                foreach (string item_id in monster.DropList.Select(x => x.Item1))
                {
                    if (!ItemManager.VerifyItemExists(item_id))
                    {
                        Console.WriteLine($"{monster.UnitName} has invalid item_id '{item_id}' listed as a droppable item!");
                    }
                }
            }

            /* =========================== *
             *         TILE CHECKS         *
             * =========================== */
            // Check to make sure all directions for all tiles correspond to real tiles
            foreach (Tile tile in TileManager.GetTileList())
            {
                foreach (string tile_id in new List<string> { tile.ToNorth, tile.ToSouth, tile.ToEast, tile.ToWest })
                {
                    if (tile_id != null && !TileManager.VerifyTileExists(tile_id))
                    {
                        Console.WriteLine($"{tile.TileID} has an invalid direction ({tile_id})!");
                    }
                }
            }

            // Check to make sure all TileIDs in use are unique
            IEnumerable<string> tile_ids = TileManager.GetTileList().Select(x => x.TileID);
            foreach (string tile_id in tile_ids)
            {
                if (tile_ids.Count(x => x == tile_id) > 1)
                {
                    Console.WriteLine($"{tile_id} is being used as a Tile ID for multiple tiles!");
                }
            }

            // Check to make sure no tiles "lead to themselves", e.g. tile.ToNorth == tile.TileID
            foreach (Tile tile in TileManager.GetTileList())
            {
                foreach (string direction in new List<string>() { tile.ToNorth, tile.ToSouth, tile.ToEast, tile.ToWest })
                {
                    if (direction == tile.TileID)
                    {
                        Console.WriteLine($"One of {tile.TileID}'s directions leads to itself.");
                    }
                }
            }

            // Check to make sure there are no one-way passages, e.g. tile.ToNorth != FindTileWithID(tile.ToNorth).ToSouth
            foreach (Tile tile in TileManager.GetTileList())
            {
                string from_north = tile.ToNorth != null ? TileManager.FindTileWithID(tile.ToNorth).ToSouth : null;
                string from_south = tile.ToSouth != null ? TileManager.FindTileWithID(tile.ToSouth).ToNorth : null;
                string from_east = tile.ToEast != null ? TileManager.FindTileWithID(tile.ToEast).ToWest : null;
                string from_west = tile.ToWest != null ? TileManager.FindTileWithID(tile.ToWest).ToEast : null;

                if ((from_north != null && tile.TileID != from_north)
                    || (from_south != null && tile.TileID != from_south)
                    || (from_east != null && tile.TileID != from_east)
                    || (from_west != null && tile.TileID != from_west))
                {
                    Console.WriteLine($"{tile.TileID} has a one-way passage.");
                }
            }

            /* =========================== *
             *         ITEM CHECKS         *
             * =========================== */
            // Check to make sure all ItemIDs in use are unique
            IEnumerable<string> item_ids = ItemManager.GetItemList().Select(x => x.ItemID);
            foreach (string item_id in item_ids)
            {
                if (item_ids.Count(x => x == item_id) > 1)
                {
                    Console.WriteLine($"{item_id} is being used as an Item ID for multiple items!");
                }
            }

            /* =========================== *
             *          NPC CHECKS         *
             * =========================== */
            // Check to make sure all NPC IDs are unique
            IEnumerable<string> npc_ids = NPCManager.GetNPCList().Select(x => x.NPCID);
            foreach (string npc_id in npc_ids)
            {
                if (npc_ids.Count(x => x == npc_id) > 1)
                {
                    Console.WriteLine($"{npc_id} is being used as an NPC ID for multiple npcs!");
                }
            }

            // Check to make sure all NPCs have valid dialogue
            foreach (NPC npc in NPCManager.GetNPCList())
            {
                foreach (List<string> conv_ids in npc.Conversations.Values)
                {
                    foreach (string conv_id in conv_ids)
                    {
                        if (!DialogueManager.VerifyConvoExists(conv_id))
                        {
                            Console.WriteLine($"{npc.NPCID} has invalid dialogue '{conv_id}' listed as a conversation!");
                        }
                    }
                }
            }

            // Check to make sure all NPCs are being used
            foreach (NPC npc in NPCManager.GetNPCList())
            {
                bool found = false;
                foreach (Town town in TownManager.GetTownList())
                {
                    if (town.People.Contains(npc.NPCID))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"NPC {npc.NPCID} is unused, is this a mistake?");
                }
            }

            /* =========================== *
             *         TOWN CHECKS         *
             * =========================== */
            // Check to make sure all ItemIDs used in general store stocks are valid items
            foreach (MarketTown town in TownManager.GetTownList().Where(x => x is MarketTown).Select(x => x as MarketTown))
            {
                foreach (string item_id in town.GenStock)
                {
                    if (!ItemManager.VerifyItemExists(item_id))
                    {
                        Console.WriteLine($"{town.TownID}'s stock has invalid item '{item_id}' listed as an item!");
                    }
                }
            }

            // Check to make sure all ItemIDs used in general store stocks are only used once
            foreach (MarketTown town in TownManager.GetTownList().Where(x => x is MarketTown).Select(x => x as MarketTown))
            {
                foreach (string item_id in town.GenStock)
                {
                    if (town.GenStock.Count(x => x == item_id) > 1)
                    {
                        Console.WriteLine($"{town.TownID}'s stock has '{item_id}' listed more than once!");
                    }
                }
            }
            
            // Check to make sure every town has valid NPC IDs attached to them
            foreach (Town town in TownManager.GetTownList())
            {
                foreach (string npc_id in town.People)
                {
                    if (!NPCManager.VerifyNPCExists(npc_id))
                    {
                        Console.WriteLine($"Town {town.TownID} has invalid NPC {npc_id} listed as an NPC!");
                    }
                }
            }
        }

        public static void SetConsoleProperties()
        {
            Console.Title = "Peasant's Ascension";
            Console.WindowHeight = 25;
            Console.BufferHeight = Math.Max(Console.BufferHeight, 75);
            Console.WindowWidth = 85;
            Console.BufferWidth = 85;
        }
        #endregion

        /* =========================== *
         *         TITLE SCREEN        *
         * =========================== */
        #region
        public static void DisplayTitlescreen()
        {
            string title_card = $@"  _____                           _
 |  __ \                         | |
 | |__) |__  __ _ ___  __ _ _ __ | |_ ___       
 |  ___/ _ \/ _` / __|/ _` | '_ \| __/ __|      
 | |  |  __/ (_| \__ \ (_| | | | | |_\__ \
 |_|   \___|\__,_|___/\__,_|_| |_|\__|___/ 
     /\
    /  \   ___  ___ ___ _ __  ___ _  ___  _ __  
   / /\ \ / __|/ __/ _ \ '_ \/ __| |/ _ \| '_ \ 
  / ____ \\__ \ (_|  __/ | | \__ \ | (_) | | | |
 /_/    \_\___/\___\___|_| |_|___/_|\___/|_| |_|
Peasant's Ascension {GameVersion} -- A Text-RPG by Stephen Center
Licensed under the GNU GPLv3: [https://www.gnu.org/copyleft/gpl.html]
Check here often for updates: [http://www.reddit.com/r/PeasantsAscension/]";
            MusicPlayer.PlaySong(SoundManager.title_music, -1);

            Console.WriteLine(title_card);
            CMethods.PrintDivider();

            while (true)
            {
                // Give the user a choice of keys to press to do specific actions
                string choice = CMethods.SingleCharInput("[P]lay Game | [S]ettings | [C]redits | [E]xit | Input Letter: ").ToLower();

                if (choice.StartsWith("p"))
                {
                    CMethods.PrintDivider();
                    return;
                }

                if (choice.StartsWith("s") && !AutoPlay)
                {
                    ConfigCommand();
                    Console.WriteLine(title_card);
                    CMethods.PrintDivider();
                }

                if (choice.StartsWith("c") && !AutoPlay)
                {
                    ShowCredits();
                    Console.WriteLine(title_card);
                    CMethods.PrintDivider();
                }

                if (choice.StartsWith("e") && !AutoPlay)
                {
                    Environment.Exit(1);
                }
            }
        }

        public static void ShowCredits()
        {
            CMethods.PrintDivider();

            try
            {
                List<string> credits = File.ReadAllLines("credits.txt").ToList();
                MusicPlayer.PlaySong(SoundManager.credits_music, -1);

                foreach (string line in credits)
                {
                    Console.WriteLine(line);
                    if (!string.IsNullOrWhiteSpace(line) && line != credits.Last())
                    {
                        CMethods.SmartSleep(2000);
                    }
                }

                CMethods.PressAnyKeyToContinue();
                CMethods.PrintDivider();
                MusicPlayer.PlaySong(SoundManager.title_music, -1);
            }

            catch (Exception ex)
            {
                // Display this is the Credits.txt file couldn't be found
                ExceptionLogger.LogException("Error finding credits.txt", ex);
                Console.WriteLine("The file 'credits.txt' could not be found.");
                CMethods.PressAnyKeyToContinue();
                CMethods.PrintDivider();
            }
        }

        public static void ExplainTheSetting()
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
        #endregion

        /* =========================== *
         *          GAME LOOP          *
         * =========================== */
        #region
        public static void MainGameLoop()
        {
            SoundManager.PlayCellMusic();

            while (true)
            {
                if (!TownManager.SearchForTowns(true))
                {
                    CMethods.PrintDivider();
                }

                DisplayGameUI();
                List<Tuple<char, string>> available_dirs = GetAvailableDirections();

                while (true)
                {
                    Gamestate = CEnums.GameState.overworld;
                    string command = CMethods.SingleCharInput("Input Command (type 'help' to view command list): ", true).ToLower();

                    if (command == "~")
                    {
                        CheatCommand();
                    }

                    else if (available_dirs.Any(x => command.StartsWith(x.Item1.ToString())))
                    {
                        MoveCommand(available_dirs, command[0]);
                        break;
                    }

                    else if (command.StartsWith("p"))
                    {
                        PlayerStatsCommand();
                    }

                    else if (command.StartsWith("m"))
                    {
                        MagicCommand();
                    }

                    else if (command.StartsWith("i"))
                    {
                        InventoryManager.PickInventoryCategory();
                    }

                    else if (command.StartsWith("t"))
                    {
                        ToolsCommand();
                    }

                    else if (command.StartsWith("l"))
                    {
                        LookCommand();
                    }

                    else if (command.StartsWith("r"))
                    {
                        RecheckCommand();
                    }

                    else if (command.StartsWith("c"))
                    {
                        ConfigCommand();
                    }

                    else if (command.StartsWith("u"))
                    {
                        InventoryManager.PickEquipmentItem();
                    }

                    else if (command.StartsWith("q"))
                    {
                        InventoryManager.ViewQuests();
                    }

                    else if (command.StartsWith("h"))
                    {
                        HelpCommand();
                    }

                    else
                    {
                        continue;
                    }

                    DisplayGameUI();

                    available_dirs = GetAvailableDirections();
                }
            }
        }

        private static void DisplayGameUI()
        {
            Tile tile = TileManager.FindTileWithID(CInfo.CurrentTile);
            List<Tuple<char, string>> available_dirs = GetAvailableDirections();

            Console.WriteLine("-CURRENT LOCATION-");
            Console.WriteLine($"{tile.GenerateAsciiArt()}\n");
            Console.WriteLine($"Region [{tile.Name}] | Province: [{TileManager.FindProvinceWithTileID(tile.TileID).ProvinceName}]");

            foreach (Tuple<char, string> direction in available_dirs)
            {
                if (direction.Item1 == 'n')
                {
                    Console.Write("    To the [N]orth");
                }

                else if (direction.Item1 == 's')
                {
                    Console.Write("    To the [S]outh");
                }

                else if (direction.Item1 == 'e')
                {
                    Console.Write("    To the [E]ast");
                }

                else if (direction.Item1 == 'w')
                {
                    Console.Write("    To the [W]est");
                }

                string adj_tile = TileManager.FindTileWithID(direction.Item2).Name;
                Console.WriteLine($" lies the {adj_tile}");
            }
        }

        private static List<Tuple<char, string>> GetAvailableDirections()
        {
            List<Tuple<char, string>> available_dirs = new List<Tuple<char, string>>();
            Tile tile = TileManager.FindTileWithID(CInfo.CurrentTile);

            // Tells the player which directions are available to go in
            foreach (string location in new List<string>() { tile.ToNorth, tile.ToSouth, tile.ToEast, tile.ToWest })
            {
                if ((location == tile.ToNorth) && (location != null))
                {
                    available_dirs.Add(new Tuple<char, string>('n', location));
                }

                else if ((location == tile.ToSouth) && (location != null))
                {
                    available_dirs.Add(new Tuple<char, string>('s', location));
                }

                else if ((location == tile.ToEast) && (location != null))
                {
                    available_dirs.Add(new Tuple<char, string>('e', location));
                }

                else if ((location == tile.ToWest) && (location != null))
                {
                    available_dirs.Add(new Tuple<char, string>('w', location));
                }
            }

            return available_dirs;
        }
        #endregion

        /* =========================== *
         *      OVERWORLD COMMANDS     *
         * =========================== */
        #region
        public static void MoveCommand(List<Tuple<char, string>> available_dirs, char direction)
        {
            Random rng = new Random();
            SoundManager.foot_steps.SmartPlay();

            CInfo.CurrentTile = available_dirs.Single(x => x.Item1 == direction).Item2;

            // If none of these fucntions return True, then a battle can occur.
            if (!UnitManager.CheckForBosses() && !TownManager.SearchForTowns(false))
            {
                // There is a 1 in 4 chance for a battle to occur (25%)
                // However, a battle cannot occur if the number of steps since the last battle is less than three,
                // and is guaranteed to occur if the number of steps is above 10.
                bool is_battle = rng.Next(0, 3) == 0;

                if (StepsWithoutBattle > 10)
                {
                    is_battle = true;
                }

                else if (StepsWithoutBattle < 3)
                {
                    is_battle = false;
                }

                // It is possible to disable spawns using cheats, and some cells have monster permanantly disabled
                if (is_battle && CInfo.DoSpawns && TileManager.FindCellWithTileID(CInfo.CurrentTile).MonstersEnabled)
                {
                    CMethods.PrintDivider();
                    StepsWithoutBattle = 0;
                    int highest_perception = UnitManager.GetAllPCUs().Max(x => x.Attributes[CEnums.PlayerAttribute.perception]);

                    if (highest_perception > rng.Next(0, 100))
                    {
                        Console.WriteLine($"You see a monster - it has not detected you yet.");

                        while (true)
                        {
                            string yes_no = CMethods.SingleCharInput("Fight it? ");

                            if (yes_no.IsYesString())
                            {
                                CMethods.PrintDivider();
                                BattleManager.BattleSystem(false);
                            }

                            else if (yes_no.IsNoString())
                            {
                                break;
                            }
                        }
                    }

                    else
                    {
                        BattleManager.BattleSystem(false);
                    }
                }

                else
                {
                    StepsWithoutBattle++;
                }
            }
        }

        public static void MagicCommand()
        {
            TargetMapping t_map = new TargetMapping(true, true, false, true);

            if (!UnitManager.player.PlayerChooseTarget("Choose a spellbook: ", t_map))
            {
                return;
            }

            PlayableCharacter caster = UnitManager.player.CurrentTarget as PlayableCharacter;

            if (SpellManager.GetCasterSpellbook(caster, CEnums.SpellCategory.healing).Count > 0)
            {
                SpellManager.PickSpell(caster, CEnums.SpellCategory.healing);
            }

            else
            {
                CMethods.PrintDivider();
                Console.WriteLine($"{caster.UnitName} has no overworld spells in their spellbook.");
                CMethods.PressAnyKeyToContinue();
            }
        }

        public static void LookCommand()
        {
            CMethods.PrintDivider();
            Console.WriteLine(TileManager.FindTileWithID(CInfo.CurrentTile).Description);
            CMethods.PressAnyKeyToContinue();
            CMethods.PrintDivider();
        }

        public static void PlayerStatsCommand()
        {
            TargetMapping t_map = new TargetMapping(true, true, false, true);

            if (!UnitManager.player.PlayerChooseTarget("Select a character to view stats for: ", t_map))
            {
                return;
            }

            CMethods.PrintDivider();
            PlayableCharacter pcu = UnitManager.player.CurrentTarget as PlayableCharacter;
            pcu.PlayerViewStats();
            CMethods.PrintDivider();
        }

        public static void ConfigCommand()
        {
            if (AutoPlay)
            {
                return;
            }

            CMethods.PrintDivider();

            while (true)
            {
                Console.WriteLine($@"Config Menu:
      [1] Music Volume --> Currently set to {(int)(SettingsManager.music_vol*100)}%
      [2] Sound Volume --> Currently set to {(int)(SettingsManager.sound_vol*100)}%
      [3] Divider Char --> Currently set to {SettingsManager.divider_char}
      [4] Divider Size --> Currently set to {SettingsManager.divider_size} characters
      [5] Toggle Blips --> Currently {(SettingsManager.do_blips ? "enabled" : "disabled")}");

                while (true)
                {
                    string setting = CMethods.SingleCharInput("Input [#] (or type 'exit'): ").ToLower();

                    if (setting == "1")
                    {
                        CMethods.PrintDivider();
                        SettingsManager.ChangeSoundVolume(CEnums.SoundType.music);
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (setting == "2")
                    {
                        CMethods.PrintDivider();
                        SettingsManager.ChangeSoundVolume(CEnums.SoundType.soundfx);
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (setting == "3")
                    {
                        CMethods.PrintDivider();
                        SettingsManager.ChangeDividerChar();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (setting == "4")
                    {
                        CMethods.PrintDivider();
                        SettingsManager.ChangeDividerSize();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (setting == "5")
                    {
                        CMethods.PrintDivider();
                        SettingsManager.ToggleBlips();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (setting.IsExitString())
                    {
                        CMethods.PrintDivider();
                        return;
                    }
                }
            }
        }

        public static void ToolsCommand()
        {
            List<string> valid_tools = new List<string>() { "monster_book", "shovel", "musicbox", "pocket_lab", "fast_map" };
            List<Item> available_tools = new List<Item>();

            foreach (Item tool in InventoryManager.GetInventoryItems()[CEnums.InvCategory.tools])
            {
                if (!available_tools.Contains(tool))
                {
                    available_tools.Add(tool);
                }
            }


            CMethods.PrintDivider();

            if (available_tools.Count == 0)
            {
                Console.WriteLine("You don't have any tools.");
                CMethods.PressAnyKeyToContinue();
                CMethods.PrintDivider();

                return;
            }

            while (true)
            {
                Console.WriteLine("Your tools: ");

                int counter = 0;
                foreach (Item item in available_tools)
                {
                    Console.WriteLine($"      [{counter + 1}] {item.ItemName}");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", available_tools.Count).ToLower();
                    Item chosen;

                    try
                    {
                        chosen = available_tools[int.Parse(choice) - 1];
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
                    chosen.UseItem(UnitManager.player);
                    CMethods.PrintDivider();

                    break;
                }
            }
        }

        public static void CheatCommand()
        {
            const string password = "swordfish";
            CMethods.PrintDivider();

            while (true)
            {
                string guess = CMethods.MultiCharInput("Please enter the password (or type 'exit'): ");

                if (guess.IsExitString())
                {
                    return;
                }

                else if (guess != password)
                {
                    Console.WriteLine("Incorrect!\n");
                }

                else
                {
                    break;
                }
            }

            MusicPlayer.PlaySong(SoundManager.town_other_moody, -1);

            CMethods.PrintDivider();
            Console.WriteLine("Welcome to the top-secret cheat menu!");
            while (true)
            {
                Console.WriteLine("Type 'help' to view a list of cheats");
                Console.WriteLine("Type 'exit' exit the cheat menu\n");

                while (true)
                {
                    string command = CMethods.MultiCharInput("Enter a cheat: ").ToLower();
                    List<string> keywords = command.Split().ToList();

                    if (command.IsExitString())
                    {
                        CMethods.PrintDivider();
                        SoundManager.PlayCellMusic();
                        return;
                    }

                    else if (command.StartsWith("h"))
                    {
                        CMethods.PrintDivider();
                        CheatEngine.Help();
                        CMethods.PrintDivider();
                    }

                    // Inventory add cheat
                    else if (keywords.Count == 4 && keywords[0] == "inventory" && keywords[1] == "add")
                    {
                        CheatEngine.Flag flag = CheatEngine.InventoryAddCheat(keywords[2], keywords[3]);

                        if (flag == CheatEngine.Flag.InvalidItemID)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'item id => {keywords[2]}' is invalid for 'inventory add [item id] [quantity]'");
                        }

                        if (flag == CheatEngine.Flag.InvalidItemQuantity)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'quantity => {keywords[3]}' is invalid for 'inventory add [item_id] [quantity]'");
                        }
                    }

                    // Spawns toggle cheat
                    else if (keywords.Count == 3 && keywords[0] == "spawns" && keywords[1] == "toggle")
                    {
                        CheatEngine.Flag flag = CheatEngine.SpawnToggleCheat(keywords[2]);

                        if (flag == CheatEngine.Flag.InvalidSpawnSetting)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'true or false => {keywords[2]}' is invalid for 'spawns toggle [true or false]'");
                        }
                    }

                    // Gold add cheat
                    else if (keywords.Count == 3 && keywords[0] == "gold" && keywords[1] == "add")
                    {
                        CheatEngine.Flag flag = CheatEngine.GoldAddCheat(keywords[2]);

                        if (flag == CheatEngine.Flag.InvalidGoldQuantity)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'quantity => {keywords[2]}' is invalid for 'gold add [quantity]'");
                        }
                    }

                    // Teleport cheat
                    else if (keywords.Count == 2 && keywords[0] == "teleport")
                    {
                        CheatEngine.Flag flag = CheatEngine.TeleportCheat(keywords[1]);

                        if (flag == CheatEngine.Flag.InvalidTileID)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'tile id => {keywords[1]}' is invalid for 'teleport [tile id]'");
                        }
                    }

                    // Player xp add cheat
                    else if (keywords.Count == 4 && keywords[0] == "player" && keywords[2] == "xp")
                    {
                        CheatEngine.Flag flag = CheatEngine.PlayerXPAddCheat(keywords[1], keywords[3]);

                        if (flag == CheatEngine.Flag.InvalidUnitID)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'pcu id => {keywords[1]}' is invalid for 'player [pcu id] xp [quantity]'");
                        }

                        else if (flag == CheatEngine.Flag.InvalidXPQuantity)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'quantity => {keywords[3]}' is invalid for 'player [pcu id] xp [quantity]'");
                        }
                    }

                    // Player heal cheat
                    else if (keywords.Count == 3 && keywords[0] == "player" && keywords[2] == "heal")
                    {
                        CheatEngine.Flag flag = CheatEngine.PlayerHealCheat(keywords[1]);

                        if (flag == CheatEngine.Flag.InvalidUnitID)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'pcu id => {keywords[1]}' is invalid for 'player [pcu id] heal'");
                        }
                    }

                    else if (keywords.Count == 3 && keywords[0] == "player" && keywords[2] == "kill")
                    {
                        CheatEngine.Flag flag = CheatEngine.PlayerKillCheat(keywords[1]);

                        if (flag == CheatEngine.Flag.InvalidUnitID)
                        {
                            CMethods.PrintDivider();
                            Console.WriteLine($"'pcu id => {keywords[1]}' is invalid for 'player [pcu id] kill'");
                        }
                    }

                    else if (keywords.Count == 2 && keywords[0] == "battle" && keywords[1] == "fight")
                    {
                        CMethods.PrintDivider();
                        CheatEngine.BattleFightCheat();
                        MusicPlayer.PlaySong(SoundManager.town_other_moody, -1);
                        break;
                    }

                    else if (keywords.Count == 2 && keywords[0] == "file" && keywords[1] == "save")
                    {
                        CMethods.PrintDivider();
                        CheatEngine.FileSaveCheat();
                    }

                    else if (keywords.Count == 2 && keywords[0] == "file" && keywords[1] == "load")
                    {
                        CMethods.PrintDivider();
                        CheatEngine.FileLoadCheat();
                    }

                    else
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"Invalid command '{command}'");
                    }

                    break;
                }
            }
        }

        public static void RecheckCommand()
        {
            TownManager.SearchForTowns(true);
            // BossManager.CheckForBosses();
        }

        public static void HelpCommand()
        {
            CMethods.PrintDivider();
            Console.WriteLine(@"Command List:
    [NSEW] Moves your party if the selected direction is unobstructed
    [Arrow Keys] Alternative to [NSEW]
    [I] View inventory and use/equip items
    [L] Display a description of your current location
    [P] Display the stats of a specific party member
    [T] Use tools without opening the inventory
    [U] View and Manage Equipment
    [Q] View Quest Log
    [M] Use healing spells outside of battle
    [R] Search the current tile for a town or boss
    [C] Open the settings list and allows you to change them in-game
    [H] Reopens this list of commands
Type the letter in brackets while on the overworld to use the command");
            CMethods.PressAnyKeyToContinue();
            CMethods.PrintDivider();
        }
        #endregion
    }

    public static class CheatEngine
    {
        public enum Flag
        {
            Success,
            InvalidItemID,
            InvalidItemQuantity,
            InvalidSpawnSetting,
            InvalidGoldQuantity,
            InvalidTileID,
            InvalidUnitID,
            InvalidXPQuantity
        }

        public static Flag InventoryAddCheat(string item_id, string quantity)
        {
            if (!ItemManager.GetItemList().Select(x => x.ItemID).Contains(item_id))
            {
                return Flag.InvalidItemID;
            }

            if (!int.TryParse(quantity, out int true_quantity) || true_quantity < 1)
            {
                return Flag.InvalidItemQuantity;
            }

            for (int i = 0; i < true_quantity; i++)
            {
                InventoryManager.AddItemToInventory(item_id);
            }

            CMethods.PrintDivider();
            Console.WriteLine($"Added {item_id} x{quantity} to inventory");

            return Flag.Success;
        }

        public static Flag SpawnToggleCheat(string new_setting)
        {
            if (!bool.TryParse(new_setting, out bool true_setting))
            {
                return Flag.InvalidSpawnSetting;
            }

            CInfo.DoSpawns = true_setting;
            CMethods.PrintDivider();
            Console.WriteLine($"Monster spawns are now {(true_setting ? "enabled" : "disabled")}");

            return Flag.Success;
        }

        public static Flag GoldAddCheat(string quantity)
        {
            if (!int.TryParse(quantity, out int true_quantity) || true_quantity < 1)
            {
                return Flag.InvalidGoldQuantity;
            }

            CInfo.GP += true_quantity;
            CMethods.PrintDivider();
            Console.WriteLine($"Gave player {true_quantity} gold");

            return Flag.Success;
        }

        public static Flag TeleportCheat(string tile_id)
        {
            if (!TileManager.GetTileList().Select(x => x.TileID).Contains(tile_id))
            {
                return Flag.InvalidTileID;
            }

            CInfo.CurrentTile = tile_id;
            CMethods.PrintDivider();
            Console.WriteLine($"Teleported party to {tile_id}");

            return Flag.Success;
        }

        public static Flag PlayerXPAddCheat(string pcu_id, string quantity)
        {
            if (!UnitManager.GetAllPCUs().Select(x => x.PlayerID).Contains(pcu_id))
            {
                return Flag.InvalidUnitID;
            }

            if (!int.TryParse(quantity, out int true_quantity) || true_quantity < 1)
            {
                return Flag.InvalidXPQuantity;
            }

            UnitManager.GetAllPCUs().Single(x => x.PlayerID == pcu_id).CurrentXP += true_quantity;
            CMethods.PrintDivider();
            Console.WriteLine($"Gave {pcu_id} {true_quantity} XP");

            return Flag.Success;
        }

        public static Flag PlayerHealCheat(string pcu_id)
        {
            if (!UnitManager.GetAllPCUs().Select(x => x.PlayerID).Contains(pcu_id))
            {
                return Flag.InvalidUnitID;
            }

            UnitManager.GetAllPCUs().Single(x => x.PlayerID == pcu_id).FullyHealUnit(true, true, true, true);
            CMethods.PrintDivider();
            Console.WriteLine($"Restored HP, MP, AP, and cured statuses for {pcu_id}");

            return Flag.Success;
        }

        public static Flag PlayerKillCheat(string pcu_id)
        {
            if (!UnitManager.GetAllPCUs().Select(x => x.PlayerID).Contains(pcu_id))
            {
                return Flag.InvalidUnitID;
            }

            PlayableCharacter pcu = UnitManager.GetAllPCUs().Single(x => x.PlayerID == pcu_id);
            pcu.Statuses = new List<CEnums.Status>() { CEnums.Status.dead };
            pcu.HP = 0;
            CMethods.PrintDivider();
            Console.WriteLine($"Set {pcu_id}'s HP to zero and flagged as dead");

            return Flag.Success;
        }

        public static void BattleFightCheat()
        {
            BattleManager.BattleSystem(false);
        }

        public static void FileSaveCheat()
        {
            SavefileManager.SaveTheGame();
            Console.WriteLine("The game has been saved.");
        }

        public static void FileLoadCheat()
        {
            JSONDeserializer.JSONLoadEverything();
            Console.WriteLine("The most recent save has been loaded.");
        }

        public static void Help()
        {
            Console.WriteLine(@"Available cheats:
 'inventory add [item id] [quantity]' => Adds quantity of item id to inventory
 'spawns toggle [true or false]' => Enables or disable monster spawns
 'gold add [quantity]' => Gives the player quantity GP
 'teleport [tile id] => Teleports the player to the tile with tile id
 'player [pcu id] xp [quantity]' => Gives the PCU quantity XP
 'player [pcu id] heal' => Fully heals the PCU
 'player [pcu id] kill' => Kills the PCU
 'battle fight' => Initiates a random encounter on the current tile
 'file save' => Saves the game
 'file load' => Loads the most recent save file");
            CMethods.PressAnyKeyToContinue();
        }
    }
}
