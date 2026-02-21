namespace Discounts.Application.Exceptions;

public class UnauthorizedException(string msg) : DomainException(msg);

