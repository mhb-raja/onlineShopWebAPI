using Core.DTOs.TableDatasource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Slider
{
    public class SliderDatasource : GenericTableDatasource<SliderDTO>
    {
        public string Text { get; set; }
        public DateTime? ActiveFromTime { get; set; }
        public string Sort { get; set; }
    }
}
