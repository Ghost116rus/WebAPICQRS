using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Aplication.Common.Exceptions;
using Notes.Aplication.Interfaces;
using Notes.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Aplication.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler
        : IRequestHandler<UpdateNoteCommand>
    {
        private readonly INotesDbContext _context;

        public UpdateNoteCommandHandler(INotesDbContext notesDbContext) => _context = notesDbContext;

        public async Task<Unit> Handle(UpdateNoteCommand request,
            CancellationToken cancellationToken)
        {
            var entity = 
                await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id); 
            }

            entity.Details = request.Details;
            entity.Title = request.Title;
            entity.EditDate = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
