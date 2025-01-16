using Economy.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAppMenuQuery : IRequest<AppMenuDto>
    {
        public int Id { get; set; }

        public GetAppMenuQuery(int id)
        {
            Id = id;
        }
    }
}
