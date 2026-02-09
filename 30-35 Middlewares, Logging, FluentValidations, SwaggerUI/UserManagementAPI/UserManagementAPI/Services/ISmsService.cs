namespace UserManagementAPI.Services;

public interface ISmsService
{
    void SendOtp(string phoneNumber);
    bool VerifyOtp(string phoneNumber, string code);
}