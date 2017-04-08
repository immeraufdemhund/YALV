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
        }

        public bool EditList()
        {
            var res = false;
            using (var viewModel = new AddFolderPathVM(this))
            {
                //viewModel.Initialize();
                DataContext = viewModel;
                ShowDialog();
                res = viewModel.ListChanged;
            }
            return res;
        }
    }
}
