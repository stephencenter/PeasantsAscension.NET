using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    internal static class ProgramUI
    {
        internal static void Run()
        {
            RunChecks();                      // Verify the game is working as intended...
            SettingsManager.ApplySettings();  // ...apply the player's chosen settings...
            DisplayTitlescreen();             // ...display the titlescreen...
            SavefileManager.LoadTheGame();    // ...check for save files...
            MainGameLoop();                   // ...and then start the game!
        }

        private static void MainGameLoop()
        {
            SoundManager.PlayCellMusic();

            while (true)
            {
                if (!TownManager.SearchForTowns())
                {
                    CMethods.PrintDivider();
                }

                DisplayGameUI();
                List<Tuple<char, string>> available_dirs = GetAvailableDirections();

                while (true)
                {
                    CInfo.Gamestate = CEnums.GameState.overworld;
                    string command = CMethods.SingleCharInput("Input Command (type 'help' to view command list): ").ToLower();

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
                        // recheck_command()
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
                }

                if (choice.StartsWith("c") && !CInfo.Debugging)
                {
                    ShowCredits();
                    Console.WriteLine(title_card);
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

        // These are important checks that prevent you from making easy mistakes
        // by alerting you when you run the game
        private static void RunChecks()
        {
            // Check that all monsters have real items assigned to them
            foreach (KeyValuePair<CEnums.MonsterGroup, List<Type>> monster_group in UnitManager.MonsterGroups)
            {
                foreach (Type monster_type in monster_group.Value)
                {
                    Monster monster = Activator.CreateInstance(monster_type) as Monster;

                    foreach (string item_id in monster.DropList.Select(x => x.Item1))
                    {
                        if (!ItemManager.VerifyItemExists(item_id))
                        {
                            Console.WriteLine($"{monster.UnitName} has invalid item_id '{item_id}' listed as a droppable item");
                        }
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
            foreach (string tile_id in TileManager.GetTileList().Select(x => x.TileID))
            {
                if (TileManager.GetTileList().Count(x => x.TileID == tile_id) > 1)
                {
                    Console.WriteLine($"{tile_id} is being used as a Tile ID for multiple tiles!");
                }
            }

            /*
            // This optional loop checks to make sure no tiles are "adjacent to themselves"
            // e.g. North on tile_a leads to tile_a
            for item4 in all_tiles:
                for check_direction in [item4.to_s, item4.to_n, item4.to_e, item4.to_w]:
                    if check_direction == item4.tile_id and not item4.allow_recursion:
                        print(f"{item4.tile_id} leads to itself - is this intended?")

            // This optional loop checks to make sure tiles are two-way passages
            // e.g. North on tile_a leads to tile_b, South on tile_b does nothing
            for item5 in all_tiles:
                if any([item5.to_s and not find_tile_with_id(item5.to_s).to_n,
                        item5.to_n and not find_tile_with_id(item5.to_n).to_s,
                        item5.to_w and not find_tile_with_id(item5.to_w).to_e,
                        item5.to_e and not find_tile_with_id(item5.to_e).to_w]) and not item5.allow_oneway:
                    print(f"{item5.tile_id} has one-way passages - is this intended?")

            // This optional loop checks to make sure all tiles are two-way passages that specifically correspond to eachother
            // e.g. North on tile_a leads to tile_b, South on tile_b leads to tile_c
            for item6 in all_tiles:
                is_error = False

                if item6.to_s and find_tile_with_id(item6.to_s).to_n and not item6.allow_noneuclidean:
                        if item6.tile_id != find_tile_with_id(item6.to_s).to_n and not item6.allow_noneuclidean:
                        is_error = True

                if item6.to_n and find_tile_with_id(item6.to_n).to_s and not item6.allow_noneuclidean:
                        if item6.tile_id != find_tile_with_id(item6.to_n).to_s and not item6.allow_noneuclidean:
                        is_error = True

                if item6.to_w and find_tile_with_id(item6.to_w).to_e and not item6.allow_noneuclidean:
                        if item6.tile_id != find_tile_with_id(item6.to_w).to_e and not item6.allow_noneuclidean:
                        is_error = True

                if item6.to_e and find_tile_with_id(item6.to_e).to_w and not item6.allow_noneuclidean:
                        if item6.tile_id != find_tile_with_id(item6.to_e).to_w and not item6.allow_noneuclidean:
                        is_error = True

                if is_error:
                    print(f"{item6.tile_id} has non-euclidean passages - is this intended?")


            for item7 in all_provinces:
                if item7.name not in valid_provinces:
                        print(f"{item7} has an invalid province!") */
        }
    }
}
