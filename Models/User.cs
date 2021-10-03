namespace UserWebAppSolution.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User 
    {

        #region Properties

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers allowed")]
        public string CellPhoneNumber { get; set; }

        public string ID { get; set; }

        #endregion Properties

    }
}
