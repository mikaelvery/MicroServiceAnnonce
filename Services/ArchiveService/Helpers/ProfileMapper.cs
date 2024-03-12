using AutoMapper;
using Events;
using ArchiveService.Models;

namespace ArchiveService;
public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<AdCreated, Ad>();
        CreateMap<AdUpdated, Ad>();
        CreateMap<AdDeleted, Ad>();
    }
}