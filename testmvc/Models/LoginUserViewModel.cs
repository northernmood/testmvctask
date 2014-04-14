using System.ComponentModel.DataAnnotations;
using testmvc.App_LocalResources;

namespace testmvc.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [Display(ResourceType = typeof(Strings), Name = "LoginLabel")]
        public string LoginName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [Display(ResourceType = typeof(Strings), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}