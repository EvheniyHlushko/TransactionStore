using AutoMapper;
using TransactionStore.Api.Models.OData;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Data.Entities;

namespace TransactionStore.Api.MapperConfiguration
{
    public class GeneralMapProfile : Profile
    {
        public GeneralMapProfile()
        {
            CreateMap<PaymentTransactionEntity, PaymentTransaction>()
                .ReverseMap();

            CreateMap<ODataQuery, ExecutableODataQuery>();
        }
    }
}