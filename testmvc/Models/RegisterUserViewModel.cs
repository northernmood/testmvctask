using System.ComponentModel.DataAnnotations;
using testmvc.App_LocalResources;

namespace testmvc.Models
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [Display(ResourceType = typeof(Strings), Name = "LoginLabel")]
        public string LoginName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [MinLength(6, ErrorMessageResourceName = "PasswordIsTooShort", ErrorMessageResourceType = typeof(Strings))]
        [Display(ResourceType = typeof(Strings), Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordConfirmationFailed")]
        [Display(ResourceType = typeof(Strings), Name = "PasswordConfirm")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessageResourceName = "EmailIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "EmailIsIncorrect", ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool DataBaseIsEmpty { get; set; }
    }
}