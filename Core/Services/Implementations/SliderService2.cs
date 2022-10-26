using Core.DTOs.Slider;
using Core.Services.Interfaces;
using Core.Utilities.Common;
using Core.Utilities.Extensions;
using DataLayer.Entities.Site;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class SliderService2 : ISliderService2
    {
        #region constructor

        private readonly IGenericRepository<Slider> sliderRepository;
        public SliderService2(IGenericRepository<Slider> sliderRepository)
        {
            this.sliderRepository = sliderRepository;
        }

        #endregion
        public async Task<SliderDTO> AddSlider(SliderDTO slider)
        {
            Slider newSlider = new Slider
            {
                Title = slider.Title,
                Description = slider.Description,
                Link = slider.Link,
                ActiveFrom = slider.ActiveFrom,
                ActiveUntil = slider.ActiveUntil,
                //Address = register.Address.SanitizeText(),                
            };
            if (!string.IsNullOrEmpty(slider.Base64Image))
            {
                var imageFile = ImageUploaderExtension.Base64ToImage(slider.Base64Image);
                var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                imageFile.AddImageToServer(imageName, PathTools.SliderImageServerPath);
                newSlider.ImageName = imageName;
            }
            await sliderRepository.AddEntity(newSlider);
            await sliderRepository.SaveChanges();

            slider.Id = newSlider.Id;

            return slider;
        }

        public void Dispose()
        {
            sliderRepository?.Dispose();
        }

        public async Task<SliderDTO> EditSlider(SliderDTO slider)
        {
            var mainSlider = await sliderRepository.GetEntityById(slider.Id);
            if (mainSlider != null)
            {
                mainSlider.Title = slider.Title;
                mainSlider.Link = slider.Link;
                mainSlider.ActiveFrom = slider.ActiveFrom;
                mainSlider.ActiveUntil = slider.ActiveUntil;
                mainSlider.Description = slider.Description;

                if (!string.IsNullOrEmpty(slider.Base64Image))
                {
                    var imageFile = ImageUploaderExtension.Base64ToImage(slider.Base64Image);
                    var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                    imageFile.AddImageToServer(imageName, PathTools.SliderImageServerPath, mainSlider.ImageName);
                    mainSlider.ImageName = imageName;
                }

                sliderRepository.UpdateEntity(mainSlider);
                await sliderRepository.SaveChanges();

                slider.Base64Image = mainSlider.ImageName;
                return slider;
            }
            return null;
        }

        public async Task<SliderDatasource> FilterSliders(SliderDatasource filter)
        {
            var slidersQuery = sliderRepository.GetEntitiesQuery().AsQueryable();
            switch (filter.Sort)
            {
                case "asc":
                    slidersQuery = slidersQuery.OrderBy(s => s.ActiveFrom);
                    break;

                case "desc":
                    slidersQuery = slidersQuery.OrderByDescending(s => s.ActiveFrom);
                    break;
                default: break;
            }
            if (!string.IsNullOrEmpty(filter.Text))
                slidersQuery = slidersQuery.Where(s => s.Title.Contains(filter.Text) || s.Description.Contains(filter.Text));

            if (filter.ActiveFromTime != null)
                slidersQuery.Where(s => s.ActiveFrom >= filter.ActiveFromTime);

            filter.TotalItems = slidersQuery.Count();
            //int count = (int)Math.Ceiling(slidersQuery.Count() / (double)filter.PageSize);

            //var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);

            var sliders = await slidersQuery.Paging(filter.PageIndex, filter.PageSize).
                Select(s => new SliderDTO
                {
                    ActiveFrom = s.ActiveFrom,
                    ActiveUntil = s.ActiveUntil,
                    Base64Image = s.ImageName,
                    Description = s.Description,
                    Id = s.Id,
                    Link = s.Link,
                    Title = s.Title
                }).ToListAsync();

            filter.SetItems(sliders);
            return filter;
        }

        public async Task<List<SliderMiniDTO>> GetActiveSliders()
        {
            return await sliderRepository.GetEntitiesQuery()
                .Where(s => !s.IsDelete && s.ActiveFrom <= DateTime.Now &&
                (s.ActiveUntil == null || s.ActiveUntil > DateTime.Now))
                .Select(s => new SliderMiniDTO
                {
                    Id = s.Id,
                    Base64Image = s.ImageName,
                    Description = s.Description,
                    Link = s.Link,
                    Title = s.Title
                })
                .ToListAsync();
        }

        public async Task<List<Slider>> GetAllSliders()
        {
            return await sliderRepository.GetEntitiesQuery().ToListAsync();
        }

        public async Task<Slider> GetSliderById(long sliderId)
        {
            return await sliderRepository.GetEntityById(sliderId);
        }

        public async Task<SliderDTO> GetSliderForEdit(long sliderId)
        {
            var slider = await sliderRepository.GetEntityById(sliderId);
            if (slider == null)
                return null;
            return new SliderDTO
            {
                Id = slider.Id,
                ActiveFrom = slider.ActiveFrom,
                ActiveUntil = slider.ActiveUntil,
                Base64Image = slider.ImageName,
                Description = slider.Description,
                Link = slider.Link,
                Title = slider.Title
            };
        }
    }
}
