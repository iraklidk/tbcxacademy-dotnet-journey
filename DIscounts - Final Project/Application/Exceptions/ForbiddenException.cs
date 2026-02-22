namespace Discounts.Application.Exceptions;

public class ForbiddenException(string msg) : DomainException(msg);

