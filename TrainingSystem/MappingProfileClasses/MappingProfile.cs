using AutoMapper;
using Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO;
using Entities.Data_Transfer_Object__DTO_.CompanyDTO;
using Entities.Data_Transfer_Object__DTO_.EmployeeDTO;
using Entities.Models;

namespace TrainingSystem.MappingProfileClasses
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //1-
            //CreateMap => create a mapping configuration from Tsource type to TDestenation type =>
            //CreateMap<Tsource, TDestenation>()
            //IMapper.Map(Tsource, TDestenation);
            //IMapper.Map<TDestenation>(Tsource);


            CreateMap<Company, GETCompanyDTO>()//here in get we want return the dto however in post in our db we want return and use our real class 
                                               //CreateMap work fine to map simliar names of properties, but if you want customize it, use forMember() 

                // Customize configuration for individual member(DTO Property). Used when the name isn't known at compile-time
                .ForMember(
                GETCompanyDTO => GETCompanyDTO.FullAdress,//individual member DTO Property(FullAdress)
                memberOptions => memberOptions.MapFrom(Company => string.Join(' ', Company.Adress, ' ', Company.Country)));



            //2-
            CreateMap<Employee, GETEmployeeDTO>();//simple mapping :)  //here in get we want return the dto however in post in our db we want return and use our real class
            //3-
            CreateMap<POSTCompanyDTO, Company>();//simple mapping :)
            //4-
            CreateMap<POSTEmployeeDTO, Employee>();//simple mapping :)
            //-5-
            CreateMap<UPDATEEmployeeDTO, Employee>().ReverseMap();//ReverseMap mean do the same map if the source is Employee and UPDATEEmployeeDTO is destenation
                                                                  // CreateMap<UPDATEEmployeeDTO, Employee>().ReverseMap() == CreateMap<UPDATEEmployeeDTO, Employee>() + CreateMap<Employee, UPDATEEmployeeDTO>()

            //6-
            CreateMap<UPDATECompanyDTO, Company>();//simple mapping :)

            //7-
            CreateMap<RegisterationUserDTO, ApplicationUser>();//simple mapping :)

        }
    }
}
