namespace LibraryManagement.Application.Exceptions;

public class UnauthorizedException(string msg) : DomainException(msg);
    
