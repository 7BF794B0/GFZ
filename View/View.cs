// View.cs
// Class of View - a part of MVC, responsible for handling graphics.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GFZ.View
{
    public class View
    {
        #region Filling the Label game information
        /// <summary>
        /// Display information about the number of resources.
        /// </summary>
        /// <param name="lstLblResource">List of Label, which shows the amount of resources.</param>
        /// <param name="lstResource">The list, which keeps the sheer number of resources.</param>
        public void ChangeResource(IList<Label> lstLblResource, IList<int> lstResource)
        {
            for (var i = 0; i < lstResource.Count; i++)
                lstLblResource[i].Text = lstResource[i].ToString();
        }

        /// <summary>
        /// Display time information in the game.
        /// </summary>
        /// <param name="lblTime">Object of class Label, which is a control lblTime.</param>
        /// <param name="month">Number of months.</param>
        /// <param name="week">Number of weeks.</param>
        /// <param name="day">Number of days.</param>
        public void ChangeTime(Label lblTime, int month, int week, int day)
        {
            lblTime.Text = $"Month: {month} Week: {week} Day: {day}";
        }

        /// <summary>
        /// Displays information about the health of squad players.
        /// </summary>
        /// <param name="lblYourSquadHp">Object of class Label, which is a control lblYourSquadHP.</param>
        /// <param name="hp">The current number of health.</param>
        /// <param name="maxHp">The maximum value of the health squad.</param>
        public void ChangeSquadHp(Label lblYourSquadHp, int hp, int maxHp)
        {
            lblYourSquadHp.ForeColor = hp < maxHp / 2 ? Color.Red : Color.Green;
            lblYourSquadHp.Text = $"The general health of your squad: {hp}";
        }

        /// <summary>
        /// Displays information about the level of squad players.
        /// </summary>
        /// <param name="lstLblSquad">List of Label, which shows the levels of units.</param>
        /// <param name="lvl">The very meaning of squad level.</param>
        public void ChangeSquadLvl(IList<Label> lstLblSquad, int lvl)
        {
            foreach (var t in lstLblSquad)
                t.Text = lvl.ToString();
        }
        #endregion

        // Render squad.
        // Remove the old image in PictureBox.
        // And draw in new.
        /// <summary>
        /// Moves squad player or computer on the playing field.
        /// </summary>
        /// <param name="previous">Object of class PictureBox, which indicates detachment from moves.</param>
        /// <param name="now">Object of class PictureBox, which indicates where moves squad.</param>
        /// <param name="ai">Boolean, which allows you to determine who made the move - the player or computer.</param>
        public void MoveSquad(PictureBox previous, PictureBox now, bool ai)
        {
            // Move player.
            now.Image = previous.Image;
            // Move "logical" player.
            now.Tag = ai ? "28" : "27";
            // To the player is not stretched in space :)
            now.SizeMode = PictureBoxSizeMode.StretchImage;
            // Clear the PictureBox.
            previous.Image = null;
            // Now here void.
            previous.Tag = "0";
        }

        /// <summary>
        /// Erase a frame from the cells of the playing field.
        /// </summary>
        /// <param name="point">Object of class PictureBox, which says that in which the control must be erased frame.</param>
        /// <param name="g">Object of class Graphics, which allows you to open a specific PictureBox drawing.</param>
        public void InvalidateBacklight(PictureBox point, Graphics g)
        {
            // Clear frame.
            point.Invalidate();
            // Remove the object.
            g.Dispose();
        }

        /// <summary>
        /// Draw a box from a cell of the playing field.
        /// </summary>
        /// <param name="g">Object of class Graphics, which allows you to open a specific PictureBox drawing.</param>
        /// <param name="color">Object of class Color, which carries a frame color.</param>
        public void RenderBacklight(Graphics g, Color color)
        {
            var rect = new Rectangle(0, 1, 48, 48);
            g.DrawRectangle(new Pen(color, .5f), rect);
        }

        /// <summary>
        /// It displays a message about the final victory and defeat.
        /// </summary>
        /// <param name="what">Boolean, which allows you to determine which event happened - the victory or defeat.</param>
        public void TheEnd(bool what)
        {
            if (!what) return;

            if (MessageBox.Show(@"Congratulations you won!", @"The end!", MessageBoxButtons.OK) == DialogResult.OK)
                Application.Exit();
            else
                if (MessageBox.Show(@"Sorry you lost!", @"The end!", MessageBoxButtons.OK) == DialogResult.OK)
                    Application.Exit();
        }

        /// <summary>
        /// It displays a message with the results of the battle.
        /// </summary>
        /// <param name="win">A variable that allows you to determine which event happened - the victory or defeat, and who caused this event.</param>
        /// <param name="actor">Whoever caused this event.</param>
        /// <param name="exp">Experience gained over the event.</param>
        public void ShowFightResult(int win, string actor, int exp)
        {
            switch (win)
            {
                case 0:
                case 3:
                    MessageBox.Show($"Congratulations to your squad wins {actor}.\nReceived {exp} experience points.", @"The result of the battle!", MessageBoxButtons.OK);
                    break;
                case 1:
                case 4:
                    if (MessageBox.Show($"{actor} won your squad.\nSorry. You lose.", @"The result of the battle!", MessageBoxButtons.OK) == DialogResult.OK)
                        Application.Exit();
                    break;
                case 2:
                    MessageBox.Show(@"While a draw, try again, just watch out for health.", @"Drawn game!", MessageBoxButtons.OK);
                    break;
            }
        }

        /// <summary>
        /// It displays a message with the results improve.
        /// </summary>
        /// <param name="success">Boolean, which allows you to determine which event happened - the successful improvement or not.</param>
        public void ShowUpdateResult(bool success)
        {
            MessageBox.Show(
                success
                ? "Congratulations, improvement was successful. The level of your team up by 1"
                : "We are sorry, you do not have enough resources to improve the squad.", @"The result is improved!",
                MessageBoxButtons.OK);
        }

        /// <summary>
        /// Redraws the whole map when loading or starting a new game.
        /// </summary>
        /// <param name="map">Array of PictureBox, which is a map of the game world.</param>
        /// <param name="startMap">Array that stores a representation of the world starting from the save.</param>
        public void RedrawAllMap(PictureBox[,] map, int[,] startMap)
        {
            var objName = new[]
            {
                "empty", "empty", "Castle0", "Castle6", "Mine0", "Mine1", "Mine2", "Mine3", "Mine4", "Mine5", "Mine6",
                "Plant0", "Plant1", "Plant2", "Plant3", "Plant4", "Plant5", "Plant6", "Plant7",
                "Resource0", "Resource1", "Resource2", "Resource3", "Resource4", "Resource5", "Resource6", "Resource7",
                "Squad0", "Squad1",
                "Unit00", "Unit01", "Unit02", "Unit03", "Unit04", "Unit05", "Unit06", "Unit07", "Unit08", "Unit09", "Unit10", "Unit11", "Unit12", "Unit13"
            };
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 20; j++)
                {
                    map[i, j].Invalidate();
                    map[i, j].Tag = startMap[i, j].ToString();

                    if (startMap[i, j] == 0)
                        map[i, j].Image = null;
                    else if (startMap[i, j] > 18 && startMap[i, j] < 27)
                        map[i, j].SizeMode = PictureBoxSizeMode.CenterImage;
                    else if (startMap[i, j] == 27 || startMap[i, j] == 28)
                        // To the player is not stretched in the space :)
                        map[i, j].SizeMode = PictureBoxSizeMode.StretchImage;

                    var resObj = Properties.Resources.ResourceManager.GetObject(objName[startMap[i, j]]);
                    map[i, j].Image = (Bitmap)resObj;
                }
            }
        }

        #region Methods that are called from the Game class to change the pictures while generating
        /// <summary>
        /// It is used in the generation of a new world in the placement of monsters in the game map.
        /// Unit11, Unit12, Unit13.
        /// </summary>
        /// <param name="map">Array of PictureBox, which is a map of the game world.</param>
        /// <param name="x">Coordinate along the axis X.</param>
        /// <param name="y">Coordinate along the axis Y.</param>
        public void WeakMonsters(PictureBox[,] map, int x, int y)
        {
            var rnd = new Random();
            var id = rnd.Next(11, 14);

            var resObjUnit = Properties.Resources.ResourceManager.GetObject($"Unit{id:00}");
            map[x, y].Image = (Bitmap)resObjUnit;

            switch (id)
            {
                case 11:
                    // Zombie.
                    map[x, y].Tag = "40";
                    break;
                case 12:
                    // Goblin.
                    map[x, y].Tag = "41";
                    break;
                case 13:
                    // Skeleton.
                    map[x, y].Tag = "42";
                    break;
            }
        }

        /// <summary>
        /// It is used in the generation of a new world in the placement of monsters in the game map.
        /// Unit07, Unit08, Unit09, Unit10.
        /// </summary>
        /// <param name="map">Array of PictureBox, which is a map of the game world.</param>
        /// <param name="x">Coordinate along the axis X.</param>
        /// <param name="y">Coordinate along the axis Y.</param>
        public void AverageMonsters(PictureBox[,] map, int x, int y)
        {
            var rnd = new Random();
            var id = rnd.Next(7, 11);

            var resObjUnit = Properties.Resources.ResourceManager.GetObject($"Unit{id:00}");
            map[x, y].Image = (Bitmap)resObjUnit;

            switch (id)
            {
                case 7:
                    // Monk.
                    map[x, y].Tag = "36";
                    break;
                case 8:
                    // Knight.
                    map[x, y].Tag = "37";
                    break;
                case 9:
                    // Elf Archer.
                    map[x, y].Tag = "38";
                    break;
                case 10:
                    // Dwarf.
                    map[x, y].Tag = "39";
                    break;
            }
        }

        /// <summary>
        /// It is used in the generation of a new world in the placement of monsters in the game map.
        /// Unit03, Unit04, Unit05, Unit06.
        /// </summary>
        /// <param name="map">Array of PictureBox, which is a map of the game world.</param>
        /// <param name="x">Coordinate along the axis X.</param>
        /// <param name="y">Coordinate along the axis Y.</param>
        public void StrongMonsters(PictureBox[,] map, int x, int y)
        {
            var rnd = new Random();
            var id = rnd.Next(3, 7);

            var resObjUnit = Properties.Resources.ResourceManager.GetObject($"Unit{id:00}");
            map[x, y].Image = (Bitmap)resObjUnit;

            switch (id)
            {
                case 3:
                    // Minotaur.
                    map[x, y].Tag = "32";
                    break;
                case 4:
                    // Paladin.
                    map[x, y].Tag = "33";
                    break;
                case 5:
                    // Archmage.
                    map[x, y].Tag = "34";
                    break;
                case 6:
                    // Leach.
                    map[x, y].Tag = "35";
                    break;
            }
        }

        /// <summary>
        /// It is used in the generation of a new world in the placement of monsters in the game map.
        /// Unit00, Unit01, Unit02.
        /// </summary>
        /// <param name="map">Array of PictureBox, which is a map of the game world.</param>
        /// <param name="x">Coordinate along the axis X.</param>
        /// <param name="y">Coordinate along the axis Y.</param>
        public void BossesMonsters(PictureBox[,] map, int x, int y)
        {
            var rnd = new Random();
            var id = rnd.Next(0, 3);

            var resObjUnit = Properties.Resources.ResourceManager.GetObject($"Unit{id:00}");
            map[x, y].Image = (Bitmap)resObjUnit;

            switch (id)
            {
                case 0:
                    // Black Dragon.
                    map[x, y].Tag = "29";
                    break;
                case 1:
                    // Red Dragon.
                    map[x, y].Tag = "30";
                    break;
                case 2:
                    // Green Dragon.
                    map[x, y].Tag = "31";
                    break;
            }
        }
        #endregion
    }
}