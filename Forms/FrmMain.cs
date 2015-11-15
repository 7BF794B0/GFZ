using System;
using System.Windows.Forms;

namespace GFZ.Forms
{
    abstract public partial class FrmMain : Form
    {
        protected FrmMain()
        {
            InitializeComponent();
        }

        virtual protected void Map_Click(object sender, EventArgs e) { }
        virtual protected void picNextTurn_Click(object sender, EventArgs e) { }
        virtual protected void btnSquadUpdate_Click(object sender, EventArgs e) { }
        virtual protected void tsmNewGame_Click(object sender, EventArgs e) { }
        virtual protected void tsmLoadGame_Click(object sender, EventArgs e) { }
        virtual protected void tsmSaveGame_Click(object sender, EventArgs e) { }
        virtual protected void tsmInfo_Click(object sender, EventArgs e) { }
    }
}