// Debra.cs
// Debra - artificial intelligence system.
// It implements all the functions associated with the work of AI.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GFZ.Model
{
    class Debra
    {
        // An array that stores therein the id of all elements of the game.
        private readonly int[,] _arrayLogic = new int[10, 20];
        // An array that keeps the coordinates of all the cells on the map.
        private readonly int[,] _arrayCoordinates = new int[10, 20];

        // Location enemy squad:
        // In the coordinate X(the previous turn).
        private int _previousX;
        // In the coordinate Y(the previous turn).
        private int _previousY;
        // On the coordinate X(current).
        public int PointX { get; private set; }
        // On the coordinate Y(current).
        public int PointY { get; private set; }

        /// <summary>
        /// Reset all fields to the initial.
        /// It is used at the start of a new game or loading of conservation.
        /// </summary>
        public void ResetToDefault()
        {
            PointX = 0;
            PointY = 0;
            _previousX = 0;
            _previousY = 0;
        }

        /// <summary>
        /// The function to get the array elements with the id of the game.
        /// </summary>
        /// <returns>An array that keeps the ID of all the elements in the game.</returns>
        public int[,] GetLogicArray()
        {
            return _arrayLogic;
        }

        /// <summary>
        /// The function to generate arrays that will be used is in the mechanics of the game.
        /// There is analysis of the map by analyzing id.
        /// </summary>
        /// <param name="map">An array is a map of the game world.</param>
        public void AnalyzingWorld(PictureBox[,] map)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 20; j++)
                {
                    _arrayLogic[i, j] = Convert.ToInt32(map[i, j].Tag.ToString());
                    _arrayCoordinates[i, j] = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(map[i, j].Name, @"[^\d]", ""));
                }
            }
        }

        /// <summary>
        /// Check whether the squad to win?
        /// </summary>
        /// <param name="monster">Monster, who is being attacked.</param>
        /// <param name="sqAi">Squad of the computer that attacked.</param>
        /// <returns>True - If the squad was able to win.</returns>
        private static bool SquadCanWin(Unit monster, Squad sqAi)
        {
            return sqAi.Damage >= monster.Hp;
        }

        /// <summary>
        /// Check whether the squad to win?
        /// </summary>
        /// <param name="sqPl">Player squad.</param>
        /// <param name="sqAi">Computer squad.</param>
        /// <returns>True - If the squad was able to win.</returns>
        private static bool SquadCanWin(Squad sqPl, Squad sqAi)
        {
            return sqAi.Damage >= sqPl.Hp;
        }

        /// <summary>
        /// Analysis of neighboring cell.
        /// </summary>
        /// <param name="x">Coordinate along the X axis.</param>
        /// <param name="y">Coordinate along the Y axis.</param>
        /// <param name="sqPl">Object of class represents Squad Player.</param>
        /// <param name="sqAi">Object of class represents Squad Computer.</param>
        /// <param name="monsters">Array monsters in the game.</param>
        /// <returns>"Coefficient of appeal" - shows how the important objects in neighboring cells.</returns>
        private int AnalysisOfTheNeighboringCells(int x, int y, Squad sqPl, Squad sqAi, IReadOnlyList<Unit> monsters)
        {
            // MAGIC IS GOING ON HERE.
            try
            {
                // If we came across an obstacle.
                if (_arrayLogic[x, y] > 0 && _arrayLogic[x, y] < 19)
                    return -1;

                // If those coordinates that we check are three coordinates,
                // Where we were on the last turn, it is necessary to return -2, which is constantly
                // Do not rush between the two neighboring empty cells.
                if (_previousX == x && _previousY == y)
                    return -2;

                // If you come across a monster.
                if (_arrayLogic[x, y] > 28 && _arrayLogic[x, y] < 43)
                {
                    if (SquadCanWin(monsters[42 - _arrayLogic[x, y]], sqAi))
                        return _arrayLogic[x, y];

                    return -1;
                }

                // If you stumbled upon an enemy unit.
                if (_arrayLogic[x, y] == 27)
                {
                    if (SquadCanWin(sqPl, sqAi))
                        return _arrayLogic[x, y];

                    return -1;
                }
            }
            catch(IndexOutOfRangeException)
            {
                // Since then an exhaustive search, it may be a situation where we were
                // At the edge of the map, in which case we Word Exception departure beyond the array
                // Make that the coefficient of attraction for such a cell is -1.
                return -1;
            }

            // In other cases.
            return _arrayLogic[x, y];
        }

        /// <summary>
        /// A random choice between two empty cells.
        /// </summary>
        /// <param name="coefficients">An array that keeps the "attraction factors".</param>
        /// <returns>Index of "attraction factor" in the array.</returns>
        private static int RandomBetween2EmptyCells(IReadOnlyList<int> coefficients)
        {
            var rnd = new Random();
            IList<int> indexOfEqual = new List<int>();

            for (var i = 0; i < coefficients.Count; i++)
                if (coefficients[i] == 0)
                    indexOfEqual.Add(i);

            // Choosing between them is random.
            return indexOfEqual[rnd.Next(indexOfEqual.Count)];
        }

        /// <summary>
        /// To make progress.
        /// </summary>
        /// <param name="sqPl">An object class represents Squad Player.</param>
        /// <param name="sqAi">An object class represents Squad Computer.</param>
        /// <param name="monsters">Array monsters in the game.</param>
        /// <returns>Index "attraction factor" in the array.</returns>
        public int Make(Squad sqPl, Squad sqAi, Unit[] monsters)
        {
            // Remember the position of the previous turn.
            _previousX = PointX;
            _previousY = PointY;

            // Find the position of squad on the map.
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 20; j++)
                {
                    if (_arrayLogic[i, j] != 28) continue;

                    PointX = i;
                    PointY = j;
                    break;
                }
            }

            // An array of factors attraction.
            int[] coefficients =
            {
                // To the left.
                AnalysisOfTheNeighboringCells(PointX, PointY - 1, sqPl, sqAi, monsters),
                // To the up.
                AnalysisOfTheNeighboringCells(PointX - 1, PointY, sqPl, sqAi, monsters),
                // To the right.
                AnalysisOfTheNeighboringCells(PointX, PointY + 1, sqPl, sqAi, monsters),
                // To the down.
                AnalysisOfTheNeighboringCells(PointX + 1, PointY, sqPl, sqAi, monsters)
            };

            // If we were at an impasse.
            if (coefficients[0] < 0 && coefficients[1] < 0 && coefficients[2] < 0 && coefficients[3] < 0)
                // Calculate the position of the minimum element in the array, the side where we were on the last course.
                return Array.IndexOf(coefficients, coefficients.Min());

            // If the detachment is not something important, you can try to make a random move.
            if (coefficients.Max() < 19)
            {
                var rnd = new Random();
                // With a 5% chance to make a random move.
                if (rnd.Next(0, 100) < 5)
                {
                    while (true)
                    {
                        var hand = coefficients[rnd.Next(coefficients.Length)];
                        if (hand >= 0)
                            return Array.IndexOf(coefficients, hand);
                    }
                }
            }

            // If the maximum coefficient in the array is equal to 0 && (AND), their number is greater than 1
            // This situation means that we have two or more empty spaces where you can walk
            // Between them it is necessary to select a random.
            if (coefficients.Max() == 0 && coefficients.Count(i => i == 0) > 1)
                return RandomBetween2EmptyCells(coefficients);

            // Calculate the position of the maximum element in the array, the side where it is necessary to be like.
            return Array.IndexOf(coefficients, coefficients.Max());
        }
    }
}