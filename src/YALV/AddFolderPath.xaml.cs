using System.Windows;
using YALV.Common.Interfaces;
using YALV.ViewModel;

namespace YALV
{
    /// <summary>
    /// Interaction logic for AddFolderPath.xaml
    /// </summary>
    public partial class AddFolderPath : Window, IWinSimple
    {
        public AddFolderPath()
        {
            InitializeComponent();
            //this.Closing += delegate { _vm.Dispose(); };
        }

        public bool EditList()
        {
            bool res = false;
            using (var viewModel = new AddFolderPathVM(this))
            {
                DataContext = viewModel;
                ShowDialog();
                res = viewModel.ListChanged;
            }
            return res;
        }
    }
}
