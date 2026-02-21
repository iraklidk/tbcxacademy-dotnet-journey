namespace Discounts.Application.Exceptions;

public class ServiceException(string msg, Exception ex) : DomainException(msg);
