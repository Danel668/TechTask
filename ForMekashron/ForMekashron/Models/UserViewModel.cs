using System.ComponentModel.DataAnnotations;

namespace ForMekashron.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
