﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos
{
    public class AppMenuDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public bool IsExternal { get; set; }
        public int? ParentMenuId { get; set; }
        public List<AppMenuDto> SubMenus { get; set; } = new List<AppMenuDto>();
    }
}
