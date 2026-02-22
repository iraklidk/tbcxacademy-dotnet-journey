using Discounts.Application.Exceptions;

namespace Application.Exceptions.User;

public class UserNotFound(string msg) : NotFoundException(msg);
