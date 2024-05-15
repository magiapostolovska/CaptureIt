using AutoMapper;
using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.DTOs.PasswordRecoveryRequest;
using CaptureIt.Models;

namespace CaptureIt.AutoMapper
{
    public class PasswordRecoveryMapper : Profile
    {
        public PasswordRecoveryMapper() 
        {
            CreateMap<PasswordRecoveryResponse, PasswordRecovery>();
            CreateMap<PasswordRecoveryRequest, PasswordRecovery>();
            CreateMap<PasswordRecovery, PasswordRecoveryResponse>();
        }
    }
}
