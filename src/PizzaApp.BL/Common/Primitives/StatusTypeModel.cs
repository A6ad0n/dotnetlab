namespace PizzaApp.BL.Common.Primitives;

public enum StatusTypeModel
{
    OrderCreated,
    OrderConfirmed,
    OrderPreparing,
    OrderReady,
    OrderDelivering,
    OrderCompleted,
    OrderCancelled,
    
    MenuActive,
    MenuInactive,
    MenuArchived,
    
    DiscountActive,
    DiscountExpired,
    DiscountScheduled
}