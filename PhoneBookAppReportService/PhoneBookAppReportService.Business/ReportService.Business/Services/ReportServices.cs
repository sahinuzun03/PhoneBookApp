using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportService.Business.Models;
using ReportService.DataAccess.Repo;
using ReportService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Business.Services
{
    public class ReportServices : IReportServices
    {
        private IConfiguration _configuration;
        private readonly IReportRepo _reportRepo;
        private readonly IMapper _mapper;
        public ReportServices(IReportRepo reportRepo,IMapper mapper, IConfiguration configuration)
        {
            _reportRepo = reportRepo;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<Report> CreateReport(ReportModel model)
        {
            var addReport = _mapper.Map<Report>(model);
            await _reportRepo.InsertAsync(addReport);   
            _reportRepo.SaveChanges();

            return addReport;
        }

        
        public async Task WantReportDetail()
        {
            var report = await CreateReport(new ReportModel());

            string requestQueueName = _configuration["RabbitQM:RequestQueueName"];
            string Uri = _configuration["RabbitQM:Uri"];


            // Request kuyruğunun oluşturulması ve mesaj gönderilmesi 
            ConnectionFactory factory = new();
            factory.Uri = new(Uri);


            using IConnection connection = factory.CreateConnection();
            using RabbitMQ.Client.IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: requestQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false);

            string replyQueueName = channel.QueueDeclare().QueueName;
            string correlationId = report.Id.ToString();

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = replyQueueName;

            byte[] body = Encoding.UTF8.GetBytes(correlationId);
            channel.BasicPublish(
                exchange:string.Empty,
                routingKey:requestQueueName,
                body: body,
                basicProperties:properties);

            //Response kuyruğunu dinleyeceğim!!

            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue:replyQueueName,
                autoAck:false,
                consumer:consumer);

            List<ReportDetailDTO> reportDetailDTOs = new List<ReportDetailDTO>();
            string JsonVeri = "";

            consumer.Received += async (sender, e) =>
            {
                if (e.BasicProperties.CorrelationId == correlationId)
                {
                    //RabbitMQ tarafından mesajı aldık
                    var reportDetailString = Encoding.UTF8.GetString(e.Body.Span);
                    var jsonReportDetails = JsonConvert.DeserializeObject<List<ReportDetailDTO>>(reportDetailString);

                    JsonVeri = reportDetailString;

                    foreach (var item in jsonReportDetails)
                    {
                        reportDetailDTOs.Add(new ReportDetailDTO
                        {
                            Location = item.Location,
                            PhoneCount = item.PhoneCount,
                            PersonCount = item.PersonCount,
                        });
                    }
                }
            };

            var test = JsonVeri;
            report.ReportStatus = ReportStatus.Completed;
            foreach (var reportDetail in reportDetailDTOs)
            {
                report.ReportDetails.Add(new ReportDetail()
                {
                    Id = Guid.NewGuid(),
                    Location = reportDetail.Location,
                    PersonCount = reportDetail.PersonCount,
                    PhoneCount = reportDetail.PhoneCount,
                });

            }

            _reportRepo.SaveChanges();
        }
    }
}
