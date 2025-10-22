using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Enums
{
    public enum BlockType
    {
        [Display(Name = "Manşet")]
        Hero = 1,

        [Display(Name = "Metin")]
        Text = 2,

        [Display(Name = "Görsel Galeri")]
        ImageGallery = 4,

        [Display(Name = "Oda Olanakları")]
        AmenityGroup = 5
    }

    //public enum BlockType
    //{
    //    Hero = 1,
    //    Text = 2,
    //    FeatureGrid = 3,
    //    ImageGallery = 4,
    //    AmenityGroup = 5,
    //    Map = 6,
    //    CTA = 7,
    //    Related = 8,
    //    Testimonial = 9,
    //    Faq = 10,
    //    Divider = 11
    //}
}
