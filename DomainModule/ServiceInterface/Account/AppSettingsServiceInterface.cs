using DomainModule.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface
{
    public interface AppSettingsServiceInterface
    {
        void UpdateSetting(AppSettingDto dto);
        void BulkUpdateSetting(List<AppSettingDto> dto);
    }
}
