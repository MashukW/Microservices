using AutoMapper;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Exceptions;

namespace Mango.Services.OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public OrderService(IRepository<Order> orderRepository, IWorkUnit workUnit, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<OrderOutgoing> Get(Guid publicOrderId)
        {
            var order = await _orderRepository.Get(x => x.PublicId == publicOrderId);
            if (order == null)
                throw new NotFoundException();

            return _mapper.Map<OrderOutgoing>(order);
        }

        public async Task ChangePaymentStatus(Guid publicOrderId, bool status)
        {
            var order = await _orderRepository.Query().FirstOrDefaultAsync(x => x.PublicId == publicOrderId);
            if (order == null)
                return;

            order.PaymentStatus = status;
            await _workUnit.SaveChanges();
        }
    }
}
