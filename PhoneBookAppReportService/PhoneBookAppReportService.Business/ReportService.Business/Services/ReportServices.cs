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
        private readonly IReportDetailService _reportDetailService;
        private readonly IMapper _mapper;
        public ReportServices(IReportRepo reportRepo, IMapper mapper, IConfiguration configuration, IReportDetailService reportDetailService)
        {
            _reportRepo = reportRepo;
            _mapper = mapper;
            _configuration = configuration;
            _reportDetailService = reportDetailService;
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


            using (IConnection connection = factory.CreateConnection())
            using (RabbitMQ.Client.IModel channel = connection.CreateModel())
            {

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

                byte[] message = Encoding.UTF8.GetBytes(correlationId);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: requestQueueName,
                    body: message,
                    basicProperties: properties);

                //Response kuyruğunu dinleyeceğim!!

                EventingBasicConsumer consumer = new(channel);
                channel.BasicConsume(
                    queue: replyQueueName,
                    autoAck: true,
                    consumer: consumer);


                List<ReportDetailDTO> reportDetailDTOs = new List<ReportDetailDTO>();
                var JsonVeri = new List<ReportDetailDTO>();
                consumer.Received += (sender, e) =>
                {
                    if (e.BasicProperties.CorrelationId == correlationId)
                    {
                        //RabbitMQ tarafından mesajı aldık
                        var reportDetailString = Encoding.UTF8.GetString(e.Body.Span);
                        var jsonReportDetails = JsonConvert.DeserializeObject<List<ReportDetailDTO>>(reportDetailString);

                        foreach (var item in jsonReportDetails)
                        {
                            reportDetailDTOs.Add(new ReportDetailDTO
                            {
                                Location = item.Location,
                                PhoneCount = item.PhoneCount,
                                PersonCount = item.PersonCount,
                            });
                        }
                        JsonVeri = reportDetailDTOs;

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
                };
            }

        }

        public async Task WantReportDetails()
        {

            var report = await CreateReport(new ReportModel());

            string routingKey = report.Id.ToString().ToUpper();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.BaseAddress = new Uri("https://localhost:7112/");
                var responseTask = await client.GetAsync("Report/GetReportDetail/" + routingKey);

                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://agynxcdk:czCiBq-QnX_MgyDat8Iqxl2bVtBOGZFH@woodpecker.rmq.cloudamqp.com/agynxcdk");

                using IConnection connection = factory.CreateConnection();
                using RabbitMQ.Client.IModel channel = connection.CreateModel();

                channel.QueueDeclare(queue: routingKey, exclusive: false);

                EventingBasicConsumer consumer = new(channel);

                channel.BasicConsume(queue: routingKey, false, consumer);

                consumer.Received += async (sender, e) =>
                {
                    report.ReportStatus = ReportStatus.Completed;
                    _reportRepo.SaveChanges();
                    var reportDetails = Encoding.UTF8.GetString(e.Body.ToArray());
                    var jsonReportDetails = JsonConvert.DeserializeObject<List<ReportDetailDTO>>(reportDetails);
                    foreach (var item in jsonReportDetails)
                    {
                        ReportDetailModel newReportDetail = new ReportDetailModel()
                        {
                            Report = report,
                            Id = Guid.NewGuid(),
                            Location = item.Location,
                            PhoneCount = item.PhoneCount,
                            PersonCount = item.PersonCount
                        };
                        await _reportDetailService.CreateReportDetail(newReportDetail);

                    }
                };
            }
        }
    }
}
