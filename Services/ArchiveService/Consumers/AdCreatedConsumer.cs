using AutoMapper;
using Events;
using MassTransit;
using MongoDB.Entities;
using ArchiveService.Models;

namespace ArchiveService.Consumers;
public class AdCreatedConsumer : IConsumer<AdCreated>
{
    private readonly IMapper _mapper;

    public AdCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AdCreated> adCreated)
    {
        Console.WriteLine("Consuming ad created " + adCreated.Message.Id);

        var ad = _mapper.Map<Ad>(adCreated.Message);

        await ad.SaveAsync();
    }
}