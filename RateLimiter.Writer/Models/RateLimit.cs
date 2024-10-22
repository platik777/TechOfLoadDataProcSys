namespace RateLimiter.Writer.Models;

public record RateLimit(string Route, int RequestsPerMinute);