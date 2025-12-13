using AutoMapper;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Mappings;

namespace PizzaApp.BL.UnitTests.Helpers;

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
                }, loggerFactory);
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }
    }
}