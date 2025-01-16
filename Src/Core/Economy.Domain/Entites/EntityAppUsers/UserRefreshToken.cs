using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.EntityAppUsers
{
	public class UserRefreshToken : BaseEntity<string>
	{
		public string UserId { get; set; } = default!;
		public string Token { get; set; } = default!;
		public DateTime Expiration { get; set; }
	}
}
