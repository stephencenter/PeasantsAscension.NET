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

using Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    internal static class ProgramUI
    {
        internal static void Run()
        {
            RunChecks();                     // Verify the game is working as intended...
            SetConsoleProperties();          // ...Set the console properties...
            SettingsManager.LoadSettings();  // ...apply the player's chosen settings...
            DisplayTitlescreen();            // ...display the titlescreen...
            SavefileManager.LoadTheGame();   // ...check for save files...
            MainGameLoop();                  // ...and then start the game!
        }

        private static void MainGameLoop()
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
                    CInfo.Gamestate = CEnums.GameState.overworld;
                    string command = CMethods.SingleCharInput("Input Command (type 'help' to view command list): ", true).ToLower();

                    if (command == "~")
                    {
                        CommandManager.CheatCommand();
                    }

                    else if (available_dirs.Any(x => command.StartsWith(x.Item1.ToString())))
                    {
                        CommandManager.MoveCommand(available_dirs, command[0]);
                        break;
                    }

                    else if (command.StartsWith("p"))
                    {
                        CommandManager.PlayerStatsCommand();
                    }

                    else if (command.StartsWith("m"))
                    {
                        CommandManager.MagicCommand();
                    }

                    else if (command.StartsWith("i"))
                    {
                        InventoryManager.PickInventoryCategory();
                    }

                    else if (command.StartsWith("t"))
                    {
                        // tools_command()
                    }

                    else if (command.StartsWith("l"))
                    {
                        CommandManager.LookCommand();
                    }

                    else if (command.StartsWith("r"))
                    {
                        CommandManager.RecheckCommand();
                    }

                    else if (command.StartsWith("c"))
                    {
                        CommandManager.ConfigCommand();
                    }

                    else if (command.StartsWith("h"))
                    {
                        CommandManager.HelpCommand();
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

        private static void DisplayTitlescreen()
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
Peasant's Ascension {CInfo.GameVersion} -- A Text-RPG by Stephen Center
Licensed under the GNU GPLv3: [https://www.gnu.org/copyleft/gpl.html]
Check here often for updates: [http://www.reddit.com/r/PeasantsAscension/]";
            SoundManager.title_music.PlayLooping();
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

                if (choice.StartsWith("s") && !CInfo.Debugging)
                {
                    CommandManager.ConfigCommand();
                    Console.WriteLine(title_card);
                    CMethods.PrintDivider();
                }

                if (choice.StartsWith("c") && !CInfo.Debugging)
                {
                    ShowCredits();
                    Console.WriteLine(title_card);
                    CMethods.PrintDivider();
                }

                if (choice.StartsWith("e") && !CInfo.Debugging)
                {
                    Environment.Exit(1);
                }
            }
        }

        private static void ShowCredits()
        {
            /*
            print('-'*save_load.divider_size)

            try:
                SoundManager.credits_music.PlayLooping();

                // Display the credits one line at a time with specific lengths
                // of time in between each line. Syncs up with the music!
                with open('../Credits.txt') as f:
                    for number, line in enumerate(f) :
                        print(''.join(line.split("\n")))
                        main.smart_sleep([0.75])

                    main.smart_sleep(3)

                    SoundManager.title_music.PlayLooping();

            except FileNotFoundError:
                // Display this is the Credits.txt file couldn't be found
                logging.exception(f'Error finding credits.txt on {time.strftime("%m/%d/%Y at %H:%M:%S")}:')
                print('The "credits.txt" file could not be found.')
                main.s_input("\nPress enter/return ")

            except OSError:
                // If there is a problem opening the Credits.txt file, but it does exist,
                // display this message and log the error
                logging.exception(f'Error loading credits.txt on {time.strftime("%m/%d/%Y at %H:%M:%S")}:')
                print('There was a problem opening "credits.txt".')
                main.s_input("\nPress enter/return ") */
        }

        private static void RunChecks()
        {
            // Check that all monsters have real items assigned to them
            foreach (Monster monster in UnitManager.MonsterList)
            {
                foreach (string item_id in monster.DropList.Select(x => x.Item1))
                {
                    if (!ItemManager.VerifyItemExists(item_id))
                    {
                        Console.WriteLine($"{monster.UnitName} has invalid item_id '{item_id}' listed as a droppable item");
                    }
                }
            }

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

            // Check to make sure all ItemIDs in use are unique
            IEnumerable<string> item_ids = ItemManager.GetItemList().Select(x => x.ItemID);
            foreach (string item_id in item_ids)
            {
                if (item_ids.Count(x => x == item_id) > 1)
                {
                    Console.WriteLine($"{item_id} is being used as an Item ID for multiple items!");
                }
            }

            // Check to make sure all ItemIDs used in general store stocks are valid items
            foreach (MarketTown town in TownManager.GetTownList().Where(x => x is MarketTown).Select(x => x as MarketTown))
            {
                foreach (string item_id in town.GenStock)
                {
                    if (!ItemManager.VerifyItemExists(item_id))
                    {
                        Console.WriteLine($"{town.TownID}'s stock has an invalid item '{item_id}'");
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
                        Console.WriteLine($"{town.TownID}'s stock has '{item_id}' listed more than once");
                    }
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
        }

        private static void SetConsoleProperties()
        {
            Console.Title = "Peasant's Ascension";
            Console.WindowHeight = 25;
            Console.BufferHeight = 50;
            Console.WindowWidth = 80;
            Console.BufferWidth = 80;
        }
    }
}
