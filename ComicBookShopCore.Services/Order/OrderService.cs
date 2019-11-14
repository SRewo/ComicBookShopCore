using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

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

        public Task AddOrderAsync(OrderInputDto order)
        {
            var ord = _mapper.Map<Data.Order>(order);
            ord.Validate();
            if (ord.HasErrors)
                throw new ValidationException(ord.GetFirstError());
            Parallel.ForEach(ord.OrderItems, x => x.Validate());
            if (ord.OrderItems.Any(x => x.HasErrors))
            {
                var item = ord.OrderItems.Single(x => x.HasErrors);
                throw new ValidationException(item.GetFirstError());
            }

            return _repository.AddAsync(ord);
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