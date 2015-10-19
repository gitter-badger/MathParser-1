using System.Windows;
using System.Windows.Input;
using MathParsing.Scripting;

namespace ScriptApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh,
                (s, e) => ResultBox.Content = new Script().Run(CodeBox.Text)));
                
        }
    }
}
