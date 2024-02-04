using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{ 
    public class OfficialDay
    {
        public string name { get; set; }
        public string nameEn { get; set; }
        public string dayName { get; set; }
        public DateTime date { get; set; }

        public OfficialDay(string name, string nameEn, string dayName, DateTime date)
        {
            this.name = name;
            this.nameEn = nameEn;
            this.dayName = dayName;
            this.date = date;
        }
    }
}
