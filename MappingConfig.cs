
using AutoMapper;
using PruebaApi.Models.Dto;
using PruebaApi.Models;

public class  MappingConfig:Profile 
{
    public MappingConfig()
    {
        CreateMap<Villa,VillaDto>().ReverseMap();
        
        CreateMap<Villa,VillaDtoUpdate>().ReverseMap();
        CreateMap<Villa,VillaDtoCreate>().ReverseMap();
        
    }
}