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

using Game;

namespace Main
{
    internal static class Program
    {
        private static void Main()
        {
            GameLoopManager.RunChecks();  // Verify the game is working as intended...
            GameLoopManager.SetConsoleProperties();  // ...Set the console properties...
            SettingsManager.LoadSettings();          // ...apply the player's chosen settings...
            GameLoopManager.DisplayTitlescreen();    // ...display the titlescreen...
            SavefileManager.LoadTheGame();           // ...check for save files...
            GameLoopManager.MainGameLoop();          // ...and then start the game!
        }
    }
}
