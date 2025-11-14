namespace PizzaApp.DataAccess.Entities.Primitives;

public enum Status
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
