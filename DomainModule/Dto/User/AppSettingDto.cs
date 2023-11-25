using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Dto
{
    public class AppSettingDto
    {
        public string Key { get; set; }
        public string? Value { get; set; }
        public string UserId { get; set; }
    }
    public class AppSettingModel
    {
        public List<AppSettingDto> AppSettings { get; set; } = new List<AppSettingDto>(); 
    }
}
