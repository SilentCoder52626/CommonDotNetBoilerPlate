using DomainModule.Dto;
using DomainModule.Dto.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DomainModule.ServiceInterface
{
    public interface UserServiceInterface
    {
        Task<UserResponseDto> Create(UserDto dto);
        Task Edit(UserEditDto dto);
        Task Activate(string id);
        Task Deactivate(string id);
    }
}
