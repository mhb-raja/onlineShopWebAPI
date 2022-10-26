using Core.DTOs.Slider;
using DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ISliderService2: IDisposable
    {
        Task<List<Slider>> GetAllSliders();
        Task<List<SliderMiniDTO>> GetActiveSliders();
        Task<SliderDTO> AddSlider(SliderDTO slider);
        Task<Slider> GetSliderById(long sliderId);
        Task<SliderDTO> GetSliderForEdit(long sliderId);

        Task<SliderDTO> EditSlider(SliderDTO slider);
        Task<SliderDatasource> FilterSliders(SliderDatasource filter);
    }
}
