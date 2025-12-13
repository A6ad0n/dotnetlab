namespace PizzaApp.BL.Common.Exceptions;

public class BusinessLogicException<TCode> : Exception where TCode : Enum
{
    public TCode? ResultCode { get; init; }

    public BusinessLogicException()
    {
    }

    public BusinessLogicException(string message) : base(message)
    {
    }

    public BusinessLogicException(TCode resultCode) : base(resultCode.ToString())
    {
        ResultCode = resultCode;
    }
    
    public BusinessLogicException(TCode resultCode, string message) : base(resultCode.ToString() + message)
    {
        ResultCode = resultCode;
    }
}