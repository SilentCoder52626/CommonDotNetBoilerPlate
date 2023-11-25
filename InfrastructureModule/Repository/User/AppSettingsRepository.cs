using DomainModule.Dto;
using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.Enums;
using DomainModule.RepositoryInterface;
using InfrastructureModule.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository
{
    public class AppSettingsRepository :  BaseRepository<AppSettings>, AppSettingsRepositoryInterface
    {
        public AppSettingsRepository(AppDbContext context) : base(context)
        {
        }

        public AppSettingModel GetAppSettingModel(string userId)
        {
            var model = new AppSettingModel();
            string[] AppSettingKeys = Enum.GetNames(typeof(AppSettingsEnum));
            var AppSettings = new List<string>(AppSettingKeys);
            foreach (var item in AppSettings)
            {
                model.AppSettings.Add(new AppSettingDto()
                {
                    Key = item,
                    UserId = userId,
                    Value = GetValueOf(item, userId)
                });
            }
            return model;
        }

        public AppSettings? GetByKey(string key,string userId)
        {
            return GetQueryable().Where(a => a.Key == key && a.UserId == userId).FirstOrDefault();
        }

        public string? GetValueOf(string key, string userId)
        {
            return GetQueryable().Where(a => a.Key == key && a.UserId == userId).FirstOrDefault()?.Value;
        }
    }
}
