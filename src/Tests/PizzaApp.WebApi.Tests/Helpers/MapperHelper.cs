using AutoMapper;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Mappings;
using PizzaApp.WebApi.Mappings;

namespace PizzaApp.WebApi.Tests.Helpers;

public static class MapperHelper
{
    private static IMapper? _mapper;

    public static IMapper Mapper
    {
        get
        {
            if (_mapper == null)
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var config = new MapperConfiguration(config =>
                {
                    config.AddProfile<DiscountBLProfile>();
                    config.AddProfile<DiscountServiceProfile>();
                }, loggerFactory);
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }
    }
}