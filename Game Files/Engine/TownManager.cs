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
using System.Linq;

namespace Engine
{
    public static class TownManager
    {
        private static readonly List<Town> town_list = new List<Town>();

        public static List<Town> GetTownList()
        {
            return town_list;
        }

        public static Town FindTownWithID(string town_id)
        {
            return GetTownList().Single(x => x.TownID == town_id);
        }

        public static bool SearchForTowns(bool enter = true)
        {
            if (TileManager.FindTileWithID(CInfo.CurrentTile).TownList.Count == 0)
            {
                return false;
            }

            foreach (string town_id in TileManager.FindTileWithID(CInfo.CurrentTile).TownList)
            {
                Town town = FindTownWithID(town_id);
                CMethods.PrintDivider();

                while (true)
                {
                    string yes_no = CMethods.SingleCharInput($"The town of {town.TownName} is nearby. Enter? [Y]es or [N]o: ");

                    if (yes_no.IsYesString())
                    {
                        CInfo.RespawnTile = CInfo.CurrentTile;
                        town.EnterTown();
                        return true;
                    }

                    else if (yes_no.IsNoString())
                    {
                        CMethods.PrintDivider();
                        break;
                    }
                }
            }

            return true;
        }
    }

    public class Town
    {
        public string TownName { get; set; }
        public string TownID { get; set; }

        public void EnterTown()
        {

        }
    }
}
