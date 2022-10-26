using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Slider
{
    public class SliderMiniDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Base64Image { get; set; }
    }
    public class SliderDTO : SliderMiniDTO
    {
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveUntil { get; set; }

    }
}
