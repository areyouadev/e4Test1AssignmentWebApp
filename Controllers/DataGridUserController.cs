namespace UserWebAppSolution.Controllers
{
    using System.Xml;
    using System.Data;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using DevExtreme.AspNet.Mvc;
    using DevExtreme.AspNet.Data;
    using Microsoft.AspNetCore.Mvc;

    using Models;
    using Service;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;

    [ApiController]
    [Route("api/[controller]")]
    public class DataGridUserController : Controller
    {
        #region  Fields

        private string filePath = string.Empty;
        UserService userService = new UserService();
        private IHostingEnvironment _hostingEnvironment;

        #endregion Fields

        #region  Constructor

        public DataGridUserController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var path = hostingEnvironment.ContentRootPath + @"\Resource\Users.xml";
            userService.filePath = path;
            filePath = path;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            DataSet ds = new DataSet();
            XmlReader xmlFile;
            xmlFile = XmlReader.Create(filePath, new XmlReaderSettings());
            ds.ReadXml(xmlFile);

            if (ds?.Tables?.Count > 0)
            {
                var result = Json(DataSourceLoader.Load(Convert(ds), loadOptions));

                xmlFile.Close();

                return result;
            }

            return null;
        }

        [HttpPost]
        public IActionResult Post([FromForm] string values)
        {

            var newUser = new User();
            JsonConvert.PopulateObject(values, newUser);

            if (!TryValidateModel(newUser))
                return BadRequest();

            var xDoc = userService.AddUser(newUser.Name, newUser.Surname, newUser.CellPhoneNumber);

            xDoc.Save(filePath);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromForm] int key, [FromForm] string values)
        {

            List<User> userList = new List<User>();
            DataSet ds = new DataSet();
            XmlReader xmlFile;
            xmlFile = XmlReader.Create(filePath, new XmlReaderSettings());
            ds.ReadXml(xmlFile);

            if (ds?.Tables?.Count > 0)
            {
                userList = Convert(ds);
                xmlFile.Close();
            }

            var exisUser = userList.First(x => x.ID == key.ToString());

            JsonConvert.PopulateObject(values, exisUser);

            if (!TryValidateModel(exisUser))
                return BadRequest();

            var xDoc = userService.UpdateUser(exisUser.Name, exisUser.Surname, exisUser.CellPhoneNumber, key.ToString());

            xDoc.Save(filePath);

            return Ok();
        }

        [HttpDelete]
        public void Delete([FromForm] int key)
        {

            var xDocument = userService.DeleteUser(key.ToString());
            xDocument.Save(filePath);
        }

        public ActionResult CheckPhoneNumber(string phoneNumber)
        {
            return Content(phoneNumber);
        }

        #endregion Methods

        #region  PrivateMethods

        private List<User> Convert(DataSet ds)
        {
            List<User> userList = new List<User>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                User user = new User();
                user.ID = row[0].ToString();
                user.Name = row[1].ToString();
                user.Surname = row[2].ToString();
                user.CellPhoneNumber = row[3].ToString();
                userList.Add(user);
            }

            return userList;
        }

        #endregion PrivateMethods
    }
}