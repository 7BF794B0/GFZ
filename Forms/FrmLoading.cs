using System.Windows.Forms;

namespace GFZ.Forms
{
    public partial class FrmLoading : Form
    {
        #region getter/setter
        public int PbMaxValue
        {
            set { pbLoading.Maximum = value; }
        }

        public int PbValueNow
        {
            get { return pbLoading.Value; }
            set { pbLoading.Value = value; }
        }
        #endregion

        public FrmLoading()
        {
            InitializeComponent();
        }
    }
}