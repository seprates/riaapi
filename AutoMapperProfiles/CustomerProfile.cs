using AutoMapper;
using RIA.API.Dtos;
using RIA.API.Models;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        // source -> target
        CreateMap<Customer, CustomerReadDto>();
        CreateMap<CustomerCreateDto, Customer>();
    }
}