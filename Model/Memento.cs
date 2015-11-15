// Memento.cs
// The class implements the pattern Memento.
// Used to save and load games.
using System.IO;

namespace GFZ.Model
{
    class Memento
    {
        /// <summary>
        /// Function to save the game.
        /// </summary>
        /// <param name="sfd">Object class SaveFileDialog.</param>
        /// <param name="d">Object class Debra.</param>
        /// <param name="g">Object of the Game class.</param>
        /// <param name="ai">Object class Squad (detachment of the computer).</param>
        /// <param name="pl">Object class Squad (squad players).</param>
        public void SaveGame(System.Windows.Forms.SaveFileDialog sfd, Debra d, Game g, Squad ai, Squad pl)
        {
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            using (Stream save = File.Open(sfd.FileName, FileMode.Create))
            using (var sw = new StreamWriter(save))
            {
                // TODO: Нормально реализовать данный функционал, а не как было.
            }
        }

        /// <summary>
        /// Function to loading games.
        /// </summary>
        /// <param name="ofd">Object class OpenFileDialog.</param>
        /// <param name="d">Object class Debra.</param>
        /// <param name="g">Object of the Game class.</param>
        /// <param name="ai">Object class Squad (detachment of the computer).</param>
        /// <param name="pl">Object class Squad (squad players).</param>
        public void LoadGame(System.Windows.Forms.OpenFileDialog ofd, Debra d, Game g, Squad ai, Squad pl)
        {
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            using (Stream save = File.Open(ofd.FileName, FileMode.Open))
            using (var sr = new StreamReader(save))
            {
                // TODO: Нормально реализовать данный функционал, а не как было.
            }
        }
    }
}