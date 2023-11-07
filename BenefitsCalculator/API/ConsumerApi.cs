using AutoMapper;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace BenefitsCalculator.API
{
    [Route("api/consumers")]
    [ApiController]
    public class ConsumerApi : ControllerBase
    {
        private readonly IConsumerRepository _consumerRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<ConsumerApi> _logger;
        private readonly IMapper _mapper;

        public ConsumerApi(IConsumerRepository consumerRepository, ICommonRepository commonRepository,
            ILogger<ConsumerApi> logger, IMapper mapper)
        {
            _consumerRepository = consumerRepository;
            _commonRepository = commonRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // api/consumers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsumerDTO>>> Get()
        {
            try
            {
                var consumers = await _consumerRepository.GetAllConsumers();

                return Ok(_mapper.Map<List<ConsumerDTO>>(consumers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Get() - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/consumers/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsumerDTO>> Get(int id)
        {
            try
            {
                // get consumer data by id
                var consumer = await _consumerRepository.GetConsumerById(id);

                // if consumer data is not null,
                // return ok, else return not found
                if (consumer != null)
                {
                    return Ok(_mapper.Map<ConsumerDTO>(consumer));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Get(int id) - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/consumers/id/consumer-setup
        [HttpGet("{id}/consumer-setup")]
        public async Task<ActionResult<ConsumerSetupDTO>> GetIncludingSetup(int id)
        {
            try
            {
                // get consumer and setup data
                var result = await _consumerRepository.GetConsumersIncludingSetupById(id);

                // assign result to a dto
                var consumerSetupModel = new ConsumerSetupDTO
                {
                    Setup = _mapper.Map<SetupDTO>(result.Setup),
                    Consumer = _mapper.Map<ConsumerDTO>(result)
                };

                return Ok(consumerSetupModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer GetIncludingSetup(int id) - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/consumers
        [HttpPost]
        public async Task<ActionResult> Post(ConsumerDTO consumer)
        {
            try
            {
                // convert ConsumerDTO to Consumer
                var newConsumer = _mapper.Map<Consumer>(consumer);

                // add consumer and save all changes
                _commonRepository.AddEntity(newConsumer);

                if (await _commonRepository.SaveAll())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Post(ConsumerDTO consumer) - Error while processing the request.");
            }

            return BadRequest();

        }

        // api/consumers
        [HttpPut]
        public async Task<ActionResult> Put(ConsumerDTO consumer)
        {
            try
            {
                // convert ConsumerDTO to Consumer
                var modConsumer = _mapper.Map<Consumer>(consumer);

                // update entity if consumer exists
                if (await _consumerRepository.ConsumerExists(modConsumer.Id))
                {
                    _commonRepository.UpdateEntity(modConsumer);

                    if (await _commonRepository.SaveAll())
                    {
                        return Ok();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Put(ConsumerDTO consumer) - Error while processing the request.");
            }

            return BadRequest();

        }

        // api/consumers/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // get consumer data for delete
                var consumer = await _consumerRepository.GetConsumerById(id);

                // check if consumer is not null
                if (consumer != null)
                {
                    // delete consumer
                    _consumerRepository.DeleteConsumer(consumer);

                    if (await _commonRepository.SaveAll())
                    {
                        return Ok();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Delete(int id) - Error while processing the request.");
            }

            return BadRequest();
        }

    }
}
