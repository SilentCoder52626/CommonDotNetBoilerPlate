using DomainModule.BaseRepo;
using DomainModule.Dto;
using DomainModule.Dto.User;
using DomainModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface
{
    public interface AppSettingsRepositoryInterface : BaseRepositoryInterface<AppSettings>
    {
        AppSettings? GetByKey(string key,string userId);
        string? GetValueOf(string key,string userId);
        AppSettingModel GetAppSettingModel(string userId);
    }
}
