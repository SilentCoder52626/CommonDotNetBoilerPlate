﻿using DomainModule.Dto.User;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModule.Service
{
    public class AppSettingsService : AppSettingsServiceInterface
    {
        private readonly AppSettingsRepositoryInterface _settingRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AppSettingsService(AppSettingsRepositoryInterface settingRepo, IUnitOfWork unitOfWork)
        {
            _settingRepo = settingRepo;
            _unitOfWork = unitOfWork;
        }

        public void UpdateSetting(AppSettingDto dto)
        {
            try
            {
                using (var tx = _unitOfWork.BeginTransaction())
                {
                    var Entity = _settingRepo.GetByKey(dto.Key, dto.UserId);
                    if(Entity == null)
                    {
                        Entity.Key = dto.Key;
                        Entity.Value = dto.Value;
                        Entity.UserId = dto.UserId;
                        _settingRepo.Insert(Entity);
                    }
                    else
                    {
                        Entity.Value = dto.Value;
                        Entity.UserId = dto.UserId;
                        _settingRepo.Update(Entity);
                    }
                    
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
