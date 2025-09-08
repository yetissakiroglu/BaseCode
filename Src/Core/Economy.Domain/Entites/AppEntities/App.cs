using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("Apps")]
    public class App : BaseEntity<int>
    {
      
        [Required]
        [MaxLength(200)]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(200)]
        public string ServerName { get; set; }

        [Required]
        [MaxLength(100)]
        public string DatabaseName { get; set; }

        public string? UserName { get; set; }

        [Required]
        public bool IsPassword { get; set; }

        public string? Password { get; set; }

        [MaxLength(250)]
        public string Domain { get; set; }

        public AppAccessMode AccessMode { get; set; } = AppAccessMode.LocalOnly;
        public AppTheme Theme { get; set; } = AppTheme.Default;

        public string? ApiKey { get; set; }   // yeni alan


        [NotMapped]
        public string ConnectionString
        {
            get
            {
                if(IsPassword)
                {
                    return $"Data Source={ServerName};Initial Catalog={DatabaseName};Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False; MultipleActiveResultSets=True; Application Intent=ReadWrite;Multi Subnet Failover=False";
                }
                else
                {
                    return $"Data Source={ServerName};Initial Catalog={DatabaseName};Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False; MultipleActiveResultSets=True; Application Intent=ReadWrite;Multi Subnet Failover=False";
                }
            }
        }


    }

}
