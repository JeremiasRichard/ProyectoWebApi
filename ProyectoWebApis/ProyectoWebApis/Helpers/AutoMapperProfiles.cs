using AutoMapper;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Models;

namespace ProyectoWebApis.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Operation,OperationCreateDTO>().ReverseMap();
            CreateMap<Operation,OperationToShowDTO>().ReverseMap(); 
            CreateMap<Record,RecordCreateDTO>().ReverseMap();
            CreateMap<Record,RecordToShowDTO>().ReverseMap();
        }
    }
}
