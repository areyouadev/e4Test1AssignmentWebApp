namespace UserWebAppSolution.Service
{
    using System.Linq;
    using System.Xml.Linq;

    public class UserService
    {
        #region Properties

        public string filePath { get; set; }

        #endregion

        #region  Methods

        public XDocument UpdateUser(string name, string surname, string cellPhoneNumber, string id)
        {
            XDocument xDoc = XDocument.Load(filePath);
            XElement node = xDoc.Element("Users").Elements("User").FirstOrDefault(a => a.Element("Id").Value.Trim() == id);
            if (node != null)
            {
                node.SetElementValue("Name", name);
                node.SetElementValue("Surname", surname);
                node.SetElementValue("CellPhoneNumber", cellPhoneNumber);
            }
            return xDoc;
        }
        public XDocument DeleteUser(string id)
        {
            XDocument xDoc = XDocument.Load(filePath);
            xDoc.Root.Elements().Where(a => a.Element("Id").Value == id).Remove();

            return xDoc;
        }
        public XDocument CreateUser(string name, string surname, string cellPhoneNumber)
        {

            XDocument xDoc = new XDocument(
             new XDeclaration("1.0", null, "yes"),
             new XElement("Users",
             new XElement("User",
             new XElement("Id", 1),
             new XElement("Name", name),
             new XElement("Surname", surname),
             new XElement("CellPhoneNumber", cellPhoneNumber)))
              );
            return xDoc;
        }
        public XDocument AddUser(string name, string surname, string cellPhoneNumber)
        {
            XDocument xDocLoad = XDocument.Load(filePath);
            int maxID = 1;
            var count = xDocLoad.Descendants("Id");
            if (xDocLoad != null && count.Count() > 0)
            {
                maxID = xDocLoad.Descendants("Id").Max(u => (int)u);
                maxID++;
            }
            xDocLoad.Element("Users").Add(
                  new XElement("User",
                  new XElement("Id", maxID),
                  new XElement("Name", name),
                  new XElement("Surname", surname),
                  new XElement("CellPhoneNumber", cellPhoneNumber))
                );
            return xDocLoad;
        }

        #endregion Methods
    }
}
