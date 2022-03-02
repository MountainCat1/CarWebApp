using AutoMapper;
using CarWebApp.Entities;
using CarWebApp.Models;

namespace CarWebApp.Data
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginModel, User>();
        }
    }
}