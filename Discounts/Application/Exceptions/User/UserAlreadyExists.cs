using Discounts.Application.Exceptions;

namespace Application.Exceptions.User;

public class UserAlreadyExists(string msg) : DomainException(msg);
    
