using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Utilitarios.ControleXaml;
using Utilitarios.LeitorXml;

namespace WpfTeste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region CamposPrivados

        private DataTable dtXML = new DataTable();
        private LerXml conv = new LerXml();
        private CriarControle criarControle = new CriarControle();
        
        #endregion

        #region Eventos

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnProcessar_Click(object sender, RoutedEventArgs e)
        {
            renderizarXml();
        }
        #endregion

        #region Metodos
        private void renderizarXml()
        {
            if (!string.IsNullOrEmpty(txtXml.Text))
            {
                definirDataTableTags();

                dtXML = conv.CarregarXDocument(txtXml.Text);

                definirControles(dtXML);
            }
            else
                MessageBox.Show("Por favor, é necessário definir um xml!");
        }

        private void definirControles(DataTable dtTags)
        {

            ColumnDefinition coluna3 = new ColumnDefinition();
            coluna3.Width = new GridLength(1, GridUnitType.Auto);
            grid1.ColumnDefinitions.Add(coluna3);

            StackPanel stackPanel3 = new StackPanel();
            stackPanel3.HorizontalAlignment = HorizontalAlignment.Left;
            stackPanel3.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(stackPanel3, 2);
            Grid.SetRow(stackPanel3, 0);

            int left = 20;
            int top = 15;
            int right = 280;
            int bottom = 0;

            foreach (DataRow iRowControle in dtTags.Rows)
            {
                if (iRowControle["Type"].ToString() == "Element")
                {
                    string name = string.Empty;
                    string value = string.Empty;
                    string typpe = string.Empty;

                    string tagName = iRowControle["TagName"].ToString();

                    if (tagName == "Numericbox" || tagName == "Textbox" || tagName == "LabelElement" || tagName == "Button")
                    {
                        string filtroName = string.Concat("TagName=", "'", "name", "'", " AND XPath=", "'", iRowControle["XPath"] + "@name", "'");
                        string filtroValue = string.Concat("TagName=", "'", "label", "'", " AND XPath=", "'", iRowControle["XPath"] + "@label", "'");
                        string filtrotyppe = string.Concat("TagName=", "'", "type", "'", " AND XPath=", "'", iRowControle["XPath"] + "@type", "'");

                        if (dtTags.Select(filtroName).Length > 0)
                            foreach (DataRow item in dtTags.Select(filtroName))
                                name = item["Name"].ToString();

                        if (dtTags.Select(filtroValue).Length > 0)
                            foreach (DataRow item in dtTags.Select(filtroValue))
                                value = item["Name"].ToString();

                        if (dtTags.Select(filtrotyppe).Length > 0)
                            foreach (DataRow item in dtTags.Select(filtrotyppe))
                                typpe = item["Name"].ToString();

                        if (iRowControle["TagName"].ToString() == "Numericbox")
                        {
                            stackPanel3.Children.Add(criarControle.LabelXaml(name, value, left, top, right, bottom, "medium"));
                            stackPanel3.Children.Add(criarControle.TextBoxXaml(name, value, left, top, right, bottom));
                        }
                        else if (iRowControle["TagName"].ToString() == "Textbox")
                        {
                            stackPanel3.Children.Add(criarControle.LabelXaml(name, value, left, top, right, bottom, "medium"));
                            stackPanel3.Children.Add(criarControle.TextBoxXaml(name, value, left, top, right, bottom));
                        }
                        else if (iRowControle["TagName"].ToString() == "LabelElement")
                            stackPanel3.Children.Add(criarControle.LabelXaml(name, value, left, top, right, bottom, typpe));
                        else if (iRowControle["TagName"].ToString() == "Button")
                            stackPanel3.Children.Add(criarControle.BotaoXaml(name, value, left, top, right, bottom));
                    }


                }
                
            }

            grid1.Children.Add(stackPanel3);

           

        }

        private void definirDataTableTags()
        {
            dtXML = new DataTable();
            dtXML.Columns.Add("TagName", typeof(string));
            dtXML.Columns.Add("Type", typeof(string));
            dtXML.Columns.Add("XPath", typeof(string));
            dtXML.Columns.Add("Value", typeof(string));
        }
        #endregion

    }
}
