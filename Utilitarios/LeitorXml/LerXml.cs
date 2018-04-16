using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utilitarios.LeitorXml
{
    public class LerXml
    {
        #region CamposPrivados

        public DataTable dtlistNameNodesAndTypeAndSize = new DataTable();

        #endregion

        #region Metodos

        public DataTable CarregarXDocument(string xmlStringForDataTable)
        {
            dtlistNameNodesAndTypeAndSize = new DataTable();
            dtlistNameNodesAndTypeAndSize.Columns.Add("TagName", typeof(string));
            dtlistNameNodesAndTypeAndSize.Columns.Add("Type", typeof(string));
            dtlistNameNodesAndTypeAndSize.Columns.Add("XPath", typeof(string));
            dtlistNameNodesAndTypeAndSize.Columns.Add("Name", typeof(string));
            dtlistNameNodesAndTypeAndSize.Columns.Add("Value", typeof(string));

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xmlStringForDataTable);

            XmlNode root = xdoc.DocumentElement;

            GetNodes(root);

            return dtlistNameNodesAndTypeAndSize;
        }

        public void GetNodes(XmlNode parent)
        {

            foreach (XmlNode child in parent.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element || child.NodeType == XmlNodeType.Attribute)
                {
                    bool contem = false;
                    string xPath = FindXPath(child);

                    if (dtlistNameNodesAndTypeAndSize.Rows.Count > 0)
                    {
                        string filtro = string.Concat("TagName=", "'", child.Name, "'", " AND XPath=", "'", xPath, "'");

                        //Não duplicar nome e xPath
                        if (dtlistNameNodesAndTypeAndSize.Select(filtro).Any())
                            contem = true;

                        //Pegar apenas 1 primeiro index daestrutura
                        if (NaoEoPrimeiroIndex(child))
                            contem = true;
                    }

                    if (!contem)
                    {
                        dtlistNameNodesAndTypeAndSize.Rows.Add(child.Name, child.NodeType, xPath, child.Value);

                        foreach (XmlAttribute atributo in child.Attributes)
                        {
                            dtlistNameNodesAndTypeAndSize.Rows.Add(atributo.Name, atributo.NodeType, xPath + "@" + atributo.Name, atributo.Value);
                        }
                    }


                    GetNodes(child);
                }
            }
        }

        public bool NaoEoPrimeiroIndex(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                return FindElementIndex((XmlElement)node) > 1
                        || node.LastChild != null
                            && string.IsNullOrEmpty(node.LastChild.Value)
                                && FindElementIndex((XmlElement)node.LastChild) > 1;
            }
            else
            {
                return false;
            }

        }

        static string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();

            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)node);
                        builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    case XmlNodeType.Text:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Elemento ou atributo não suportado");
                }
            }
            throw new ArgumentException("Não reconhecido");
        }

        static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("não foi possivel encontrar o node filho");
        }

        #endregion
    }
}
