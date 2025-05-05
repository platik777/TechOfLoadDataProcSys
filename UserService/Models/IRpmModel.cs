namespace UserService.Models;

public interface IRpmModel {
    long UserId { get; set; } 
    string Endpoint { get; set; } 
    int Rpm { get; set; } 
};