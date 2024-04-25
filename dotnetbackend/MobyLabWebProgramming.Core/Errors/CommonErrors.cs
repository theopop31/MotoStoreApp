using Microsoft.AspNetCore.StaticFiles;
using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
    public static ErrorMessage NotAClient => new(HttpStatusCode.Unauthorized, "Only clients can add items.", ErrorCodes.CannotAdd);
    public static ErrorMessage NoPermissions => new(HttpStatusCode.Unauthorized, "The user does not have necessary permissions!", ErrorCodes.NotEnoughPermissions);
    public static ErrorMessage OrderNotFound => new(HttpStatusCode.NotFound, "The order doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage BadOrderId => new(HttpStatusCode.Conflict, "The order id does not belong to requesting user.", ErrorCodes.CannotAdd);
    public static ErrorMessage ProductNotFound => new(HttpStatusCode.NotFound, "Product does not exist.", ErrorCodes.EntityNotFound);
    public static ErrorMessage NoStock => new(HttpStatusCode.Conflict, "Stock is not sufficient.", ErrorCodes.NotEnoughStock);
    public static ErrorMessage OrderDetailNotFound => new(HttpStatusCode.NotFound, "OrderDetail does not exist.", ErrorCodes.EntityNotFound);
    public static ErrorMessage ProducerNotFound => new(HttpStatusCode.NotFound, "The producer doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ProducerAlreadyExists => new(HttpStatusCode.Conflict, "The producer already exists!", ErrorCodes.ProducerAlreadyExists);
    public static ErrorMessage UserProfileNotFound => new(HttpStatusCode.NotFound, "UserProfile not found.", ErrorCodes.EntityNotFound);
}
