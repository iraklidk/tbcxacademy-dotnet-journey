namespace LibraryManagement.Application.Exceptions;

public class NotFoundException(string msg) : DomainException(msg);
    
