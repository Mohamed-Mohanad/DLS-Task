using DLS.Application.Features.Cart.Queries.GetCart;
using Mapster;

namespace DLS.Application.Features.Cart.MappingConfig;

internal class CartMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.Cart, CartResponse>()
            .Map(dest => dest.ItemCount, src => src.Items.Count)
            .Map(dest => dest.TotalPrice, src => src.Items.Sum(i => i.TotalPrice));
    }
}
