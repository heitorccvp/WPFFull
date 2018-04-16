using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utilitarios.ControleXaml
{
    public class CriarControle
    {
        public TextBox TextBoxXaml(string nm_controle, string value, int left, int top, int right, int bottom)
        {
            TextBox txt = new TextBox();
            txt.Name = nm_controle;
            txt.Height = 20;
            txt.Width = 150;
            txt.Text = value;
            txt.Foreground = new SolidColorBrush(Colors.Silver);
            txt.Margin = new Thickness(left, top, right, bottom);
            return txt;

        }

        public Button BotaoXaml(string nm_controle, string value, int left, int top, int right, int bottom)
        {
            Button btn = new Button();
            btn.Name = nm_controle;
            btn.Height = 20;
            btn.Width = 80;
            btn.Content = value;
            btn.Background = new SolidColorBrush(Colors.Silver);
            btn.Margin = new Thickness(left, top, right, bottom);
            return btn;
        }

        public TextBlock LabelXaml(string nm_controle, string value, int left, int top, int right, int bottom, string typpe)
        {
            TextBlock lbl = new TextBlock();
            lbl.Name = nm_controle;
            lbl.Height = 25;
            lbl.Width = 100;
            lbl.Text = value;
            lbl.FontSize = typpe == "small" ? 8 : typpe == "medium" ? 18 : typpe == "big" ? 28 : 8;
            lbl.Margin = new Thickness(left, top, right, bottom);
            return lbl;
        }

    }
}
