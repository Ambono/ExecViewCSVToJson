using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExecViewCSVtoJS.Models
{
    public class FilePathParam
    {
        [DisplayName("Enter CSV file path(example c:\\User\\bulls.csv)")]
        [Required(ErrorMessage = "Please enter CSV file path")]
        [FileExtensions(Extensions = "CSV,csv)", ErrorMessage = "Please upload valid file format(csv)")]
        public string uploadinput { get; set; }

        //[RegularExpression(@"^[\w\s_\\.\-:]+ (.JS|.js)$", ErrorMessage = "This json file path is incorect")]
        [FileExtensions(Extensions = "JS,js)", ErrorMessage = "Please upload valid file format(js)")]
        [DisplayName("Enter Json file path(example c:\\User\\bulls.js)")]
        [Required(ErrorMessage = "Please enter json file path")]
        public string uploadoutput { get; set; }

        [DisplayName("Browsing function to help find paths")]
        public string pathchecker { get; }
    }
}