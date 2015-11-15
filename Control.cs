// Control.cs
// The main form of child class - organizes all the interactions with the mechanics of the game.
// Is part of the MVC - controller that interacts model and view.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
// Project forms.
using GFZ.Forms;
// "Model" - part of the MVC.
using GFZ.Model;

namespace GFZ
{
    class Control : FrmMain
    {
        // An array representing our map (game world).
        private PictureBox[,] _map;
        // An array that stores a start state map.
        private readonly int[,] _startMap = new int[10, 20];
        // List of label, which displays information about the amount of resources to the screen.
        private readonly IList<Label> _lstLblResource = new List<Label>();
        // List of label, which displays information about the level of squad.
        private readonly IList<Label> _lstLblSquad = new List<Label>();

        private readonly Game _game = new Game();
        private readonly View.View _view = new View.View();
        private readonly Memento _memento = new Memento();
        private readonly Debra _debra = new Debra();

        // An array that contains all the monsters that are in the game.
        private readonly Unit[] _arrayMonsters = new Unit[14];

        private readonly Squad _squadPlayer = new Squad();
        private readonly Squad _squadAi = new Squad();

        // Object class to play music at background.
        private readonly WMPLib.WindowsMediaPlayer _wmp = new WMPLib.WindowsMediaPlayer();

        // AI On.
        private bool _aiEnable = true;
        // Variable that checks in what position is the cell with the player - pressed / depressed.
        private bool _cellIsPressed;
        // Variable is true when, the move is completed and when you need to make the transition progress.
        private bool _turnIsCompleted;
        // A variable that keeps the last key cell.
        private PictureBox _temp = new PictureBox();

        /// <summary>
        /// Initialize all (variables, objects, arrays) that we have.
        /// </summary>
        private void Init()
        {
            // Associates each PictureBox with its place in the array (that is exactly SOLID IMAGES).
            _map = new[,]
            {
                {pic0, pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13, pic14, pic15, pic16, pic17, pic18, pic19},
                {pic20, pic21, pic22, pic23, pic24, pic25, pic26, pic27, pic28, pic29, pic30, pic31, pic32, pic33, pic34, pic35, pic36, pic37, pic38, pic39},
                {pic40, pic41, pic42, pic43, pic44, pic45, pic46, pic47, pic48, pic49, pic50, pic51, pic52, pic53, pic54, pic55, pic56, pic57, pic58, pic59},
                {pic60, pic61, pic62, pic63, pic64, pic65, pic66, pic67, pic68, pic69, pic70, pic71, pic72, pic73, pic74, pic75, pic76, pic77, pic78, pic79},
                {pic80, pic81, pic82, pic83, pic84, pic85, pic86, pic87, pic88, pic89, pic90, pic91, pic92, pic93, pic94, pic95, pic96, pic97, pic98, pic99},
                {pic100, pic101, pic102, pic103, pic104, pic105, pic106, pic107, pic108, pic109, pic110, pic111, pic112, pic113, pic114, pic115, pic116, pic117, pic118, pic119},
                {pic120, pic121, pic122, pic123, pic124, pic125, pic126, pic127, pic128, pic129, pic130, pic131, pic132, pic133, pic134, pic135, pic136, pic137, pic138, pic139},
                {pic140, pic141, pic142, pic143, pic144, pic145, pic146, pic147, pic148, pic149, pic150, pic151, pic152, pic153, pic154, pic155, pic156, pic157, pic158, pic159},
                {pic160, pic161, pic162, pic163, pic164, pic165, pic166, pic167, pic168, pic169, pic170, pic171, pic172, pic173, pic174, pic175, pic176, pic177, pic178, pic179},
                {pic180, pic181, pic182, pic183, pic184, pic185, pic186, pic187, pic188, pic189, pic190, pic191, pic192, pic193, pic194, pic195, pic196, pic197, pic198, pic199}
            };

            // Remove mold start state.
            for (var i = 0; i < 10; i++)
                for (var j = 0; j < 20; j++)
                    _startMap[i, j] = Convert.ToInt32(_map[i, j].Tag.ToString());

            // Adding each lblResource to the list.
            _lstLblResource.Add(lblResource0);
            _lstLblResource.Add(lblResource1);
            _lstLblResource.Add(lblResource2);
            _lstLblResource.Add(lblResource3);
            _lstLblResource.Add(lblResource4);
            _lstLblResource.Add(lblResource5);
            _lstLblResource.Add(lblResource6);

            // Adding to the list every lblYourUnit.
            _lstLblSquad.Add(lblYourUnit0);
            _lstLblSquad.Add(lblYourUnit1);
            _lstLblSquad.Add(lblYourUnit2);
            _lstLblSquad.Add(lblYourUnit3);
            _lstLblSquad.Add(lblYourUnit4);

            // Initialization monsters.
            string[] names = { "Skeleton", "Goblin", "Zombie", "Dwarf", "Elf Archer", "Knight", "Monk", "Leach", "Archmage", "Paladin", "Minotaur", "Green Dragon", "Red Dragon", "Black Dragon" };
            int[] hp = { 20, 30, 40, 60, 80, 100, 120, 140, 160, 180, 200, 300, 400, 500 };
            int[] dmg = { 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200, 250 };
            int[] exp = { 50, 50, 50, 100, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
            for (var i = 0; i < names.Length; i++)
                _arrayMonsters[i] = new Unit(i + 1, names[i], 1, hp[i], dmg[i], exp[i]);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Control()
        {
            Init();
            // Fill army squad units.
            CreateArmy();
            SendAllToView();
            PlayMusic();
        }

        /// <summary>
        /// Fills squads player and computer units.
        /// </summary>
        private void CreateArmy()
        {
            // For Player:
            var unitDwarfForSquadPlayer = new Unit(10, "Dwarf", 1, 60, 30, 100);
            var unitElfArcherForSquadPlayer = new Unit(9, "Elf Archer", 1, 80, 40, 100);
            var unitKnightForSquadPlayer = new Unit(8, "Knight", 1, 100, 50, 200);
            var unitArchmageForSquadPlayer = new Unit(5, "Archmage", 1, 160, 80, 500);
            var unitPaladinForSquadPlayer = new Unit(4, "Paladin", 1, 180, 90, 600);

            // Add them to the army:
            _squadPlayer.AddUnitToArmy(unitDwarfForSquadPlayer);
            _squadPlayer.AddUnitToArmy(unitElfArcherForSquadPlayer);
            _squadPlayer.AddUnitToArmy(unitKnightForSquadPlayer);
            _squadPlayer.AddUnitToArmy(unitArchmageForSquadPlayer);
            _squadPlayer.AddUnitToArmy(unitPaladinForSquadPlayer);

            // For computer:
            var unitSkeletonForSquadAi = new Unit(13, "Skeleton", 1, 20, 10, 50);
            var unitGoblinForSquadAi = new Unit(12, "Goblin", 1, 30, 15, 50);
            var unitZombie = new Unit(11, "Zombie", 1, 40, 20, 50);

            // Add them to the army:
            _squadAi.AddUnitToArmy(unitSkeletonForSquadAi);
            _squadAi.AddUnitToArmy(unitGoblinForSquadAi);
            _squadAi.AddUnitToArmy(unitZombie);
        }

        /// <summary>
        /// Starts playing music.
        /// </summary>
        private void PlayMusic()
        {
            _wmp.URL = "music/music.mp3";
            _wmp.controls.play();
        }

        /// <summary>
        /// Send all the data on the drawing (as an analysis of the map).
        /// </summary>
        private void SendAllToView()
        {
            // Scan the map.
            _debra.AnalyzingWorld(_map);
            // Change resources.
            _view.ChangeResource(_lstLblResource, _game.GetListAllResource());
            // Change health squad.
            _view.ChangeSquadHp(lblYourSquadHP, _squadPlayer.Hp, _squadPlayer.MaxHp);
            // Change the level of squad.
            _view.ChangeSquadLvl(_lstLblSquad, _squadPlayer.Level);
            // Change the time in the game.
            _view.ChangeTime(lblTime, _game.Month, _game.Week, _game.Day);
        }

        /// <summary>
        /// Event: Click on the map.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void Map_Click(object sender, EventArgs e)
        {
            var point = (PictureBox)sender;
            var g = point.CreateGraphics();

            // Progress has not yet been committed?
            if (_turnIsCompleted) return;

            // If the player presses.
            if (point.Tag.ToString() == "27")
            {
                // The player has already been presses.
                if (_cellIsPressed)
                {
                    // Remove frame.
                    _view.InvalidateBacklight(point, g);
                    // Players will no longer presses.
                    _cellIsPressed = false;
                }
                // The player has not been pressed.
                else
                {
                    // Draw frame.
                    _view.RenderBacklight(g, System.Drawing.Color.Red);
                    // Remember cell where we were, because all of a sudden we're like.
                    _temp = point;
                    // Player pressed.
                    _cellIsPressed = true;
                }
            }

            // The player clicked and we can not push the player.
            if (!_cellIsPressed || point.Tag.ToString() == "27") return;

            // If such a move is possible.
            if (_game.SuchActionIsPossible(Convert.ToInt32(point.Tag.ToString()),
                Convert.ToInt32(_temp.Tag.ToString()),
                point.Name,
                _temp.Name,
                _squadPlayer,
                _squadAi,
                _arrayMonsters))
            {
                // Drawn game in the fight with the monster.
                if (_game.Win == 2)
                {
                    // Remove frame.
                    _view.InvalidateBacklight(_temp, g);
                    // To display a message with the results of the battle.
                    _view.ShowFightResult(_game.Win, _game.Actor, _game.Experience);
                    // Display the changing HP.
                    _view.ChangeSquadHp(lblYourSquadHP, _squadPlayer.Hp, _squadPlayer.MaxHp);
                }
                else
                {
                    // Remove frame.
                    _view.InvalidateBacklight(_temp, g);

                    // The fight was on this course?
                    if (_game.FightOccurred)
                        // To display a message with the results of the battle.
                        _view.ShowFightResult(_game.Win, _game.Actor, _game.Experience);

                    // Move squad.
                    _view.MoveSquad(_temp, point, false);

                    // When we won the detachment, the.
                    if (_game.Win == 3)
                    {
                        // It is necessary to disable AI.
                        _aiEnable = false;
                        // Reset the neutral state (otherwise this condition will be checked regularly).
                        _game.Win = -1;
                    }

                    // Change the resources - in the event if we have something picked up.
                    _view.ChangeResource(_lstLblResource, _game.GetListAllResource());
                    // Change the level of squad.
                    _view.ChangeSquadLvl(_lstLblSquad, _squadPlayer.Level);
                    // Display the changing HP.
                    _view.ChangeSquadHp(lblYourSquadHP, _squadPlayer.Hp, _squadPlayer.MaxHp);

                    // Progress made - you can not walk more.
                    _turnIsCompleted = true;
                    // Player no longer pressed.
                    _cellIsPressed = false;
                }
            }

            // If we decided to take mine.
            if (_game.IsMine)
            {
                // Draw frame.
                _view.RenderBacklight(g, System.Drawing.Color.Aqua);
                // Remove frame.
                _view.InvalidateBacklight(_temp, g);

                // Display the changing HP.
                _view.ChangeSquadHp(lblYourSquadHP, _squadPlayer.Hp, _squadPlayer.MaxHp);
                // Change the resources - in the event if we have something picked up.
                _view.ChangeResource(_lstLblResource, _game.GetListAllResource());

                // Progress made - you can not walk more.
                _turnIsCompleted = true;
                // Player no longer pressed.
                _cellIsPressed = false;
            }

            // This is a victory?
            if (_game.GlobalWin) _view.TheEnd(true);
        }

        /// <summary>
        /// Function to select the side (where the course will be accomplished) (for AI).
        /// </summary>
        /// <param name="hand">"Coefficient of appeal".</param>
        /// <param name="side">Side (left, right, top, bottom).</param>
        /// <param name="x">Coordinate along the X axis.</param>
        /// <param name="y">Coordinate along the Y axis.</param>
        private void SelectSide(int hand, int side, int x, int y)
        {
            // Where AI decided to be like = side, we check?
            if (hand != side) return;

            // And in addition, there still is an enemy unit.
            if (Convert.ToInt32(_map[_debra.PointX + x, _debra.PointY + y].Tag) == 27)
            {
                _game.SquadVsSquad(_squadAi, _squadPlayer);

                // The battle took place?
                if (_game.FightOccurred)
                    // To display a message with the results of the battle.
                    _view.ShowFightResult((_game.Win == 3) ? 4 : 3, _game.Actor, _game.Experience);

                // Computer lost.
                if (_game.Win == 4)
                {
                    _map[_debra.PointX, _debra.PointY].Image = null;
                    _map[_debra.PointX, _debra.PointY].Tag = "0";
                    _aiEnable = false;
                }

                // Computer won.
                if (_game.Win == 3)
                    // Move squad.
                    _view.MoveSquad(_map[_debra.PointX, _debra.PointY], _map[_debra.PointX + x, _debra.PointY + y], true);
            }
            else
                // In all other cases, the same squad move.
                _view.MoveSquad(_map[_debra.PointX, _debra.PointY], _map[_debra.PointX + x, _debra.PointY + y], true);
        }

        /// <summary>
        /// Event: Click on the button "End turn".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void picNextTurn_Click(object sender, EventArgs e)
        {
            _game.Day++;
            _squadPlayer.Hp += 10;
            _game.UpdateListAllResource();

            if (_aiEnable)
            {
                // Move squad.
                // Choosing sides, which have to go.
                var hand = _debra.Make(_squadPlayer, _squadAi, _arrayMonsters);
                // To the left.
                SelectSide(hand, 0, 0, -1);
                // To the right.
                SelectSide(hand, 1, -1, 0);
                // To the up.
                SelectSide(hand, 2, 0, 1);
                // To the down.
                SelectSide(hand, 3, 1, 0);
            }

            SendAllToView();

            // You can walk again.
            _turnIsCompleted = false;
        }

        /// <summary>
        /// Event: Click on the "Update of squad".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void btnSquadUpdate_Click(object sender, EventArgs e)
        {
            // Check the required amount of resources.
            if (_game.Wood > 20 &&
                _game.Stone > 20 &&
                _game.Mercury > 10 &&
                _game.Sulfur > 10 &&
                _game.Crystals > 10 &&
                _game.Gems > 10 &&
                _game.Gold > 20000)
            {
                _game.UpdateSquad();
                _squadPlayer.Experience = 1000;

                // Change resources.
                _view.ChangeResource(_lstLblResource, _game.GetListAllResource());
                // Change health squad.
                _view.ChangeSquadHp(lblYourSquadHP, _squadPlayer.Hp, _squadPlayer.MaxHp);
                // Change the level of squad.
                _view.ChangeSquadLvl(_lstLblSquad, _squadPlayer.Level);
                // Display a message with the results of update.
                _view.ShowUpdateResult(true);
            }
            else
                _view.ShowUpdateResult(false);
        }

        #region Верхнее Меню
        /// <summary>
        /// Event: Click on the menu item "New Game".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void tsmNewGame_Click(object sender, EventArgs e)
        {
            var load = new FrmLoading {PbMaxValue = 16};
            load.Show();

            _cellIsPressed = false;
            _turnIsCompleted = false;

            _debra.ResetToDefault();
            _game.ResetToDefault();
            _squadAi.ResetToDefault();
            _squadPlayer.ResetToDefault();

            _view.RedrawAllMap(_map, _startMap);
            _game.Create(_map, _view, load);
            CreateArmy();
            SendAllToView();

            load.Dispose();
        }

        /// <summary>
        /// Event: Click on the menu item "Load Game".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void tsmLoadGame_Click(object sender, EventArgs e)
        {
            _memento.LoadGame(openFileDialog, _debra, _game, _squadAi, _squadPlayer);
            _view.RedrawAllMap(_map, _debra.GetLogicArray());
            _game.UpdateListAllResource();
            SendAllToView();
        }

        /// <summary>
        /// Event: Click on the menu item "Save game".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void tsmSaveGame_Click(object sender, EventArgs e)
        {
            // Before we continue, it is necessary to get the latest state of the world.
            _debra.AnalyzingWorld(_map);
            _memento.SaveGame(saveFileDialog, _debra, _game, _squadAi, _squadPlayer);
        }

        /// <summary>
        /// Event: Click on the menu item "About".
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        override protected void tsmInfo_Click(object sender, EventArgs e)
        {
            var frm = new FrmInfo();
            frm.Show();
        }
        #endregion
    }
}