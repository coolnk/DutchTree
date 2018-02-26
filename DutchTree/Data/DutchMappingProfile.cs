using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTree.Data.Entities;
using DutchTree.ViewModel;

namespace DutchTree.Data
{
    public class DutchMappingProfile: Profile
    {

        public DutchMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember( ovm=> ovm.OrderId, ex => ex.MapFrom(o => o.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
;        }
    }
}
