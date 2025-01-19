﻿using Economy.Application.Dtos;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public record GetAppMenuQuery(int Id) : IRequest<AppMenuDto>;

}
