using System;
using EasyNetQ;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using GOC.Inventory.Domain.Enums;
using GOC.Inventory.Domain.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GOC.Inventory.API.EventBus
{
    public class MessageRouter : IMessageRouter
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ILogger _logger;

        public  MessageRouter(ICompanyRepository companyRepository, IVendorRepository vendorRepository, ILoggerFactory loggerFactory)
        {
            _companyRepository = companyRepository;
            _vendorRepository = vendorRepository;
            _logger = loggerFactory.CreateLogger<MessageRouter>();
        }

        public void Route(string message)
        {
            try
            {
                if (message.Contains(EventTypes.CompanyCreated.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage  = JsonConvert.DeserializeObject<CompanyCreated>(message);
                    if(parsedMessage.CompanyCreatedObj == null)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        _companyRepository.CreateCompany(parsedMessage.CompanyCreatedObj);
                    }
                }
                else if (message.Contains(EventTypes.CompanyEdited.ToString()))
                {
                    var parsedMessage = JsonConvert.DeserializeObject<CompanyEdited>(message);
                    _companyRepository.EditCompany(parsedMessage.CompanyEditedObj);
                }
                else if (message.Contains(EventTypes.CompanyDeleted.ToString()))
                {
                    var parsedMessage = JsonConvert.DeserializeObject<CompanyDeleted>(message);
                    _companyRepository.DeleteCompany(parsedMessage.CompanyDeletedId);
                }

            }
            catch (Exception ex)
            {
                throw new EasyNetQException("unable to parse", ex);
            }
        }
    }
}
