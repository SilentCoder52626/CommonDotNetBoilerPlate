using DomainModule.Dto.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface.Email
{
	public interface IEmailSenderService
	{
		void SendEmail(MessageDto message);
		Task SendEmailAsync(MessageDto message);

	}
}
