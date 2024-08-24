using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _repository;
        private readonly IMapper _mapper;

        public SettingsService(ISettingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginationDto<SettingsItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Settings> items = new List<Settings>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                        x => x.Key, false, isDeleted, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                        x => x.Key, true, isDeleted, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false).ToListAsync();
                    break;
            }

            ICollection<SettingsItemDto> dtos = _mapper.Map<ICollection<SettingsItemDto>>(items);

            return new()
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<SettingsGetDto> GetByIdAsync(string id)
        {
            Settings item = await _getByIdAsync(id, false);

            SettingsGetDto dto = _mapper.Map<SettingsGetDto>(item);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(SettingUpdateDto dto)
        {
            Settings existedItem = await _getByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Key.ToLower() == dto.Key.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Key}-This Setting is already exist!");

            existedItem = _mapper.Map(dto, existedItem);
            _repository.Update(existedItem);
            await _repository.SaveChangeAsync();

            return new($"{dto.Key} - Setting is successfully updated");
        }

        private async Task<Settings> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Settings item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new NotFoundException($"Setting not found({id})!");

            return item;
        }
    }
}
