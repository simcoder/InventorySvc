using System;
using System.Threading.Tasks;
using EasyNetQ;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using GOC.Inventory.Domain.Enums;
using GOC.Inventory.Domain.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public async Task RouteAsync(string message)
        {
            try
            {
                if(message.Contains(EventTypes.CompanyCreated.ToString()))
                {
                    Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();
                    var f = new GuidJsonConverter();

                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage  = JsonConvert.DeserializeObject<CompanyCreated>(message);

                    if(parsedMessage.CompanyCreatedObj == null)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue or 
                    }
                    else
                    {
                        await _companyRepository.CreateCompanyAsync(parsedMessage.CompanyCreatedObj);
                    }
                }
                else if(message.Contains(EventTypes.CompanyEdited.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage = JsonConvert.DeserializeObject<CompanyEdited>(message);

                    if (parsedMessage.CompanyEditedObj == null)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        await _companyRepository.EditCompanyAsync(parsedMessage.CompanyEditedObj, parsedMessage.UserId);
                    }
                }
                else if(message.Contains(EventTypes.CompanyDeleted.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage = JsonConvert.DeserializeObject<CompanyDeleted>(message);

                    if (parsedMessage.CompanyDeletedId == Guid.Empty)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        await _companyRepository.DeleteCompanyAsync(parsedMessage.CompanyDeletedId);
                    }
                }
                else if (message.Contains(EventTypes.VendorCreated.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage = JsonConvert.DeserializeObject<VendorCreated>(message);

                    if (parsedMessage.VendorCreatedObj == null)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        await _vendorRepository.CreateVendorAsync(parsedMessage.VendorCreatedObj);
                    }
                }
                else if (message.Contains(EventTypes.VendorEdited.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage = JsonConvert.DeserializeObject<VendorEdited>(message);

                    if (parsedMessage.VendorEditedObj == null)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        await _vendorRepository.EditVendorAsync(parsedMessage.VendorEditedObj.Id, parsedMessage.VendorEditedObj, parsedMessage.UserId);
                    }
                }
                else if (message.Contains(EventTypes.VendorDeleted.ToString()))
                {
                    _logger.LogDebug($"routing message {message}");
                    var parsedMessage = JsonConvert.DeserializeObject<VendorDeleted>(message);

                    if (parsedMessage.VendorDeletedId == Guid.Empty)
                    {
                        _logger.LogDebug("deserialization failure message being swallowed");
                        //TODO requeue
                    }
                    else
                    {
                        await _vendorRepository.DeleteVendorAsync(parsedMessage.VendorDeletedId);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new EasyNetQException("unable to parse", ex);
            }
        }
    }
    /// <summary>
    ///     JSON converter for <see cref="Guid"/>.
    /// </summary>
    public class GuidJsonConverter : JsonConverter
    {
        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static GuidJsonConverter Instance { get; } = new GuidJsonConverter();

        /// <summary>
        ///     Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter"/> can read JSON.
        /// </summary>
        /// <value><see langword="true"/> if this <see cref="T:Newtonsoft.Json.JsonConverter"/> can read JSON; otherwise, <see langword="false"/>.
        /// </value>
        public override bool CanRead => true;

        /// <summary>
        ///     Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter"/> can write JSON.
        /// </summary>
        /// <value><see langword="true"/> if this <see cref="T:Newtonsoft.Json.JsonConverter"/> can write JSON; otherwise, <see langword="false"/>.
        /// </value>
        public override bool CanWrite => true;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">
        /// Kind of the object.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this instance can convert the specified object type; otherwise,
        /// .
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Guid)) || objectType.IsAssignableFrom(typeof(Guid?));
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="serializer">
        /// The calling serializer.
        /// </param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue(default(string));
            }
            else if (value is Guid)
            {
                var guid = (Guid)value;
                writer.WriteValue(guid.ToString("N"));
            }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.
        /// </param>
        /// <param name="objectType">
        /// Kind of the object.
        /// </param>
        /// <param name="existingValue">
        /// The existing value of object being read.
        /// </param>
        /// <param name="serializer">
        /// The calling serializer.
        /// </param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            var str = reader.Value as string;
            return str != null ? Guid.Parse(str) : default(Guid);
        }
    }

}
