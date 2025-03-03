﻿using Economy.Application.Dtos.AppMenuDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public record GetAllAppMenuByParentMenuIdQuery(int? ParentMenuId) : IRequest<ResponseModel<List<AppMenuDto>>>;
}
