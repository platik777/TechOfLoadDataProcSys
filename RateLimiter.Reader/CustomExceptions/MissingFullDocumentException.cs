namespace RateLimiter.Reader.CustomExceptions;

public class MissingFullDocumentException(string message) : Exception(message);