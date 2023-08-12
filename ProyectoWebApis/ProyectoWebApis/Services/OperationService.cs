using AutoMapper;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Interfaces;
using ProyectoWebApis.Models;
using ProyectoWebApis.Repositories;
using System.Collections.Immutable;

namespace ProyectoWebApis.Services
{
    public class OperationService
    {
        private readonly IMapper _mapper;
        private readonly IOperation _operationRepository;

        public OperationService(IMapper mapper, IOperation operationRepository)
        {
            _mapper = mapper;
            _operationRepository = operationRepository;
        }

        public (bool Succes, int StatusCode, List<string> ErrorMessage) CreateOperation(OperationCreateDTO createDTO)
        {

            var operation = _operationRepository.GetAll()
                .Where(x => x.OperationName.Trim().ToLower() ==  createDTO.OperationName.TrimEnd().ToLower())
                .FirstOrDefault();

            if(operation != null)
            {
                return (false, 422, new List<string> { "Operation already exist!" });
            }

            var operationMap = _mapper.Map<Operation>(createDTO);

            if(!_operationRepository.Create(operationMap))
            {   
                return (false, 500, new List<string> { "Something went wrong while saving" });
            }

            return (true, 200, null);
        }

        public List<OperationToShowDTO> GetAllOperations()
        {
            var result = _mapper.Map<List<OperationToShowDTO>>(_operationRepository.GetAll());
            return result;
        }

        public string GenerateCalculation(int opType, string numberOne, string numberTwo)
        {
            dynamic answer;

            double objOne = double.Parse(numberOne);

            double objTwo = double.Parse(numberTwo);

            double aux; 
            
            switch (opType)
            {
                case 1:
                    aux = objOne + objTwo;
                    answer =Convert.ToString(aux);
                    break;
                case 2:
                    aux = objOne - objTwo;
                    answer = Convert.ToString(aux);
                    break;
                case 3:
                    aux = objOne * objTwo;
                    answer = Convert.ToString(aux);
                    break;
                case 4:
                    aux = objOne / objTwo;
                    answer = Convert.ToString(aux);
                    break;
                case 5:
                    aux = Math.Pow(objOne, objTwo);
                    answer = Convert.ToString(aux);
                    break;
                case 6:
                    answer = "RANDOM STRING!";
                    break;
                default:
                    answer = "INSERT A VALID OPERATION ID!";
                    break;
            }

            return answer;
        }

    }
}
