using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProyectoWebApis.DataBase;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Helpers;
using ProyectoWebApis.Interfaces;
using ProyectoWebApis.Models;
using ProyectoWebApis.Repositories;


namespace ProyectoWebApis.Services
{
    public class RecordService
    {
        private readonly IRecord _recordRepository;
        private readonly UserManager<User> _userManager;
        private readonly IOperation _operationService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RecordService(IRecord record, IMapper mapper, UserManager<User> userManager, IOperation operationService, ApplicationDbContext dbContext)
        {
            _recordRepository = record;
            _mapper = mapper;
            _userManager = userManager;
            _operationService = operationService;
            _dbContext = dbContext;
        }

        public async Task<(bool Succes, int StatusCode, List<string> ErrorMessage)> CreateRecordAsync(RecordCreateDTO createDTO, User actual)
        {
            var operation = _operationService.GetById(createDTO.Operation_Id);

            var response = "";

            var recordMap = _mapper.Map<Record>(createDTO);

            recordMap.User_Id = actual.Id;
            recordMap.User_Balance = actual.Balance;
            recordMap.Ammount = operation.Cost;
            recordMap.DateTime = DateTime.Now;
            recordMap.State = true;

            if (operation != null && actual.Balance >= operation.Cost)
            {
                recordMap.User_Balance = actual.Balance;

                if (_recordRepository.Create(recordMap))
                {
                    response = "Record created successfully";
                    actual.Balance -= operation.Cost;
                    await _userManager.UpdateAsync(actual);
                    SetOperationResponse(recordMap, response);
                    return (true, 200, new List<string> { response });
                }
                else
                {
                    response = "Something went wrong while saving";
                    recordMap.OperationResponse = response;
                    _recordRepository.Create(recordMap);
                    return (false, 500, new List<string> { response });
                }
            }
            response = "Insuficient balance to complete te operation";
            recordMap.OperationResponse = response;
            _recordRepository.Create(recordMap);
            return (false, 420, new List<string> { response });
        }

        public List<Record> GetAllOperations()
        {
            var result = _mapper.Map<List<Record>>(_recordRepository.GetAll());
            return result;
        }

        public void SetOperationResponse(Record record, string response)
        {
            record.OperationResponse = response;
            _recordRepository.Update(record);
            _recordRepository.Save();
        } 


        public (bool Succes, int StatusCode, List<string> ErrorMessage) Delete(int id)
        {   
            if(_recordRepository.Exist(id))
            {
                if(_recordRepository.SoftDeleted(id))
                {
                    return (true, 204, new List<string> { "Record Removed succesfully" });
                }
                return (false, 500, new List<string> { "Record couldn't be removed" });
            }
            return (false, 404, new List<string> { "Record desn't exist!" });
        }

        public Record GetRecordById(int id)
        {
            return _recordRepository.GetById(id);
        }

        public IQueryable<Record> GetQueryable()
        {
            return _dbContext.Records.AsQueryable();
        }

        public async Task<ActionResult<List<RecordToShowDTO>>> GetAll(PaginationDTO paginationDTO)
        {
            var products = await _dbContext.Records.OrderBy(record => record.Id).ToPaginate(paginationDTO).ToListAsync();

            return _mapper.Map<List<RecordToShowDTO>>(products);
        }

    }
}
