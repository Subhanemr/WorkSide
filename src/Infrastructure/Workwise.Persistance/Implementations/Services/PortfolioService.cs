using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;
using Workwise.Persistance.Utilities;
using static System.Net.WebRequestMethods;

namespace Workwise.Persistance.Implementations.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _repository;
        public readonly ICLoudService _cloud;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;

        public PortfolioService(IPortfolioRepository repository, IMapper mapper, IHttpContextAccessor http, 
            ICLoudService cloud)
        {
            _repository = repository;
            _mapper = mapper;
            _http = http;
            _cloud = cloud;
        }

        public async Task<ResultDto> CreateAsync(PortfolioCreateDto dto)
        {
            if (await _repository.CheckUniqueAsync(x => x.Name == dto.Name))
                throw new AlreadyExistException($"{dto.Name} - This Portfolio is already exist!");

            Portfolio item = _mapper.Map<Portfolio>(dto);
            if (dto.File != null)
            {
                item.Url = await _cloud.FileCreateAsync(dto.File);
            }

            await _repository.AddAsync(item);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Portfolio is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Portfolio item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.SoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Portfolio has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Portfolio item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.ReverseSoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Portfolio has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Portfolio item = await _getByIdAsync(id);

            _repository.Delete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Portfolio has been permanently deleted");
        }

        public async Task<PaginationDto<PortfolioItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            string[] includes = { $"{nameof(Portfolio.AppUser)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Portfolio> items = new List<Portfolio>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<PortfolioItemDto> dtos = _mapper.Map<ICollection<PortfolioItemDto>>(items);

            return new ()
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<PortfolioGetDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            string[] includes = { $"{nameof(Portfolio.AppUser)}" };
            Portfolio item = await _getByIdAsync(id, false, includes);

            PortfolioGetDto dto = _mapper.Map<PortfolioGetDto>(item);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(PortfolioUpdateDto dto)
        {
            Portfolio existedItem = await _getByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower() == dto.Name.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Name}-This Portfolio is already exist!");

            existedItem = _mapper.Map(dto, existedItem);
            _repository.Update(existedItem);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Portfolio is successfully updated");
        }

        private async Task<Portfolio> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Portfolio item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new NotFoundException($"Portfolio not found({id})!");

            return item;
        }
    }
}