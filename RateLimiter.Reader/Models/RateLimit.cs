namespace RateLimiter.Reader.Models;

public record RateLimit(string Route, int RequestsPerMinute);