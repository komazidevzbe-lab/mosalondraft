using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // ===============================
        // RegisterUserDto → AppUser
        // Maps signup form data into the Identity user entity.
        // ===============================

        CreateMap<RegisterUserDto, AppUser>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Trim().ToLower()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLower()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.Trim().ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.Trim().ToUpper()))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
            .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.UtcNow)));

        // ===============================
        // AppUser → UserDto
        // Returns safe user information to the Angular client.
        // Token and roles are added manually where needed.
        // ===============================

        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber ?? string.Empty))
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        // ===============================
        // GalleryImage → GalleryImageDto
        // Returns database-driven gallery cards to Angular.
        // IsFavorite is set manually in GalleryService per logged-in user.
        // ===============================

        CreateMap<GalleryImage, GalleryImageDto>()
            .ForMember(dest => dest.IsFavorite, opt => opt.Ignore());
    }
}