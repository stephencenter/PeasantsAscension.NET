﻿/* This file is part of Peasant's Ascension.
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
    public static class TileManager
    {
        #region
        // Tile descriptions
        private const string nearton_desc = 
@"Nearton is located in the Forest Plains of Overshire, and is surrounded by 
a large natural moat. This place is very peaceful, there's no monsters here.";

        private const string southford_desc =
@"Southford is the only other town in the Forest Plains. This town is a lot 
busier, and as such has attracted a fair share of monsters. You better arm
yourself!";

        private const string overshire_city_desc =
@"Overshire City is the capital of both the province of Overshire, and the 
Kingdom of Harconia.  Some of the towers in the city can be seen from miles 
away. Caravans carrying food and materials can be seen flocking to the city 
to be sold here.";
        #endregion

        /* =========================== *
         *            LISTS            *
         * =========================== */
        private static readonly List<Tile> tile_list = new List<Tile>()
        {
            // Nearton
            #region
            new Tile("Village of Nearton", "nearton_tile", string.Concat(nearton_desc, "\n",
@"The village of Nearton is mere minutes away from this point! Stopping by
there might be a smart idea."),
                town_list: new List<string>() { "town_nearton" },
                north: "nearton_n",
                south: "nearton_s",
                east: "nearton_e",
                west: "nearton_w"),

            new Tile("Nearton Outskirts", "nearton_sw", nearton_desc,
                north: "nearton_w",
                east: "nearton_s"),

            new Tile("Nearton Outskirts", "nearton_s", nearton_desc,
                north: "nearton_tile",
                east: "nearton_se",
                west: "nearton_sw"),

            new Tile("Nearton Outskirts", "nearton_se", nearton_desc,
                north: "nearton_e",
                west: "nearton_s"),

            new Tile("Nearton Outskirts", "nearton_w", nearton_desc,
                north: "nearton_nw",
                south: "nearton_sw",
                east: "nearton_tile",
                boss_list: new List<string>() { "master_slime" }),

            new Tile("Nearton Outskirts", "nearton_e", nearton_desc,
                gem_list: new List<Tuple<string, bool>>() { new Tuple<string, bool>("amethyst_gem", false) },
                north: "nearton_ne",
                south: "nearton_se",
                west: "nearton_tile"),

            new Tile("Nearton Outskirts", "nearton_nw", nearton_desc,
                south: "nearton_w",
                east: "nearton_n"),

            new Tile("Nearton Outskirts", "nearton_n", nearton_desc,
                south: "nearton_tile",
                east: "nearton_ne",
                west: "nearton_nw"),

            new Tile("Nearton Outskirts", "nearton_ne", nearton_desc,
                south: "nearton_e",
                west: "nearton_n"),
            #endregion

            // Southford
            #region
            new Tile("Town of Southford", "southford_tile", string.Concat(southford_desc, "\n",
@"The town of Southford is mere minutes away from this point! Stopping by
there might be a smart idea."),
                town_list: new List<string>() { "town_southford" },
                north: "southford_n",
                south: "southford_s",
                east: "southford_e",
                west: "southford_w"),

            new Tile("Southford Outskirts", "southford_sw", southford_desc,
                north: "southford_w",
                east: "southford_s"),

            new Tile("Southford Outskirts", "southford_s", southford_desc,
                north: "southford_tile",
                east: "southford_se",
                west: "southford_sw"),

            new Tile("Southford Outskirts", "southford_se", southford_desc,
                north: "southford_e",
                west: "southford_s"),

            new Tile("Southford Outskirts", "southford_w", southford_desc,
                north: "southford_nw",
                south: "southford_sw",
                east: "southford_tile"),

            new Tile("Southford Outskirts", "southford_e", southford_desc,
                north: "southford_ne",
                south: "southford_se",
                west: "southford_tile"),

            new Tile("Southford Outskirts", "southford_nw", southford_desc,
                south: "southford_w",
                east: "southford_n"),

            new Tile("Southford Outskirts", "southford_n", southford_desc,
                south: "southford_tile",
                east: "southford_ne",
                west: "southford_nw"),

            new Tile("Southford Outskirts", "southford_ne", southford_desc,
                south: "southford_e",
                west: "southford_n"),
            #endregion

            // Nearton
            #region
            new Tile("Overshire City", "overshire_city_tile", string.Concat(overshire_city_desc, "\n",
@"Overshire City is mere minutes away from this point! Stopping by
there might be a smart idea."),
                town_list: new List<string>() { "town_overshire_city" },
                north: "overshire_city_n",
                south: "overshire_city_s",
                east: "overshire_city_e",
                west: "overshire_city_w"),
    
            new Tile("Overshire Outskirts", "overshire_city_sw", overshire_city_desc,
                north: "overshire_city_w",
                east: "overshire_city_s"),
    
            new Tile("Overshire Outskirts", "overshire_city_s", overshire_city_desc,
                north: "overshire_city_tile",
                east: "overshire_city_se",
                west: "overshire_city_sw"),
    
            new Tile("Overshire Outskirts", "overshire_city_se", overshire_city_desc,
                north: "overshire_city_e",
                west: "overshire_city_s"),
    
            new Tile("Overshire Outskirts", "overshire_city_w", overshire_city_desc,
                north: "overshire_city_nw",
                south: "overshire_city_sw",
                east: "overshire_city_tile"),
    
            new Tile("Overshire Outskirts", "overshire_city_e", overshire_city_desc,
                north: "overshire_city_ne",
                south: "overshire_city_se",
                west: "overshire_city_tile"),
    
            new Tile("Overshire Outskirts", "overshire_city_nw", overshire_city_desc,
                south: "overshire_city_w",
                east: "overshire_city_n"),
    
            new Tile("Overshire Outskirts", "overshire_city_n", overshire_city_desc,
                south: "overshire_city_tile",
                east: "overshire_city_ne",
                west: "overshire_city_nw"),
    
            new Tile("Overshire Outskirts", "overshire_city_ne", overshire_city_desc,
                south: "overshire_city_e",
                west: "overshire_city_n"),
            #endregion
        };

        private static readonly List<Cell> cell_list = new List<Cell>()
        {
            new NeartonCell(), new SouthfordCell()
        };

        private static readonly List<Province> province_list = new List<Province>()
        {
            new Province("Overshire", new List<string>() { "nearton_cell", "southford_cell" }, "overshire_prov")
        };

        /* =========================== *
         *           METHODS           *
         * =========================== */
        public static List<Tile> GetTileList()
        {
            return tile_list;
        }

        public static List<Cell> GetCellList()
        {
            return cell_list;
        }

        public static List<Province> GetProvinceList()
        {
            return province_list;
        }

        public static Tile FindTileWithID(string tile_id)
        {
            // Takes in a TileID, returns the tile that matches the ID
            try
            {
                return GetTileList().Single(x => x.TileID == tile_id);
            }

            catch (InvalidOperationException)
            {
                // InvalidOperationException means that .Single() found either 0 tiles matching tile_id, or more than 1
                ExceptionLogger.LogException($"Tile with id {tile_id} either doesn't exist or is duplicated", new InvalidOperationException());
                return GetTileList()[0];
            }
        }

        public static Cell FindCellWithID(string cell_id)
        {
            // Takes in a CellID, returns the cell that matches the ID
            try
            {
                return GetCellList().Single(x => x.CellID == cell_id);
            }

            catch (InvalidOperationException)
            {
                // InvalidOperationException means that .Single() found either 0 cells matching cell_id, or more than 1
                ExceptionLogger.LogException($"Cell with id {cell_id} either doesn't exist or is duplicated", new InvalidOperationException());
                return GetCellList()[0];
            }
        }

        public static Province FindProvinceWithID(string prov_id)
        {
            // Takes in a ProvID, returns the province that matches the ID
            try
            {
                return GetProvinceList().Single(x => x.ProvID == prov_id);
            }

            catch (InvalidOperationException)
            {
                // InvalidOperationException means that .Single() found either 0 provinces matching prov_id, or more than 1
                ExceptionLogger.LogException($"Province with id {prov_id} either doesn't exist or is duplicated", new InvalidOperationException());
                return GetProvinceList()[0];
            }
        }

        public static Cell FindCellWithTileID(string tile_id)
        {
            // Takes in a TileID, returns the cell that matches the ID
            try
            {
                return GetCellList().Single(x => x.TileList.Contains(tile_id));
            }

            catch (InvalidOperationException)
            {
                // InvalidOperationException means that .Single() found either 0 cells matching tile_id, or more than 1
                ExceptionLogger.LogException($"Cell with tile_id {tile_id} either doesn't exist or is duplicated", new InvalidOperationException());
                return GetCellList()[0];
            }
        }

        public static Province FindProvinceWithTileID(string tile_id)
        {
            // Takes in a TileID, returns the province that matches the ID
            try
            {
                string cell_id = GetCellList().Single(x => x.TileList.Contains(tile_id)).CellID;
                return GetProvinceList().Single(x => x.CellList.Contains(cell_id));
            }

            catch (InvalidOperationException)
            {
                // InvalidOperationException means that .Single() found either 0 provinces matching tile_id, or more than 1
                ExceptionLogger.LogException($"Province with tile_id {tile_id} either doesn't exist or is duplicated", new InvalidOperationException());
                return GetProvinceList()[0];
            }
        }
    }

    public class Tile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ToNorth { get; set; }
        public string ToSouth { get; set; }
        public string ToEast { get; set; }
        public string ToWest { get; set; }
        public string TileID { get; set; }
        public List<string> TownList { get; set; }
        public List<Tuple<string, bool>> GemList { get; set; }
        public List<string> BossList { get; set; }

        public string GenerateAsciiArt()
        {
            int num_adj_tiles = 0;

            foreach (string str in new List<string>() { ToNorth, ToSouth, ToEast, ToWest })
            {
                if (str != null)
                {
                    num_adj_tiles++;
                }
            }

            // Calculate which tile ascii art to display
            if (num_adj_tiles == 1)
            {
                if (ToNorth != null)
                {
                    return @"
                | N |
                |   |
                | X |
                |___| X = Player Party";
                }

                else if (ToSouth != null)
                {
                    return @" 
                 ___
                |   |
                | X |
                |   |
                | S | X = Player Party";
                }

                else if (ToEast != null)
                {
                    return @"
                 ________
                | X     E
                |________ X = Player Party";
                }

                else if (ToWest != null)
                {
                    return @"
            ________
            W     X |
            ________| X = Player Party";
                }
            }

            else if (num_adj_tiles == 2)
            {
                if ((ToNorth != null) && (ToWest != null))
                {
                    return @"
                | N |
            ____|   |
            W     X |
            ________| X = Player Party";
                }

                else if ((ToNorth != null) && (ToEast != null))
                {
                    return @"
                | N |
                |   |____
                | X    E
                |________ X = Player Party";
                }

                else if ((ToNorth != null) && (ToSouth != null))
                {
                    return @"
                | N |
                |   |
                | X |
                |   |
                | S | X = Player Party";
                }

                else if ((ToWest != null) && (ToEast != null))
                {
                    return @"
            _____________
            W     X     E
            _____________ X = Player Party";
                }

                else if ((ToWest != null) && (ToSouth != null))
                {
                    return @"
            ________
            W     X |
            ____    |
                |   |
                | S | X = Player Party";
                }

                else if ((ToEast != null) && (ToSouth != null))
                {
                    return @"
                 ________
                | X     E
                |    ____
                |   |
                | S | X = Player Party";
                }
            }

            else if (num_adj_tiles == 3)
            {
                if ((ToNorth != null) && (ToWest != null) && (ToEast != null))
                {
                    return @"
                | N |
            ____|   |____
            W     X     E
            _____________ X = Player Party";
                }


                else if ((ToNorth != null) && (ToWest != null) &&  (ToSouth != null))
                {
                    return @"
                | N |
            ____|   |
            W     X |
            ____    |
                |   |
                | S | X = Player Party";
                }

                else if ((ToNorth != null) && (ToEast != null) && (ToSouth != null))
                {
                    return @"
                | N |
                |   |____
                | X     E
                |    ____
                |   |
                | S | X = Player Party";
                }

                else if ((ToSouth != null) && (ToEast != null) && (ToWest != null))
                {
                    return @"
            _____________
            W     X     E
            ____     ____
                |   |
                | S | X = Player Party";
                }
            }

            else if (num_adj_tiles == 4)
            {
                return @"
                | N |
            ____|   |____
            W     X     E
            ____     ____
                |   |
                | S | X = Player Party";
            }

            ExceptionLogger.LogException($"Failed to generate ascii art for tile {TileID}", new InvalidOperationException());
            return "ERROR";
        }

        public Tile(string name, string tile_id, string desc, List<string> town_list = null, List<Tuple<string, bool>> gem_list = null, List<string> boss_list = null, string north = null, string south = null, string east = null, string west = null)
        {
            Name = name;
            Description = desc;
            ToNorth = north;
            ToSouth = south;
            ToEast = east;
            ToWest = west;
            TownList = town_list ?? new List<string>();
            GemList = gem_list ?? new List<Tuple<string, bool>>();
            BossList = boss_list ?? new List<string>();
            TileID = tile_id;
        }
    }

    public abstract class Cell
    {
        // Cells are containers for tiles
        // They store information related to monster spawning, music, and store item quality
        // which are all cell-specific and not tile-specific
        // The Cell class is abstract. Using a constructor to create a Cell would get
        // messy so all the information is preloaded into custom Cell subclasses
        public string CellName { get; set; }
        public List<string> TileList { get; set; }
        public string PrimaryTile { get; set; }
        public List<CEnums.MonsterGroup> MonsterGroups { get; set; }
        public string Music { get; set; }
        public bool MonstersEnabled { get; set; }
        public int MinMonsterLevel { get; set; }
        public int MaxMonsterLevel { get; set; }
        public int StoreLevel { get; set; }
        public string CellID { get; set; }

        protected Cell()
        {

        }
    }

    public class Province
    {
        public string ProvinceName { get; set; }
        public List<string> CellList { get; set; }
        public string ProvID { get; set; }

        public Province(string name, List<string> cells, string prov_id)
        {
            ProvinceName = name;
            CellList = cells;
            ProvID = prov_id;
        }
    }

    // Custom Cells
    internal class NeartonCell : Cell
    {
        public NeartonCell()
        {
            CellName = "Nearton"; 
            TileList = new List<string>() { "nearton_tile", "nearton_w", "nearton_ne", "nearton_e", "nearton_s", "nearton_n", "nearton_se", "nearton_nw", "nearton_sw" };
            PrimaryTile = "nearton_tile";
            Music = SoundManager.area_forest_music;
            StoreLevel = 1;
            CellID = "nearton_cell";

            MonstersEnabled = true;
            MinMonsterLevel = 1;
            MaxMonsterLevel = 2;
            MonsterGroups = new List<CEnums.MonsterGroup>() { CEnums.MonsterGroup.animal, CEnums.MonsterGroup.monster };

        }
    }

    internal class SouthfordCell : Cell
    {
        public SouthfordCell()
        {
            CellName = "Southford";
            TileList = new List<string>() { "southford_tile", "southford_w", "southford_ne", "southford_e", "southford_s", "southford_n", "southford_se", "southford_nw", "southford_sw" };
            PrimaryTile = "southford_tile";
            Music = SoundManager.area_forest_music;
            StoreLevel = 1;
            CellID = "southford_cell";

            MonstersEnabled = true;
            MinMonsterLevel = 1;
            MaxMonsterLevel = 2;
            MonsterGroups = new List<CEnums.MonsterGroup>() { CEnums.MonsterGroup.animal, CEnums.MonsterGroup.monster };
        }
    }

    //internal class OvershireCityCell : Cell
    //{
    //    public OvershireCityCell()
    //    {
    //        TileList = new List<string>()
    //        {
    //            "overshire_city_tile",
    //            "overshire_city_w",
    //            "overshire_city_ne",
    //            "overshire_city_e",
    //            "overshire_city_s",
    //            "overshire_city_n",
    //            "overshire_city_se",
    //            "overshire_city_nw",
    //            "overshire_city_sw"
    //        };
    //    }
    //}
}
