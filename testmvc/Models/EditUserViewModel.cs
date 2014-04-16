using System.ComponentModel.DataAnnotations;
using testmvc.App_LocalResources;
using Newtonsoft.Json;

namespace testmvc.Models
{
    using System;

    [Serializable]
    
    public class EditUserViewModel
    {

        public int UserId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "EmailIsRequired", ErrorMessageResourceType = typeof(Strings))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "EmailIsIncorrect", ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}