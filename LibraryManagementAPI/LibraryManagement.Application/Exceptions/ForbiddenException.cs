namespace LibraryManagement.Application.Exceptions;

public class ForbiddenException(string msg) : DomainException(msg);

