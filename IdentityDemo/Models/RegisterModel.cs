using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        [DataType( DataType.Password )]
        public string Password { get; set; }

        [Compare( nameof( Password ) )]
        [DataType( DataType.Password )]
        public string ConfirmPassword { get; set; }
    }
}
