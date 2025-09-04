using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppDtos
{
    public class AppCreateEditDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(200)]
        public string ServerName { get; set; }

        [Required]
        [MaxLength(100)]
        public string DatabaseName { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        public bool IsPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(250)]
        public string Domain { get; set; }

        public AppAccessMode AccessMode { get; set; }
        public AppTheme Theme { get; set; }
        public string? ApiKey { get; set; }   // yeni alan

    }
}
