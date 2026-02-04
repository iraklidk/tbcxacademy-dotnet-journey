namespace LibraryManagement.Application.Exceptions;

public class ValidationException(string msg) : DomainException(msg);