using Library.Entities;
using Library.Exceptions;
using Library.Repositories;
using MediatR;

namespace Library.Commands
{
    public class CreateRoomCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if (request.Name is null)
            {
                throw new ForbiddenException("Room needs to have a name");
            }
            Room room = new Room();
            room.Name = request.Name;
            await _roomRepository.AddRoom(room, cancellationToken);
            return room.Id;
        }
    }
}