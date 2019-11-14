using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

//TODO: Order service tests
namespace ComicBookShopCore.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepositoryAsync _repository;

        public OrderService(IOrderRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderBasicDto>> OrderListAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.ProjectTo<OrderBasicDto>(list);
        }

        public async Task<OrderDetailsDto> OrderDetailsAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrderDetailsDto>(order);
        }

        public async Task<Dictionary<string, string>> AddOrderAsync(OrderInputDto order)
        {
            var ord = _mapper.Map<Data.Order>(order);
            ord.Validate();
            var result = new Dictionary<string, string>();

            if (ord.HasErrors)
            {
                var errors = ord.GetErrors();

                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Value) result.Add(error.Key, errorMessage);
                }

                return result;
            }

            Parallel.ForEach(ord.OrderItems, x => x.Validate());

            if (ord.OrderItems.Any(x => x.HasErrors))
            {
                var items = ord.OrderItems.Where(x => x.HasErrors);

                foreach (var item in items)
                {
                    foreach (var error in item.GetErrors())
                    {
                        foreach (var errorMessage in error.Value)
                        {
                           result.Add($"{item.ComicBookId}: {error.Key}", errorMessage); 
                        } 
                    } 
                }

            }

            await _repository.AddAsync(ord);
            return result;
        }

        public async Task RemoveOrderAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                throw new NullReferenceException();
            await _repository.RemoveAsync(order);
        }
    }
}