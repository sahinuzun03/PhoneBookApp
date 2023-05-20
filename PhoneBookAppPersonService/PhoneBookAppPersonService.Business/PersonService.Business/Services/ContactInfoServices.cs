using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonService.Business.Models;
using PersonService.DataAccess.Repo;
using PersonService.Entities;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PersonService.Business.Services
{
    public class ContactInfoServices : IContactInfoServices
    {
        private readonly IPersonRepo _personRepo;
        private readonly IMapper _mapper;
        private readonly IContactInfoRepo _contactInfoRepo;
        private IConfiguration _configuration;
        public ContactInfoServices(IMapper mapper, IPersonRepo personRepo, IContactInfoRepo contactInfoRepo, IConfiguration configuration)
        {

            _mapper = mapper;
            _personRepo = personRepo;
            _contactInfoRepo = contactInfoRepo;
            _configuration = configuration;

        }
        public async Task CreateContactInfo(Guid personId, ContactInfoModel contactInfoModel)
        {
            var person = _personRepo.GetById(personId);
            var addContact = _mapper.Map<ContactInfo>(contactInfoModel);
            addContact.Person = person;
            await _contactInfoRepo.InsertAsync(addContact);
            _contactInfoRepo.SaveChanges();
        }

        public async Task DeleteContactInfo(Guid contactID)
        {
            _contactInfoRepo.DeleteById(contactID);
            await _contactInfoRepo.SaveChangesAsync();

        }

        public async Task<List<ContactInfoViewModel>> GetContactInfo(Guid personId)
        {
            var getPersonContacts = await _contactInfoRepo.GetDataWithLinqExp(x => x.DeletedAt == null && x.PersonId == personId).Select(x => new ContactInfoViewModel
            {
                ContactContent = x.ContactContent,
                EmailAddress = x.EmailAddress,
                Id = x.Id,
                Location = x.Location,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();

            return getPersonContacts;
        }

        public void ListenToQueue()
        {
            string requestQueueName = _configuration["RabbitQM:RequestQueueName"];
            string Uri = _configuration["RabbitQM:Uri"];

            // Request kuyruğunun oluşturulması ve mesaj gönderilmesi 
            ConnectionFactory factory = new();
            factory.Uri = new(Uri);

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();


            channel.QueueDeclare(
                queue: requestQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false);

            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue: requestQueueName,
                autoAck: true,
                consumer: consumer);


            consumer.Received += async (sender, e) =>
            {
                var reportDetails = GiveReportDetail();
                var responseMessageString = JsonConvert.SerializeObject(reportDetails);
                byte[] responseMessage = Encoding.UTF8.GetBytes(responseMessageString);

                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = e.BasicProperties.CorrelationId;
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: e.BasicProperties.ReplyTo,
                    basicProperties: properties,
                    body: responseMessage);

                channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            };
        }


        public List<ReportDetailDTO> GiveReportDetail()
        {
            var reportDetails = _contactInfoRepo.GetDataWithLinqExp(x => x.DeletedAt == null && x.Location != null).GroupBy(x => x.Location).Select(x => new ReportDetailDTO
            {
                Location = x.Key,
                PhoneCount = x.Select(x => x.PhoneNumber).ToList().Count.ToString(),
                PersonCount = x.Select(x => x.PersonId != null).Distinct().ToList().Count.ToString()
            }).ToList();

            return reportDetails;
        }


        //Buraya 1 tane ID göndereceğiz
        public void SendReportDetailstoRabbitMQ(string reportID)
        {

            var reportDetails = GiveReportDetail();
            var reportDetailsJson = JsonConvert.SerializeObject(reportDetails);

            ConnectionFactory factory = new();
            factory.Uri = new("amqps://agynxcdk:czCiBq-QnX_MgyDat8Iqxl2bVtBOGZFH@woodpecker.rmq.cloudamqp.com/agynxcdk");




            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();


            channel.QueueDeclare(queue: reportID, exclusive: false);
            //channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

            byte[] byteMessage = Encoding.UTF8.GetBytes(reportDetailsJson);

            channel.BasicPublish(
                exchange: "",
                routingKey: reportID,
                body: byteMessage);
        }
    }
}
