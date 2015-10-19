using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Sequention
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void CalculateClick(object sender = null, RoutedEventArgs e = null)
        {
            //try
            {
                if (LIndex.Value > UIndex.Value)
                {
                    Resultant.Text = "Upper Index must be greater than Lower Index";
                    return;
                }
                else if (string.IsNullOrWhiteSpace(ExpressionBox.Text))
                {
                    Resultant.Text = "Empty Expression";
                    return;
                }

                if (Options.SelectedIndex == 0)
                {
                    var Parser = new MathParsing.MathParser();
                    Parser.Variables.Add(Iterate.Text, new MathParsing.Variable(LIndex.Value));
                    Resultant.Text = Parser.Evaluate(ExpressionBox.Text).ToString();
                }

                else if (Options.SelectedIndex == 1)
                    Resultant.Text = Sigmation.Sigma(ExpressionBox.Text, LIndex.Value, UIndex.Value, char.Parse(Iterate.Text), Step.Value).ToString();

                else if (Options.SelectedIndex == 2)
                    Resultant.Text = Sigmation.Pi(ExpressionBox.Text, LIndex.Value, UIndex.Value, char.Parse(Iterate.Text), Step.Value).ToString();

                else if (Options.SelectedIndex == 3)
                    Resultant.Text = Sigmation.Integral(ExpressionBox.Text, LIndex.Value, UIndex.Value, char.Parse(Iterate.Text), IntegrationKind.SelectedIndex).ToString();
            }
            //catch { Resultant.Text = "Error"; }
        }

        void ExpressionChanged(object sender, TextChangedEventArgs e) { Resultant.Text = "Ready"; }

        void Reset(object sender, RoutedEventArgs e)
        {
            Resultant.Text = "Ready";
            LIndex.Value = 1;
            Step.Value = 1;
            UIndex.Value = 10;
            ExpressionBox.Text = string.Empty;
            Iterate.Text = "r";
        }

        void Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                StepPanel.Visibility = (Options.SelectedIndex != 3 && Options.SelectedIndex != 0) ? Visibility.Visible : Visibility.Hidden;
                IntegrationKind.Visibility = Options.SelectedIndex == 3 ? Visibility.Visible : Visibility.Hidden;
                UIndex.Visibility = Options.SelectedIndex != 0 ? Visibility.Visible : Visibility.Hidden;
            }
            catch { }
        }

        void TextBox_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Return) CalculateClick(); }
    }
}
